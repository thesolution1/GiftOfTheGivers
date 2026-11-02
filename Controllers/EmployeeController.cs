using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeeController(ApplicationDbContext context) { _context = context; }

        private bool IsEmployee()
        {
            return HttpContext.Session.GetString("Role") == "Employee";
        }

        public async Task<IActionResult> Dashboard()
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Account");

            ViewBag.TotalVolunteers = await _context.Volunteers.CountAsync();
            ViewBag.TotalReliefProjects = await _context.ReliefProjects.CountAsync();
            ViewBag.TotalDonations = await _context.Donations.CountAsync();
            ViewBag.TotalZarDonations = await _context.Donations.Where(d => d.Currency == "ZAR").SumAsync(d => d.Amount);
            return View();
        }

        public async Task<IActionResult> Volunteers()
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Account");
            var volunteers = await _context.Volunteers.Include(v => v.User).ToListAsync();
            return View(volunteers);
        }

        public async Task<IActionResult> Projects(string searchTerm)
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Account");
            var projects = _context.ReliefProjects.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchTerm))
                projects = projects.Where(p => p.ProjectName.Contains(searchTerm));
            
            var list = await projects.OrderByDescending(p => p.StartDate).ToListAsync();
            ViewBag.SearchTerm = searchTerm;
            return View(list);
        }

        public async Task<IActionResult> Donations()
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Account");
            var donations = await _context.Donations.Include(d => d.User).OrderByDescending(d => d.DonationDate).ToListAsync();
            return View(donations);
        }

        [HttpGet] public IActionResult CreateProject()
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost] [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(ReliefProject project)
        {
            if (!IsEmployee()) return RedirectToAction("Login", "Account");
            if (!ModelState.IsValid) return View(project);

            project.CreatedByUserID = HttpContext.Session.GetInt32("UserID") ?? 1;
            project.StartDate = DateTime.Now;
            _context.ReliefProjects.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Projects));
        }
    }
}