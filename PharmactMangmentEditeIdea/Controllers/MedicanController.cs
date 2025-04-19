using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmactMangmentBLL.Interfaces;
using PharmactMangmentBLL.Repositories;
using PharmactMangmentDAL.Data.Contexts;
using PharmactMangmentDAL.Models;
using PharmactMangmentEditeIdea.HelperImage;
using PharmactMangmentEditeIdea.ViewModel;

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

        #region DashbordForAddMedican
        [HttpGet]
        public IActionResult DashbordForAddMedican()
        {
            IEnumerable<Medication> x = _dbContext.Medications.ToList();
            return View(x);
        }

        // Updated to accept PharmacyMedicationDto objects
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DashbordForAddMedican(List<PharmacyMedicationDto> medications)
        {
            if (medications == null || !medications.Any())
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

                // Process each medication with its quantity and stock status
                foreach (var medication in medications)
                {
                    // 2️⃣ Check if medication exists
                    var medicationEntity = await _dbContext.Medications.FindAsync(medication.MedicationId);
                    if (medicationEntity == null)
                    {
                        continue; // Skip this medication and continue with the next one
                    }

                    // 3️⃣ Check if medication is already added to the pharmacy
                    var existingEntry = await _dbContext.Set<Med_Phar>()
                        .FirstOrDefaultAsync(mp => mp.MedicationId == medication.MedicationId && mp.PharmacyId == pharmacy.Id);

                    if (existingEntry == null)
                    {
                        // 4️⃣ Add medication to pharmacy with the provided quantity and stock status
                        var medPhar = new Med_Phar
                        {
                            MedicationId = medication.MedicationId,
                            PharmacyId = pharmacy.Id,
                            Quantity = medication.Quantity,
                            InStock = medication.InStock
                        };

                        _dbContext.Set<Med_Phar>().Add(medPhar);
                        addedCount++;
                    }
                    else
                    {
                        // Update existing entry if needed
                        existingEntry.Quantity = medication.Quantity;
                        existingEntry.InStock = medication.InStock;
                        _dbContext.Set<Med_Phar>().Update(existingEntry);
                    }
                }

                // Save changes if any medications were added or updated
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
            // هات من ال med_phar الحاجات دي
            var medications = await _dbContext.Set<Med_Phar>().Include(mp => mp.medican)
                .Where(mp => mp.PharmacyId == pharmacy.Id)
                .Select(mp => new PharmacyMedicationListDto
                {
                    MedicationId = mp.MedicationId,
                    PharmacyId=mp.PharmacyId,
                    MedicationImageName = mp.medican.ImageName,
                    MedicationName = mp.medican.Name,
                    InStock = mp.InStock,
                    Quantity = mp.Quantity,
                    MedicationPrice = mp.medican.Price,
                })
                .ToListAsync();

            return View(medications);
        }
        #endregion


        #region Delet medicine from pharmact

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedication( int medicationId,  int pharmacyId)
        {


          var item = await _dbContext.Med_Phars
          .FirstOrDefaultAsync(mp => mp.MedicationId == medicationId && mp.PharmacyId == pharmacyId);


            if (item == null)
            {
                return NotFound(); // العلاقة مش موجودة
            }

            _dbContext.Med_Phars.Remove(item);
           int count= await _dbContext.SaveChangesAsync();

            if(count > 0)
            {
                TempData["Message"] = "Medicane Deleted Successfully";

            }
            return RedirectToAction("AllMedicationsForPharmacy"); // أو أي View تاني
        }
        #endregion

    }






    // Add this DTO class to the same namespace or move it to a separate file
    public class PharmacyMedicationDto
    {
        public int MedicationId { get; set; }
        public int Quantity { get; set; }
        public bool InStock { get; set; }
    }
}