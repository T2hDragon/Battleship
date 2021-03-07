using System;
using System.Collections.Generic;
using System.Data;
using Domain.Enums;
using GameBrain;

namespace GameConsoleUI
{
    public class PlayerTurn
    {
        public static (int x, int y) ChooseBombLocation(Player activePlayer, Player opponentPlayer,
            EBoatsCanTouch eBoatsCanTouch)
        {
            ConsoleKeyInfo key;
            GameBoard opponentPlayerBoard = opponentPlayer.PlayerBoard;
            var opponentPosition = opponentPlayerBoard.GetFirstEmptyCell(eBoatsCanTouch);
            do
            {
                var tempState = opponentPlayerBoard.GetCellState(opponentPosition);
                opponentPlayerBoard.SetCellState(opponentPosition, ECellState.Picked);
                Console.Clear();
                BattleshipUI.DrawPlayerBoard(activePlayer, ConsoleColor.White, false, eBoatsCanTouch);
                BattleshipUI.DrawPlayerBoard(opponentPlayer, ConsoleColor.DarkBlue, true, eBoatsCanTouch);
                key = Console.ReadKey(true);
                opponentPlayerBoard.SetCellState(opponentPosition, tempState);


                // Go through the battlefield
                // decrease/increase current item if key pressed is down/up
                // If curItem goes out of bounds, it loops around to the other end.
                if (key.Key.ToString() == "DownArrow")
                    opponentPosition =
                        opponentPlayerBoard.GetFirstEmptyCellInDirection(opponentPosition, (0, 1), eBoatsCanTouch);
                else if (key.Key.ToString() == "UpArrow")
                    opponentPosition =
                        opponentPlayerBoard.GetFirstEmptyCellInDirection(opponentPosition, (0, -1), eBoatsCanTouch);
                else if (key.Key.ToString() == "LeftArrow")
                    opponentPosition =
                        opponentPlayerBoard.GetFirstEmptyCellInDirection(opponentPosition, (-1, 0), eBoatsCanTouch);
                else if (key.Key.ToString() == "RightArrow")
                    opponentPosition =
                        opponentPlayerBoard.GetFirstEmptyCellInDirection(opponentPosition, (1, 0), eBoatsCanTouch);
                else if (key.Key.ToString() == "Spacebar") return (-3, -3);
                else if (key.Key.ToString() == "T") return (-2, -2);
                // Loop around until the user presses the enter go or escape.
            } while (key.KeyChar != 13);


            return opponentPosition;
        }


        public void SetPlacementBoatOnBoard(Player currentPlayer, EBoatsCanTouch eBoatsCanTouch)
        {
            ConsoleKeyInfo key;
            (int x, int y) playerPosition;
            (int x, int y) facingDirection = (1, 0);
            try
            {
                currentPlayer.GetBoatBeingPlaced().CellLocations = new List<(int x, int y)> {(0, 0), facingDirection};
                playerPosition = currentPlayer.PlayerBoard.GetFirstEmptyBoatCells(eBoatsCanTouch);
            }
            catch (EvaluateException)
            {
                facingDirection = (facingDirection.y, facingDirection.x);
                currentPlayer.GetBoatBeingPlaced().CellLocations = new List<(int x, int y)> {(0, 0), facingDirection};
                playerPosition = currentPlayer.PlayerBoard.GetFirstEmptyBoatCells(eBoatsCanTouch);
            }

            currentPlayer.GetBoatBeingPlaced().PlaceBoat(playerPosition, facingDirection);
            do
            {
                Console.Clear();
                BattleshipUI.DrawPlayerBoard(currentPlayer, ConsoleColor.DarkBlue, false, eBoatsCanTouch);
                key = Console.ReadKey(true);

                // Go through the battlefield
                // decrease/increase current item if key pressed is down/up
                // If curItem goes out of bounds, it loops around to the other end.
                if (key.Key.ToString() == "DownArrow")
                    currentPlayer.MovePlacementBoat((0, 1), eBoatsCanTouch);
                else if (key.Key.ToString() == "UpArrow")
                    currentPlayer.MovePlacementBoat((0, -1), eBoatsCanTouch);
                else if (key.Key.ToString() == "LeftArrow")
                    currentPlayer.MovePlacementBoat((-1, 0), eBoatsCanTouch);
                else if (key.Key.ToString() == "RightArrow")
                    currentPlayer.MovePlacementBoat((1, 0), eBoatsCanTouch);
                else if (key.Key.ToString() == "R") currentPlayer.RotateBoatBeingPlaced(eBoatsCanTouch);
                // Loop around until the user presses the enter go or escape.
            } while (key.KeyChar != 13);
        }

        public static Player? ChooseOpponent(Player player, List<Player> playerOpponents, EBoatsCanTouch eBoatsCanTouch)
        {
            if (playerOpponents.Count == 1) return playerOpponents[0];
            ConsoleKeyInfo key;
            var opponentIndex = 0;
            do
            {
                Console.Clear();

                BattleshipUI.DrawPlayerBoard(player, ConsoleColor.White, false, eBoatsCanTouch);
                for (var i = 0; i < playerOpponents.Count; i++)
                    if (opponentIndex == i)
                        BattleshipUI.DrawPlayerBoard(playerOpponents[i], ConsoleColor.Yellow, true, eBoatsCanTouch);
                    else BattleshipUI.DrawPlayerBoard(playerOpponents[i], ConsoleColor.DarkBlue, true, eBoatsCanTouch);

                key = Console.ReadKey(true);
                if (key.Key.ToString() == "DownArrow")
                {
                    opponentIndex++;
                    if (opponentIndex == playerOpponents.Count) opponentIndex = 0;
                }
                else if (key.Key.ToString() == "UpArrow")
                {
                    opponentIndex--;
                    if (opponentIndex == -1) opponentIndex = playerOpponents.Count - 1;
                }
                else if (key.Key.ToString() == "T")
                {
                    return null;
                }
            } while (key.KeyChar != 13);

            return playerOpponents[opponentIndex];
        }
    }
}