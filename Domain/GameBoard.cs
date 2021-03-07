using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class GameBoard
    {
        [MaxLength(64)] public string PlayerName { get; set; } = "Guest";

        public int GameBoardId { get; set; }

        public ICollection<GameBoat>? GameBoats { get; set; }

        [ForeignKey(nameof(TurnSave))] public int? AttackerId { get; set; }

        public TurnSave? Attacker { get; set; }

        [ForeignKey(nameof(TurnSave))] public int? DefenderId { get; set; }
        public TurnSave? Defender { get; set; }

        public string BoardJson { get; set; } = default!;

        public int GameId { get; set; }
        public Game Game { get; set; } = default!;

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public DateTime? DeletedDateTime { get; set; }
    }
}