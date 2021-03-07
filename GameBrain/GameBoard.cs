using System;
using System.Collections.Generic;
using System.Data;
using Domain.Enums;

namespace GameBrain
{
    public class GameBoard
    {
        public GameBoard(int width, int height, ECellState[,] cellStates, Player player)
        {
            Board = cellStates;
            Player = player;
            Height = height;
            Width = width;
        }

        public ECellState[,] Board { get; }
        public int Width { get; }

        public Player Player { get; set; }
        public int Height { get; }

        public int GameBoardId { get; set; }


        public ECellState GetCellState((int x, int y) location)
        {
            return Board[location.x, location.y];
        }

        public void SetCellState((int x, int y) location, ECellState cellState)
        {
            Board[location.x, location.y] = cellState;
        }


        public bool PlacementBoatHasSuitableCorners((int X, int Y) startingLocation)
        {
            var tempStartingLocation = (startingLocation.X, startingLocation.Y);
            Boat expectedBoat = new(Player.GetBoatBeingPlaced().GetName())
            {
                Length = Player.GetBoatBeingPlaced().GetLength(),
                CellLocations = new List<(int x, int y)>()
            };
            (int X, int Y) facing = Player.GetBoatBeingPlaced().GetFacingDirection();
            for (var i = 0; i < expectedBoat.GetLength(); i++)
            {
                int x;
                int y;

                (x, y) = (tempStartingLocation.X + i * facing.X, tempStartingLocation.Y + i * facing.Y);
                expectedBoat.GetCellLocations().Add((x, y));
                if (i == 0)
                {
                    if (facing.X == 1 && (Player.HasBoatInLocation((x - 1, y + 1))
                                          || Player.HasBoatInLocation((x - 1, y - 1)))) return false;

                    if (facing.Y == 1 && (Player.HasBoatInLocation((x - 1, y - 1)) ||
                                          Player.HasBoatInLocation((x + 1, y - 1)))) return false;
                }

                if (i == expectedBoat.GetLength() - 1)
                {
                    if (facing.X == 1 && (Player.HasBoatInLocation((x + 1, y + 1))
                                          || Player.HasBoatInLocation((x + 1, y - 1)))) return false;

                    if (facing.Y == 1 && (Player.HasBoatInLocation((x - 1, y + 1)) ||
                                          Player.HasBoatInLocation((x + 1, y + 1)))) return false;
                }
            }

            foreach (Boat playerBoat in Player.Boats)
            {
                facing = playerBoat.GetFacingDirection();
                for (var i = 0; i < playerBoat.GetLength(); i++)
                {
                    int x;
                    int y;
                    (x, y) = playerBoat.GetCellLocations()[i];
                    if (i == 0)
                    {
                        if (facing.X == 1 && (expectedBoat.GetCellLocations().Contains((x - 1, y + 1))
                                              || expectedBoat.GetCellLocations().Contains((x - 1, y - 1))))
                            return false;

                        if (facing.Y == 1 && (expectedBoat.GetCellLocations().Contains((x - 1, y - 1)) ||
                                              expectedBoat.GetCellLocations().Contains((x + 1, y - 1)))) return false;
                    }

                    if (i == playerBoat.GetLength() - 1)
                    {
                        if (facing.X == 1 && (expectedBoat.GetCellLocations().Contains((x + 1, y + 1))
                                              || expectedBoat.GetCellLocations().Contains((x + 1, y - 1))))
                            return false;

                        if (facing.Y == 1 && (expectedBoat.GetCellLocations().Contains((x - 1, y + 1)) ||
                                              expectedBoat.GetCellLocations().Contains((x + 1, y + 1)))) return false;
                    }
                }
            }

            return true;
        }

        public (int x, int y) GetFirstEmptyCellInDirection((int X, int Y) location, (int X, int Y) moveDirection,
            EBoatsCanTouch eBoatsCanTouch)
        {
            location.Y += moveDirection.Y;
            if (location.Y > Height - 1)
                location.Y = 0;
            else if (location.Y < 0) location.Y = Height - 1;

            location.X += moveDirection.X;
            if (location.X < 0) location.X = Width - 1;
            if (location.X > Width - 1) location.X = 0;


            return GetCellState((location.X, location.Y)) == ECellState.Empty &&
                   !Player.IsLocationLocked(location, eBoatsCanTouch)
                ? (location.X, location.Y)
                : GetFirstEmptyCellInDirection(location, moveDirection, eBoatsCanTouch);
        }

        public (int x, int y) GetFirstEmptyCell(EBoatsCanTouch eBoatsCanTouch)
        {
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                if (Board[x, y] == ECellState.Empty)
                {
                    if (Player.IsLocationLocked((x, y), eBoatsCanTouch)) continue;
                    return (x, y);
                }

            return (0, 0);
        }

