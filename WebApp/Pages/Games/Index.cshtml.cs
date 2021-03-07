using System.Collections.Generic;
using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages_Games
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Pages.IndexModel> _logger;


        public IndexModel(ILogger<Pages.IndexModel> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IList<Game> Games { get; set; } = default!;


        public async Task OnGetAsync()
        {
            Games = await _context.Games.Include(game => game.GameBoards).ToListAsync();
        }


        public IActionResult OnGetLoadGame(int gameId)
        {
            return RedirectToPage("../GamePlay/Index", new {gameId});
        }
    }
}