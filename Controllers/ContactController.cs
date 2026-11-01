using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Controllers;

[AllowAnonymous]
public class ContactController : Controller
{
    private readonly ApplicationDbContext _context;

    public ContactController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ContactRequest request)
    {
        if (!ModelState.IsValid)
        {
            return View(request);
        }

        _context.ContactRequests.Add(request);
        await _context.SaveChangesAsync();

        TempData["ContactMessage"] =
            "Your contact request was submitted successfully.";

        return RedirectToAction(nameof(Create));
    }
}