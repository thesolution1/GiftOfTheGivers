using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiftOfTheGivers.Pages.Volunteers
{
    [Authorize(Roles = "Employee")]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public Volunteer Volunteer { get; set; } = new Volunteer();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var v = await _context.Volunteers.FindAsync(id);
            if (v == null) return NotFound();

            Volunteer = v;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var v = await _context.Volunteers.FindAsync(id);
            if (v == null) return NotFound();

            _context.Volunteers.Remove(v);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Volunteer deleted.";
            return RedirectToPage("./Index");
        }
    }
}