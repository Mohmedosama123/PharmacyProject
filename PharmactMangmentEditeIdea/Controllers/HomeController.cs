using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using Microsoft.EntityFrameworkCore;
using PharmactMangmentDAL.Data.Contexts;
using PharmactMangmentEditeIdea.Models;
using PharmactMangmentEditeIdea.ViewModel;

namespace PharmactMangmentEditeIdea.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PharmaceDbContext _context;

        public HomeController(ILogger<HomeController> logger, PharmaceDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult SearchAboutMedicine()
        {
            return View(new SrearchAboutMedicineDto());
        }

        [HttpPost]
        public IActionResult SearchAboutMedicine(SrearchAboutMedicineDto model)
        {
            var query = _context.Med_Phars.Include(m => m.pharmacy).Include(m => m.medican);
            
            // add where conditions
            
            var results = query.Select(m => new MedicineListDto 
            {
                MedicineName = m.medican.Name,
                MedicineImg = m.medican.ImageName,
                PharmacyBaseAddress = m.pharmacy.Governorate,
                PharmacySecondaryAddress = m.pharmacy.FullAddress,
                PharmacyName = m.pharmacy.NameOfPharmacy,
                PharmacyOpenningHours = m.pharmacy.OperatingHours,
                PharmacyPhoneNumber = m.pharmacy.PhoneNumber,
            }).ToList();

            model.MedicineList = results;
            return View(model);
        }
    }
}
