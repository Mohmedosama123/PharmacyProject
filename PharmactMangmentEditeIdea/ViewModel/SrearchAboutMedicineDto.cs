namespace PharmactMangmentEditeIdea.ViewModel
{
    public class SrearchAboutMedicineDto
    {
        public string? MedcineName { get; set; }
        public string? Govornorate { get; set; }
        public string? District { get; set; }
        public string? Area { get; set; }

        public List<MedicineListDto> MedicineList { get; set; } = new List<MedicineListDto>();
    }
}
