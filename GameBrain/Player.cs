using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics.Tracing;
using System.Linq;
using Domain.Enums;

namespace GameBrain
{
    public class Player
    {
        public Player(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public GameBoard PlayerBoard { get; set; } = null!;

        public List<Boat> Boats { get; set; } = new();


        public Boat? BoatBeingPlaced { get; set; }

        public List<Boat> NotPlacedBoats { get; set; } = new();

        public Boat GetBoatBeingPlaced()
        {
            return BoatBeingPlaced ?? throw new EventSourceException("There is no boat being placed!");
        }

        public void SetNextPlacementBoat()
        {
            BoatBeingPlaced = PopUnplacedBoat();
            BoatBeingPlaced.UnPlaceBoat();
        }

        public bool HasBoatsToBePlaced()
        {
            return BoatBeingPlaced != null || NotPlacedBoats.Count != 0;
        }

        public void MovePlacementBoat((int x, int y) moveDirection, EBoatsCanTouch eBoatsCanTouch)
        {
            GetBoatBeingPlaced().PlaceBoat(PlayerBoard.GetFirstEmptyBoatCellInDirection(moveDirection, eBoatsCanTouch),
                GetBoatBeingPlaced().GetFacingDirection()
            );
        }

        public void PlaceRemainingBoats(EBoatsCanTouch eBoatsCanTouch)
        {
            Random r = new();
            if (BoatBeingPlaced == null) SetNextPlacementBoat();
            while (!(NotPlacedBoats.Count == 0 && BoatBeingPlaced == null))
            {
                var location = ((int x, int y)) (r.NextDouble() * (PlayerBoard.Width - 1),
                    r.NextDouble() * (PlayerBoard.Height - 1));
                var turnBoat = r.NextDouble() > 0.5;
                GetBoatBeingPlaced().PlaceBoat(location, turnBoat ? (1, 0) : (0, 1));
                var validBoatPlacement = ValidateBoatPlacement(eBoatsCanTouch);
                if (validBoatPlacement)
                {
                    TransferPlacementBoat();
                    if (NotPlacedBoats.Count != 0) SetNextPlacementBoat();
                }
            }
        }


        public bool ValidateBoatPlacement(EBoatsCanTouch eBoatsCanTouch)
        {
            foreach (var cellLocation in GetBoatBeingPlaced().GetCellLocations().ToList())
            {
                if (!IsLocationOnBoard(cellLocation) || HasBoatInLocation(cellLocation)) return false;

                if (eBoatsCanTouch == EBoatsCanTouch.Corner)
                    if (!PlayerBoard.PlacementBoatHasSuitableCorners(cellLocation))
                        return false;

                if (eBoatsCanTouch == EBoatsCanTouch.No)
                    if (!PlayerBoard.PlacementBoatHasSuitableEdges(cellLocation) &&
                        !LocationHasHitInTheCorner(cellLocation))
                        return false;
            }

            return true;
        }

        private bool IsLocationOnBoard((int x, int y) location)
        {
            return location.x >= 0 && location.y >= 0 && location.x < PlayerBoard.Width &&
                   location.y < PlayerBoard.Height;
        }

        private Boat PopUnplacedBoat()
        {
            if (NotPlacedBoats.Count == 0) throw new ApplicationException("No more NotPlacedBoats");

            Boat result = NotPlacedBoats[0];
            NotPlacedBoats.RemoveAt(0);
            return result;
        }


        public void TransferPlacementBoat()
        {
            if (BoatBeingPlaced != null) Boats.Add(BoatBeingPlaced);

            else throw new EvaluateException("No boat to transfer");
            BoatBeingPlaced = null;
        }


        public void AddUnplacedBoat(string name, int length)
        {
            NotPlacedBoats.Add(new Boat(name) {Length = length});
        }

        public bool HasBoatInLocation((int x, int y) location)
        {
            return Boats.Any(boat => boat.GetCellLocations().Any(cell => cell == location));
        }

        public bool HasBeenDefeated()
        {
            return GetRemainingBoatsCount() == 0;
        }


        public void RotateBoatBeingPlaced(EBoatsCanTouch eBoatsCanTouch)
        {
            GetBoatBeingPlaced().Rotate();
            var facingDirection = GetBoatBeingPlaced().GetFacingDirection();
            while (true)
                try
                {
                    GetBoatBeingPlaced().PlaceBoat(PlayerBoard.GetFirstEmptyBoatCellInDirection((1, 0), eBoatsCanTouch),
                        GetBoatBeingPlaced().GetFacingDirection()
                    );
                    break;
                }
                catch (ArithmeticException e1)
                {
                    try
                    {
                        GetBoatBeingPlaced()
                            .PlaceBoat(
                                (GetBoatBeingPlaced().GetCellLocations()[0].x + 1,
                                    GetBoatBeingPlaced().GetCellLocations()[0].y), facingDirection);
                        GetBoatBeingPlaced().PlaceBoat(
                            PlayerBoard.GetFirstEmptyBoatCellInDirection((1, 0), eBoatsCanTouch),
                            GetBoatBeingPlaced().GetFacingDirection()
                        );
                        break;
                    }
                    catch (ArithmeticException e2)
                    {
                        GetBoatBeingPlaced()
                            .PlaceBoat(
                                (GetBoatBeingPlaced().GetCellLocations()[0].x,
                                    GetBoatBeingPlaced().GetCellLocations()[0].y + 1), facingDirection);
                    }
                }
        }

        public int GetRemainingBoatsCount()
        {
            return Boats.Count(boat => boat.GetCellLocations().Any(cellLocation =>
                PlayerBoard.Board[cellLocation.x, cellLocation.y] == ECellState.Empty));
        }

        public bool LocationHasHitInTheCorner((int x, int y) location)
        {
            for (var xMove = -1; xMove < 2; xMove += 2)
            for (var yMove = -1; yMove < 2; yMove += 2)
            {
                if (xMove + location.x < 0 || yMove + location.y < 0 || xMove + location.x >= PlayerBoard.Width ||
                    yMove + location.y >= PlayerBoard.Height) continue;
                if (PlayerBoard.Board[xMove + location.x, yMove + location.y] == ECellState.Hit) return true;
            }

            return false;
        }


        private Boat GetBoatInLocation((int x, int y) location)
        {
            foreach (Boat boat in Boats)
                if (boat.GetCellLocations().Contains(location))
                    return boat;

            throw new CheckoutException("No boats in location!");
        }

        public bool HasBoatSunkInLocation((int x, int y) location)
        {
            if (!HasBoatInLocation(location)) return false;
            Boat boatInLocation = GetBoatInLocation(location);
            return boatInLocation.GetCellLocations().All(cl => PlayerBoard.Board[cl.x, cl.y] == ECellState.Hit);
        }

        private bool LocationInSunkBoatCorner((int X, int Y) location)
        {
            for (var xMove = -1; xMove < 2; xMove += 2)
            for (var yMove = -1; yMove < 2; yMove += 2)
                if (HasBoatSunkInLocation((location.X + xMove, location.Y + yMove)))
                {
                    Boat boat = GetBoatInLocation((location.X + xMove, location.Y + yMove));
                    return !boat.GetBoatEdges().Contains(location);
                }

            return false;
        }

        private bool HasBoatSunkInLocationSides((int X, int Y) location)
        {
            List<(int x, int y)> sides = new() {(0, -1), (1, 0), (0, 1), (-1, 0)};

            foreach (var side in sides)
            {
                if (!(side.x + location.X >= 0 && side.y + location.Y >= 0 && side.x + location.X < PlayerBoard.Width &&
                      side.y + location.Y < PlayerBoard.Height)) continue;
                if (HasBoatSunkInLocation((location.X + side.x, location.Y + side.y))) return true;
            }

            return false;
        }


        public bool IsLocationLocked((int X, int Y) location, EBoatsCanTouch eBoatsCanTouch)
        {
            if (eBoatsCanTouch == EBoatsCanTouch.Yes) return false;
            if (eBoatsCanTouch == EBoatsCanTouch.Corner) return LocationInSunkBoatCorner(location);
            if (eBoatsCanTouch == EBoatsCanTouch.No)
                return LocationHasHitInTheCorner(location) || HasBoatSunkInLocationSides(location);
            throw new ConstraintException("Unknown EBoatsCanTouch enum");
        }

        public void MakeNewBoard(int getWidth, int getHeight, ECellState[,] cellStates)
        {
            PlayerBoard = new GameBoard(getWidth, getHeight, cellStates, this);
        }
    }
}