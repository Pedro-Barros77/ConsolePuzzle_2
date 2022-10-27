using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.ConsoleUtility;
using ConsolePuzzle_2.Utility.Extensions;

namespace ConsolePuzzle_2.Content.Tutorials
{
    public class Tutorial_11 : Level
    {
        public override void Startup()
        {
            string json = @"
                {
                  'mapping': [
                    '(P)&( )&{0}&(1)&{0}&( )&( )&{p}&( )',
                    '( )&( )&( )&{0}&( )&( )&( )&{p}&($)',
                    '( )&( )&( )&( )&( )&( )&( )&{p}&( )',
                    '[2]&[2]&( )&( )&( )&( )&[ ]&[ ]&[ ]',
                    '(0)&[2]&( )&( )&( )&( )&{0}&{1}&(@)',
                    '[2]&[2]&( )&( )&( )&( )&[ ]&[ ]&[ ]',
                    '( )&( )&( )&( )&( )&( )&( )&{P}&( )',
                    '( )&( )&( )&{1}&( )&( )&( )&{P}&($)',
                    '(p)&( )&{1}&(#)&{1}&( )&( )&{P}&( )'
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
            Header.Fill(6);

            Header.Lines[0].Add(new LineItem(CenterText(ActiveGame.Title.Replace('_', ' '), bothSides: true), ConsoleColor.DarkMagenta, ConsoleColor.White));
            Header.Lines[1].Add(new LineItem(""));
            Header.Lines[2].Add(new LineItem(menuController.GetString("Sometimes two players are needed to complete a level") + ".", centerLine: true));
            Header.Lines[3].Add(new LineItem(menuController.GetString("You can control the blue player with WASD keys") + ".", centerLine: true));
            Header.Lines[4].Add(new LineItem(menuController.GetString("These") + " \"", centerLine: true));
            Header.Lines[4].Add(new LineItem("[", fgColor:ConsoleColor.Magenta));
            Header.Lines[4].Add(new LineItem("p", fgColor: ConsoleColor.DarkMagenta));
            Header.Lines[4].Add(new LineItem("]", fgColor: ConsoleColor.Magenta));
            Header.Lines[4].Add(new LineItem("\", \""));
            Header.Lines[4].Add(new LineItem("[", fgColor: ConsoleColor.Blue));
            Header.Lines[4].Add(new LineItem("p", fgColor: ConsoleColor.DarkBlue));
            Header.Lines[4].Add(new LineItem("]", fgColor: ConsoleColor.Blue));
            Header.Lines[4].Add(new LineItem("\" " + menuController.GetString("are the Player Doors") + "."));
            Header.Lines[5].Add(new LineItem(menuController.GetString("Only the player with the specified color can cross") + ".", centerLine: true));

            DefaultGameFooter(ActiveGame);

            ActiveGame.Header = Header;
            ActiveGame.Footer = Footer;
        }

        //Not used in game-pages
        protected override bool OnOptionChosen() { return false; }
        protected override string[] GenerateDescriptions() { return Array.Empty<string>(); }
    }
}
