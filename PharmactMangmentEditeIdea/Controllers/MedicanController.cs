using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmactMangmentBLL.Interfaces;
using PharmactMangmentDAL.Data.Contexts;
using PharmactMangmentDAL.Models;

namespace PharmactMangmentEditeIdea.Controllers
{
    [Authorize(Roles = "Pharmacy")]
    public class MedicanController : Controller
    {
        private readonly PharmaceDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork ofWork;

        public MedicanController(PharmaceDbContext dbContext, UserManager<AppUser> userManager, IUnitOfWork ofWork)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            this.ofWork = ofWork;
        }

        #region Eng/ Mahmode Created
        /*
        [HttpGet]
        public IActionResult Index()
        {
            var result = _dbContext.Medications.ToList();
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateMedicationDto model)
        {
            string? imgName = null;
            if (model.Image is not null)
            {
                imgName = DecumentSettings.UploadImage(model.Image, "Images/Medications");
            }

            _dbContext.Medications.Add(new Medication
            {
                Category = model.Category,
                Description = model.Description,
                ImageName = imgName,
                Name = model.Name,
                Price = model.Price,
            });
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var medication = _dbContext.Medications.Find(id);

            if (medication == null)
                return View("Medication not found!");

            return View(new EditMedicationDto
            {
                Id = id,
                Category = medication.Category,
                Description = medication.Description,
                ImageName = medication.ImageName,
                Name = medication.Name,
                Price = medication.Price,
            });
        }

        [HttpPost]
        public IActionResult Edit(EditMedicationDto model)
        {
            var medication = _dbContext.Medications.Find(model.Id);

            if (medication == null)
                return View("Medication not found!");

            string? imgName = null;
            if (model.Image is not null)
            {
                if (!string.IsNullOrEmpty(model.ImageName))
                    DecumentSettings.DeleteImage("Images/Medications", model.ImageName);
                imgName = DecumentSettings.UploadImage(model.Image, "Images/Medications");
            }

            medication.Description = model.Description;
            medication.ImageName = imgName ?? medication.ImageName;
            medication.Name = model.Name;
            medication.Price = model.Price;
            medication.Category = model.Category;

            _dbContext.Update(medication);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        */
        #endregion

        #region DashbordForAddMedican
        [HttpGet]
        public IActionResult DashbordForAddMedican()
        {
            IEnumerable<Medication> x = _dbContext.Medications.ToList();
            return View(x);
        }

        // Update the POST action to accept an array of medication IDs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DashbordForAddMedican(List<int> medicationIds)
        {
            if (medicationIds == null || !medicationIds.Any())
            {
                TempData["ErrorMessage"] = "No medications selected.";
                return RedirectToAction("Dashbord");
            }

            try
            {
                // 1️⃣ Get the current user's pharmacy
                var userId = _userManager.GetUserId(User);
                var pharmacy = await _dbContext.Pharmacies.FirstOrDefaultAsync(p => p.userId == userId);

                if (pharmacy == null)
                {
                    TempData["ErrorMessage"] = "Pharmacy not found.";
                    return RedirectToAction("Dashbord");
                }

                int addedCount = 0;

                // Process each medication ID
                foreach (var medicationId in medicationIds)
                {
                    // 2️⃣ Check if medication exists
                    var medication = await _dbContext.Medications.FindAsync(medicationId);
                    if (medication == null)
                    {
                        continue; // Skip this medication and continue with the next one
                    }

                    // 3️⃣ Check if medication is already added to the pharmacy
                    var existingEntry = await _dbContext.Set<Med_Phar>()
                        .FirstOrDefaultAsync(mp => mp.MedicationId == medicationId && mp.PharmacyId == pharmacy.Id);

                    if (existingEntry == null)
                    {
                        // 4️⃣ Add medication to pharmacy
                        var medPhar = new Med_Phar
                        {
                            MedicationId = medicationId,
                            PharmacyId = pharmacy.Id,
                            // Set default values for Quantity and InStock until handel in view
                            Quantity = 1,
                            InStock = true
                        };

                        _dbContext.Set<Med_Phar>().Add(medPhar);
                        addedCount++;
                    }
                }

                // Save changes if any medications were added
                if (addedCount > 0)
                {
                    await _dbContext.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"{addedCount} medications added to your pharmacy.";

                }
                else
                {
                    TempData["InfoMessage"] = "No new medications were added to your pharmacy.";
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            return RedirectToAction("AllMedicationsForPharmacy");
        }

        #endregion

        #region AllMedicationsForPharmacy

        [HttpGet]
        public async Task<IActionResult> AllMedicationsForPharmacy()
        {
            var userId = _userManager.GetUserId(User);
            var pharmacy = await _dbContext.Pharmacies.FirstOrDefaultAsync(p => p.userId == userId);

            if (pharmacy == null)
            {
                return NotFound("Pharmacy not found.");
            }

            var medications = await _dbContext.Set<Med_Phar>()
                .Where(mp => mp.PharmacyId == pharmacy.Id)
                .Select(mp => mp.medican)
                .ToListAsync();

            return View(medications);
        } 
        #endregion

    }
}
