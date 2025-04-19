using PharmactMangmentEditeIdea.Enums;

namespace PharmactMangmentEditeIdea.ViewModel
{
    public class SrearchAboutMedicineDto
    {
        public string? MedcineName { get; set; }
        public string? Govornorate { get; set; }
        public string? District { get; set; }
        public string? Area { get; set; }
        public OrderBy OrderBy { get; set; } = OrderBy.MedicationName;
        public bool IsAsc { get; set; } = true;

        public List<MedicineListDto> MedicineList { get; set; } = new List<MedicineListDto>();
    }
}
