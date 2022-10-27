using ConsolePuzzle_2;
using ConsolePuzzle_2.Content;
using ConsolePuzzle_2.Content.Game_Objects;
using ConsolePuzzle_2.Content.Game_Objects.Basics;
using ConsolePuzzle_2.Content.Game_Objects.Collectables;
using ConsolePuzzle_2.Content.Game_Objects.Enemies;
using ConsolePuzzle_2.Content.Game_Objects.Gates;
using ConsolePuzzle_2.Content.Game_Objects.Interactable;
using ConsolePuzzle_2.Screens;
using ConsolePuzzle_2.Utility.Extensions;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using static ConsolePuzzle_2.Utility.Enums;
using Newtonsoft.Json.Linq;
using System.Reflection.PortableExecutable;
using ConsolePuzzle_2.Services;
using System.Runtime.InteropServices;
using ConsolePuzzle_2.Services.Models;
using System.Collections.Immutable;
using System.Text.RegularExpressions;
using System.Reflection;

namespace ConsolePuzzle_2
{
    public class GameController
    {
        private readonly MenuController menuController;
        #region Singleton
        private GameController()
        {
            menuController = MenuController.GetInstance();
        }
        private static GameController? _instance;

        public static GameController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameController();
            }
            return _instance;
        }
        #endregion

        /// <summary>
        /// The list of available tutorials.
        /// </summary>
        public readonly ImmutableArray<Type> TutorialsList = ImmutableArray.Create(new Type[]{
            typeof(Content.Tutorials.Tutorial_01),
            typeof(Content.Tutorials.Tutorial_02),
            typeof(Content.Tutorials.Tutorial_03),
            typeof(Content.Tutorials.Tutorial_04),
            typeof(Content.Tutorials.Tutorial_05),
            typeof(Content.Tutorials.Tutorial_06),
            typeof(Content.Tutorials.Tutorial_07),
            typeof(Content.Tutorials.Tutorial_08),
            typeof(Content.Tutorials.Tutorial_09),
            typeof(Content.Tutorials.Tutorial_10),
            typeof(Content.Tutorials.Tutorial_11),
            typeof(Content.Tutorials.Tutorial_12),
        });

        /// <summary>
        /// Contains information about the pressed key in the current frame.
        /// </summary>
        public ConsoleKeyInfo PressedKey { get; private set; }

        /// <summary>
        /// If the control key was pressed or not.
        /// </summary>
        public bool CtrlKeyHeld { get; private set; }

        /// <summary>
        /// The list of the indexes of completed tutorials.
        /// </summary>
        public List<int> CompletedTutorials { get; set; } = new();

        /// <summary>
        /// The list of the indexes of completed levels.
        /// </summary>
        public List<int> CompletedLevels { get; set; } = new();

        /// <summary>
        /// If a game level is currently running or not.
        /// </summary>
        public bool Playing { get; set; }

        /// <summary>
        /// Objects that player can stand on, so their content should be restored after standing out.
        /// </summary>
        public readonly ObjectTypes[] ObjToKeepContent = new ObjectTypes[]
        {
            ObjectTypes.CoinDoor,
            ObjectTypes.KeyDoor,
            ObjectTypes.CodeDoor,
            ObjectTypes.Portal,
            ObjectTypes.StandingButton,
            ObjectTypes.PlayerDoor
        };

        /// <summary>
        /// Objects that the box can stand on.
        /// </summary>
        public readonly ObjectTypes[] BoxContainers = new ObjectTypes[]
        {
            ObjectTypes.Blank,
            ObjectTypes.StandingButton,
            ObjectTypes.KeyDoor,
            ObjectTypes.CoinDoor,
            ObjectTypes.CodeDoor,
            ObjectTypes.PlayerDoor
        };

        /// <summary>
        /// Objects that blocks players from jumping over one-ways.
        /// </summary>
        public readonly ObjectTypes[] NotJumpableObjs = new ObjectTypes[]
        {
            ObjectTypes.Player,
            ObjectTypes.Player2,
            ObjectTypes.Wall,
            ObjectTypes.OneWay,
            ObjectTypes.Box,
            ObjectTypes.Portal,
            ObjectTypes.Enemy_Cannon
        };

        /// <summary>
        /// Objects that can be cleared after player moves over (usually collectables).
        /// </summary>
        public readonly ObjectTypes[] ObjectsToClear = new ObjectTypes[]
        {
            ObjectTypes.Coin,
            ObjectTypes.Key
        };

        /// <summary>
        /// Objects that enemies can walk over.
        /// </summary>
        public readonly ObjectTypes[] EnemiesContainers = new ObjectTypes[]
        {
            ObjectTypes.Blank,
            ObjectTypes.Player,
            ObjectTypes.Player2,
            ObjectTypes.StandingButton,
            ObjectTypes.CoinDoor,
            ObjectTypes.CodeDoor,
            ObjectTypes.KeyDoor,
            ObjectTypes.OneWay
        };

        /// <summary>
        /// Gates that can be opened.
        /// </summary>
        public readonly ObjectTypes[] Gates = new ObjectTypes[]
        {
            ObjectTypes.CoinDoor,
            ObjectTypes.CodeDoor,
            ObjectTypes.KeyDoor
        };

        /// <summary>
        /// Stops current level, reset all it's data and start over.
        /// </summary>
        /// <param name="activeLevel">The level to be restarted.</param>
        public void RestartLevel(Level activeLevel)
        {
            if (!Playing)
                menuController.ReturnPage();
            activeLevel.InfoBarChanged = true;
            activeLevel.Startup();
        }

        /// <summary>
        /// Start a new level.
        /// </summary>
        /// <param name="lvl">The level to be started.</param>
        public void Play(Level lvl)
        {
            menuController.ResetPagesAndScheduleNew(Pages.Level, lvl);
        }

        /// <summary>
        /// Returns from current page and resume active level.
        /// </summary>
        /// <param name="activeLevel">Level to be resumed.</param>
        public void ContinueGame(Level activeLevel)
        {
            ResetKey();
            Console.Clear();
            menuController.ReturnPage();
            activeLevel.WriteGameUI();
            Playing = true;
        }

        public void CompleteLevel(Game game)
        {
            GameObject p1 = game.BoardObjects[game.PlayerYPos][game.PlayerXPos];
            p1.FgColor = ConsoleColor.Green;
            p1.BracketsFgColor = ConsoleColor.DarkGreen;
            p1.Brackets = "<>";

            if (game.IsMultiplayer)
            {
                GameObject p2 = game.BoardObjects[game.Player2YPos][game.Player2XPos];
                p2.FgColor = ConsoleColor.Green;
                p2.BracketsFgColor = ConsoleColor.DarkGreen;
                p2.Brackets = "<>";
            }

            Console.SetCursorPosition(game.Board.BoardCursorLeft, game.Board.BoardCursorTop);
            game.Board.Build();
            Thread.Sleep(400);
            menuController.OpenPage(Pages.CompletedLevel, game.ActiveLevel);
        }

        /// <summary>
        /// Kills the player and restarts the game.
        /// </summary>
        /// <param name="player"></param>
        public void KillPlayer(Game gameLvl)
        {
            string deathText = menuController.GetString("Eliminated") + "!";
            Console.SetCursorPosition(0, gameLvl.HeaderRows + gameLvl.FooterRows + 2);
            gameLvl.Board.Build();

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Red;

            Console.SetCursorPosition(((Console.BufferWidth - deathText.Length) / 2) - 1, (Console.BufferHeight / 2) - 1);
            Console.WriteLine(new string(' ', deathText.Length + 2));

            Console.SetCursorPosition(((Console.BufferWidth - deathText.Length) / 2) - 1, Console.BufferHeight / 2);
            Console.Write(" ");

            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(deathText);

            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(" ");

            Console.SetCursorPosition(((Console.BufferWidth - deathText.Length) / 2) - 1, (Console.BufferHeight / 2) + 1);
            Console.WriteLine(new string(' ', deathText.Length + 2));

            Console.ResetColor();
            Thread.Sleep(800);
            RestartLevel(gameLvl.ActiveLevel);
        }

        /// <summary>
        /// Start's a new task that reads keyboard input in a loop.
        /// </summary>
        public void StartReadingInput()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (menuController.DisableGameInput) return;
                    PressedKey = Console.ReadKey(true);

                    CtrlKeyHeld = (PressedKey.Modifiers & ConsoleModifiers.Control) != 0;
                }
            });
        }

        /// <summary>
        /// Clears the current pressed key.
        /// </summary>
        public void ResetKey()
        {
            PressedKey = new ConsoleKeyInfo((char)ConsoleKey.K, ConsoleKey.K, false, false, false);
        }

        /// <summary>
        /// Increments collected coins of current level and plays a beep sound in a separate thread.
        /// </summary>
        /// <param name="lvl">The active level to increment collected coins.</param>
        public void CollectCoin(Game lvl)
        {
            lvl.CollectedCoins++;
            lvl.OnUIBarChanged();
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;

            if (ConfigManager.HasSoundFX)
            {
                Task.Factory.StartNew(() =>
                {
                    Console.Beep(2200, 200);
                });
            }
        }

        /// <summary>
        /// Builds the game board from a json string.
        /// </summary>
        /// <param name="jsonText">The json string containg all mappings and configs.</param>
        /// <param name="gameLvl">The level that the board will be added.</param>
        /// <returns>A game board, essentially a list (rows) of lists (columns) of gameobjects.</returns>
        /// <exception cref="ArgumentException"></exception>
        public List<List<GameObject>> BuildBoardObjects(string jsonText, Game gameLvl)
        {
            ValidateBoardJson(jsonText);

            JObject json = JObject.Parse(jsonText);
            var mapping = json["mapping"]!;
            string[][] coords = mapping.Values<string>().Select(l => l!.Split('&')).ToArray();

            string[] portals = coords.SelectMany(x => x.Select(y => y)).Where(x => x.Count(c => c == '|') == 2)?.ToArray() ?? Array.Empty<string>();
            if (portals.Length > 0 && portals.Count(x => char.IsUpper(x[1])) != portals.Count(x => char.IsLower(x[1])))
                throw new ArgumentException("The number of input portals is different of output portals. Received: " + string.Join(",", portals.Select(x => '\'' + x + '\'')));


            int width = coords[0].Length;
            int height = coords.Length;
            bool hasEnemy = false;

            List<List<GameObject>> board = Enumerable.Range(1, height).Select(y => Enumerable.Range(1, width).Select(x => (GameObject)new Blank(x, y)).ToList()).ToList();

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var newObj = MapText(coords[y][x], x, y, gameLvl);
                    board[y][x] = newObj;

                    if (newObj.ObjectType.ToString().StartsWith("Enemy"))
                    {
                        hasEnemy = true;
                    }
                }
            }

            if (!hasEnemy)
                return board;

            int _guard = 0, _cannon = 0, _seeker = 0;

            var config = json["config"];
            var enemiesData = config?["enemiesData"];
            if (enemiesData == null)
                return board;
            var enemiesObj = JsonConvert.SerializeObject(enemiesData);
            List<EnemyConfig> enemiesConfigs = JsonConvert.DeserializeObject<List<EnemyConfig>>(enemiesObj) ?? new List<EnemyConfig>();
            foreach (EnemyConfig item in enemiesConfigs)
            {
                Enemy enemy = (Enemy)board[item.Y][item.X];
                enemy.Speed = item.Speed;
                enemy.Delay = item.Delay;
                enemy.Dir = item.Dir switch { "up" => Direction.Up, "right" => Direction.Right, "down" => Direction.Down, "left" => Direction.Left, _ => Direction.Down };
                enemy.StartDelay = item.StartDelay;
                enemy.BulletSpeed = item.BulletSpeed;
                enemy.StartBulletSpeed = item.BulletSpeed;
                enemy.RateOfFire = item.RateOfFire;
                enemy.StartRateOfFire = item.RateOfFire;
                enemy.SightRadius = item.SightRadius;
                enemy.Name = enemy.ObjectType.ToString().Replace("Enemy_", "") + "_" +
                    enemy.ObjectType switch
                    {
                        ObjectTypes.Enemy_Guard => ++_guard,
                        ObjectTypes.Enemy_Cannon => ++_cannon,
                        ObjectTypes.Enemy_Seeker => ++_seeker,
                        _ => 0
                    };
                enemy.UpdateUI();
                if (!gameLvl.BoardEnemies.ContainsKey(enemy.Name))
                    gameLvl.BoardEnemies.Add(enemy.Name, enemy);
            }

            return board;
        }

        /// <summary>
        /// Check if a string is a valid board json.
        /// </summary>
        /// <param name="jsonText">The json string.</param>
        /// <exception cref="ArgumentException">Json is invalid.</exception>
        public void ValidateBoardJson(string jsonText)
        {
            if(jsonText[0] != '{' || jsonText[^1] != '}' || CountOccurrences(jsonText, new string[] { "\\([P]\\)", "\\(\\@\\)", "mapping", "config", "enemiesData" }).Any(num => num != 1))
            {
                throw new ArgumentException("Invalid JSON format. Received: " + jsonText);
            }
        }

        private int[] CountOccurrences(string source, string[] values)
        {
            int[] result = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                result[i] = Regex.Matches(source, values[i], RegexOptions.None).Count;
            }
            return result;
        }

        /// <summary>
        /// Maps a gameobject from it's string representation.
        /// </summary>
        /// <param name="text">The text to be converted.</param>
        /// <param name="x">Gameobject's X position.</param>
        /// <param name="y">Gameobject's Y position.</param>
        /// <param name="gameLvl">The active game level it'll belong to.</param>
        /// <returns>The new gameobject mapped.</returns>
        /// <exception cref="ArgumentException">Throws exception if object could not be mapped from text.</exception>
        private GameObject MapText(string text, int x, int y, Game gameLvl)
        {
            switch (text)
            {
                case "( )":
                    return new Blank(x, y);
                case "(P)":
                    return new Player(x, y);
                case "(p)":
                    return new Player2(x, y);
                case "(@)":
                    return new Exit(x, y);
                case "[ ]":
                    return new Wall(x, y);
                case "($)":
                    return new Coin(x, y);
                case "(#)":
                    return new Box(x, y);
                case "{P}":
                    return new PlayerDoor(x, y, 1);
                case "{p}":
                    return new PlayerDoor(x, y, 2);
                case "(X)":
                    return new Enemy_Guard(x, y, "", gameLvl);
                case "|?|":
                    return new Enemy_Seeker(x, y, "", gameLvl);
            }

            if (text.HasBrackets('(', ')') && char.IsLetter(text[1]))
                return new Key(x, y, text[1]);
            else if (text.HasBrackets('[', ']') && text[1..^1].All(c => char.IsNumber(c)))
                return new CoinDoor(x, y, int.Parse(text[1..^1]), gameLvl);
            else if (text.HasBrackets('[', ']') && char.IsLetter(text[1]))
                return new KeyDoor(x, y, text[1], gameLvl);
            else if (text.HasBrackets('{', '}') && text[1..^1].All(c => char.IsNumber(c)))
                return new CodeDoor(x, y, int.Parse(text[1..^1]), gameLvl);
            else if (text.HasBrackets('|', '|') && char.IsLetter(text[1]))
            {
                gameLvl.BoardPortals.Add(new(text[1].ToString(), x, y));
                return new Portal(x, y, text[1]);
            }
            else if (text.HasBrackets('(', ')') && text[1..^1].All(c => char.IsNumber(c)))
                return new StandingButton(x, y, int.Parse(text[1..^1]));
            else if (text.HasBrackets('{', '}') && new char[] { '↑', '>', '↓', '<' }.Contains(text[1]))
                return new OneWay(x, y, text[1] switch
                {
                    '↑' => Direction.Up,
                    '>' => Direction.Right,
                    '↓' => Direction.Down,
                    '<' => Direction.Left,
                    _ => Direction.Up
                });
            else if (new string[] { @"/'\", "[)=", @"\./", "=(]" }.Contains(text))
                return new Enemy_Cannon(x, y, "", text switch
                {
                    @"/'\" => Direction.Up,
                    "[)=" => Direction.Right,
                    @"\./" => Direction.Down,
                    "=(]" => Direction.Left,
                    _ => Direction.Up
                }, gameLvl);

            throw new ArgumentException($"Couldn't find an object for '{text}' at X:{x}, Y:{y}. Check for typing errors and try again.");
        }
    }
}
