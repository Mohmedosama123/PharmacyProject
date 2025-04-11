using System.ComponentModel.DataAnnotations;

namespace PharmactMangmentEditeIdea.ViewModel
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        [Required(ErrorMessage = "ConfirmPassword Is Required")]
        [DataType(DataType.Password)]

        [Compare("Password", ErrorMessage = "Password And ConfirmPassword Do Not Match")]
        public string ConfirmPassword { get; set; }
    }
}
