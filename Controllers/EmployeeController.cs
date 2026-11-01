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

        // Employee Dashboard
        public async Task<IActionResult> Dashboard()
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

    return View();
}
        // View Volunteers
        public async Task<IActionResult> Volunteers()
        {
            var volunteers = await _context.Volunteers
                .ToListAsync();

            return View(volunteers);
        }

        // View and Search Relief Projects
        [HttpGet]
        public async Task<IActionResult> Projects(string searchTerm)
        {
            var projects = _context.ReliefProjects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                projects = projects.Where(p =>
                    p.ProjectName.Contains(searchTerm));
            }

            var projectList = await projects
                .OrderByDescending(p => p.UpdateDate)
                .ToListAsync();

            ViewBag.SearchTerm = searchTerm;

            return View(projectList);
        }

        // View Donations
        public async Task<IActionResult> Donations()
        {
            var donations = await _context.Donations
                .OrderByDescending(d => d.DonationDate)
                .ToListAsync();

            return View(donations);
        }

        // Display Create Project Page
        [HttpGet]
        public IActionResult CreateProject()
        {
            return View();
        }

        // Create a New Relief Project
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

        // View Volunteer Requests
        [HttpGet]
        public async Task<IActionResult> VolunteerRequests()
        {
            var requests = await _context.VolunteerRequests
                .Include(request => request.ReliefProject)
                .OrderBy(request => request.Status)
                .ThenByDescending(request => request.SubmittedAt)
                .ToListAsync();

            return View(requests);
        }

        // Approve Volunteer Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveVolunteer(int id)
        {
            var request = await _context.VolunteerRequests
                .FindAsync(id);

            if (request is null)
            {
                return NotFound();
            }

            request.Status = VolunteerRequestStatus.Approved;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(VolunteerRequests));
        }

        // Reject Volunteer Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectVolunteer(int id)
        {
            var request = await _context.VolunteerRequests
                .FindAsync(id);

            if (request is null)
            {
                return NotFound();
            }

            request.Status = VolunteerRequestStatus.Rejected;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(VolunteerRequests));
        }
    }
}
