using System.Diagnostics;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Online() => View();

        public async Task<IActionResult> Index()
        {
            // Check Employee via Session, not Identity
            if (HttpContext.Session.GetString("Role") == "Employee")
            {
                ViewBag.TotalVolunteers = await _context.Volunteers.CountAsync();
                ViewBag.TotalReliefProjects = await _context.ReliefProjects.CountAsync();
                ViewBag.TotalDonations = await _context.Donations.CountAsync();
                ViewBag.TotalZarDonations = await _context.Donations
                    .Where(d => d.Currency == "ZAR")
                    .SumAsync(d => d.Amount);
            }
            return View();
        }

        public IActionResult About() => View();

        [HttpGet]
        public IActionResult Donate()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Donate(Donation donation)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(donation);

            donation.UserID = userId.Value;
            donation.DonationDate = DateTime.Now;
            donation.ReferenceNumber = "DON-" + Guid.NewGuid().ToString("N")[..8].ToUpper();

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            return RedirectToAction("TaxCertificate", new { id = donation.DonationID });
        }

        [HttpGet] public IActionResult Volunteer() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Volunteer(Volunteer volunteer)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            volunteer.UserID = userId.Value; // Link to logged in user
            if (!ModelState.IsValid) return View(volunteer);

            _context.Volunteers.Add(volunteer);
            await _context.SaveChangesAsync();

            TempData["VolunteerSuccess"] = "Thank you for registering as a volunteer.";
            return RedirectToAction(nameof(Volunteer));
        }

        [HttpGet]
        public async Task<IActionResult> TaxCertificate(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            var donation = await _context.Donations.FirstOrDefaultAsync(d => d.DonationID == id && d.UserID == userId.Value);
            if (donation == null) return NotFound();

            ViewBag.DonorEmail = HttpContext.Session.GetString("Email");
            ViewBag.DonorName = HttpContext.Session.GetString("FullName");
            return View(donation);
        }

        public IActionResult Contact() => View();
        public IActionResult Privacy() => View();
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}