using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    [Authorize(Roles = "Employee")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Volunteers()
        {
            var volunteers = await _context.Volunteers.ToListAsync();

            return View(volunteers);
        }

        public async Task<IActionResult> Projects()
        {
            var projects = await _context.ReliefProjects
                .OrderByDescending(p => p.UpdateDate)
                .ToListAsync();

            return View(projects);
        }

        public async Task<IActionResult> Donations()
        {
            var donations = await _context.Donations
                .OrderByDescending(d => d.DonationDate)
                .ToListAsync();

            return View(donations);
        }

        [HttpGet]
        public IActionResult CreateProject()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(
            ReliefProject project)
        {
            if (!ModelState.IsValid)
            {
                return View(project);
            }

            project.UpdateDate = DateTime.Now;

            _context.ReliefProjects.Add(project);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Projects));
        }
    }
}