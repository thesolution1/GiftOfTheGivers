using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Online()
        {
            return View();
        }

        public IActionResult Index()
        {
            // Check if user is logged in via Session
            if (HttpContext.Session.GetInt32("UserID") == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        // Submit donation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Donation donation)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (!ModelState.IsValid)
            {
                return View("Index", donation);
            }

            donation.UserID = userId.Value; // int, not string
            donation.DonationDate = DateTime.Now;
            donation.ReferenceNumber = "DON-" + Guid.NewGuid().ToString("N")[..8].ToUpper();

            _context.Donations.Add(donation);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Certificate), new { id = donation.DonationID });
        }

        [HttpGet]
        public async Task<IActionResult> Certificate(int id)
        {
            var donation = await _context.Donations
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.DonationID == id);

            if (donation == null) return NotFound();

            return View(donation);
        }

        [HttpGet]
        public async Task<IActionResult> MyDonations()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var donations = await _context.Donations
                .Where(d => d.UserID == userId.Value) // Now int == int
                .OrderByDescending(d => d.DonationDate)
                .ToListAsync();

            return View(donations);
        }
    }
}