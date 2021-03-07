using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class TurnSave
    {
        public int CellX { get; set; }

        public int CellY { get; set; }

        public int TurnSaveId { get; set; }


        [ForeignKey(nameof(GameBoard))] public int AttackerId { get; set; }

        public GameBoard Attacker { get; set; } = default!;


        [ForeignKey(nameof(GameBoard))] public int DefenderId { get; set; }

        public GameBoard Defender { get; set; } = default!;


        public int GameId { get; set; }
        public Game Game { get; set; } = default!;

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public DateTime? DeletedDateTime { get; set; }
    }
}