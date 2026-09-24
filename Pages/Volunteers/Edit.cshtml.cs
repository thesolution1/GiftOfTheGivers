using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Pages.Volunteers
{
    [Authorize(Roles = "Employee")]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context) => _context = context;

        [BindProperty]
        public Volunteer Volunteer { get; set; } = new Volunteer();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Volunteer = await _context.Volunteers.FindAsync(id);

            if (Volunteer == null) return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Attach(Volunteer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Volunteers.AnyAsync(v => v.VolunteerID == Volunteer.VolunteerID))
                {
                    return NotFound();
                }
                throw;
            }

            TempData["SuccessMessage"] = "Volunteer updated.";
            return RedirectToPage("./Index");
        }
    }
}