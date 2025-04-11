using System.ComponentModel.DataAnnotations;

namespace PharmactMangmentEditeIdea.ViewModel
{
    public class ForgetPasswordViewModel
    {

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


    }
}
