using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.ConsoleUtility;

namespace ConsolePuzzle_2.Content.Tutorials
{
    public class UserLevel : Level
    {
        private readonly string Json;
        public UserLevel(string json)
        {
            Json = json;
        }

        public override void Startup()
        {
            ActiveGame = new Game(Json, this);

            Console.ResetColor();
            Console.Clear();

            ActiveGame.Title = "User Level";
            LevelNumber = 0;

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
            Header.Lines[2].Add(new LineItem(menuController.GetString("This is a") + " ", centerLine: true));
            Header.Lines[2].Add(new LineItem("User Level", fgColor: ConsoleColor.Magenta));
            Header.Lines[2].Add(new LineItem(". " + menuController.GetString("Custom description will be available soon") + "."));

            DefaultGameFooter(ActiveGame);

            ActiveGame.Header = Header;
            ActiveGame.Footer = Footer;
        }

        //Not used in game-pages
        protected override bool OnOptionChosen() { return false; }
        protected override string[] GenerateDescriptions() { return Array.Empty<string>(); }

    }
}