        public (int x, int y) GetFirstEmptyBoatCells(EBoatsCanTouch eBoatsCanTouch)
        {
            (int X, int Y) facingDirection = Player.GetBoatBeingPlaced().GetFacingDirection();
            for (var x = 0; x < Width - facingDirection.X * Player.GetBoatBeingPlaced().GetLength(); x++)
            for (var y = 0; y < Height - facingDirection.Y * Player.GetBoatBeingPlaced().GetLength(); y++)
            {
                //check for edges
                if (eBoatsCanTouch == EBoatsCanTouch.No)
                {
                    if (!PlacementBoatHasSuitableEdges((x, y)) && !Player.LocationHasHitInTheCorner((x, y))) continue;
                    return (x, y);
                }

                if (Player.HasBoatInLocation((x, y))) continue;
                for (var i = 0; i < Player.GetBoatBeingPlaced().GetLength(); i++)
                {
                    var checkX = x + facingDirection.X * i;
                    var checkY = y + facingDirection.Y * i;

                    if (Player.HasBoatInLocation((checkX, checkY))) break;


                    if (i == Player.GetBoatBeingPlaced().GetLength() - 1)
                    {
                        //check for corners
                        if (eBoatsCanTouch == EBoatsCanTouch.Corner &&
                            !PlacementBoatHasSuitableCorners((x, y))) break;
                        return (x, y);
                    }
                }
            }

            throw new EvaluateException("No room to place a boat");
        }


        public (int x, int y) GetFirstEmptyBoatCellInDirection((int X, int Y) moveDirection,
            EBoatsCanTouch touchRule)
        {
            Boat boat = Player.GetBoatBeingPlaced();
            (int X, int Y) boatStartingLocation = boat.GetCellLocations()[0];
            var hasLoopedAround = false;
            (int X, int Y) boatRootLocation = boat.GetCellLocations()[0];
            (int X, int Y) facingDirection = boat.GetFacingDirection();


            while (true)
            {
                var freeCellsCount = 0;
                var x = (boatRootLocation.X + moveDirection.X) % Player.PlayerBoard.Width;
                var y = (boatRootLocation.Y + moveDirection.Y) % Player.PlayerBoard.Height;
                if (x < 0) x = Player.PlayerBoard.Width + x;
                if (y < 0) y = Player.PlayerBoard.Height + y;
                boatRootLocation = (x, y);

                //check for edges
                if (touchRule == EBoatsCanTouch.No)
                {
                    if (!PlacementBoatHasSuitableEdges((x, y))) continue;
                    return boatRootLocation;
                }

                for (var i = 0; i < boat.GetLength(); i++)
                {
                    var tempX = boatRootLocation.X + facingDirection.X * i;
                    var tempY = boatRootLocation.Y + facingDirection.Y * i;


                    //Collision detection
                    if (Player.HasBoatInLocation((tempX, tempY)) ||
                        tempX < 0 || tempX >= Player.PlayerBoard.Width ||
                        tempY < 0 || tempY >= Player.PlayerBoard.Height)
                        break;

                    freeCellsCount++;
                }

                if (freeCellsCount == boat.GetLength())
                {
                    //check for corners
                    if (touchRule == EBoatsCanTouch.Corner)
                        if (!PlacementBoatHasSuitableCorners((x, y)))
                            continue;


                    return boatRootLocation;
                }

                if (hasLoopedAround) throw new ArithmeticException("Couldn't find location for boat!");
                if (boatRootLocation.X == boatStartingLocation.X && boatRootLocation.Y == boatStartingLocation.Y)
                    hasLoopedAround = true;
            }

            throw new ApplicationException("Unable to find an location for boat");
        }

        public bool PlacementBoatHasSuitableEdges((int x, int y) startingLocation)
        {
            Boat boat = Player.GetBoatBeingPlaced();
            var facingDirection = boat.GetFacingDirection();
            for (var i = 0; i < boat.GetLength(); i++)
            {
                var (x, y) = (startingLocation.x + facingDirection.x * i,
                    startingLocation.y + facingDirection.y * i);
                if (i == 0)
                    for (var j = -1; j < 2; j++)
                    {
                        (int x, int y) checkingLocation = facingDirection.x == 1 ? (x + -1, y + j) : (x + j, y + -1);
                        if (Player.HasBoatInLocation(checkingLocation)) return false;
                    }

                for (var j = -1; j < 2; j++)
                {
                    (int x, int y) checkingLocation = facingDirection.x == 1 ? (x + 0, y + j) : (x + j, y + 0);
                    if (Player.HasBoatInLocation(checkingLocation) || x < 0 || y < 0 || x >= Player.PlayerBoard.Width ||
                        y >= Player.PlayerBoard.Height) return false;
                }

                if (i == boat.GetLength() - 1)
                    for (var j = -1; j < 2; j++)
                    {
                        (int x, int y) checkingLocation = facingDirection.x == 1 ? (x + 1, y + j) : (x + j, y + 1);
                        if (Player.HasBoatInLocation(checkingLocation)) return false;
                    }
            }

            return true;
        }
    }
}