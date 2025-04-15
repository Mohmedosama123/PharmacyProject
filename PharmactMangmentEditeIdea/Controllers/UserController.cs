using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmactMangmentEditeIdea.HelperImage;
using PharmactMangmentDAL.Models;
using PharmactMangmentEditeIdea.ViewModel;

namespace PharmactMangmentEditeIdea.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<UserToReturnViewModel> users;
            users = _userManager.Users.Select(U => new UserToReturnViewModel()
            {
                Id = U.Id,
                UserName = U.UserName,
                OwnerName = U.OwnerName,
                NameOfPharmacy = U.NameOfPharmacy,
                Email = U.Email,
                Roles = _userManager.GetRolesAsync(U).Result,
                ImageName = string.IsNullOrEmpty(U.ImageName) ? "OIP.jpg" : U.ImageName, // ✅ تعيين الصورة الافتراضية

            }).ToList();

            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound(new { StatusCode = 404, Message = $"Employee with id : {id} not found" });
            var userViewmodel = new UserToReturnViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                OwnerName = user.OwnerName,
                NameOfPharmacy = user.NameOfPharmacy,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result,
                ImageName = string.IsNullOrEmpty(user.ImageName) ? "OIP.jpg" : user.ImageName // ✅ تعيين الصورة الافتراضية

            };
            return View(userViewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, UserToReturnViewModel userToReturnDTO)
        {
            if (ModelState.IsValid)
            {
                var usercheck = await _userManager.FindByNameAsync(userToReturnDTO.UserName);

                if (usercheck is not null)
                {
                    usercheck = await _userManager.FindByEmailAsync(userToReturnDTO.Email);

                    if (usercheck is not null)
                    {
                        // if image is not null and image name is not null
                        if (userToReturnDTO.ImageName is not null && userToReturnDTO.Image is not null)
                        {
                            // delet image
                            DecumentSettings.DeleteImage("Images/Profiles", userToReturnDTO.ImageName);
                        }

                        if (userToReturnDTO.Image is not null)
                        {
                            // save image
                            userToReturnDTO.ImageName = DecumentSettings.UploadImage(userToReturnDTO.Image, "Images/Profiles");
                        }

                        if (id != userToReturnDTO.Id) return BadRequest("Invalid Operation");
                        var user = await _userManager.FindByIdAsync(id);
                        if (user == null) return BadRequest("Invalid Operation");

                        user.UserName = userToReturnDTO.UserName;
                        user.NameOfPharmacy = userToReturnDTO.NameOfPharmacy;
                        user.OwnerName = userToReturnDTO.OwnerName;
                        user.Email = userToReturnDTO.Email;

                        //if (userToReturnDTO.Image is not null)
                        //{
                        //    // حفظ الصورة وتحديث اسمها في قاعدة البيانات
                        //    var fileName = DecumentSettings.UploadImage(userToReturnDTO.Image, "Images");
                        //    user.ImageName = fileName;  // ✅ التحديث على `user`
                        //}

                        var result = await _userManager.UpdateAsync(user);
                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                return View(userToReturnDTO);
            }

            return View(userToReturnDTO);

        }


        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound(new { StatusCode = 404, Message = $"Employee with id : {id} not found" });
            var userViewmodel = new UserToReturnViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                OwnerName = user.OwnerName,
                NameOfPharmacy = user.NameOfPharmacy,
                Email = user.Email,
                Roles = _userManager.GetRolesAsync(user).Result,
                ImageName = string.IsNullOrEmpty(user.ImageName) ? "OIP.jpg" : user.ImageName // ✅ تعيين الصورة الافتراضية

            };
            return View(userViewmodel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, UserToReturnViewModel userToReturnDTO)
        {
            if (ModelState.IsValid)
            {
                if (id != userToReturnDTO.Id) return BadRequest("Invalid Operation");
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return BadRequest("Invalid Operation");

                user.UserName = userToReturnDTO.UserName;
                user.NameOfPharmacy = userToReturnDTO.NameOfPharmacy;
                user.OwnerName = userToReturnDTO.OwnerName;
                user.Email = userToReturnDTO.Email;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                    
                }
                return View(userToReturnDTO);
        }

    }
}
