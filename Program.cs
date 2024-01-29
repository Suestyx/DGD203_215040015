using System;
using System.Numerics;
using System.Threading;

namespace DGD203
{
    internal class TheGame
    {
        private const int DefaultMapWidth = 1;
        private const int DefaultMapHeight = 12;
        private static Map _gameMap;
        private static string _currentChoice;
        private static string _playerName;
        


        private static void Main()
        {
            
            Console.WriteLine("Welcome to an enchanting universe.\nWhich region would you like to go to?");
            Console.WriteLine("1. Harmony Hollow");
            Console.WriteLine("2. Serenity Springs");
            Console.WriteLine("3. Oakshire");

            int choice;
            bool validChoice;

            do
            {
                validChoice = int.TryParse(Console.ReadLine(), out choice);

                if (validChoice && choice >= 1 && choice <= 3)
                {
                    switch (choice)
                    {
                        case 1:
                            Console.WriteLine("You have chosen the Harmony Hollow region.");
                            _currentChoice = "Harmony Hollow";
                            break;
                        case 2:
                            Console.WriteLine("Serenity Springs region is a beautiful choice. Have a great game!");
                            _currentChoice = "Serenity Springs";
                            break;
                        case 3:
                            Console.WriteLine("You've chosen the Oakshire region. No need to be afraid, have a great game!");
                            _currentChoice = "Oakshire";
                            break;
                    }
                }

            } while (!validChoice);

            Console.Write("What is your name? ");
            _playerName = Console.ReadLine();

            if (_playerName == "Onur")
            {
                Console.WriteLine($"Have we met before {_playerName}??? The name sounds familiar. Let's have some fun together!");
            }
            else
            {
                Console.WriteLine($"Pleased to meet you {_playerName}, Let's have some fun together!");
            }

            Console.WriteLine("Starting the game....");

            for (int i = 4; i > 0; i--)
            {
                Console.WriteLine(i);
                Thread.Sleep(1000);
            }

            Console.WriteLine($"Created map with size {DefaultMapHeight}x{DefaultMapWidth}");
            Console.WriteLine($"Welcome to the {_currentChoice}.\nHow about pressing the D key? ");
            

            _gameMap = new Map(new[] { 0, 12 }, new[] { 0, 0 });


            // Get commands
            while (true)
            {
                string command = Console.ReadLine().ToUpper();

                if (command == "D")
                {
                    _gameMap.MoveRight();
                    Console.WriteLine("You advanced one unit.");
                    //Console.WriteLine(DefaultMapHeight + "player pos" + _gameMap.GetPlayerPositionX());

                    if (_gameMap.GetPlayerPositionX() >= DefaultMapHeight)
                    {
                        Console.WriteLine("GAME OVER!");
                        break;
                    }
                }
                else if (command == "W")
                {
                    _gameMap.Attack();
                }
                else if (command == "EXIT")
                {
                    Console.WriteLine("Exiting the game. Goodbye!");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid command. Try again.");
                }


            }


        }
    }

    public class Map
    {
        private int[] _widthBoundaries;
        private int[] _heightBoundaries;
        private Location[,] _locations;
        private int _playerPositionX;


        public Map(int[] widthBoundaries, int[] heightBoundaries)
        {
            _widthBoundaries = widthBoundaries;
            _heightBoundaries = heightBoundaries;

            GenerateLocationsWithNPCsAndItems();
            _playerPositionX = 0;
        }

        public void MoveRight()
        {
            int newPlayerPositionX = _playerPositionX + 1;

            if (newPlayerPositionX <= _widthBoundaries[1] - _widthBoundaries[0] && newPlayerPositionX <= 12)
            {
                _playerPositionX = newPlayerPositionX;
                CheckForLocation(_playerPositionX, 0);
            }
            else
            {
                Console.WriteLine("It looks like we've reached the limit of the map.");
            }
        }


