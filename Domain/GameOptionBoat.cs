using System;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class GameOptionBoat
    {
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public DateTime? DeletedDateTime { get; set; }

        [Range(1, int.MaxValue)] public int Amount { get; set; }

        public int GameOptionBoatId { get; set; }


        public int DefaultBoatId { get; set; }
        public DefaultBoat DefaultBoat { get; set; } = default!;

        public int GameOptionId { get; set; }
        public GameOption GameOption { get; set; } = default!;
    }
}