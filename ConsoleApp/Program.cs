using System;
using System.Collections.Generic;
using DAL;
using Domain;
using Domain.Enums;
using GameBrain;
using GameConsoleUI;
using MenuSystem;
using Microsoft.EntityFrameworkCore;
using DefaultBoat = Domain.DefaultBoat;

namespace ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var dbOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(@"
                        Server=barrel.itcollege.ee,1533;
                        User id=student;
                        Password=Student.Bad.password.0;
                        MultipleActiveResultSets=true;
                        Database=kaalte_BattleshipDB");
            using var dbContext = new ApplicationDbContext(dbOptionsBuilder.Options);
            //Create db migration: dotnet ef migrations add InitialMigration --project DAL --startup-project ConsoleApp
            //Remove db migration: ef migrations remove
            //Make the db by running the ConsoleApp

            var runOldDataBase = true;
            if (runOldDataBase)
            {
                dbContext.Database.Migrate();
            }
            else
            {
                Console.WriteLine("Deleting Database");
                dbContext.Database.EnsureDeleted();
                Console.WriteLine("Creating Database");
                dbContext.Database.Migrate();
                Console.WriteLine("Inserting data");


                //Create default GameOption
                var BoatsCanTouch = new GameOption
                {
                    BoardHeight = 10,
                    BoardWidth = 10,
                    EBoatsCanTouch = EBoatsCanTouch.Yes,
                    ENextMoveAfterHit = ENextMoveAfterHit.SamePlayer,
                    GameOptionBoats = null,
                    Name = "Boats Can Touch"
                };
                var CornersCantTouch = new GameOption
                {
                    BoardHeight = 10,
                    BoardWidth = 15,
                    EBoatsCanTouch = EBoatsCanTouch.Corner,
                    ENextMoveAfterHit = ENextMoveAfterHit.SamePlayer,
                    GameOptionBoats = null,
                    Name = "Corners Can't Touch"
                };
                var BoatsCantTouch = new GameOption
                {
                    BoardHeight = 10,
                    BoardWidth = 15,
                    EBoatsCanTouch = EBoatsCanTouch.No,
                    ENextMoveAfterHit = ENextMoveAfterHit.SamePlayer,
                    GameOptionBoats = null,
                    Name = "Boats Can't Touch"
                };
                var QuickGame = new GameOption
                {
                    BoardHeight = 5,
                    BoardWidth = 5,
                    EBoatsCanTouch = EBoatsCanTouch.Yes,
                    ENextMoveAfterHit = ENextMoveAfterHit.SamePlayer,
                    GameOptionBoats = null,
                    Name = "Quick Game"
                };
                dbContext.GameOptions.Add(BoatsCanTouch);
                dbContext.GameOptions.Add(CornersCantTouch);
                dbContext.GameOptions.Add(BoatsCantTouch);
                dbContext.GameOptions.Add(QuickGame);
                dbContext.SaveChanges();
                // Default Boats 
                /*
                Carrier     5
                Battleship	4
                Submarine	3
                Cruiser	    2
                Patrol	    1
                */
                DefaultBoat patrol = new()
                {
                    Length = 1,
                    Name = "Patrol"
                };

                DefaultBoat cruiser = new()
                {
                    Length = 2,
                    Name = "Cruiser"
                };

                DefaultBoat submarine = new()
                {
                    Length = 3,
                    Name = "Submarine"
                };

                DefaultBoat battleship = new()
                {
                    Length = 4,
                    Name = "Battleship"
                };

                DefaultBoat carrier = new()
                {
                    Length = 5,
                    Name = "Carrier"
                };


                dbContext.DefaultBoats.AddRange(new List<DefaultBoat>
                    {carrier, battleship, submarine, cruiser, patrol});
                dbContext.SaveChanges();

                List<GameOption> gameOptions = new() {BoatsCanTouch, CornersCantTouch, BoatsCantTouch};
                foreach (GameOption gameOption in gameOptions)
                {
                    var gameOptionBoatCarrier = new GameOptionBoat
                    {
                        Amount = 1,
                        DefaultBoat = carrier,
                        GameOption = gameOption
                    };

                    var gameOptionBoatBattleship = new GameOptionBoat
                    {
                        Amount = 1,
                        DefaultBoat = battleship,
                        GameOption = gameOption
                    };

                    var gameOptionBoatSubmarine = new GameOptionBoat
                    {
                        Amount = 1,
                        DefaultBoat = submarine,
                        GameOption = gameOption
                    };

                    var gameOptionBoatCruiser = new GameOptionBoat
                    {
                        Amount = 1,
                        DefaultBoat = cruiser,
                        GameOption = gameOption
                    };

                    var gameOptionBoatPatrol = new GameOptionBoat
                    {
                        Amount = 1,
                        DefaultBoat = patrol,
                        GameOption = gameOption
                    };
                    dbContext.GameOptionBoats.AddRange(new List<GameOptionBoat>
                    {
                        gameOptionBoatCarrier, gameOptionBoatBattleship, gameOptionBoatSubmarine,
                        gameOptionBoatCruiser, gameOptionBoatPatrol
                    });
                }

                dbContext.GameOptionBoats.AddRange(new List<GameOptionBoat>
                {
                    new()
                    {
                        Amount = 2,
                        DefaultBoat = patrol,
                        GameOption = QuickGame
                    },
                    new()
                    {
                        Amount = 1,
                        DefaultBoat = cruiser,
                        GameOption = QuickGame
                    }
                });

                dbContext.SaveChanges();
            }

            GameMenuPages gameMenuPages = new(new BattleShip(dbContext));


            var notImplementedMenu = new Menu(MenuLevel.Level2Plus, "!!! NOT IMPLEMENTED MENU !!!");

            var GameOptionsManagerMenu = new Menu(MenuLevel.Level1, "Game Options Manager");
            GameOptionsManagerMenu.AddMenuItem(new MenuItem("Add new Game Option", gameMenuPages.AddToGameOptions));
            GameOptionsManagerMenu.AddMenuItem(new MenuItem("Delete Game Option ",
                gameMenuPages.DeleteFromGameOptions));

            var playMenu = new Menu(MenuLevel.Level1, "Choose game");
            playMenu.AddMenuItem(new MenuItem("Choose Prebuilt Game Options", gameMenuPages.RunGameFromOptions));
            playMenu.AddMenuItem(new MenuItem("Make a game with own Game Options", gameMenuPages.RunGameNew));
            playMenu.AddMenuItem(new MenuItem("Load Game", gameMenuPages.LoadGame));
            playMenu.AddMenuItem(new MenuItem("Delete Game", gameMenuPages.DeleteGame));

            var mainMenu = new Menu(MenuLevel.Level0, "Battleship");
            mainMenu.AddMenuItem(new MenuItem("Play", playMenu.RunMenu));
            mainMenu.AddMenuItem(new MenuItem("Manage Game Options", GameOptionsManagerMenu.RunMenu));
            mainMenu.AddMenuItem(new MenuItem("Tutorial", GameMenuPages.RunTutorial));

            mainMenu.RunMenu();
        }
    }
}