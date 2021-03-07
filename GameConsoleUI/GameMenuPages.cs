using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Enums;
using GameBrain;
using DefaultBoat = GameBrain.DefaultBoat;

namespace GameConsoleUI
{
    public class GameMenuPages
    {
        private readonly BattleShip _game;
        private readonly ConsoleColor DefaultForegroundColour = ConsoleColor.Blue;
        private readonly ConsoleColor SelectedForegroundColour = ConsoleColor.Yellow;


        public GameMenuPages(BattleShip game)
        {
            _game = game;
        }


        public int RunGameNew()
        {
            MakeGameSettings();

            PlaceShips();

            return RunGame();
        }

        public int RunGameFromOptions()
        {
            if (ChooseGameSettings() != 0) return -2;

            PlaceShips();

            return RunGame();
        }


        private int RunGame()
        {
            Player? currentPlayer = null;
            _game.GameStarted();
            while (!_game.GameHasEnded())
            {
                _game.LoadFromDatabase(_game.GameId).Wait();
                int playerIndex;
                chooseOpponent:
                (playerIndex, currentPlayer) = _game.GetCurrentPlayerWithIndex();

                var opponent = PlayerTurn.ChooseOpponent(currentPlayer, _game.GetPlayerOpponents(playerIndex),
                    _game.GetBoatsCanTouch());
                if (opponent == null)
                {
                    _game.UndoTurn();
                    var goToMainPage = AskToEndGame(_game.GetCurrentPlayer().Name);
                    if (goToMainPage) return -2;
                    goto chooseOpponent;
                }

                var bombLocation = PlayerTurn.ChooseBombLocation(currentPlayer, opponent, _game.GetBoatsCanTouch());
                if (bombLocation == (-3, -3)) continue;
                
                if (bombLocation == (-2, -2)) //Undo button was pressed
                {
                    _game.UndoTurn();
                    var goToMainPage = AskToEndGame(_game.GetCurrentPlayer().Name);
                    if (goToMainPage) return -2;
                    goto chooseOpponent;
                }

                var playerTurnEnded = _game.MakeAMove(opponent, bombLocation).Result;
                if (playerTurnEnded)
                {
                    var goToMainPage = AskToEndGame(_game.GetCurrentPlayer().Name);
                    if (goToMainPage) return -2;
                }
            }

            Console.WriteLine("---------" + "Player " + currentPlayer!.Name + " has won the game!" + "-----------");
            foreach (Player player in _game.GetPlayers())
                BattleshipUI.DrawPlayerBoard(player, ConsoleColor.Cyan, false, EBoatsCanTouch.Yes);
            Console.WriteLine("Press any key to go back to menu");
            _game.DeleteFromDatabase(_game.GameId).Wait();
            Console.ReadKey();
            return -2;
        }

        private bool AskToEndGame(string playerName)
        {
            return ChooseItemFromList(new List<string> {"Start turn", "Close game"},
                "It is now " + playerName + "'s turn.") == 1;
        }


        private int ChooseGameSettings()
        {
            List<(int, string)> gameOptions = _game.GetGameOptionNamesWithIds();
            if (gameOptions.Count == 0)
            {
                ShowNoEntries("Game Options");
                return -2;
            }

            var index = ChooseItemFromList(gameOptions.Select(options => options.Item2).ToList(),
                "Choose preset rules");
            var chosenGameOptionId = gameOptions.Select(options => options.Item1).ToList()[index];
            GameOption gameOption = _game.GetGameOptionById(chosenGameOptionId);
            string gameName = AskString(1, 25, "What is the Game Name?");
            List<string> playerNames = AskPlayerNames();
            _game.CreateGameSettings(gameName, playerNames, gameOption);
            return 0;
        }

        private void MakeGameSettings()
        {
            askGameName:
            string name = AskString(1, 25, "What is the Game Name?");
            var width = AskInteger("How width do you want your board?", 5, 25);
            var height = AskInteger("How high do you want your board?", 5, 15);
            askBoatsCanTouch:
            var boatsCanTouch = AskBoatsCanTouchEnum();
            if (boatsCanTouch == null) goto askGameName;
            var moveAfterHit = AskNextMoveAfterHitEnum();
            if (moveAfterHit == null) goto askBoatsCanTouch;
            List<DefaultBoat> defaultBoats = AskDefaultBoats(width, width, (EBoatsCanTouch) boatsCanTouch);
            List<string> playerNames = AskPlayerNames();
            _game.CreateGameSettings(name, width, height, (ENextMoveAfterHit) moveAfterHit,
                (EBoatsCanTouch) boatsCanTouch, playerNames, defaultBoats);
        }


