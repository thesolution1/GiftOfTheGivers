using System.Security.Claims;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers;

[Authorize]
public class DonorController(
    ApplicationDbContext context,
    UserManager<IdentityUser> userManager)
    : Controller
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<IdentityUser> _userManager = userManager;

    [HttpGet]
    public async Task<IActionResult> Projects()
    {
        if (User.IsInRole("Employee"))
        {
            return Forbid();
        }

        var projects = await _context.ReliefProjects
            .OrderByDescending(project => project.UpdateDate)
            .ToListAsync();

        return View(projects);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (User.IsInRole("Employee"))
        {
            return Forbid();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Challenge();
        }

        var approved = await _context.VolunteerRequests.AnyAsync(request =>
            request.UserId == userId &&
            request.ReliefProjectId == id &&
            request.Status == VolunteerRequestStatus.Approved);

        if (!approved)
        {
            return Forbid();
        }

        var project = await _context.ReliefProjects
            .FirstOrDefaultAsync(item => item.ReliefProjectID == id);

        return project is null ? NotFound() : View(project);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Volunteer(
        int projectId,
        string skills,
        string availability)
    {
        if (User.IsInRole("Employee"))
        {
            return Forbid();
        }

        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Challenge();
        }

        var projectExists = await _context.ReliefProjects
            .AnyAsync(project => project.ReliefProjectID == projectId);

        if (!projectExists)
        {
            return NotFound();
        }

        var duplicate = await _context.VolunteerRequests.AnyAsync(request =>
            request.UserId == user.Id &&
            request.ReliefProjectId == projectId);

        if (duplicate)
        {
            return Conflict("A request already exists for this project.");
        }

        var request = new VolunteerRequest
        {
            UserId = user.Id,

            // The browser cannot override this value.
            Name = user.UserName ?? user.Email ?? "Registered donor",

            ReliefProjectId = projectId,
            Skills = skills,
            Availability = availability,
            Status = VolunteerRequestStatus.Pending,
            SubmittedAt = DateTime.UtcNow
        };

        _context.VolunteerRequests.Add(request);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Projects));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upcoming(
        string skills,
        string availability)
    {
        if (User.IsInRole("Employee"))
        {
            return Forbid();
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId is null)
        {
            return Challenge();
        }

        _context.UpcomingProjectRequests.Add(
            new UpcomingProjectRequest
            {
                UserId = userId,
                Skills = skills,
                Availability = availability,
                SubmittedAt = DateTime.UtcNow
            });

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Projects));
    }
}