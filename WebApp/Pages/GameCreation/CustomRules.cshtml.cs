using System.Threading.Tasks;
using DAL;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.GameCreation
{
    public class CustomRules : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<IndexModel> _logger;


        public CustomRules(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty] public EBoatsCanTouch EBoatsCanTouch { get; set; }

        [BindProperty] public ENextMoveAfterHit ENextMoveAfterHit { get; set; }


        [BindProperty] public string GameName { get; set; } = default!;

        [BindProperty] public int Height { get; set; }
        [BindProperty] public int Width { get; set; }
        public int PlayerCount { get; set; } = 2;


        public void OnGetAsync(int playerCount)
        {
            PlayerCount = playerCount;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("./CustomRulesBoats",
                new
                {
                    gameName = GameName, eBoatsCanTouch = EBoatsCanTouch, eNextMoveAfterHit = ENextMoveAfterHit,
                    height = Height, width = Width, playerCount = PlayerCount
                });
        }
    }
}