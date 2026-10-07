using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GiftOfTheGivers.Pages.Volunteers
{
    [Authorize(Roles = "Employee")]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context) => _context = context;

        public Volunteer Volunteer { get; set; } = new Volunteer();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var v = await _context.Volunteers.FindAsync(id);
            if (v == null) return NotFound();

            Volunteer = v;
            return Page();
        }
    }
}