        public void Attack()
        {
         
            EnemyWithHealth enemy = _locations[_playerPositionX - _widthBoundaries[0], 0 - _heightBoundaries[0]].NPC as EnemyWithHealth;

            if (enemy != null && enemy.IsAlive)
            {
                enemy.Attack();
            }
            else
            {
                Console.WriteLine("No enemy to attack.");
            }
        }
        public int GetPlayerPositionX()
        {
            return _playerPositionX;
        }


        private void GenerateLocationsWithNPCsAndItems()
        {
            _locations = new Location[_widthBoundaries[1] - _widthBoundaries[0] + 1, _heightBoundaries[1] - _heightBoundaries[0] + 1];

            for (int x = _widthBoundaries[0]; x <= _widthBoundaries[1]; x++)
            {
                for (int y = _heightBoundaries[0]; y <= _heightBoundaries[1]; y++)
                {
                    _locations[x - _widthBoundaries[0], y - _heightBoundaries[0]] = new Location();
                }
            }

            // NPC
            _locations[3 - _widthBoundaries[0], 0 - _heightBoundaries[0]].NPC = new AssistantNPC("Be careful!! The ahead may be dangerous.");

            _locations[4 - _widthBoundaries[0], 0 - _heightBoundaries[0]].NPC = new AssistantNPC("Hımm this place is already abandoned.");

            // Item
            _locations[5 - _widthBoundaries[0], 0 - _heightBoundaries[0]].Item = new ValuableItem("Woow, you found a sword!");

            // Enemys
            _locations[6 - _widthBoundaries[0], 0 - _heightBoundaries[0]].NPC = new EnemyWithHealth("Be careful!! An enemy approaches. \nHow about attacking by pressing the 'W' key?", 20);

            _locations[10 - _widthBoundaries[0], 0 - _heightBoundaries[0]].NPC = new EnemyWithHealth("Be careful!! An enemy approaches. \nHow about attacking by pressing the 'W' key?", 20);

        }


        private void CheckForLocation(int x, int y)
        {
            if (_locations[x - _widthBoundaries[0], y - _heightBoundaries[0]].NPC != null)
            {
                Console.WriteLine(_locations[x - _widthBoundaries[0], y - _heightBoundaries[0]].NPC.Message);
            }

            if (_locations[x - _widthBoundaries[0], y - _heightBoundaries[0]].Item != null)
            {
                Console.WriteLine(_locations[x - _widthBoundaries[0], y - _heightBoundaries[0]].Item.Message);
            }
        }


        private class AssistantNPC : NPC
        {
            public AssistantNPC(string message) : base("AssistantNPC", message)
            {
            }
        }

        private class ValuableItem : Item
        {
            public ValuableItem(string message) : base("Sword", message)
            {
            }
        }

        private class Enemy : NPC
        {
            public bool IsAlive { get; set; }

            public Enemy(string message) : base("Enemy", message)
            {
                IsAlive = true;
            }
        }
       

        private class OneTimeEnemy : Enemy
        {
            public OneTimeEnemy(string message) : base(message)
            {
            }

            public void Kill()
            {
                IsAlive = false;
            }
        }


        private class EnemyWithHealth : Enemy
        {
            public int Health { get; private set; }

            public EnemyWithHealth(string message, int health) : base(message)
            {
                Health = health;
            }

            public void Attack()
            {
                Health -= 5;
                Console.WriteLine($"Enemy's stamina decreased to {Health}");

                if (Health <= 0)
                {
                    Console.WriteLine("You did it!!!");
                    IsAlive = false;
                }
            }
        }
        private class NPC
        {
            public string Name { get; }
            public string Message { get; }

            public NPC(string name, string message)
            {
                Name = name;
                Message = message;
            }
        }

        private class Item
        {
            public string Name { get; }
            public string Message { get; }

            public Item(string name, string message)
            {
                Name = name;
                Message = message;
            }
        }

        private class Location
        {
            public NPC NPC { get; set; }
            public Item Item { get; set; }
        }
    }
}