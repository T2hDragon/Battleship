using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;

namespace GameBrain
{
    public class Boat
    {
        public Boat(string name)
        {
            Name = name;
        }

        private string Name { get; }
        public int Length { get; set; }

        public int GameBoatId { get; set; }

        public List<(int x, int y)>? CellLocations { get; set; }

        public void PlaceBoat((int X, int Y) startingLocation, (int X, int Y) facingDirection)
        {
            CellLocations = new List<(int x, int y)>();
            for (var i = 0; i < Length; i++)
                CellLocations.Add((startingLocation.X + i * facingDirection.X,
                    startingLocation.Y + i * facingDirection.Y));
        }

        public void UnPlaceBoat()
        {
            CellLocations = null;
        }

        public int GetLength()
        {
            return Length;
        }


        public string GetName()
        {
            return Name;
        }


        public List<(int x, int y)> GetCellLocations()
        {
            return CellLocations ?? throw new AuthenticationException("Ship does not have cell locations");
        }

        public (int x, int y) GetFacingDirection()
        {
            List<int> xCellLocations = GetCellLocations().Select(cl => cl.x).ToList();
            List<int> yCellLocations = GetCellLocations().Select(cl => cl.y).ToList();
            (int x, int y) result = (xCellLocations.Any(el => el != xCellLocations[0]) ? 1 : 0,
                yCellLocations.Any(el => el != yCellLocations[0]) ? 1 : 0);
            if (result.x == result.y) result = (1, 1);
            return result;
        }

        public void Rotate()
        {
            switch (GetFacingDirection())
            {
                case (0, 1):
                    for (var i = 0; i < Length; i++)
                        GetCellLocations()[i] = (GetCellLocations()[i].x + i, GetCellLocations()[0].y);

                    break;
                case (1, 0):
                    for (var i = 0; i < Length; i++)
                        GetCellLocations()[i] = (GetCellLocations()[0].x, GetCellLocations()[i].y + i);
                    break;
            }
        }

        public List<(int x, int y)> GetBoatEdges()
        {
            List<(int x, int y)> sides = new() {(0, -1), (1, 0), (0, 1), (-1, 0)};

            List<(int x, int y)> result = new();

            foreach (var cellLocation in GetCellLocations())
            foreach (var side in sides)
                result.Add((cellLocation.x + side.x, cellLocation.y + side.y));

            return result;
        }
    }
}