using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using PharmactMangmentDAL.Data.Contexts;
using PharmactMangmentDAL.Models;
using PharmactMangmentEditeIdea.ViewModel;
using PharmactMangmentEditeIdea.HelperMethod;

namespace PharmactMangmentEditeIdea.Controllers
{
    public class AccountController : Controller
    {
        private readonly PharmaceDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(PharmaceDbContext dbContext ,UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region SignUp
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel signUpView)
        {
            if (ModelState.IsValid)
            {

                var user = await _userManager.FindByNameAsync(signUpView.UserName);

                if (user is null)
                {
                    user = await _userManager.FindByEmailAsync(signUpView.Email);

                    if (user is null)
                    {
                        // Manual Maping 
                        user = new AppUser
                        {
                            UserName = signUpView.UserName,
                            Email = signUpView.Email,
                            IsAgree = true,
                            NameOfPharmacy = signUpView.NameOfPharmacy,
                            OwnerName = signUpView.OwnerName,
                            License_Number = signUpView.License_Number,
                            PhoneNumber = signUpView.PhoneNumber,
                            OperatingHours = signUpView.OperatingHours,
                            FullAddress = signUpView.FullAddress,
                            Governorate = signUpView.Governorate,
                            District = signUpView.District,
                            Area = signUpView.Area

                        };

                        var result = await _userManager.CreateAsync(user, signUpView.Password);
                        if (result.Succeeded)
                        {
                            var pharmacy= new Pharmacy
                            {
                                Email = signUpView.Email,
                                NameOfPharmacy = signUpView.NameOfPharmacy,
                                OwnerName = signUpView.OwnerName,
                                License_Number = signUpView.License_Number,
                                PhoneNumber = signUpView.PhoneNumber,
                                OperatingHours = signUpView.OperatingHours,
                                FullAddress = signUpView.FullAddress,
                                Governorate = signUpView.Governorate,
                                District = signUpView.District,
                                Area = signUpView.Area,
                                userId = user.Id
                            };

                            _dbContext.Pharmacies.Add(pharmacy);
                            await _dbContext.SaveChangesAsync();
                            return RedirectToAction("SignIn");
                        }
                        foreach (var error in result.Errors)
                        {

                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }

                ModelState.AddModelError("", "Invalid SinUp !!");

            }
            return View();

        }
        #endregion


        #region SingIn login
        
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel signInView)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(signInView.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, signInView.Password);
                    if (flag)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, signInView.Password, signInView.RememberMe ?? false, false);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Dashbord", "Medican");
                        }
                    }
                }
                ModelState.AddModelError("", "Invalid Email Or Password");
            }
            return View();
        }

        #endregion

        #region SingOut
        // new for hide the signout from the user
        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }
        #endregion


        #region ForgetPassword
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendRestePasswordUrl(ForgetPasswordViewModel model)
        {
       
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    // Generate Token
                    // علشان اعمل token للريست باسوورد
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    //Create URL
                    // tokek used in encrept the email url
                    // هعمل token عشان احمي اليوزر من ان حد يقدر يفتح اللينك بتاع الريست باسوورد
                    var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

                    //Create Email 
                    var email = new HelperMethod.Email()
                    {
                        To = model.Email,
                        Subject = "Reset Password",
                        Body = url
                    };

                    //// send Email الطريقه القديمه 
                    var flag = EmailSettings.SendEmail(email);
                    if (flag)
                    {
                        // can creat popup hire
                        //Check Your Email Inbox
                        return RedirectToAction("CheckYourInbox");
                    }
                }

            }
            ModelState.AddModelError("", "Invalid Reset Password !!");
            return View("ForgetPassword", model);
            
        }

        //[HttpPost]
        //public async Task<IActionResult> SendRestePasswordSms(ForgetPasswordDTO model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.FindByEmailAsync(model.Email);
        //        if (user is not null)
        //        {
        //            // Generate Token
        //            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        //            //Create URL
        //            var url = Url.Action("ResetPassword", "Account", new { email = model.Email, token }, Request.Scheme);

        //            //Create Sms 
        //            var sms = new Sms()
        //            {
        //                To = user.PhoneNumber,
        //                Body = url
        //            };

        //            //_mailService.SendEmail(email);

        //            _twilioService.SendSms(sms);

        //            return RedirectToAction("CheckYourInbox");

        //        }

        //    }
        //    ModelState.AddModelError("", "Invalid Reset Password !!");
        //    return View("ForgetPassword", model);
        //    //return View();
        //}

        #endregion


        #region Check Your Inbox

        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }

        #endregion


        #region ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            // الايميل والتوكن هبعتهم من اللينك اللي هيتبعت في الميل علشان يغير ال password
            TempData["email"] = email;
            TempData["token"] = token;


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var email = TempData["email"] as string;
                var token = TempData["token"] as string;


                if (email is not null && token is not null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if (user is not null)
                    {
                        // user - token - password
                        var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("SignIn");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
                else
                {
                    return BadRequest("Invalid opeeration !! ");
                }

            }
            return View();
        }

        #endregion


    }
}
