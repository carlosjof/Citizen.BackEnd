using System.ComponentModel.DataAnnotations;

namespace PRUEBASB.Application.ViewModel
{
    public class CitizenCreateDto
    {
        public string CIN { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
    }
}
