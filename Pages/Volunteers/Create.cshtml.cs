using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiftOfTheGivers.Pages.Volunteers
{
    [Authorize(Roles = "Employee")]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public Volunteer Volunteer { get; set; } = new Volunteer();

        public IActionResult OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Volunteers.Add(Volunteer);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Volunteer created.";
            return RedirectToPage("./Index");
        }
    }
}