        private void PlaceShips()
        {
            PlayerTurn playerTurn = new();
            List<Player> players = _game.GetPlayersNotPlaced();
            do
            {
                List<string> menuEntries = players.Select(player => "Place ships for " + player.Name).ToList();
                var playersCount = menuEntries.Count;
                menuEntries.AddRange(players.Select(player =>
                    "Automatically place remaining ships for " + player.Name));
                var index = ChooseItemFromList(menuEntries, "Choose placement");
                if (index < playersCount)
                {
                    Player chosenPlayer = players[index];
                    while (chosenPlayer.HasBoatsToBePlaced())
                    {
                        chosenPlayer.SetNextPlacementBoat();
                        playerTurn.SetPlacementBoatOnBoard(chosenPlayer, _game.GetBoatsCanTouch());
                        _game.TransferPlayerPlacementBoat(chosenPlayer).Wait();
                    }
                }
                else
                {
                    Player chosenPlayer = players[index - playersCount];
                    _game.PlacePlayerRemainingBoats(chosenPlayer).Wait();
                }

                players = _game.GetPlayers().Where(player => player.GetRemainingBoatsCount() < _game.GetMaxBoatCount())
                    .ToList();
            } while (players.Count != 0);
        }


        private int ChooseItemFromList(List<string> itemName, string Title)
        {
            List<string> menuItems = new();
            menuItems.AddRange(itemName);
            var curItem = 0;
            ConsoleKeyInfo key;

            do
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine(Title);
                Console.ForegroundColor = DefaultForegroundColour;

                // Go through all menu items
                for (var c = 0; c < menuItems.Count; c++)
                    // Point out current option
                    if (curItem == c)
                    {
                        Console.ForegroundColor = SelectedForegroundColour;
                        Console.WriteLine(menuItems[c]);
                        Console.ForegroundColor = DefaultForegroundColour;
                    }
                    // Give other options
                    else
                    {
                        Console.ForegroundColor = DefaultForegroundColour;
                        Console.WriteLine(menuItems[c]);
                    }

                key = Console.ReadKey(true);
                if (key.Key.ToString() == "DownArrow")
                {
                    curItem++;
                    if (curItem > menuItems.Count - 1) curItem = 0;
                }
                else if (key.Key.ToString() == "UpArrow")
                {
                    curItem--;
                    if (curItem < 0) curItem = menuItems.Count - 1;
                }
            } while (key.KeyChar != 13);

            return curItem;
        }


        public static int RunTutorial()
        {
            Console.WriteLine("Battleship is a strategy type guessing game for two players.\n" +
                              "It is played on ruled grids on which each player's \n" +
                              "fleet of ships (including battleships) are marked.\n" +
                              "The locations of the fleets are concealed from the other player. \n" +
                              "Players roate turns calling \"shots\" at other player's ships,\n" +
                              "and the objective of the game is to destroy the opposing player's fleet.");

            Console.WriteLine();
            Console.WriteLine("You move around using the arrow keys" +
                              "Confirm bombing location using Enter.\n" +
                              "To undo a turn you press T.\n" +
                              "The game also auto-saves your latest move.\n" +
                              "Good luck Commander!");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press any key to go back");
            Console.ReadKey(true);
            return -9;
        }

        private string AskString(int minLength, int maxLength, string question)
        {
            string input;
            //Name
            while (true)
            {
                Console.Clear();
                Console.WriteLine(question + " (" + minLength + "-" + maxLength + ")");
                Console.Write(">");
                Console.ForegroundColor = SelectedForegroundColour;
                input = Console.ReadLine()!;
                Console.ForegroundColor = DefaultForegroundColour;
                Console.WriteLine();
                if (input!.Length < minLength)
                    Console.WriteLine("Given length was too low");
                else if (input!.Length > maxLength)
                    Console.WriteLine("Given length was too high");
                else
                    break;

                Console.WriteLine();
                Console.WriteLine("Press any key to re-enter");
                Console.ReadKey(true);
            }

            return input;
        }


