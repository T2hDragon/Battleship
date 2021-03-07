using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages_Games
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Game? Game { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            Game = await _context.Games
                .Include(g => g.GameOption).Include(a => a.TurnSaves).Include(a => a.GameBoards)
                .FirstOrDefaultAsync(m => m.GameId == id);

            if (Game == null) return NotFound();
            return Page();
        }
    }
}