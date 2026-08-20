using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DonationController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Submit(Donation donation)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", donation);
            }

            donation.DonationDate = DateTime.Now;

            _context.Donations.Add(donation);
            _context.SaveChanges();

            TempData["DonationAmount"] = donation.Amount.ToString("F2");
            TempData["DonationCurrency"] = donation.Currency;
            TempData["DonationType"] = donation.DonationType;

            return RedirectToAction("Certificate");
        }

        public IActionResult Certificate()
        {
            return View();
        }
    }
}