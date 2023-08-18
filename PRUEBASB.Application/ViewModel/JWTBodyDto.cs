using System.ComponentModel.DataAnnotations;

namespace PRUEBASB.Application.ViewModel
{
    public class JWTBodyDto
    {
        [Required]
        public string JWTAuthinticate { get; set; }
    }
}
