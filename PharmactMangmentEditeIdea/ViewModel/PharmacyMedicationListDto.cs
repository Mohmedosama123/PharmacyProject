namespace PharmactMangmentEditeIdea.ViewModel
{
    public class PharmacyMedicationListDto
    {
        public int MedicationId { get; set; }
        public int PharmacyId { get; set; }
        public string? MedicationImageName { get; set; }
        public string MedicationName { get; set; } = string.Empty;
        public decimal MedicationPrice { get; set; }
        public int Quantity { get; set; }
        public bool InStock { get; set; }
    }
}
