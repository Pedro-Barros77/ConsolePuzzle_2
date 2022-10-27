using ConsolePuzzle_2.Screens;
using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.Enums;
using static ConsolePuzzle_2.Utility.ConsoleUtility;

namespace ConsolePuzzle_2.Content
{
    public abstract class Level : Page
    {
        /// <summary>
        /// The currently active game.
        /// </summary>
        public Game ActiveGame { get; protected set; }

        public bool InfoBarChanged { get; set; } = true;

        protected Level() : base(Pages.Level)
        {

        }

        /// <summary>
        /// Builds the game page and writes it's UI to the screen.
        /// </summary>
        public abstract void WriteGameUI();

        /// <summary>
        /// Prints info bar on game pages, containing collected coins, keys, etc.
        /// </summary>
        protected void InfoBar()
        {
            UIBlock Bar = new();
            Bar.Lines.Add(new List<LineItem>());
            Bar.Lines.Add(new List<LineItem>());

            Bar.Lines[0].Add(new LineItem(new string('_', Console.WindowWidth - 1), ConsoleColor.Black, ConsoleColor.White));
            Bar.Lines[1].Add(new LineItem(menuController.GetString("Collected Coins") + ": ", ConsoleColor.DarkGray, ConsoleColor.Black, centerLine: true));
            Bar.Lines[1].Add(new LineItem(ActiveGame.CollectedCoins.ToString(), ConsoleColor.DarkGray, ConsoleColor.Yellow));
            Bar.Lines[1].Add(new LineItem(" - " + menuController.GetString("Collected Keys") + ": ", ConsoleColor.DarkGray, ConsoleColor.Black));
            Bar.Lines[1].Add(new LineItem(string.Join(", ", ActiveGame.CollectedKeys), ConsoleColor.DarkGray, ConsoleColor.Cyan));
            Bar.Lines[1].Add(new LineItem(" - " + menuController.GetString("Pressed Buttons") + ": ", ConsoleColor.DarkGray, ConsoleColor.Black));
            Bar.Lines[1].Add(new LineItem(string.Join(", ", ActiveGame.PressedButtons), ConsoleColor.DarkGray, ConsoleColor.Green));
            Bar.Lines[1].Add(new LineItem("", ConsoleColor.DarkGray, ConsoleColor.Green, centerLine: true));

            bool centerLine = false;
            for (int line = 0; line < Bar.Lines.Count; line++)
            {
                for (int item = 0; item < Bar.Lines[line].Count; item++)
                {
                    int lineLength = 0;
                    if (Bar.Lines[line][item].CenterLine)
                    {
                        centerLine = true;
                        lineLength = Bar.Lines[line].Sum(w => w.Text.Length);
                    }
                    Console.BackgroundColor = Bar.Lines[line][item].BgColor;
                    Console.ForegroundColor = Bar.Lines[line][item].FgColor;
                    if (centerLine)
                    {
                        Console.Write(CenterText(new string(' ', lineLength), spacesOnly: true));
                        centerLine = false;
                    }
                    Console.Write(Bar.Lines[line][item].Text);
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        /// <summary>
        /// Prints default footer of game pages, containing the instructions of controls.
        /// </summary>
        /// <param name="GameLevel">The game object of current level.</param>
        protected void DefaultGameFooter(Game GameLevel)
        {
            Footer.Fill(6);

            if (GameLevel.IsMultiplayer)
            {
                Footer.Lines[0].Add(new LineItem(menuController.GetString("Move Player") + " 1: ", fgColor: ConsoleColor.DarkGray));
                Footer.Lines[0].Add(new LineItem(menuController.GetString("Arrow keys"), fgColor: ConsoleColor.Magenta));
                Footer.Lines[0].Add(new LineItem(", " + menuController.GetString("Move Player") + " 2: ", fgColor: ConsoleColor.DarkGray));
                Footer.Lines[0].Add(new LineItem("WASD", fgColor: ConsoleColor.Magenta));
            }
            else
            {
                Footer.Lines[0].Add(new LineItem(menuController.GetString("Move") + ": ", fgColor: ConsoleColor.DarkGray));
                Footer.Lines[0].Add(new LineItem(menuController.GetString("Arrow keys") + "/WASD", fgColor: ConsoleColor.Magenta));
            }

            Footer.Lines[1].Add(new LineItem(menuController.GetString("Grab box") + ": ", fgColor: ConsoleColor.DarkGray));
            Footer.Lines[1].Add(new LineItem(menuController.GetString("Hold") + "CTRL", fgColor: ConsoleColor.Magenta));

            Footer.Lines[2].Add(new LineItem(menuController.GetString("Pause") + ": ", fgColor: ConsoleColor.DarkGray));
            Footer.Lines[2].Add(new LineItem("P", fgColor: ConsoleColor.Magenta));

            Footer.Lines[3].Add(new LineItem(menuController.GetString("Quick Restart") + ": ", fgColor: ConsoleColor.DarkGray));
            Footer.Lines[3].Add(new LineItem("R", fgColor: ConsoleColor.Magenta));
        }
    }
}