        private int AskInteger(string question, int minValue, int maxValue)
        {
            int boardWidth;
            while (true)
            {
                Console.Clear();
                Console.WriteLine(question + " (" + minValue + "-" + maxValue + ")");
                Console.Write(">");
                Console.ForegroundColor = SelectedForegroundColour;
                string input = Console.ReadLine()!;
                Console.ForegroundColor = DefaultForegroundColour;
                Console.WriteLine();
                if (!int.TryParse(input, out boardWidth))
                    Console.WriteLine("You need to write a number.");
                else if (boardWidth < minValue)
                    Console.WriteLine("Given value was too low");
                else if (boardWidth > maxValue)
                    Console.WriteLine("Given value was too high");
                else
                    break;

                Console.WriteLine();
                Console.WriteLine("Press any key to re-enter");
                Console.ReadKey(true);
            }

            return boardWidth;
        }


        private ENextMoveAfterHit? AskNextMoveAfterHitEnum()
        {
            while (true)
            {
                Console.Clear();
                var itemIndex = ChooseItemFromList(new List<string> {"Same PLayer", "Next Player", "Go Back"},
                    "Choose who plays after hit");
                switch (itemIndex)
                {
                    case 0:
                        return ENextMoveAfterHit.SamePlayer;
                    case 1:
                        return ENextMoveAfterHit.SamePlayer;
                    case 2:
                        return null;
                    default:
                        throw new ApplicationException("Unknown item from list");
                }
            }
        }


        private EBoatsCanTouch? AskBoatsCanTouchEnum()
        {
            var itemIndex =
                ChooseItemFromList(
                    new List<string>
                        {"Boats can touch", "Boat corners can not touch", "Boats can not touch", "Go Back"},
                    "Choose boat touching rule");
            switch (itemIndex)
            {
                case 0:
                    return EBoatsCanTouch.Yes;
                case 1:
                    return EBoatsCanTouch.Corner;
                case 2:
                    return EBoatsCanTouch.No;
                case 3:
                    return null;
                default:
                    throw new ApplicationException("Unknown item from list");
            }
        }

        private List<DefaultBoat> AskDefaultBoats(int width, int height, EBoatsCanTouch eBoatsCanTouch)
        {
            var wiggleRoom = eBoatsCanTouch switch
            {
                EBoatsCanTouch.Corner => 0.1,
                EBoatsCanTouch.No => 0.1,
                EBoatsCanTouch.Yes => 0.7,
                _ => throw new Exception("Unknown EBoatsCanTouch enum")
            };

            var space = width * height;

            int patrolCount;
            int cruiserCount;
            int submarineCount;
            int battleshipCount;
            int carrierCount;
            (space, patrolCount) = AskBoatAmount(space, 1, "patrol", wiggleRoom);
            (space, cruiserCount) = AskBoatAmount(space, 2, "cruiser", wiggleRoom);
            (space, submarineCount) = AskBoatAmount(space, 3, "submarine", wiggleRoom);
            (space, battleshipCount) = AskBoatAmount(space, 4, "battleship", wiggleRoom);
            (_, carrierCount) = AskBoatAmount(space, 5, "carrier", wiggleRoom);

            List<DefaultBoat> defaultBoats = new()
            {
                new DefaultBoat("Patrol", 1, patrolCount),
                new DefaultBoat("Cruiser", 2, cruiserCount),
                new DefaultBoat("Submarine", 3, submarineCount),
                new DefaultBoat("Battleship", 4, battleshipCount),
                new DefaultBoat("Carrier", 5, carrierCount)
            };

            return defaultBoats;
        }

        private (int space, int shipCount) AskBoatAmount(int space, int shipSize, string shipName,
            double sizeWiggleRoom)
        {
            int shipCount;
            var oneShipSpace = shipSize / sizeWiggleRoom;
            var maxShipCount = (int) (space / oneShipSpace);
            while (true)
            {
                if (maxShipCount < 1)
                {
                    shipCount = 0;
                    break;
                }

                Console.Clear();
                Console.WriteLine("How many " + shipName + "s are on the map?(0-" + maxShipCount + ")");
                Console.Write(">");
                Console.ForegroundColor = SelectedForegroundColour;
                string input = Console.ReadLine()!;
                Console.ForegroundColor = DefaultForegroundColour;
                Console.WriteLine();
                if (!int.TryParse(input, out shipCount))
                    Console.WriteLine("You need to write a number.");
                else if (shipCount < 0)
                    Console.WriteLine("Given amount was too low");
                else if (shipCount > maxShipCount)
                    Console.WriteLine("Given amount was too high");
                else
                    break;
                Console.WriteLine();
                Console.WriteLine("Press any key to re-enter");
                Console.ReadKey(true);
            }

            return (Convert.ToInt32(space - oneShipSpace * shipCount), shipCount);
        }

