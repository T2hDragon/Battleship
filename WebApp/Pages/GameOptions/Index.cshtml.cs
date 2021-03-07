using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages_GameOptions
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<GameOption> GameOption { get; set; } = null!;

        public async Task OnGetAsync()
        {
            GameOption = await _context.GameOptions.Include(option => option.Games)
                .Where(option => option.Games == null || option.Games.Count == 0).ToListAsync();
        }
    }
}