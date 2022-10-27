using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.ConsoleUtility;
using ConsolePuzzle_2.Utility.Extensions;

namespace ConsolePuzzle_2.Content.Tutorials
{
    public class Tutorial_03 : Level
    {
        public override void Startup()
        {
            string json = @"
                {
                  'mapping': [
                    '( )&( )&( )&( )&($)&[ ]&[ ]&[ ]',
                    '( )&( )&( )&[ ]&[ ]&[ ]&[ ]&[ ]',
                    '( )&( )&( )&( )&( )&[2]&( )&( )',
                    '( )&[ ]&[ ]&( )&( )&[ ]&[ ]&( )',
                    '(P)&[ ]&($)&( )&( )&[ ]&[ ]&(@)'
                  ],
                  'config': {
                    'enemiesData': []
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
            Header.Fill(3);

            Header.Lines[0].Add(new LineItem(CenterText(ActiveGame.Title.Replace('_', ' '), bothSides: true), ConsoleColor.DarkMagenta, ConsoleColor.White));
            Header.Lines[1].Add(new LineItem(""));
            Header.Lines[2].Add(new LineItem(menuController.GetString("This") + " \"", centerLine: true));
            Header.Lines[2].Add(new LineItem("[", fgColor: ConsoleColor.DarkYellow));
            Header.Lines[2].Add(new LineItem("2", fgColor: ConsoleColor.Yellow));
            Header.Lines[2].Add(new LineItem("]", fgColor: ConsoleColor.DarkYellow));
            Header.Lines[2].Add(new LineItem("\"" + menuController.GetString("is a door that only opens after collecting some coins") + " \""));
            Header.Lines[2].Add(new LineItem("$", fgColor: ConsoleColor.Yellow));
            Header.Lines[2].Add(new LineItem("\"."));

            DefaultGameFooter(ActiveGame);

            ActiveGame.Header = Header;
            ActiveGame.Footer = Footer;
        }

        //Not used in game-pages
        protected override bool OnOptionChosen() { return false; }
        protected override string[] GenerateDescriptions() { return Array.Empty<string>(); }
    }
}
