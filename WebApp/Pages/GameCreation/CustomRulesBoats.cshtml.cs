using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DAL;
using Domain.Enums;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.GameCreation
{
    public class CustomRulesBoats : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly ILogger<IndexModel> _logger;


        public CustomRulesBoats(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty] public EBoatsCanTouch EBoatsCanTouch { get; set; }

        [BindProperty] public ENextMoveAfterHit ENextMoveAfterHit { get; set; }

        [BindProperty] public bool PlaceBoatsAutomatically { get; set; }

        [BindProperty] public string GameName { get; set; } = default!;

        [BindProperty] public int Height { get; set; }
        [BindProperty] public int Width { get; set; }
        [BindProperty] public int PatrolAmount { get; set; }
        [BindProperty] public int CruiserAmount { get; set; }
        [BindProperty] public int SubmarineAmount { get; set; }
        [BindProperty] public int BattleshipAmount { get; set; }
        [BindProperty] public int CarrierAmount { get; set; }
        [BindProperty] public string[] PlayerNames { get; set; } = default!;
        public Dictionary<string, int> MaxBoatAmount { get; set; } = default!;

        private int calculateShipAmount(int boatSize, int space)
        {
            var boatSpace = EBoatsCanTouch switch
            {
                EBoatsCanTouch.Corner => 4 * boatSize + 6,
                EBoatsCanTouch.No => 4 * boatSize + 6,
                EBoatsCanTouch.Yes => boatSize * 1.5,
                _ => throw new InvalidEnumArgumentException("Unknown enum")
            };
            return (int) (space / boatSpace);
        }

        private Dictionary<string, int> GetBoatMaxAmounts(int width, int height)
        {
            Dictionary<string, int> boatMaxAmounts = new();
            var boardSize = width * height;
            var shipAmount = calculateShipAmount(5, boardSize);
            var usedSpace = shipAmount * (3 * 5 + 6);
            boatMaxAmounts.Add("Carrier", shipAmount);
            shipAmount = calculateShipAmount(4, boardSize - usedSpace);
            usedSpace += shipAmount * (3 * 4 + 6);
            boatMaxAmounts.Add("Battleship", shipAmount);
            shipAmount = calculateShipAmount(3, boardSize - usedSpace);
            usedSpace += shipAmount * (3 * 3 + 6);
            boatMaxAmounts.Add("Submarine", shipAmount);
            shipAmount = calculateShipAmount(2, boardSize - usedSpace);
            usedSpace += shipAmount * (3 * 2 + 6);
            boatMaxAmounts.Add("Cruiser", shipAmount);
            shipAmount = calculateShipAmount(1, boardSize - usedSpace);
            usedSpace += shipAmount * (3 * 1 + 6);
            boatMaxAmounts.Add("Patrol", shipAmount);
            if (usedSpace < 0) throw new AmbiguousMatchException($"Not enough space left: {usedSpace}");
            return boatMaxAmounts;
        }

        public void OnGetAsync(string gameName, int width, int height, string[] playerNames,
            EBoatsCanTouch eBoatsCanTouch, ENextMoveAfterHit eNextMoveAfterHit, int playerCount)
        {
            GameName = gameName;
            Width = width;
            Height = height;
            PlayerNames = playerNames;
            EBoatsCanTouch = eBoatsCanTouch;
            ENextMoveAfterHit = eNextMoveAfterHit;
            PlayerNames = new string[playerCount];
            MaxBoatAmount = GetBoatMaxAmounts(width, height);
        }


        public async Task<IActionResult> OnPostAsync()
        {
            BattleShip battleShip = new(_context);
            List<DefaultBoat> defaultBoats = new()
            {
                new DefaultBoat("Patrol", 1, PatrolAmount),
                new DefaultBoat("Cruiser", 2, CruiserAmount),
                new DefaultBoat("Submarine", 3, SubmarineAmount),
                new DefaultBoat("Battleship", 4, BattleshipAmount),
                new DefaultBoat("Carrier", 5, CarrierAmount)
            };

            battleShip.CreateGameSettings(GameName, Width, Height, ENextMoveAfterHit, EBoatsCanTouch,
                PlayerNames.ToList(), defaultBoats);
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