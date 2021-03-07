using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameBoat
    {
        [Range(1, 128)] public string Name { get; set; } = default!;

        public int Length { get; set; }

        public int? FacingX { get; set; }
        public int? FacingY { get; set; }

        public int? LocationX { get; set; }
        public int? LocationY { get; set; }

        public int GameBoatId { get; set; }

        public int GameBoardId { get; set; }
        public GameBoard GameBoard { get; set; } = default!;

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public DateTime? DeletedDateTime { get; set; }
    }
}