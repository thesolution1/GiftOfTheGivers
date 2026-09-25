        using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Pages.Volunteers
{
    [Authorize(Roles = "Employee")]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context) => _context = context;

        public IList<Volunteer> Volunteers { get; set; } = new List<Volunteer>();

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Volunteers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchString))
            {
                query = query.Where(v =>
                    v.Name.Contains(SearchString) ||
                    v.Skills.Contains(SearchString));
            }

            Volunteers = await query.OrderBy(v => v.Name).ToListAsync();
        }
    }
}