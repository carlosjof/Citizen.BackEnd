using System.ComponentModel.DataAnnotations;

namespace PRUEBASB.Domain.Entities
{
    public class Citizen
    {
        [Key]
        public int Id { get; set; }
        public string CIN { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public bool? Status { get; set; } = true;
        public DateTime? DtCreate { get; set; } = DateTime.Now;
        public DateTime? DtEdit { get; set; }
    }
}
