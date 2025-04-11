namespace PharmactMangmentEditeIdea.ViewModel
{
    public class UserToReturnViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string OwnerName { get; set; }
        public string NameOfPharmacy { get; set; }
        public string Email { get; set; }
        public IFormFile? Image { get; set; }
        public string? ImageName { get; set; }
        public IEnumerable<string>? Roles { get; set; }

       
    }
}
