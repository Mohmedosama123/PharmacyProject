using Microsoft.AspNetCore.Mvc;
using PharmactMangmentBLL.Interfaces;
using PharmactMangmentBLL.Repositories;
using PharmactMangmentDAL.Data.Contexts;
using PharmactMangmentDAL.Models;
using PharmactMangmentEditeIdea.HelperImage;
using PharmactMangmentEditeIdea.ViewModel;

namespace PharmactMangmentEditeIdea.Controllers
{
    public class MangMidicineByAdminController : Controller
    {
        private readonly PharmaceDbContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;

        public MangMidicineByAdminController(PharmaceDbContext dbContext , IUnitOfWork unitOfWork)
        {
            _dbContext = dbContext;
            _unitOfWork = unitOfWork;
        }

        // all medicine in admin dashbord
        public IActionResult Index()
        {
            IEnumerable<Medication> AllMedicine =_dbContext.Medications.ToList();
            return View(AllMedicine);
        }


        #region Creat medicine in admin dashbord
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(AddMedadicineByAdminViewModel creatMedican)
        {
            if (ModelState.IsValid)
            {

                // contain information of Pharmacy this is relation hir
                //User.Claims
                if (creatMedican.Imags is not null)
                {
                    // save image
                    creatMedican.ImageName = DecumentSettings.UploadImage(creatMedican.Imags, "Images/Medications");
                }

                var medicain = new Medication()
                {
                    Name = creatMedican.Name,
                    Description = creatMedican.Description,
                    Price = creatMedican.Price,
                    Category = creatMedican.Category,
                    ImageName = creatMedican.ImageName

                };


                await _unitOfWork.pharmaceRepository.AddPharmacyAsync(medicain);
                int count =  _dbContext.SaveChanges();


                if (count > 0)
                {
                    TempData["Message"] = "Medican Added Successfully";
                    return RedirectToAction("Index");
                }
            }
            return View(creatMedican);
        }


        #endregion


        #region Edite medicine in admin dashbord
        [HttpGet]
        public async Task<IActionResult> EditMedication([FromRoute] int? id)
        {
            if (id is null)
                return BadRequest();
            var medication = await _unitOfWork.pharmaceRepository.GetMedicanAbyIdAsync(id.Value);
            if (medication is null)
                return NotFound(new { StatusCode = 400, Message = $"Medican with id : {id} not found" });

            var medicainDto = new AddMedadicineByAdminViewModel()
            {
                Name = medication.Name,
                Description = medication.Description,
                Price = medication.Price,
                Category = medication.Category,
                ImageName = medication.ImageName
            };
            return View(medicainDto);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMedication([FromRoute] int? id, AddMedadicineByAdminViewModel dto)
        {
            if (ModelState.IsValid)
            {


                //// لو في صوره هيحذف
                if (dto.ImageName is not null && dto.Imags is not null)
                {
                    // delet image
                    DecumentSettings.DeleteImage("Images/Medications", dto.ImageName);
                }

                if (dto.Imags is not null)
                {
                    // save image
                    dto.ImageName = DecumentSettings.UploadImage(dto.Imags, "Images/Medications");
                }
                //=================== Using AutoMapper ===================
                var medicain = new Medication()
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Price = dto.Price,
                    Category = dto.Category,
                    ImageName = dto.ImageName
                };

                medicain.Id = id.Value;
                await _unitOfWork.pharmaceRepository.UpdateMedican(medicain);
                int count = await _unitOfWork.completeAsync();

                if (count > 0)
                {
                    TempData["Message"] = "Medican Updated Successfully";
                    return RedirectToAction("Index");
                }
                // هيروح علي لوحه التحكم
                return View(medicain);
            }
            return View(dto);
        }

        #endregion



        #region Delet  medicine in admin dashbord


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedication([FromRoute] int? id)
        {
            if (ModelState.IsValid)
            {
                if (id is null)
                    return BadRequest();
                var medication = await _unitOfWork.pharmaceRepository.GetMedicanAbyIdAsync(id.Value);
                if (medication is null)
                    return NotFound(new { StatusCode = 400, Message = $"Medican with id : {id} not found" });
                // هيحذف الدواء هنا
                _unitOfWork.pharmaceRepository.DeleteMedican(medication);
                int count = await _unitOfWork.completeAsync();

                if (count > 0)
                {
                    DecumentSettings.DeleteImage("Images/Medications", medication.ImageName);
                    TempData["Message"] = "Medication Deleted Successfully";
                    return RedirectToAction("Index");
                }
                // هيروح علي لوحه التحكم
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        #endregion
    }
}
