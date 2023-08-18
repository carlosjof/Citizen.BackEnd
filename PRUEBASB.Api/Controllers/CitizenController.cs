using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRUEBASB.Application.Interface;
using PRUEBASB.Application.ViewModel;
using PRUEBASB.Domain.Entities;

namespace PRUEBASB.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitizenController : ControllerBase
    {
        private readonly IPruebaSBService _service;
        private readonly IMapper _mapper;
        private readonly IValidator<CitizenCreateDto> _validatorCreate;
        private readonly IValidator<CitizenUpdateDto> _validatorUpdate;
        private readonly IValidator<string> _validatorCIN;

        public CitizenController(
            IPruebaSBService service, 
            IMapper mapper, 
            IValidator<CitizenCreateDto> validatorCreate, 
            IValidator<CitizenUpdateDto> validatorUpdate,
            IValidator<string> validatorCIN)
        {
            _service = service;
            _mapper = mapper;
            _validatorCreate = validatorCreate;
            _validatorUpdate = validatorUpdate;
            _validatorCIN = validatorCIN;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAllCitizen([FromQuery]int page,[FromQuery] int pageSize)
        {
            var result = await _service.GetPagedCitizen(page, pageSize);

            return Ok(result);
        }

        [HttpGet("{cin}")]
        [Authorize]
        public async Task<IActionResult> GetCitizen(string cin)
        {
            var validation = await _validatorCIN.ValidateAsync(cin);
            if (!validation.IsValid)
            {
                return BadRequest(new ErrorResponse(false, validation.ToDictionary()));
            }

            var validateCIN = await _service.CitizenExist(cin);
            if (!validateCIN)
            {
                return BadRequest(new ErrorResponse(false, "CIN already doesn't exist"));
            }

            var result = await _service.GetCitizen(cin);

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddCitizen([FromBody] CitizenCreateDto citizenCreateDto)
        {
            var validation = await _validatorCreate.ValidateAsync(citizenCreateDto);
            if (!validation.IsValid)
            {
                return BadRequest(new ErrorResponse(false, validation.ToDictionary()));
            }

            var validateCIN = await _service.CitizenExist(citizenCreateDto.CIN);
            if (validateCIN)
            {
                return BadRequest(new ErrorResponse(false, "CIN already exist"));
            }

            var citizen = _mapper.Map<CitizenCreateDto, Citizen>(citizenCreateDto);
            var result = await _service.CreateCitizen(citizen);

            if (!result.IsSuccess)
            {
                return StatusCode(501, result);
            }

            return Ok(result);
        }

        [HttpPut("{cin}")]
        [Authorize]
        public async Task<IActionResult> EditCitizen(string cin, [FromBody] CitizenUpdateDto citizenUpdateDto)
        {
            var validationCIN = await _validatorCIN.ValidateAsync(cin);
            if (!validationCIN.IsValid)
            {
                return BadRequest(new ErrorResponse(false, validationCIN.ToDictionary()));
            }

            var validation = await _validatorUpdate.ValidateAsync(citizenUpdateDto);
            if (!validation.IsValid)
            {
                return BadRequest(new ErrorResponse(false, validation.ToDictionary()));
            }

            var validateCIN = await _service.CitizenExist(cin);
            if (!validateCIN)
            {
                return BadRequest(new ErrorResponse(false, "CIN doesn't exist"));
            }

            var citizen = _mapper.Map<CitizenUpdateDto, CitizenVMUpdate>(citizenUpdateDto);
            var result = await _service.UpdateCitizen(cin, citizen);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpDelete("{cin}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCitizen(string cin)
        {
            var validationCIN = await _validatorCIN.ValidateAsync(cin);
            if (!validationCIN.IsValid)
            {
                return BadRequest(new ErrorResponse(false, validationCIN.ToDictionary()));
            }

            if (!User.IsInRole("admin")) 
            {
                return Unauthorized("Your role doesn't have permission to make the request");
            }

            var validateCIN = await _service.CitizenExist(cin);
            if (!validateCIN)
            {
                return BadRequest(new ErrorResponse(false, "CIN doesn't exist"));
            }

            var result = await _service.DeleteCitizen(cin);

            if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return BadRequest(new ErrorResponse(false, "No result"));
            }

            if (!result.IsSuccess)
            {
                return StatusCode(501, result);
            }

            return Ok(result);
        }
    }
}
