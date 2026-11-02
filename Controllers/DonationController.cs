using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    [Authorize]
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DonationController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Online()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(Donation donation)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            if (!ModelState.IsValid)
            {
                return View("Index", donation);
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
                nameof(Certificate),
                new { id = donation.DonationID });
        }

        [HttpGet]
        public async Task<IActionResult> Certificate(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var donation = await _context.Donations
                .AsNoTracking()
                .FirstOrDefaultAsync(d =>
                    d.DonationID == id &&
                    d.UserID == user.Id);

            if (donation == null)
            {
                return NotFound();
            }

            return View(donation);
        }

        [HttpGet]
        public async Task<IActionResult> MyDonations()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Challenge();
            }

            var donations = await _context.Donations
                .AsNoTracking()
                .Where(d => d.UserID == user.Id)
                .OrderByDescending(d => d.DonationDate)
                .ToListAsync();

            return View(donations);
        }
    }
}