using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Domain;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GameBrain
{
    public class BattleShip
    {
        private readonly ApplicationDbContext _dbContext;
        private int _boardTurn;
        private string _name = null!;


        public BattleShip(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int GameId { get; set; }
        public Game Game { get; set; } = null!;

        private List<Player> Players { get; set; } = new();
        private List<DefaultBoat> DefaultBoats { get; set; } = new();
        private EBoatsCanTouch eBoatsCanTouch { get; set; }
        private ENextMoveAfterHit eNextMoveAfterHit { get; set; }
        public int GetHeight { get; private set; }

        public int GetWidth { get; private set; }

        public EBoatsCanTouch GetBoatsCanTouch()
        {
            return eBoatsCanTouch;
        }

        public List<DefaultBoat> GetDefaultBoats()
        {
            return DefaultBoats;
        }

        public List<Player> GetPlayers()
        {
            return Players;
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public void CreateGameSettings(string gameName, int boardsWidth, int boardsHeight,
            ENextMoveAfterHit nextMoveAfterHit, EBoatsCanTouch boatsCanTouch, List<string> playerNames,
            List<DefaultBoat> defaultBoats)
        {
            GetHeight = boardsHeight;
            GetWidth = boardsWidth;
            eBoatsCanTouch = boatsCanTouch;
            eNextMoveAfterHit = nextMoveAfterHit;

            FinalizeGameCreationSettings(gameName, playerNames, defaultBoats).Wait();
        }

        public void CreateGameSettings(string gameName, List<string> playerNames, GameOption gameOption)
        {
            GetHeight = gameOption.BoardHeight;
            GetWidth = gameOption.BoardWidth;
            eBoatsCanTouch = gameOption.EBoatsCanTouch;
            eNextMoveAfterHit = gameOption.ENextMoveAfterHit;

            List<DefaultBoat> defaultBoats = new();
            List<GameOptionBoat> gameOptionBoats = _dbContext.GameOptionBoats
                .Where(boat => boat.GameOptionId == gameOption.GameOptionId).Include(boat => boat.DefaultBoat).ToList();
            foreach (var gameOptionBoat in gameOptionBoats)
                defaultBoats.Add(new DefaultBoat(gameOptionBoat.DefaultBoat.Name, gameOptionBoat.DefaultBoat.Length,
                    gameOptionBoat.Amount));
            FinalizeGameCreationSettings(gameName, playerNames, defaultBoats).Wait();
        }

        private async Task FinalizeGameCreationSettings(string gameName, List<string> playerNames,
            List<DefaultBoat> defaultBoats)
        {
            Players = new List<Player>();
            DefaultBoats = new List<DefaultBoat>();
            Game game = new()
            {
                GameOver = true,
                Name = gameName
            };
            await _dbContext.Games.AddAsync(game);

            foreach (string playerName in playerNames)
            {
                Domain.GameBoard gameBoard = new()
                {
                    PlayerName = playerName,
                    Game = game
                };

                ECellState[,] cellStates = new ECellState[GetWidth, GetHeight];
                for (var x = 0; x < GetWidth; x++)
                for (var y = 0; y < GetHeight; y++)
                    cellStates[x, y] = ECellState.Empty;

                Player player = new(playerName);
                foreach (var defaultBoat in defaultBoats)
                    for (var amount = 0; amount < defaultBoat.Amount; amount++)
                        player.AddUnplacedBoat(defaultBoat.Name, defaultBoat.Length);
                player.MakeNewBoard(GetWidth, GetHeight, cellStates);
                gameBoard.BoardJson = GetSerializeBoard(player.PlayerBoard);
                await _dbContext.GameBoards.AddAsync(gameBoard);
                await _dbContext.SaveChangesAsync();
                player.PlayerBoard.GameBoardId = gameBoard.GameBoardId;

                Players.Add(player);
            }

            game.BoardTurnId = Players[0].PlayerBoard.GameBoardId;
            await AddGameOption(game, GetWidth, GetHeight, " gameOption for " + gameName, eBoatsCanTouch,
                eNextMoveAfterHit, defaultBoats);

            DefaultBoats = defaultBoats;
            _name = gameName;
            GameId = game.GameId;
            Game = game;
        }


        public List<Player> GetPlayersNotPlaced()
        {
            return GetPlayers().Where(player => player.GetRemainingBoatsCount() < GetMaxBoatCount()).ToList();
        }

        public int GetMaxBoatCount()
        {
            return GetDefaultBoats().Sum(boat => boat.Amount);
        }

        public bool PlayerTurnEnded(ECellState eCellState)
        {
            var playerTurnEnded = !(eNextMoveAfterHit == ENextMoveAfterHit.SamePlayer && eCellState == ECellState.Hit);
            if (playerTurnEnded)
            {
                _boardTurn = (_boardTurn + 1) % Players.Count;
                Game.BoardTurnId = Players[_boardTurn].PlayerBoard.GameBoardId;
                _dbContext.SaveChanges();
            }

            return playerTurnEnded;
        }


        public bool GameHasEnded()
        {
            return Players.Count(player => player.GetRemainingBoatsCount() > 0) < 2;
        }

        public async Task<bool> MakeAMove(Player attackedPlayer, (int x, int y) location)
        {
            if (location.x < 0)
                location.x = location.x % GetWidth + location.x;
            else if (location.x >= GetWidth) location.x %= GetWidth;
            if (location.y < 0)
                location.y %= GetHeight + location.y;
            else if (location.y >= GetHeight) location.y %= GetHeight;

            GameBoard attackedBoard = attackedPlayer.PlayerBoard;
            var eCellState = attackedPlayer.HasBoatInLocation((location.x, location.y))
                ? ECellState.Hit
                : ECellState.Miss;
            attackedBoard.SetCellState(location, eCellState);
            Domain.GameBoard attackedGameBoard =
                (Game.GameBoards ?? throw new InvalidOperationException("Game does not have GameBoards")).First(b =>
                    b.GameBoardId == attackedBoard.GameBoardId);
            attackedGameBoard.BoardJson = GetSerializeBoard(attackedBoard);

            TurnSave turnSave = new()
            {
                CellX = location.x,
                CellY = location.y,
                AttackerId = Game.BoardTurnId,
                DefenderId = attackedGameBoard.GameBoardId,
                Game = Game
            };
            await _dbContext.TurnSaves.AddAsync(turnSave);
            await _dbContext.SaveChangesAsync();
            return PlayerTurnEnded(attackedBoard.GetCellState(location));
        }

        public List<(int, string)> GetGameOptionNamesWithIds()
        {
            List<( int id, string Name)> results = new();
            foreach (var gameOption in _dbContext.GameOptions.Include(go => go.Games)
                .Where(option => option.Games == null || option.Games.Count == 0))
                results.Add((gameOption.GameOptionId, gameOption.Name));
            return results;
        }

        public async Task AddGameOption(Game game, int boardWidth, int boardHeight, string name,
            EBoatsCanTouch eBoatsCanTouch, ENextMoveAfterHit eNextMoveAfterHit, List<DefaultBoat> defaultBoats)
        {
            await _dbContext.SaveChangesAsync();

            GameOption gameOption = await MakeGameOption(boardWidth, boardHeight, name, eBoatsCanTouch,
                eNextMoveAfterHit, defaultBoats);

            game.GameOption = gameOption;
            await _dbContext.SaveChangesAsync();
        }


        public async void AddGameOption(int boardWidth, int boardHeight, string name, EBoatsCanTouch eBoatsCanTouch,
            ENextMoveAfterHit eNextMoveAfterHit, List<DefaultBoat> defaultBoats)
        {
            MakeGameOption(boardWidth, boardHeight, name, eBoatsCanTouch,
                eNextMoveAfterHit, defaultBoats).Wait();
            await _dbContext.SaveChangesAsync();
        }

        private async Task<GameOption> MakeGameOption(int boardWidth, int boardHeight, string name,
            EBoatsCanTouch eBoatsCanTouch, ENextMoveAfterHit eNextMoveAfterHit, List<DefaultBoat> defaultBoats)
        {
            GameOption gameOption = new()
            {
                BoardHeight = boardHeight,
                BoardWidth = boardWidth,
                EBoatsCanTouch = eBoatsCanTouch,
                ENextMoveAfterHit = eNextMoveAfterHit,
                GameOptionBoats = null,
                Name = name
            };

            List<GameOptionBoat> gameDefaultBoats = new();
            foreach (DefaultBoat defaultBoat in defaultBoats)
                gameDefaultBoats.Add(new GameOptionBoat
                {
                    Amount = defaultBoat.Amount,
                    DefaultBoat = await _dbContext.DefaultBoats.FirstAsync(db => db.Name == defaultBoat.Name),
                    GameOption = gameOption
                });
            await _dbContext.GameOptions.AddAsync(gameOption);
            await _dbContext.SaveChangesAsync();
            await _dbContext.GameOptionBoats.AddRangeAsync(gameDefaultBoats);
            await _dbContext.SaveChangesAsync();
            return gameOption;
        }


        public GameOption GetGameOptionById(int gameOptionId)
        {
            return _dbContext.GameOptions.Include(go => go.GameOptionBoats).Include(go => go.Games)
                .First(go => go.GameOptionId == gameOptionId);
        }

        public List<(int playerIndex, Player player)> NotDefeatedPlayers()
        {
            List<(int playerIndex, Player playe)> result = new();
            for (var i = 0; i < Players.Count; i++)
                if (Players[i].GetRemainingBoatsCount() != 0)
                    result.Add((i, Players[i]));
            return result;
        }

        public Player GetCurrentPlayer()
        {
            while (true)
            {
                if (!Players[_boardTurn].HasBeenDefeated()) return Players[_boardTurn];
                _boardTurn = (_boardTurn + 1) % Players.Count;
            }
        }

        public (int playerIndex, Player player) GetCurrentPlayerWithIndex()
        {
            while (true)
            {
                if (!Players[_boardTurn].HasBeenDefeated()) return (_boardTurn, Players[_boardTurn]);
                _boardTurn = (_boardTurn + 1) % Players.Count;
            }
        }

        public List<Player> GetPlayerOpponents(int playerIndex)
        {
            return NotDefeatedPlayers().Where(tuple => tuple.playerIndex != playerIndex).Select(tuple => tuple.player)
                .ToList();
        }

        public void RemoveGameOptionWithId(int id)
        {
            GameOption gameOption = _dbContext.GameOptions.First(option => option.GameOptionId == id);
            _dbContext.GameOptions.Remove(gameOption);
            _dbContext.SaveChanges();
        }


        public async Task TransferPlayerPlacementBoat(Player player)
        {
            Boat boat = player.GetBoatBeingPlaced();
            var cellLocation = boat.GetCellLocations().First();
            var facingDirection = boat.GetFacingDirection();
            GameBoat gameBoat = new()
            {
                Name = boat.GetName(),
                Length = boat.Length,
                GameBoardId = player.PlayerBoard.GameBoardId,
                LocationX = cellLocation.x,
                LocationY = cellLocation.y,
                FacingX = facingDirection.x,
                FacingY = facingDirection.y
            };
            await _dbContext.GameBoats.AddAsync(gameBoat);
            await _dbContext.SaveChangesAsync();
            boat.GameBoatId = gameBoat.GameBoatId;

            await _dbContext.SaveChangesAsync();
            player.TransferPlacementBoat();
        }

        public async Task PlacePlayerRemainingBoats(Player player)
        {
            player.PlaceRemainingBoats(eBoatsCanTouch);
            foreach (Boat boat in player.Boats)
            {
                var cellLocation = boat.GetCellLocations().First();
                var facingDirection = boat.GetFacingDirection();
                GameBoat gameBoat = new()
                {
                    Name = boat.GetName(),
                    Length = boat.Length,
                    GameBoardId = player.PlayerBoard.GameBoardId,
                    LocationX = cellLocation.x,
                    LocationY = cellLocation.y,
                    FacingX = facingDirection.x,
                    FacingY = facingDirection.y
                };
                await _dbContext.GameBoats.AddAsync(gameBoat);
                await _dbContext.SaveChangesAsync();
                boat.GameBoatId = gameBoat.GameBoatId;
            }

            await _dbContext.SaveChangesAsync();
        }

        public string GetSerializeBoard(GameBoard board)
        {
            StringBuilder result = new();
            for (var x = 0; x < GetWidth; x++)
            for (var y = 0; y < GetHeight; y++)
                //x,y:ECellState;
                result.Append(x).Append(",").Append(y).Append(":").Append(board.Board[x, y]).Append(";");

            result.Length--;
            return result.ToString();
        }

        public ECellState[,] GetBoardStateFromJson(string jsonBoard)
        {
            ECellState[,] result = new ECellState[GetWidth, GetHeight];
            string[] jsonCells = jsonBoard.Split(";");

            foreach (string jsonCell in jsonCells)
            {
                string[] temp = jsonCell.Split(":");
                ECellState cellState;
                switch (temp[1])
                {
                    case "Empty":
                        cellState = ECellState.Empty;
                        break;
                    case "Miss":
                        cellState = ECellState.Miss;
                        break;
                    case "Hit":
                        cellState = ECellState.Hit;
                        break;
                    case "Picked":
                        cellState = ECellState.Picked;
                        break;
                    default:
                        throw new EvaluateException("Unknown ECellState " + temp[1]);
                }

                temp = temp[0].Split(",");
                result[int.Parse(temp[0]), int.Parse(temp[1])] = cellState;
            }

            return result;
        }

        public void GameStarted()
        {
            Game.GameOver = false;
            _dbContext.SaveChanges();
        }

        public void UndoTurn()
        {
            if (Game.TurnSaves == null || Game.TurnSaves.Count == 0) return;
            var turnSaveId = Game.TurnSaves.Max(save => save.TurnSaveId);
            TurnSave turnSave = Game.TurnSaves.First(save => save.TurnSaveId == turnSaveId);
            var defenderBoard = Players.Select(player => player.PlayerBoard)
                .First(gameBoard => gameBoard.GameBoardId == turnSave.DefenderId);
            var attackerBoard = Players.Select(player => player.PlayerBoard)
                .First(gameBoard => gameBoard.GameBoardId == turnSave.AttackerId);
            defenderBoard.Board[turnSave.CellX, turnSave.CellY] = ECellState.Empty;
            (Game.GameBoards ?? throw new InvalidOperationException("Game Has No GameBoards"))
                .First(board => board.GameBoardId == turnSave.DefenderId).BoardJson = GetSerializeBoard(defenderBoard);
            Game.BoardTurnId = attackerBoard.GameBoardId;
            _boardTurn =
                Players.IndexOf(Players.First(player => player.PlayerBoard.GameBoardId == attackerBoard.GameBoardId));
            _dbContext.TurnSaves.Remove(turnSave);
            _dbContext.SaveChanges();
            LoadFromDatabase(GameId).Wait();
        }

        public List<(int id, string name)> GetGameNamesWithIds()
        {
            List<(int id, string name)> result = new();
            foreach (var game in _dbContext.Games.Where(game => game.GameOver == false))
                result.Add((game.GameId, game.Name));

            return result;
        }

        public async Task LoadFromDatabase(int id)
        {
            Game = await _dbContext.Games.Include(game => game.TurnSaves).ThenInclude(save => save.Attacker)
                .Include(game => game.TurnSaves).ThenInclude(save => save.Defender)
                .Include(game => game.GameBoards).ThenInclude(board => board.GameBoats)
                .Include(game => game.GameOption).ThenInclude(option => option!.GameOptionBoats)
                .ThenInclude(boat => boat.DefaultBoat)
                .FirstAsync(game => game.GameId == id);
            _name = Game.Name;
            GameId = Game.GameId;
            GameOption gameOption = Game.GameOption ?? throw new InvalidOperationException("No db GameOption");
            GetWidth = gameOption.BoardWidth;
            GetHeight = gameOption.BoardHeight;
            eBoatsCanTouch = gameOption.EBoatsCanTouch;
            eNextMoveAfterHit = gameOption.ENextMoveAfterHit;
            List<Player> players = new();
            List<DefaultBoat> defaultBoats = new();
            if (gameOption.GameOptionBoats == null) throw new InvalidOperationException("No db GameOptionBoats");
            foreach (GameOptionBoat gameOptionBoat in gameOption.GameOptionBoats)
                defaultBoats.Add(new DefaultBoat(gameOptionBoat.DefaultBoat.Name, gameOptionBoat.DefaultBoat.Length,
                    gameOptionBoat.Amount));

            foreach (Domain.GameBoard gameBoard in Game.GameBoards!)
            {
                Player player = new(gameBoard.PlayerName);

                List<Boat> playerBoats = new();
                foreach (GameBoat gameBoat in (gameBoard.GameBoats ??
                                               throw new InvalidOperationException("No db gameBoats")).ToList())
                {
                    Boat boat = new(gameBoat.Name)
                    {
                        Length = gameBoat.Length
                    };
                    boat.PlaceBoat(((int X, int Y)) (gameBoat.LocationX!, gameBoat.LocationY!),
                        ((int X, int Y)) (gameBoat.FacingX!, gameBoat.FacingY!));
                    boat.GameBoatId = boat.GameBoatId;
                    playerBoats.Add(boat);
                }

                player.Boats = playerBoats;
                ECellState[,] board = GetBoardStateFromJson(gameBoard.BoardJson);


                player.PlayerBoard = new GameBoard(gameOption.BoardWidth, gameOption.BoardHeight, board, player)
                {
                    GameBoardId = gameBoard.GameBoardId
                };
                players.Add(player);
            }

            Players = players.OrderBy(player => player.PlayerBoard.GameBoardId).ToList();
            DefaultBoats = defaultBoats;


            _boardTurn = Players.Select(player => player.PlayerBoard.GameBoardId).ToList().IndexOf(Game.BoardTurnId);
        }

        public async Task DeleteFromDatabase(int gameId)
        {
            Game game = await _dbContext.Games.Include(g => g.GameOption).FirstAsync(g => g.GameId == gameId);
            GameOption gameOption = game.GameOption!;
            _dbContext.Games.Remove(game);
            _dbContext.GameOptions.Remove(gameOption);
            await _dbContext.SaveChangesAsync();
        }

        public async Task LoadFromDatabaseOfBoard(int gameId, int boardId)
        {
            Game = await _dbContext.Games.Include(game => game.TurnSaves).ThenInclude(save => save.Attacker)
                .Include(game => game.TurnSaves).ThenInclude(save => save.Defender)
                .Include(game => game.GameOption).ThenInclude(option => option!.GameOptionBoats)
                .ThenInclude(boat => boat.DefaultBoat)
                .FirstAsync(game => game.GameId == gameId);
            Domain.GameBoard gameBoard = await _dbContext.GameBoards.Include(board => board.GameBoats)
                .FirstAsync(board => board.GameBoardId == boardId);
            _name = Game.Name;
            GameId = Game.GameId;
            GameOption gameOption = Game.GameOption ?? throw new InvalidOperationException("No db GameOption");
            GetWidth = gameOption.BoardWidth;
            GetHeight = gameOption.BoardHeight;
            eBoatsCanTouch = gameOption.EBoatsCanTouch;
            eNextMoveAfterHit = gameOption.ENextMoveAfterHit;
            List<DefaultBoat> defaultBoats = new();
            if (gameOption.GameOptionBoats == null) throw new InvalidOperationException("No db GameOptionBoats");
            foreach (GameOptionBoat gameOptionBoat in gameOption.GameOptionBoats)
                defaultBoats.Add(new DefaultBoat(gameOptionBoat.DefaultBoat.Name, gameOptionBoat.DefaultBoat.Length,
                    gameOptionBoat.Amount));
            Player player = new(gameBoard.PlayerName);

            foreach (GameBoat gameBoat in
                (gameBoard.GameBoats ?? throw new InvalidOperationException("No db gameBoats")).ToList())
            {
                Boat boat = new(gameBoat.Name)
                {
                    Length = gameBoat.Length
                };
                boat.PlaceBoat(((int X, int Y)) (gameBoat.LocationX!, gameBoat.LocationY!),
                    ((int X, int Y)) (gameBoat.FacingX!, gameBoat.FacingY!));
                boat.GameBoatId = boat.GameBoatId;
                player.Boats.Add(boat);
            }

            foreach (GameOptionBoat gameOptionBoat in (Game.GameOption.GameOptionBoats ??
                                                       throw new InvalidOperationException("GameOption has no Boats"))
                .ToList())
            {
                string boatName = gameOptionBoat.DefaultBoat.Name;
                var missingBoatAmount = gameOptionBoat.Amount - player.Boats.Count(boat => boat.GetName() == boatName);
                for (var i = 0; i < missingBoatAmount; i++)
                    player.NotPlacedBoats.Add(new Boat(boatName) {Length = gameOptionBoat.DefaultBoat.Length});
            }

            ECellState[,] board = GetBoardStateFromJson(gameBoard.BoardJson);

            player.PlayerBoard = new GameBoard(gameOption.BoardWidth, gameOption.BoardHeight, board, player)
            {
                GameBoardId = gameBoard.GameBoardId
            };
            DefaultBoats = defaultBoats;
            Players = new List<Player> {player};
        }


        public Player GetPlayerByBoardId(int boardId)
        {
            return Players.First(player => player.PlayerBoard.GameBoardId == boardId);
        }
    }
}