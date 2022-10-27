using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.ConsoleUtility;
using ConsolePuzzle_2.Utility.Extensions;

namespace ConsolePuzzle_2.Content.Tutorials
{
    public class Tutorial_09 : Level
    {
        public override void Startup()
        {
            string json = @"
                {
                  'mapping': [
                    '( )&( )&( )&( )&( )&( )&( )&( )&( )&=(]',
                    '(P)&[ ]&( )&[ ]&( )&[ ]&( )&[ ]&( )&(a)',
                    '[a]&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]',
                    '( )&( )&( )&( )&( )&( )&( )&( )&( )&( )',
                    '( )&( )&( )&( )&( )&( )&( )&( )&( )&( )',
                    '[ ]&( )&( )&($)&( )&($)&( )&( )&[ ]&( )',
                    '[ ]&( )&( )&( )&( )&( )&( )&( )&[ ]&( )',
                    '[ ]&( )&( )&( )&( )&( )&( )&( )&[ ]&( )',
                    '[ ]&( )&( )&( )&( )&( )&( )&( )&[ ]&( )',
                    '[ ]&( )&( )&( )&( )&( )&( )&( )&[ ]&( )',
                    '[ ]&/\'\\&/\'\\&/\'\\&/\'\\&/\'\\&/\'\\&/\'\\&[ ]&( )',
                    '(@)&( )&( )&( )&( )&( )&( )&( )&[2]&( )'
                  ],
                  'config': {
                    'enemiesData': [
                      {
                        'x': 9,
                        'y': 0,
                        'speed': 0,
                        'delay': 0,
                        'dir': 'left',
                        'startDelay': 0,
                        'bulletSpeed': 4,
                        'rateOfFire': 0.6,
                        'sightRadius': 0
                      },
                      {
                        'x': 1,
                        'y': 10,
                        'speed': 0,
                        'delay': 0,
                        'dir': 'up',
                        'startDelay': 0,
                        'bulletSpeed': 5,
                        'rateOfFire': 0.5,
                        'sightRadius': 0
                      },
                      {
                        'x': 2,
                        'y': 10,
                        'speed': 0,
                        'delay': 0,
                        'dir': 'up',
                        'startDelay': 300,
                        'bulletSpeed': 5,
                        'rateOfFire': 0.5,
                        'sightRadius': 0
                      },
                      {
                        'x': 3,
                        'y': 10,
                        'speed': 0,
                        'delay': 0,
                        'dir': 'up',
                        'startDelay': 450,
                        'bulletSpeed': 5,
                        'rateOfFire': 0.5,
                        'sightRadius': 0
                      },
                      {
                        'x': 4,
                        'y': 10,
                        'speed': 0,
                        'delay': 0,
                        'dir': 'up',
                        'startDelay': 600,
                        'bulletSpeed': 5,
                        'rateOfFire': 0.5,
                        'sightRadius': 0
                      },
                      {
                        'x': 5,
                        'y': 10,
                        'speed': 0,
                        'delay': 0,
                        'dir': 'up',
                        'startDelay': 750,
                        'bulletSpeed': 5,
                        'rateOfFire': 0.5,
                        'sightRadius': 0
                      },
                      {
                        'x': 6,
                        'y': 10,
                        'speed': 0,
                        'delay': 0,
                        'dir': 'up',
                        'startDelay': 900,
                        'bulletSpeed': 5,
                        'rateOfFire': 0.5,
                        'sightRadius': 0
                      },
                      {
                        'x': 7,
                        'y': 10,
                        'speed': 0,
                        'delay': 0,
                        'dir': 'up',
                        'startDelay': 1050,
                        'bulletSpeed': 5,
                        'rateOfFire': 0.5,
                        'sightRadius': 0
                      }
                    ]
                  }
                }
            ".CompactJSON();

            ActiveGame = new Game(json, this);

            Console.ResetColor();
            Console.Clear();

            string className = ToString() ?? "";
            ActiveGame.Title = className[(className.IndexOf("Tutorials") + 10)..];
            string nums = string.Concat(ActiveGame.Title.Where(char.IsNumber));
            LevelNumber = int.Parse(nums ?? "0");

            WriteGameUI();
        }

        public override void Run()
        {
            if (gameController.Playing)
            {
                if (InfoBarChanged)
                {
                    Console.SetCursorPosition(0, ActiveGame.HeaderRows);
                    InfoBar();
                    InfoBarChanged = false;
                }

                ActiveGame.Board.Build();

                ActiveGame.HandleInput(gameController.PressedKey.Key);

                ActiveGame.RunEnemies();
            }

            if (ActiveGame.LevelCompleted)
            {
                gameController.CompleteLevel(ActiveGame);
            }
        }

        public override void WriteGameUI()
        {
            BuildPage();
            ActiveGame.WriteGameUI();
        }

        protected override void BuildPage()
        {
            Header.Fill(4);

            Header.Lines[0].Add(new LineItem(CenterText(ActiveGame.Title.Replace('_', ' '), bothSides: true), ConsoleColor.DarkMagenta, ConsoleColor.White));
            Header.Lines[1].Add(new LineItem(""));
            Header.Lines[2].Add(new LineItem(menuController.GetString("These") + " \"", centerLine: true));
            Header.Lines[2].Add(new LineItem("=(]", fgColor:ConsoleColor.Red));
            Header.Lines[2].Add(new LineItem(", "));
            Header.Lines[2].Add(new LineItem("/'\\", fgColor:ConsoleColor.Red));
            Header.Lines[2].Add(new LineItem("\" " + menuController.GetString("are The Cannons") + "."));
            Header.Lines[3].Add(new LineItem(menuController.GetString("You better get cover!"), centerLine: true));

            DefaultGameFooter(ActiveGame);

            ActiveGame.Header = Header;
            ActiveGame.Footer = Footer;
        }

        //Not used in game-pages
        protected override bool OnOptionChosen() { return false; }
        protected override string[] GenerateDescriptions() { return Array.Empty<string>(); }
    }
}