        private List<string> AskPlayerNames()
        {
            List<string> menuItems = new() {"Add Player", "Reset players", "Start placing ships"};
            startAskingPlayerNames:
            List<string> playerNames = new();
            playerNames.Add(GetPlayerName(playerNames.Count + 1));
            playerNames.Add(GetPlayerName(playerNames.Count + 1));

            while (playerNames.Count <= 4)
            {
                var actionIndex = ChooseItemFromList(menuItems, "Choose action");
                switch (actionIndex)
                {
                    case 1:
                        goto startAskingPlayerNames;
                    case 0:
                        playerNames.Add(GetPlayerName(playerNames.Count + 1));
                        break;
                }

                if (actionIndex == 2) break;
            }

            return playerNames;
        }


        private string GetPlayerName(int playerIndex)
        {
            string input;
            //Name
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Player " + playerIndex + " name:");
                Console.Write(">");
                Console.ForegroundColor = SelectedForegroundColour;
                input = Console.ReadLine()!;
                Console.ForegroundColor = DefaultForegroundColour;
                Console.WriteLine();
                if (input!.Length < 1)
                    Console.WriteLine("Given name was too short");
                else if (input.Length > 60)
                    Console.WriteLine("Given name was too long");
                else
                    break;

                Console.WriteLine();
                Console.WriteLine("Press any key to re-enter");
                Console.ReadKey(true);
            }

            return input;
        }

        public int AddToGameOptions()
        {
            askGameName:
            string name = AskString(1, 25, "What is the Game Option name?");
            var width = AskInteger("How width do you want your board?", 5, 25);
            var height = AskInteger("How high do you want your board?", 5, 15);
            askBoatsCanTouch:
            var boatsCanTouch = AskBoatsCanTouchEnum();
            if (boatsCanTouch == null) goto askGameName;
            var moveAfterHit = AskNextMoveAfterHitEnum();
            if (moveAfterHit == null) goto askBoatsCanTouch;
            List<DefaultBoat> defaultBoats = AskDefaultBoats(width, width, (EBoatsCanTouch) boatsCanTouch);
            _game.AddGameOption(width, height, name, (EBoatsCanTouch) boatsCanTouch, (ENextMoveAfterHit) moveAfterHit,
                defaultBoats);
            return -2;
        }

        public int DeleteFromGameOptions()
        {
            List<(int id, string name)> gameOptionNamesWithIds = _game.GetGameOptionNamesWithIds();
            if (gameOptionNamesWithIds.Count == 0)
            {
                ShowNoEntries("Game Options");
                return -2;
            }

            List<string> entries = gameOptionNamesWithIds.Select(e => e.name).ToList();
            entries.Add("Go Back");
            var index = ChooseItemFromList(entries, "Choose Game Option to delete");
            if (index == entries.Count - 1) return -2;

            _game.RemoveGameOptionWithId(gameOptionNamesWithIds[index].id);

            return -2;
        }

        private void ShowNoEntries(string entryName)
        {
            Console.ForegroundColor = DefaultForegroundColour;
            Console.WriteLine("There are no entries for " + entryName);
            Console.WriteLine("Press any key to go back to menu");
            Console.ReadKey();
        }

        public int LoadGame()
        {
            List<(int id, string name)> result = _game.GetGameNamesWithIds();
            List<string> entries = result.Select(result => result.name).ToList();
            entries.Add("Go Back");
            var index = ChooseItemFromList(entries, "Load Game");
            if (index == entries.Count() - 1) return -9;

            _game.LoadFromDatabase(result[index].id).Wait();
            return RunGame();
        }

        public int DeleteGame()
        {
            List<(int id, string name)> result = _game.GetGameNamesWithIds();
            List<string> entries = result.Select(result => result.name).ToList();
            entries.Add("Go Back");
            var index = ChooseItemFromList(entries, "Delete Game");
            if (index == entries.Count() - 1) return -9;

            _game.DeleteFromDatabase(result[index].id).Wait();
            return -2;
        }
    }
}