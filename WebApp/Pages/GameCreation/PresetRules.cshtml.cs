using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.GameCreation
{
    public class PresetRules : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<IndexModel> _logger;


        public PresetRules(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
            GameOptions = context.GameOptions.Where(option => option.Games == null || option.Games.Count == 0)
                .ToListAsync().Result;
        }

        public IList<GameOption> GameOptions { get; set; }

        [BindProperty] public bool PlaceBoatsAutomatically { get; set; }

        [BindProperty] public int GameOptionId { get; set; }

        public int PlayerCount { get; set; } = 2;

        [BindProperty] public string[] PlayerNames { get; set; } = default!;

        [BindProperty] public string GameName { get; set; } = default!;


        public void OnGetAsync(int playerCount)
        {
            PlayerCount = playerCount;
            PlayerNames = new string[playerCount];
        }

        public async Task<IActionResult> OnPostAsync()
        {
            BattleShip battleShip = new(_context);
            battleShip.CreateGameSettings(GameName, PlayerNames.ToList(),
                await _context.GameOptions.Include(e => e.GameOptionBoats).ThenInclude(e => e.DefaultBoat)
                    .FirstAsync(option => option.GameOptionId == GameOptionId));
            if (PlaceBoatsAutomatically)
            {
                foreach (Player player in battleShip.GetPlayers()) await battleShip.PlacePlayerRemainingBoats(player);
                battleShip.GameStarted();
                return RedirectToPage("../GamePlay/Index",
                    new {gameId = battleShip.GameId});
            }

            return RedirectToPage("./PlaceBoats",
                new {boardId = battleShip.GetPlayers()[0].PlayerBoard.GameBoardId, gameId = battleShip.GameId});
        }
    }
}