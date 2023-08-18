
namespace PRUEBASB.Application.ViewModel
{
    public class CitizenVMUpdate
    {
        public string CIN { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public bool? Status { get; set; }
        public DateTime? DtEdit { get; set; } = DateTime.Now;
    }
}
