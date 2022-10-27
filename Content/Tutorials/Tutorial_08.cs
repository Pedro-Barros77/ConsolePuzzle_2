using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.ConsoleUtility;
using ConsolePuzzle_2.Utility.Extensions;

namespace ConsolePuzzle_2.Content.Tutorials
{
    public class Tutorial_08 : Level
    {
        public override void Startup()
        {
            string json = @"
                {
                  'mapping': [
                    '( )&( )&( )&( )&[ ]&[ ]&[ ]&[ ]',
                    '(P)&( )&( )&(X)&( )&($)&[2]&(X)',
                    '( )&( )&( )&( )&[ ]&[ ]&[ ]&( )',
                    '[1]&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&( )',
                    '($)&[ ]&( )&(X)&( )&( )&( )&( )',
                    '[ ]&[ ]&( )&( )&( )&(X)&[ ]&( )',
                    '(a)&[ ]&( )&[ ]&[ ]&[ ]&[ ]&[ ]',
                    '( )&( )&( )&[a]&( )&( )&( )&(@)'
                  ],
                  'config': {
                    'enemiesData': [
                      {
                        'x': 3,
                        'y': 1,
                        'speed': 1,
                        'delay': 500,
                        'dir': 'up',
                        'startDelay': 0,
                        'bulletSpeed': 0,
                        'rateOfFire': 0,
                        'sightRadius': 0
                      },
                      {
                        'x': 7,
                        'y': 1,
                        'speed': 1,
                        'delay': 500,
                        'dir': 'down',
                        'startDelay': 0,
                        'bulletSpeed': 0,
                        'rateOfFire': 0,
                        'sightRadius': 0
                      },
                      {
                        'x': 5,
                        'y': 5,
                        'speed': 1,
                        'delay': 500,
                        'dir': 'up',
                        'startDelay': 0,
                        'bulletSpeed': 0,
                        'rateOfFire': 0,
                        'sightRadius': 0
                      },
                      {
                        'x': 5,
                        'y': 5,
                        'speed': 1,
                        'delay': 300,
                        'dir': 'up',
                        'startDelay': 0,
                        'bulletSpeed': 0,
                        'rateOfFire': 0,
                        'sightRadius': 0
                      },
                      {
                        'x': 3,
                        'y': 4,
                        'speed': 1,
                        'delay': 300,
                        'dir': 'down',
                        'startDelay': 0,
                        'bulletSpeed': 0,
                        'rateOfFire': 0,
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
            Header.Lines[2].Add(new LineItem("X", fgColor:ConsoleColor.Red));
            Header.Lines[2].Add(new LineItem("\" " + menuController.GetString("are The Guards") + "."));
            Header.Lines[3].Add(new LineItem(menuController.GetString("They move linearly") + ". " + menuController.GetString("Be careful!"), centerLine: true));

            DefaultGameFooter(ActiveGame);

            ActiveGame.Header = Header;
            ActiveGame.Footer = Footer;
        }

        //Not used in game-pages
        protected override bool OnOptionChosen() { return false; }
        protected override string[] GenerateDescriptions() { return Array.Empty<string>(); }
    }
}
