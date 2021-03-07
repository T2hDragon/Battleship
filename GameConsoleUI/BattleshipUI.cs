using System;
using System.Linq;
using System.Text;
using Domain.Enums;
using GameBrain;

namespace GameConsoleUI
{
    public static class BattleshipUI
    {
        private const ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;
        private const ConsoleColor PickedForegroundColor = ConsoleColor.Yellow;
        private const ConsoleColor PickedBackgroundColor = ConsoleColor.Yellow;
        private const ConsoleColor ShipColour = ConsoleColor.Green;
        private static ConsoleColor DefaultForegroundColor = ConsoleColor.DarkBlue;


        public static void DrawPlayerBoard(Player currentPlayer, ConsoleColor color, bool boatsHidden,
            EBoatsCanTouch eBoatsCanTouch)
        {
            DefaultForegroundColor = color;
            Console.ForegroundColor = DefaultForegroundColor;
            Console.WriteLine();
            var width = currentPlayer.PlayerBoard.Width;
            var height = currentPlayer.PlayerBoard.Height;
            Console.WriteLine(currentPlayer.Name + "'s battlefield");
            WriteTableLetters(width, height);
            Console.WriteLine();
            // current Player board
            WriteBattlefieldRowDividerLine(width, height);

            Console.WriteLine();
            for (var rowIndex = 0; rowIndex < height; rowIndex++)

            {
                WriteBattlefieldActionRow(currentPlayer, width, rowIndex, height, boatsHidden, eBoatsCanTouch);

                Console.WriteLine();

                WriteBattlefieldRowDividerLine(width, height);

                Console.WriteLine();
            }

            Console.BackgroundColor = ConsoleColor.Black;
        }

        private static StringBuilder GetTableNumberEmptyStringBuilder(int height)
        {
            StringBuilder preString = new(" ");
            for (var i = 0; i < (int) Math.Floor(Math.Log10(height)) + 1; i++) preString.Append(" ");

            return preString;
        }


        private static void WriteBattlefieldActionRow(Player player, int width, int rowIndex, int height,
            bool boatsHidden, EBoatsCanTouch eBoatsCanTouch)
        {
            WriteTableNumber(rowIndex, height);

            Console.Write("| ");
            (int x, int y) location = (0, rowIndex);

            for (var colIndex = 0; colIndex < width; colIndex++)
            {
                location.x = colIndex;
                var hasBoatInLocation = player.HasBoatInLocation(location);
                var hasPlacementBoatInLocation = player.BoatBeingPlaced != null &&
                                                 player.BoatBeingPlaced.GetCellLocations()
                                                     .Contains(location);


                var eCellState = player.PlayerBoard.GetCellState(location);

                // Set cellState
                switch (eCellState)
                {
                    case ECellState.Hit:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case ECellState.Miss:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case ECellState.Picked:
                        Console.ForegroundColor = PickedForegroundColor;
                        Console.BackgroundColor = PickedBackgroundColor;
                        break;
                    default:
                        Console.ForegroundColor = DefaultForegroundColor;
                        break;
                }

                string cellString = CellString(eCellState);

                if (player.HasBoatSunkInLocation(location))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    cellString = "#";
                }
                else if (player.IsLocationLocked(location, eBoatsCanTouch))
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    cellString = "*";
                }
                //Modify CellState presentation based on ship locations and game rules
                else if (!boatsHidden && (hasBoatInLocation || hasPlacementBoatInLocation) &&
                         eCellState == ECellState.Empty)
                {
                    Boat possibleBoat = hasBoatInLocation
                        ? player.Boats.First(boat => boat.GetCellLocations().Contains(location))
                        : player.GetBoatBeingPlaced();
                    Console.ForegroundColor = hasBoatInLocation ? ShipColour : ConsoleColor.Yellow;
                    var index = possibleBoat.GetCellLocations().IndexOf(location);
                    cellString = index == 0 ? "<" : index != possibleBoat.GetCellLocations().Count() - 1 ? "S" : ">";
                }


                Console.Write(cellString);
                Console.ForegroundColor = DefaultForegroundColor;
                Console.BackgroundColor = DefaultBackgroundColor;
                Console.Write(" |");
                if (location.x != width - 1) Console.Write(" ");
            }
        }


        private static string CellString(ECellState cellState)
        {
            switch (cellState)
            {
                case ECellState.Empty: return " ";
                case ECellState.Picked: return "^";
                case ECellState.Miss: return "O";
                case ECellState.Hit: return "X";
            }

            return "-";
        }


        private static void WriteBattlefieldRowDividerLine(int width, int height)
        {
            StringBuilder result = GetTableNumberEmptyStringBuilder(height);
            Console.ForegroundColor = DefaultForegroundColor;
            for (var colIndex = 0; colIndex < width; colIndex++)
            {
                if (colIndex == 0) result.Append("+");
                result.Append("---");
                if (colIndex == width - 1)
                    result.Append("+");
                else
                    result.Append("+");
            }

            Console.Write(result.ToString());
        }


        private static void WriteTableLetters(int width, int height)
        {
            char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            StringBuilder result = new StringBuilder().Append(GetTableNumberEmptyStringBuilder(height)).Append(" ");
            if (width <= alphabet.Length)
                for (var i = 0; i < width; i++)
                    result.Append(" ").Append(alphabet[i]).Append("  ");
            else
                for (var i = 0; i < width; i++)
                    result.Append(alphabet[i / alphabet.Length]).Append(" ").Append(alphabet[i % alphabet.Length])
                        .Append(" ");

            Console.Write(result.ToString());
        }

        private static void WriteTableNumber(int number, int maxNumber)
        {
            number++;
            maxNumber++;
            var maxLog = (int) Math.Floor(Math.Log10(maxNumber));
            var log = (int) Math.Floor(Math.Log10(number));
            Console.Write(new string('0', maxLog - log) + number + " ");
        }
    }
}