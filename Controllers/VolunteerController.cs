using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Controllers;

[AllowAnonymous]
public class VolunteerController : Controller
{
    private readonly ApplicationDbContext _context;

    public VolunteerController(ApplicationDbContext context)
    {
        _context = context;
    }

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
