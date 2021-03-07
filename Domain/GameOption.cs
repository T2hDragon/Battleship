using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain
{
    public class GameOption
    {
        [MaxLength(128)] public string Name { get; set; } = default!;

        public int BoardWidth { get; set; } = 10;
        public int BoardHeight { get; set; } = 10;


        public int GameOptionId { get; set; }

        public EBoatsCanTouch EBoatsCanTouch { get; set; } = EBoatsCanTouch.Yes;

        public ENextMoveAfterHit ENextMoveAfterHit { get; set; } = ENextMoveAfterHit.SamePlayer;

        public ICollection<GameOptionBoat>? GameOptionBoats { get; set; }

        public ICollection<Game>? Games { get; set; }


        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public DateTime? DeletedDateTime { get; set; }

        public void Delete()
        {
            DeletedDateTime = DateTime.Now;
        }
    }
}