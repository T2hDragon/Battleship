using System;
using System.Collections.Generic;

namespace MenuSystem
{
    public enum MenuLevel
    {
        Level0,
        Level1,
        Level2Plus
    }


    public class Menu
    {
        private readonly MenuLevel _menuLevel;
        private readonly string _menuTitle;


        public Menu(MenuLevel level, string menuTitle)
        {
            _menuLevel = level;
            _menuTitle = menuTitle;
        }


        private Dictionary<int, MenuItem> MenuItems { get; } = new();


        public void AddMenuItem(MenuItem item)
        {
            MenuItems.Add(MenuItems.Count, item);
        }

        public int RunMenu() //needs to be of type Func<string>
        {
            Console.CursorVisible = false;
            // Our array of Items for the menu (in order)
            string[] menuItems = new string[MenuItems.Count + (int) _menuLevel + 1];
            var arrayIndex = 0;

            foreach (var menuItem in MenuItems)
            {
                menuItems[arrayIndex] = menuItem.Value.GetLabel();
                arrayIndex++;
            }

            switch (_menuLevel)
            {
                case MenuLevel.Level0:
                    menuItems[arrayIndex] = "eXit";
                    break;
                case MenuLevel.Level1:

                    menuItems[arrayIndex] = "Return to Main";
                    menuItems[arrayIndex + 1] = "eXit";
                    break;
                case MenuLevel.Level2Plus:

                    menuItems[arrayIndex] = "Return to Previous";
                    menuItems[arrayIndex + 1] = "Return to Main";
                    menuItems[arrayIndex + 2] = "eXit";
                    break;
                default:
                    throw new Exception("Unknown menu depth!");
            }

            int curItem;
            int userChoice;


            ConsoleKeyInfo key;
            while (true)
            {
                curItem = 0;

                do
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine($"================> {_menuTitle} <================");

                    // Go through all menu items
                    for (var c = 0; c < menuItems.Length; c++)
                        // Point out current option
                        if (curItem == c)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine(menuItems[c]);
                        }
                        // Give other options
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.WriteLine(menuItems[c]);
                        }

                    key = Console.ReadKey(true);

                    // decrease/increase current item if key pressed is down/up
                    // If curItem goes out of bounds, it loops around to the other end.
                    if (key.Key.ToString() == "DownArrow")
                    {
                        curItem++;
                        if (curItem > menuItems.Length - 1) curItem = 0;
                    }
                    else if (key.Key.ToString() == "UpArrow")
                    {
                        curItem--;
                        if (curItem < 0) curItem = menuItems.Length - 1;
                    }

                    // Loop around until the user presses the enter go.
                } while (key.KeyChar != 13);


                //userChoice -9 means go back one lair
                userChoice = curItem;


                // if it is not a reserved menu option
                if (userChoice >= 0 && userChoice < MenuItems.Count)
                {
                    // not it wasn't, try to find menuOption in MenuItems
                    if (MenuItems.TryGetValue(curItem, out var userMenuItem))
                    {
                        Console.Clear();
                        userChoice = userMenuItem.MethodToExecute();
                    }
                    else
                    {
                        Console.WriteLine("I don't have this option!");
                    }
                }


                if (userChoice == menuItems.Length - 1 || userChoice == -1)
                {
                    if (_menuLevel == MenuLevel.Level0) Console.WriteLine("Closing Down...");

                    userChoice = -1;
                    break;
                }

                if ((userChoice == menuItems.Length - 2 || userChoice == -2) && _menuLevel != MenuLevel.Level0)
                {
                    userChoice = -2;
                    break;
                }

                if ((userChoice == menuItems.Length - 3 || userChoice == -3) && _menuLevel == MenuLevel.Level2Plus)
                {
                    userChoice = -9;
                    break;
                }
            }

            return userChoice;
        }
    }
}