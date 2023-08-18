using Google.Apis.Auth;
using Konscious.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PRUEBASB.Application.ViewModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PRUEBASB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly Dictionary<string, (string HashedPassword, byte[] Salt)> _userDataBase;
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _userDataBase = new Dictionary<string, (string, byte[])>();
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] JWTBodyDto tokenRequest)
        {
            try
            {
                var payload = GoogleJsonWebSignature.ValidateAsync(tokenRequest.JWTAuthinticate, new GoogleJsonWebSignature.ValidationSettings()).Result;

                string userId = payload.Subject;
                string userEmail = payload.Email;

                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, userEmail),
                    new Claim(ClaimTypes.Role, "guest")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["GoogleKey:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:7130",
                    audience: "https://localhost:7130",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                );

                return Ok(new { customJwtToken = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [AllowAnonymous]
        [HttpPost("authenticated")]
        public IActionResult LoginAuthenticated([FromBody] LoginUserDto loginUserDto)
        {
            RegisterUser();

            try
            {
                var validateCredentials = AuthenticateUser(loginUserDto.Email, loginUserDto.Password);
                if (!validateCredentials)
                {
                    return BadRequest(new ErrorResponse(false, "Incorrect username or password, please try again."));
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.Email, loginUserDto.Email),
                    new Claim(ClaimTypes.Role, "admin")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["GoogleKey:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:7130",
                    audience: "https://localhost:7130",
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                );

                return Ok(new { customJwtToken = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponse(false, ex.Message));
            }
        }

        private bool AuthenticateUser(string username, string password) 
        {
            if (_userDataBase.TryGetValue(username, out var user))
            {
                string hashedPassword = HashPassword(password, user.Salt);
                return user.HashedPassword == hashedPassword;
            }

            return false;
        }


        private void RegisterUser(string email = "prueba@sb.gob.do", string password = "S3_Cure") 
        {
            byte[] salt = GenerateSalt();
            string hashedPassword = HashPassword(password, salt);
            _userDataBase[email] = (hashedPassword, salt);
        }

        private byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private string HashPassword(string password, byte[] salt)
        {
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8, // Número de hilos paralelos
                MemorySize = 65536,     // Tamaño de la memoria en KiB
                Iterations = 4          // Número de iteraciones
            };
            argon2.Salt = salt;
            byte[] hash = argon2.GetBytes(32); // 256 bits hash
            return Convert.ToBase64String(hash);
        }
    }
}
