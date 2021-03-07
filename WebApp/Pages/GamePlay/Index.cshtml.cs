using System.Threading.Tasks;
using DAL;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApp.Pages.GamePlay
{
    public class Index : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;


        public Index(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public int Width { get; set; }
        public int Height { get; set; }

        public bool Hidden { get; set; }

        public BattleShip BattleShip { get; set; } = null!;

        public char[] Alphabet { get; set; } =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z'
        };

        public IActionResult OnGetExitToMainPage()
        {
            return RedirectToPage("../Index");
        }

        public async Task OnGetAsync(int gameId, int? x, int? y, int? boardId, bool? hidden, bool? undo)
        {
            if (hidden == null) hidden = true;

            Hidden = (bool) hidden;
            BattleShip = new BattleShip(_context);
            await BattleShip.LoadFromDatabase(gameId);
            if (undo != null && undo == true) BattleShip.UndoTurn();
            Width = BattleShip.GetWidth;
            Height = BattleShip.GetHeight;
            if (x != null && y != null && boardId != null)
            {
                Player player = BattleShip.GetPlayerByBoardId((int) boardId);
                (int x, int y) location = ((int) x, (int) y);
                await BattleShip.MakeAMove(player, location);
            }
        }
    }
}