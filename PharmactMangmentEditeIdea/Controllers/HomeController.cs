using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
using PharmactMangmentEditeIdea.Models;
using PharmactMangmentEditeIdea.ViewModel;

namespace PharmactMangmentEditeIdea.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
        public IActionResult SearchAboutMedicine(SrearchAboutMedicineDto dto)
        {
            dto.MedicineList.Add(new MedicineListDto
            {
                MedicineImg = "test",
                MedicineName = "Test",
                PharmacyBaseAddress = "test",
                PharmacyName = "test",
                PharmacyOpenningHours = "test",
                PharmacyPhoneNumber = "test",
                PharmacySecondaryAddress = "test",

            });
            dto.MedicineList.Add(new MedicineListDto
            {
                MedicineImg = "test",
                MedicineName = "Test",
                PharmacyBaseAddress = "test",
                PharmacyName = "test",
                PharmacyOpenningHours = "test",
                PharmacyPhoneNumber = "test",
                PharmacySecondaryAddress = "test",

            });
            dto.MedicineList.Add(new MedicineListDto
            {
                MedicineImg = "test",
                MedicineName = "Test",
                PharmacyBaseAddress = "test",
                PharmacyName = "test",
                PharmacyOpenningHours = "test",
                PharmacyPhoneNumber = "test",
                PharmacySecondaryAddress = "test",

            });
            dto.MedicineList.Add(new MedicineListDto
            {
                MedicineImg = "test",
                MedicineName = "Test",
                PharmacyBaseAddress = "test",
                PharmacyName = "test",
                PharmacyOpenningHours = "test",
                PharmacyPhoneNumber = "test",
                PharmacySecondaryAddress = "test",

            });
            dto.MedicineList.Add(new MedicineListDto
            {
                MedicineImg = "test",
                MedicineName = "Test",
                PharmacyBaseAddress = "test",
                PharmacyName = "test",
                PharmacyOpenningHours = "test",
                PharmacyPhoneNumber = "test",
                PharmacySecondaryAddress = "test",

            });
            dto.MedicineList.Add(new MedicineListDto
            {
                MedicineImg = "test",
                MedicineName = "Test",
                PharmacyBaseAddress = "test",
                PharmacyName = "test",
                PharmacyOpenningHours = "test",
                PharmacyPhoneNumber = "test",
                PharmacySecondaryAddress = "test",

            });
            dto.MedicineList.Add(new MedicineListDto
            {
                MedicineImg = "test",
                MedicineName = "Test",
                PharmacyBaseAddress = "test",
                PharmacyName = "test",
                PharmacyOpenningHours = "test",
                PharmacyPhoneNumber = "test",
                PharmacySecondaryAddress = "test",

            });
            dto.MedicineList.Add(new MedicineListDto
            {
                MedicineImg = "test",
                MedicineName = "Test",
                PharmacyBaseAddress = "test",
                PharmacyName = "test",
                PharmacyOpenningHours = "test",
                PharmacyPhoneNumber = "test",
                PharmacySecondaryAddress = "test",

            });
            dto.MedicineList.Add(new MedicineListDto
            {
                MedicineImg = "test",
                MedicineName = "Test",
                PharmacyBaseAddress = "test",
                PharmacyName = "test",
                PharmacyOpenningHours = "test",
                PharmacyPhoneNumber = "test",
                PharmacySecondaryAddress = "test",

            });
            dto.MedicineList.Add(new MedicineListDto
            {
                MedicineImg = "test",
                MedicineName = "Test",
                PharmacyBaseAddress = "test",
                PharmacyName = "test",
                PharmacyOpenningHours = "test",
                PharmacyPhoneNumber = "test",
                PharmacySecondaryAddress = "test",

            });
            dto.MedicineList.Add(new MedicineListDto
            {
                MedicineImg = "test",
                MedicineName = "Test",
                PharmacyBaseAddress = "test",
                PharmacyName = "test",
                PharmacyOpenningHours = "test",
                PharmacyPhoneNumber = "test",
                PharmacySecondaryAddress = "test",

            });
            dto.MedicineList.Add(new MedicineListDto
            {
                MedicineImg = "test",
                MedicineName = "Test",
                PharmacyBaseAddress = "test",
                PharmacyName = "test",
                PharmacyOpenningHours = "test",
                PharmacyPhoneNumber = "test",
                PharmacySecondaryAddress = "test",

            });
            return View(dto);
        }
    }
}
