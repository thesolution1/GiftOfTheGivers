using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public VolunteerController(ApplicationDbContext context) { _context = context; }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Projects = new SelectList(await _context.ReliefProjects.ToListAsync(), "ReliefProjectID", "ProjectName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Submit(Volunteer volunteer)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null) return RedirectToAction("Login", "Account");

            volunteer.UserID = userId.Value;

            if (!ModelState.IsValid)
            {
                ViewBag.Projects = new SelectList(await _context.ReliefProjects.ToListAsync(), "ReliefProjectID", "ProjectName");
                return View("Index", volunteer);
            }

            _context.Volunteers.Add(volunteer);
            await _context.SaveChangesAsync();

            TempData["VolunteerMessage"] = "Thank you for registering as a volunteer.";
            return RedirectToAction("Success");
        }

        public IActionResult Success() => View();
    }
}