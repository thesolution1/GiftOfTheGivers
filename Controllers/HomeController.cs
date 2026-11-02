using System.Diagnostics;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Online()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            if (User.IsInRole("Employee"))
            {
                ViewBag.TotalVolunteers =
                    await _context.Volunteers.CountAsync();

                ViewBag.TotalReliefProjects =
                    await _context.ReliefProjects.CountAsync();

                ViewBag.TotalDonations =
                    await _context.Donations.CountAsync();

                ViewBag.TotalZarDonations =
                    await _context.Donations
                        .Where(d => d.Currency == "ZAR")
                        .SumAsync(d => d.Amount);
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Donate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Donate(Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return View(donation);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            donation.UserID = user.Id;
            donation.DonationDate = DateTime.Now;
            donation.ReferenceNumber =
                "DON-" + Guid.NewGuid().ToString("N")[..8].ToUpper();

            _context.Donations.Add(donation);

            await _context.SaveChangesAsync();

            donation.TaxCertificateNumber =
                $"TAX-{donation.DonationID:D6}";

            donation.TaxCertificateIssuedAt = DateTime.Now;
            donation.TaxCertificateDonorName = user.UserName;
            donation.TaxCertificateDonorEmail = user.Email;

            await _context.SaveChangesAsync();

            return RedirectToAction(
                nameof(TaxCertificate),
                new { id = donation.DonationID });
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TaxCertificate(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var donation = await _context.Donations
                .FirstOrDefaultAsync(d =>
                    d.DonationID == id &&
                    d.UserID == user.Id);

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