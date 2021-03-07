using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class DefaultBoat
    {
        [Range(1, int.MaxValue)] public int Length { get; set; }

        [MaxLength(32)] public string Name { get; set; } = default!;

        public int DefaultBoatId { get; set; }

        public ICollection<GameOptionBoat>? GameOptionBoats { get; set; }

        public DateTime CreatedDateTime { get; set; } = DateTime.Now;

        public DateTime? DeletedDateTime { get; set; }

        public void Delete()
        {
            DeletedDateTime = DateTime.Now;
        }
    }
}