using System.Linq;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; } = null!;

        public DbSet<TurnSave> TurnSaves { get; set; } = null!;

        public DbSet<GameBoat> GameBoats { get; set; } = null!;

        public DbSet<GameBoard> GameBoards { get; set; } = null!;
        public DbSet<GameOption> GameOptions { get; set; } = null!;
        public DbSet<GameOptionBoat> GameOptionBoats { get; set; } = null!;
        public DbSet<DefaultBoat> DefaultBoats { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            foreach (var relationship in modelBuilder.Model
                .GetEntityTypes()
                .Where(e => !e.IsOwned())
                .SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;


            modelBuilder
                .Entity<TurnSave>()
                .HasOne<GameBoard>()
                .WithOne(x => x.Attacker!)
                .HasForeignKey<GameBoard>(x => x.AttackerId);

            modelBuilder
                .Entity<TurnSave>()
                .HasOne<GameBoard>()
                .WithOne(x => x.Defender!)
                .HasForeignKey<GameBoard>(x => x.DefenderId);

            modelBuilder
                .Entity<GameBoard>()
                .HasOne<TurnSave>()
                .WithOne(x => x.Attacker)
                .HasForeignKey<TurnSave>(x => x.AttackerId);

            modelBuilder
                .Entity<GameBoard>()
                .HasOne<TurnSave>()
                .WithOne(x => x.Defender)
                .HasForeignKey<TurnSave>(x => x.DefenderId);


            // Restrict Attacker upon GameBoard delete
            modelBuilder.Entity<GameBoard>().HasOne(game => game.Attacker).WithOne(turnSave => turnSave!.Attacker)
                .OnDelete(DeleteBehavior.Restrict);

            // Remove Attacker upon GameBoard delete
            modelBuilder.Entity<GameBoard>().HasOne(game => game.Defender).WithOne(turnSave => turnSave!.Defender)
                .OnDelete(DeleteBehavior.Restrict);


            // On delete Cascades

            // Remove TurnSave upon Game delete
            modelBuilder.Entity<Game>().HasMany(game => game.TurnSaves).WithOne(turnSave => turnSave.Game)
                .OnDelete(DeleteBehavior.Cascade);


            // Remove GameOptionBoat upon GameOption delete
            modelBuilder.Entity<GameOption>().HasMany(gameOption => gameOption.GameOptionBoats)
                .WithOne(gameOptionBoat => gameOptionBoat.GameOption)
                .OnDelete(DeleteBehavior.Cascade);

            // Remove GameOptionBoat upon DefaultBoat delete
            modelBuilder.Entity<DefaultBoat>().HasMany(defaultBoat => defaultBoat.GameOptionBoats)
                .WithOne(gameOptionBoat => gameOptionBoat.DefaultBoat)
                .OnDelete(DeleteBehavior.Cascade);

            // Remove GameOption upon Game delete
            modelBuilder.Entity<Game>().HasOne(game => game.GameOption).WithMany(gameOption => gameOption!.Games)
                .OnDelete(DeleteBehavior.Cascade);

            // Remove GameBoard upon Game delete
            modelBuilder.Entity<Game>().HasMany(game => game.GameBoards).WithOne(gameBoard => gameBoard!.Game)
                .OnDelete(DeleteBehavior.Cascade);

            // Remove GameBoard upon Game delete
            modelBuilder.Entity<GameBoard>().HasMany(gameBoard => gameBoard.GameBoats)
                .WithOne(gameBoat => gameBoat.GameBoard)
                .OnDelete(DeleteBehavior.Cascade);

            ;
        }
    }
}