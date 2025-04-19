using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PharmactMangmentDAL.Data.Contexts;
using PharmactMangmentDAL.Models;
using PharmactMangmentEditeIdea.Models;
using PharmactMangmentEditeIdea.ViewModel;
using System.Diagnostics;

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
            var query = _context.Med_Phars.Where(x => 1 == 1);
            query = query.Include(m => m.pharmacy).Include(m => m.medican);

            ApplyFilters(model, ref query);

            var results = query.Select(m => new MedicineListDto
            {
                MedicineName = m.medican.Name,
                MedicineImg = m.medican.ImageName,
                MedicinePrice = m.medican.Price,
                PharmacyBaseAddress = m.pharmacy.Governorate,
                PharmacySecondaryAddress = m.pharmacy.FullAddress,
                PharmacyName = m.pharmacy.NameOfPharmacy,
                PharmacyOpenningHours = m.pharmacy.OperatingHours,
                PharmacyPhoneNumber = m.pharmacy.PhoneNumber,
            }).ToList();

            model.MedicineList = results;
            return View(model);
        }

        private void ApplyFilters(SrearchAboutMedicineDto model, ref IQueryable<Med_Phar> query)
        {
            if (query == null || model == null)
                return;

            if (!string.IsNullOrWhiteSpace(model.MedcineName))
            {
                query = query.Where(x => x.medican.Name.Contains(model.MedcineName.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(model.Govornorate))
            {
                query = query.Where(x => x.pharmacy.Governorate.Contains(model.Govornorate.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(model.District))
            {
                query = query.Where(x => x.pharmacy.District.Contains(model.District.Trim()));
            }

            if (!string.IsNullOrWhiteSpace(model.Area))
            {
                query = query.Where(x => x.pharmacy.Area.Contains(model.Area.Trim()));
            }

            switch (model.OrderBy)
            {
                case Enums.OrderBy.PhamrmacyName:
                    if (model.IsAsc)
                        query.OrderBy(x => x.pharmacy.NameOfPharmacy);
                    else
                        query.OrderByDescending(x => x.pharmacy.NameOfPharmacy);
                    break;

                case Enums.OrderBy.MedicationName:
                    if (model.IsAsc)
                        query.OrderBy(x => x.medican.Name);
                    else
                        query.OrderByDescending(x => x.medican.Name);
                    break;

                case Enums.OrderBy.Price:
                    if (model.IsAsc)
                        query.OrderBy(x => x.medican.Price);
                    else
                        query.OrderByDescending(x => x.medican.Price);
                    break;

                case Enums.OrderBy.Distance:
                    break;

                default:
                    break;
            }
        }
    }
}
