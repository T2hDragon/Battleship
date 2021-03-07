using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DAL;
using Domain;
using GameBrain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GameBoard = Domain.GameBoard;

namespace WebApp.Pages.GameCreation
{
    public class PlaceBoats : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;


        public PlaceBoats(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public int Width { get; set; }
        public int Height { get; set; }


        public bool Vertical { get; set; }

        public Game Game { get; set; } = default!;

        public BattleShip BattleShip { get; set; } = default!;

        public Player Player { get; set; } = default!;
        public int PosX { get; set; }
        public int PosY { get; set; }

        public char[] Alphabet { get; set; } =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U',
            'V', 'W', 'X', 'Y', 'Z'
        };


        public async Task OnGetAsync(int gameId, int boardId, int? posX, int? posY, string? dir, bool? vertical)
        {
            Game = await _context.Games.FirstAsync(g => g.GameId == gameId);
            BattleShip = new BattleShip(_context);
            await BattleShip.LoadFromDatabaseOfBoard(gameId, boardId);

            Player = BattleShip.GetPlayers()[0];
            Width = BattleShip.GetWidth;
            Height = BattleShip.GetHeight;

            if (posX != null && posY != null && dir != null && vertical != null)
            {
                Vertical = (bool) vertical;
                (int x, int y) direction = Vertical ? (0, 1) : (1, 0);
                PosX = (int) posX;
                PosY = (int) posY;
                (int x, int y)? moveDirection = default;

                Player.SetNextPlacementBoat();
                Player.GetBoatBeingPlaced().PlaceBoat((PosX, PosY), direction);
                switch (dir)
                {
                    case "up-left":
                        moveDirection = (-1, -1);
                        break;
                    case "up":
                        moveDirection = (0, -1);
                        break;
                    case "up-right":
                        moveDirection = (1, -1);
                        break;
                    case "left":
                        moveDirection = (-1, 0);
                        break;
                    case "right":
                        moveDirection = (1, 0);
                        break;
                    case "down-left":
                        moveDirection = (-1, 1);
                        break;
                    case "down":
                        moveDirection = (0, 1);
                        break;
                    case "down-right":
                        moveDirection = (1, 1);
                        break;
                    case "rotate":
                        Player.RotateBoatBeingPlaced(BattleShip.GetBoatsCanTouch());
                        Vertical = !Vertical;
                        break;
                    default:
                        throw new InvalidEnumArgumentException($"Unknown direction: {dir}");
                }


                if (moveDirection != null)
                    Player.MovePlacementBoat(((int x, int y)) moveDirection, BattleShip.GetBoatsCanTouch());

                (PosX, PosY) = Player.BoatBeingPlaced!.CellLocations![0];
            }
            else
            {
                PosX = 0;
                PosY = 0;
                Vertical = false;
                (int x, int y) facingDirection = (1, 0);
                Player.SetNextPlacementBoat();
                try
                {
                    Player.GetBoatBeingPlaced().CellLocations = new List<(int x, int y)> {(0, 0), facingDirection};
                    (PosX, PosY) = Player.PlayerBoard.GetFirstEmptyBoatCells(BattleShip.GetBoatsCanTouch());
                }
                catch (EvaluateException)
                {
                    facingDirection = (facingDirection.y, facingDirection.x);
                    Player.GetBoatBeingPlaced().CellLocations = new List<(int x, int y)> {(0, 0), facingDirection};
                    (PosX, PosY) = Player.PlayerBoard.GetFirstEmptyBoatCells(BattleShip.GetBoatsCanTouch());
                }

                Player.GetBoatBeingPlaced().PlaceBoat((PosX, PosY), facingDirection);
            }
        }


        public async Task<IActionResult> OnGetPlaceBoat(int gameId, int boardId, int posX, int posY, bool vertical)
        {
            Game = await _context.Games.Include(game => game.GameBoards).FirstAsync(g => g.GameId == gameId);
            BattleShip = new BattleShip(_context);
            await BattleShip.LoadFromDatabaseOfBoard(gameId, boardId);

            Player = BattleShip.GetPlayers()[0];
            Vertical = vertical;
            (int x, int y) direction = Vertical ? (0, 1) : (1, 0);
            PosX = posX;
            PosY = posY;
            var tempBoardId = boardId;
            Player.SetNextPlacementBoat();
            Player.GetBoatBeingPlaced().PlaceBoat((PosX, PosY), direction);
            _context.GameBoats.Add(new GameBoat()
            {
                Name = Player.GetBoatBeingPlaced().GetName(),
                FacingX = direction.x,
                FacingY = direction.y,
                GameBoard = Game.GameBoards!.First(gb => gb.GameBoardId == tempBoardId),
                GameBoardId = tempBoardId,
                Length = Player.GetBoatBeingPlaced().Length,
                LocationX = Player.GetBoatBeingPlaced().GetCellLocations()[0].x,
                LocationY = Player.GetBoatBeingPlaced().GetCellLocations()[0].y
            });
            Player.TransferPlacementBoat();

            if (Player.NotPlacedBoats.Count == 0)
            {
                int? nextBoardId = null;
                foreach (GameBoard gameBoard in Game.GameBoards ??
                                                throw new InvalidExpressionException("Game has no GameBoards"))
                {
                    if (gameBoard.GameBoardId <= tempBoardId) continue;
                    nextBoardId = gameBoard.GameBoardId;
                    break;
                }

                if (nextBoardId == null)
                {
                    BattleShip.GameStarted();
                    return RedirectToPage("../GamePlay/Index",
                        new {gameId = BattleShip.GameId});
                }

                tempBoardId = (int) nextBoardId;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("../GameCreation/PlaceBoats",
                new {gameId = BattleShip.GameId, boardId = tempBoardId});
        }
    }
}