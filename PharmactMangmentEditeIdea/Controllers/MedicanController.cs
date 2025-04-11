using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmactMangmentBLL.Interfaces;
using PharmactMangmentDAL.Data.Contexts;
using PharmactMangmentDAL.Models;

namespace PharmactMangmentEditeIdea.Controllers
{
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


        //#region MyRegion
        //[HttpGet]
        //public IActionResult Dashbord()
        //{
        //    IEnumerable<Medication> x = _dbContext.Medications.ToList();
        //    return View(x);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Dashbord(int medicationId)
        //{
        //    // 1️⃣ استرجاع المستخدم المسجل حاليًا
        //    var userId = _userManager.GetUserId(User); // جلب ID المستخدم الحالي
        //    var pharmacy = await _dbContext.Pharmacies.FirstOrDefaultAsync(p => p.userId == userId);

        //    if (pharmacy == null)
        //    {
        //        return NotFound("Pharmacy not found.");
        //    }

        //    // 2️⃣ التأكد من أن الدواء موجود
        //    var medication = await _dbContext.Medications.FindAsync(medicationId);
        //    if (medication == null)
        //    {
        //        return NotFound("Medication not found.");
        //    }

        //    // 3️⃣ التأكد أن الدواء غير مضاف مسبقًا للصيدلية
        //    var existingEntry = await _dbContext.Set<Med_Phar>()
        //        .FirstOrDefaultAsync(mp => mp.MedicationId == medicationId && mp.PharmacyId == pharmacy.Id);

        //    if (existingEntry == null)
        //    {
        //        // 4️⃣ إضافة الدواء إلى الصيدلية
        //        var medPhar = new Med_Phar
        //        {
        //            MedicationId = medicationId,
        //            PharmacyId = pharmacy.Id,
        //            Quantity = 1,
        //            InStock = true
        //        };

        //        _dbContext.Set<Med_Phar>().Add(medPhar);
        //        await _dbContext.SaveChangesAsync();
        //    }

        //    return RedirectToAction("Dashbord");
        //}


        ////إضافة الأدوية عبر POST


        ////[HttpPost]
        ////public async Task<IActionResult> Dashbord([FromBody] List<int> medicationIds)
        ////{
        ////    var userId = _userManager.GetUserId(User);
        ////    var pharmacy = await _dbContext.Pharmacies.FirstOrDefaultAsync(p => p.userId == userId);

        ////    if (pharmacy == null)
        ////    {
        ////        return NotFound(new { message = "Pharmacy not found." });
        ////    }

        ////    foreach (var medicationId in medicationIds)
        ////    {
        ////        var medication = await _dbContext.Medications.FindAsync(medicationId);
        ////        if (medication == null) continue; // لو الدواء غير موجود، نكمل الباقي

        ////        var existingEntry = await _dbContext.Set<Med_Phar>()
        ////            .FirstOrDefaultAsync(mp => mp.MedicationId == medicationId && mp.PharmacyId == pharmacy.Id);

        ////        if (existingEntry == null)
        ////        {
        ////            var medPhar = new Med_Phar
        ////            {
        ////                MedicationId = medicationId,
        ////                PharmacyId = pharmacy.Id,
        ////                Quantity = 1,
        ////                InStock = true
        ////            };

        ////            _dbContext.Set<Med_Phar>().Add(medPhar);
        ////        }
        ////    }

        ////    await _dbContext.SaveChangesAsync();
        ////    return Ok(new { message = "Medications added successfully!" });
        ////}

        //#endregion

        // Keep your existing GET action
        [HttpGet]
        public IActionResult Dashbord()
        {
            IEnumerable<Medication> x = _dbContext.Medications.ToList();
            return View(x);
        }

        // Update the POST action to accept an array of medication IDs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Dashbord(List<int> medicationIds)
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

    }
}
