using System;
using System.Collections.Generic;

namespace Domain
{
    public class Game
    {
        public bool GameOver { get; set; } = false;

        public string Name { get; set; } = default!;

        public int BoardTurnId { get; set; }

        public int GameId { get; set; }

        public int? GameOptionId { get; set; }
        public GameOption? GameOption { get; set; }


        public ICollection<TurnSave>? TurnSaves { get; set; }

        public ICollection<GameBoard>? GameBoards { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public DateTime? DeletedDateTime { get; set; }
    }
}