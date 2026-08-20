using System.Diagnostics;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Donate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Donate(Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return View(donation);
            }

            donation.DonationDate = DateTime.Now;

            _context.Donations.Add(donation);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Thank you! Your donation has been recorded successfully.";

            TempData["DonationID"] = donation.DonationID;

            return RedirectToAction(nameof(Donate));
        }
        [HttpGet]
        public IActionResult Volunteer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Volunteer(Volunteer volunteer)
        {
            if (!ModelState.IsValid)
            {
                return View(volunteer);
            }

            _context.Volunteers.Add(volunteer);

            await _context.SaveChangesAsync();

            TempData["VolunteerSuccess"] =
                "Thank you for registering your interest as a volunteer.";

            return RedirectToAction(nameof(Volunteer));
        }

        public async Task<IActionResult> TaxCertificate(int id)
        {
            var donation = await _context.Donations.FindAsync(id);

            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id
                               ?? HttpContext.TraceIdentifier
                });

        }
    }
}