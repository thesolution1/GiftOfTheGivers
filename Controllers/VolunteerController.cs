using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Controllers;

[AllowAnonymous]
public class VolunteerController(ApplicationDbContext context) : Controller
{
    private readonly ApplicationDbContext _context = context;

    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction("Create", "Contact");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Submit()
    {
        return RedirectToAction("Create", "Contact");
    }
}
