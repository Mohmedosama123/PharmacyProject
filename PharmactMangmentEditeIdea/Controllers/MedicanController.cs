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
