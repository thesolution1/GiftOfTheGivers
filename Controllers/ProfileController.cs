using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    public class ProfileController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProfileController(ApplicationDbContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId.Value);
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId.Value);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User model)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserID == userId.Value);
            if (user == null) return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid) return View(model);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            await _context.SaveChangesAsync();

            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("FullName", user.FirstName + " " + user.LastName);

            TempData["SuccessMessage"] = "Your profile updated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}