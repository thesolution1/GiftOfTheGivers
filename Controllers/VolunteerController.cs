using System.Collections.Generic;
using GiftOfTheGivers.Models;
using GiftOfTheGivers.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers;

[AllowAnonymous]
public class VolunteerController(
    ApplicationDbContext context,
    UserManager<IdentityUser> userManager) : Controller
{
    private readonly ApplicationDbContext _context = context;
    private readonly UserManager<IdentityUser> _userManager = userManager;

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (User.Identity?.IsAuthenticated != true)
        {
            return View(new VolunteerPageViewModel
            {
                IsAuthenticated = false
            });
        }

        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Challenge();
        }

        var approvedProjectIds = new HashSet<int>(
            await _context.VolunteerRequests
                .Where(r =>
                    r.UserId == user.Id &&
                    r.Status == VolunteerRequestStatus.Approved)
                .Select(r => r.ReliefProjectId)
                .ToListAsync());

        var pendingProjectIds = new HashSet<int>(
            await _context.VolunteerRequests
                .Where(r =>
                    r.UserId == user.Id &&
                    r.Status == VolunteerRequestStatus.Pending)
                .Select(r => r.ReliefProjectId)
                .ToListAsync());

        var projects = await _context.ReliefProjects
            .AsNoTracking()
            .OrderByDescending(p => p.UpdateDate)
            .Select(p => new ProjectCardViewModel
            {
                Id = p.ReliefProjectID,
                ProjectName = p.ProjectName,
                Summary = p.Update,
                IsApproved = approvedProjectIds.Contains(p.ReliefProjectID),
                HasPendingRequest = pendingProjectIds.Contains(p.ReliefProjectID)
            })
            .ToListAsync();

        return View(new VolunteerPageViewModel
        {
            IsAuthenticated = true,
            DonorName = user.UserName ?? user.Email,
            Projects = projects
        });
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(
        int projectId,
        VolunteerSubmissionViewModel input)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", await BuildPageModelAsync());
        }

        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Challenge();
        }

        var projectExists = await _context.ReliefProjects
            .AnyAsync(p => p.ReliefProjectID == projectId);

        if (!projectExists)
        {
            return NotFound();
        }

        var accountName = user.UserName ?? user.Email;

        if (string.IsNullOrWhiteSpace(accountName))
        {
            return BadRequest("Your account does not contain a valid name.");
        }

        var request = await _context.VolunteerRequests
            .SingleOrDefaultAsync(r =>
                r.UserId == user.Id &&
                r.ReliefProjectId == projectId);

        if (request is null)
        {
            request = new VolunteerRequest
            {
                UserId = user.Id,
                ReliefProjectId = projectId
            };

            _context.VolunteerRequests.Add(request);
        }

        request.Name = accountName;
        request.Skills = input.Skills;
        request.Availability = input.Availability;
        request.Status = VolunteerRequestStatus.Pending;
        request.SubmittedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        TempData["VolunteerMessage"] =
            "Your volunteer request is pending employee approval.";

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RequestUpcomingProject(
        UpcomingProjectSubmissionViewModel input)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", await BuildPageModelAsync());
        }

        var userId = _userManager.GetUserId(User);

        if (userId is null)
        {
            return Challenge();
        }

        var request = new UpcomingProjectRequest
        {
            UserId = userId,
            Skills = input.Skills,
            Availability = input.Availability,
            SubmittedAt = DateTime.UtcNow,
            IsProcessed = false
        };

        _context.UpcomingProjectRequests.Add(request);
        await _context.SaveChangesAsync();

        TempData["VolunteerMessage"] =
            "You have been added to the upcoming projects list.";

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var userId = _userManager.GetUserId(User);

        if (userId is null)
        {
            return Challenge();
        }

        var approved = await _context.VolunteerRequests
            .AnyAsync(r =>
                r.UserId == userId &&
                r.ReliefProjectId == id &&
                r.Status == VolunteerRequestStatus.Approved);

        if (!approved)
        {
            return Forbid();
        }

        var project = await _context.ReliefProjects
            .AsNoTracking()
            .SingleOrDefaultAsync(p => p.ReliefProjectID == id);

        return project is null ? NotFound() : View(project);
    }

    private async Task<VolunteerPageViewModel> BuildPageModelAsync()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return new VolunteerPageViewModel
            {
                IsAuthenticated = false
            };
        }

        var approvedIds = new HashSet<int>(
            await _context.VolunteerRequests
                .Where(r =>
                    r.UserId == user.Id &&
                    r.Status == VolunteerRequestStatus.Approved)
                .Select(r => r.ReliefProjectId)
                .ToListAsync());

        var pendingIds = new HashSet<int>(
            await _context.VolunteerRequests
                .Where(r =>
                    r.UserId == user.Id &&
                    r.Status == VolunteerRequestStatus.Pending)
                .Select(r => r.ReliefProjectId)
                .ToListAsync());

        var projects = await _context.ReliefProjects
            .AsNoTracking()
            .Select(p => new ProjectCardViewModel
            {
                Id = p.ReliefProjectID,
                ProjectName = p.ProjectName,
                Summary = p.Update,
                IsApproved = approvedIds.Contains(p.ReliefProjectID),
                HasPendingRequest = pendingIds.Contains(p.ReliefProjectID)
            })
            .ToListAsync();

        return new VolunteerPageViewModel
        {
            IsAuthenticated = true,
            DonorName = user.UserName ?? user.Email,
            Projects = projects
        };
    }
}
