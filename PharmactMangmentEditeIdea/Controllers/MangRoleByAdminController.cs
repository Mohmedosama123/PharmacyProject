using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PharmactMangmentDAL.Models;
using PharmactMangmentEditeIdea.HelperImage;
using PharmactMangmentEditeIdea.ViewModel;

namespace PharmactMangmentEditeIdea.Controllers
{
    public class MangRoleByAdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public MangRoleByAdminController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }


        [HttpGet]
        public async Task<IActionResult> IndexRole()
        {
            IEnumerable<RoleViewModel> Role;
            Role = _roleManager.Roles.Select(R => new RoleViewModel()
            {
                Id = R.Id,
                Name = R.Name

            }); 

            return View(Role);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }



        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(RoleViewModel roleToReturnDTO)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByNameAsync(roleToReturnDTO.Name);
                if (role == null)
                {
                    role = new IdentityRole()
                    {
                        Name = roleToReturnDTO.Name,
                    };

                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("IndexRole");
                    }
                }
            }

            // لو في خطأ، نرجع نفس البيانات عشان الفيو يشتغل صح
            ViewData["Role"] = _roleManager.Roles.Select(r => new RoleViewModel
            {
                Id = r.Id,
                Name = r.Name
            }).ToList();

            return View(roleToReturnDTO);
        }




        [HttpGet]
        public async Task<IActionResult> EditRole(string? id)
        {
            if (id is null) return BadRequest("Invalid Id");
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound(new { StatusCode = 404, Message = $"role with id : {id} not found" });
            var UserRole = new RoleViewModel()
            {
                Name = role.Name,
            };
            return View(UserRole);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole([FromRoute] string? id, RoleViewModel roleToReturnDTO)
        {
            if (ModelState.IsValid)
            {
                if (id != roleToReturnDTO.Id) return BadRequest("Invalid Operation");
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return BadRequest("Invalid Operation");

                var result01 = await _roleManager.FindByNameAsync(roleToReturnDTO.Name);
                if (result01 is null)
                {
                    role.Name = roleToReturnDTO.Name;

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(IndexRole));
                    }
                }
                ModelState.AddModelError("", "Invalid Operation");

            }
            return View("EditRole");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole([FromRoute] string? id, RoleViewModel roleDeletDTO)
        {

            if (ModelState.IsValid)
            {
                if (id != roleDeletDTO.Id) return BadRequest("Invalid Operation");
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null) return BadRequest("Invalid Operation");


                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(IndexRole));
                }

                ModelState.AddModelError("", "Invalid Operation");

            }
            return View("DeleteRole");
        }

    }
}
