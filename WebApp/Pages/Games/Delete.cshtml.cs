using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages_Games
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty] public Game? Game { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();


            Game = await _context.Games
                .Include(g => g.GameOption).Include(a => a.TurnSaves).Include(a => a.GameBoards)
                .FirstAsync(m => m.GameId == id);


            if (Game == null) return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null) return NotFound();

            Game = await _context.Games
                .Include(g => g.GameOption).Include(a => a.TurnSaves).Include(a => a.GameBoards)
                .FirstAsync(m => m.GameId == id);

            if (Game != null)
            {
                GameOption gameOption = Game.GameOption!;
                _context.Games.Remove(Game);
                _context.GameOptions.Remove(gameOption);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}