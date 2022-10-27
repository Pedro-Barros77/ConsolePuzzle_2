using static ConsolePuzzle_2.Utility.Enums;
using static ConsolePuzzle_2.Utility.ConsoleUtility;
using ConsolePuzzle_2.Services.Models;
using ConsolePuzzle_2.Content;

namespace ConsolePuzzle_2.Screens
{
    public abstract class Page
    {
        /// <summary>
        /// The type of this page.
        /// </summary>
        public Pages PageType { get; set; }
        /// <summary>
        /// The index of the level, if it is one.
        /// </summary>
        public int LevelNumber { get; protected set; }
        /// <summary>
        /// The optional panel of the page.
        /// </summary>
        public ConsolePanel? PagePanel { get; protected set; }
        /// <summary>
        /// The index of the first row of the header that is a clickable option.
        /// </summary>
        public int FirstOptionIndex { get; set; }
        /// <summary>
        /// The index of the current option selected.
        /// </summary>
        public int CurrentOptionIndex { get; set; }
        /// <summary>
        /// The cursor distance from the top of the screen, after the panel was written.
        /// </summary>
        protected int CursorTop { get; set; }
        /// <summary>
        /// A list containg the description of each page option in order.
        /// </summary>
        protected string[] OptionsDescriptions;
        /// <summary>
        /// Gets the description of the currently selected page option.
        /// </summary>
        protected string OptionDescription => OptionsDescriptions[CurrentOptionIndex];
        /// <summary>
        /// The header of the page, wich is placed below the panel and contains all clickable options.
        /// </summary>
        protected UIBlock Header = new();
        /// <summary>
        /// The footer of the page, wich is placed below the header and contains all instructions.
        /// </summary>
        protected UIBlock Footer = new();

        protected readonly MenuController menuController;
        protected readonly GameController gameController;

        protected Page(Pages pageType)
        {
            menuController = MenuController.GetInstance();
            gameController = GameController.GetInstance();
            PageType = pageType;
            OptionsDescriptions = Array.Empty<string>();
        }

        /// <summary>
        /// Clears the page and builds it again.
        /// </summary>
        public void UpdateUI()
        {
            Header.Lines.Clear();
            Footer.Lines.Clear();
            BuildPage();
        }

        /// <summary>
        /// Writes the content of the header on screen.
        /// </summary>
        protected void WriteHeader()
        {
            //count the rows length and set the longest one
            int _longestLine = Header.Lines.Where(L => Header.Lines.IndexOf(L) > FirstOptionIndex).Max(L => L.Sum(x => x.Text.Length));

            Console.CursorTop = CursorTop;
            Console.CursorLeft = 0;
            for (int line = 0; line < Header.Lines.Count; line++)
            {
                bool centerLine = false;
                bool alignRows = false;
                int lineLength = Header.Lines[line].Sum(x => x.Text.Length);

                for (int item = 0; item < Header.Lines[line].Count; item++)
                {
                    if (Header.Lines[line][item].CenterLine)
                        centerLine = true;
                    alignRows = Header.Lines[line][item].AlignRows;

                    if (CurrentOptionIndex == line - FirstOptionIndex)
                    {
                        if (Header.Lines[line][0].Active)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        else
                        {
                            Console.BackgroundColor = ConsoleColor.DarkGray;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                    }
                    else
                    {
                        Console.BackgroundColor = Header.Lines[line][item].BgColor;
                        Console.ForegroundColor = Header.Lines[line][0].Active ? Header.Lines[line][item].FgColor : ConsoleColor.DarkGray;
                    }

                    if (centerLine && item == 0)
                    {
                        if (alignRows)
                            Console.Write(CenterText(new string(' ', _longestLine + 2), spacesOnly: true));
                        else
                            Console.Write(CenterText(new string(' ', lineLength + 2), spacesOnly: true));
                    }
                    Console.Write(Header.Lines[line][item].Text);
                }

                if (line >= FirstOptionIndex)
                {
                    if (centerLine)
                    {
                        if (alignRows)
                        {
                            int previousLength = lineLength + CenterText(new string(' ', _longestLine), spacesOnly: true).Length;
                            Console.Write(new string(' ', Console.WindowWidth - previousLength));
                        }
                        else
                        {
                            Console.Write(CenterText(new string(' ', lineLength + 1), spacesOnly: true));
                        }
                    }
                    else
                    {
                        Console.Write(new string(' ', (Console.WindowWidth - lineLength) - 1));
                    }
                }
                Console.Write("\n");
            }
        }

        /// <summary>
        /// Writes the content of the footer on screeen.
        /// </summary>
        protected void WriteFooter()
        {
            int _longestLine = Footer.Lines.Max(L => L.Sum(x => x.Text.Length));

            for (int line = 0; line < Footer.Lines.Count; line++)
            {
                bool centerLine = false;
                bool alignRows = false;
                int lineLength = Footer.Lines[line].Sum(x => x.Text.Length);

                for (int item = 0; item < Footer.Lines[line].Count; item++)
                {
                    centerLine = Footer.Lines[line][item].CenterLine;
                    alignRows = Footer.Lines[line][item].AlignRows;

                    if (centerLine && item == 0)
                    {
                        if (alignRows)
                        {
                            Console.Write(CenterText(new string(' ', _longestLine + 2), spacesOnly: true));
                        }
                        else
                        {
                            Console.Write(CenterText(new string(' ', lineLength + 2), spacesOnly: true));
                        }
                    }
                    Console.BackgroundColor = Footer.Lines[line][item].BgColor;
                    Console.ForegroundColor = Footer.Lines[line][item].FgColor;
                    Console.Write(Footer.Lines[line][item].Text);
                }
                Console.Write("\n");
            }
        }

        /// <summary>
        /// Builds the default footer for menu screens, containing instructions of navigation.
        /// </summary>
        protected void DefaultFooter()
        {
            Footer.Fill(5);

            Footer.Lines[0].Add(new LineItem(new string('_', Console.WindowWidth - 1), fgColor: ConsoleColor.DarkGray));

            Footer.Lines[1].Add(new LineItem(menuController.GetString("Navigate") + ": ", fgColor: ConsoleColor.DarkGray));
            Footer.Lines[1].Add(new LineItem(menuController.GetString("up/down arrows"), fgColor: ConsoleColor.Magenta));
            Footer.Lines[1].Add(new LineItem(new string(' ', Console.WindowWidth - (Footer.Lines[1][0].Text.Length + Footer.Lines[1][1].Text.Length) - 1)));

            Footer.Lines[2].Add(new LineItem(menuController.GetString("Select") + ": ", fgColor: ConsoleColor.DarkGray));
            Footer.Lines[2].Add(new LineItem(menuController.GetString("enter/SpaceBar/right arrow"), fgColor: ConsoleColor.Magenta));
            Footer.Lines[2].Add(new LineItem(new string(' ', Console.WindowWidth - (Footer.Lines[2][0].Text.Length + Footer.Lines[2][1].Text.Length) - 1)));

            Footer.Lines[3].Add(new LineItem(menuController.GetString("Go back") + ": ", fgColor: ConsoleColor.DarkGray));
            Footer.Lines[3].Add(new LineItem(menuController.GetString("BackSpace/left arrow"), fgColor: ConsoleColor.Magenta));
            Footer.Lines[3].Add(new LineItem(new string(' ', Console.WindowWidth - (Footer.Lines[3][0].Text.Length + Footer.Lines[3][1].Text.Length) - 1)));

            Footer.Lines[4].Add(new LineItem(" ", centerLine: true));
        }


        /// <summary>
        /// Page function to run once before Run();
        /// </summary>
        public abstract void Startup();

        /// <summary>
        /// Page function to run every frame.
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Function to add all UI content to the page.
        /// </summary>
        protected abstract void BuildPage();

        /// <summary>
        /// Called when user chooses option (spacebar/enter/right arrow).
        /// </summary>
        /// <returns>True if the selected option opens a new page.</returns>
        protected abstract bool OnOptionChosen();

        /// <summary>
        /// Generates an array of descriptions of each page option, on the current language.
        /// </summary>
        /// <returns>The generated string descriptions.</returns>
        protected abstract string[] GenerateDescriptions();

        /// <summary>
        /// Called when user changes the currently hovered option (up/down arrow).
        /// </summary>
        protected virtual void OnOptionChanged()
        {
            OptionsDescriptions = GenerateDescriptions();
            Header.Lines[0][0].Text = CenterText(OptionDescription, bothSides: true);
        }

        /// <summary>
        /// Called when user requests to go back one page.
        /// </summary>
        protected virtual void OnGoBack()
        {
            if (menuController.ReturnPage())
            {
                Console.Clear();
                gameController.ResetKey();
            }
        }

        /// <summary>
        /// Handles default input behavior (navigate and select).
        /// </summary>
        protected virtual bool GetInput()
        {
            ConsoleKey dirKey = gameController.PressedKey.Key;
            switch (dirKey)
            {
                case ConsoleKey.UpArrow:
                    if (CurrentOptionIndex > 0)
                    {
                        CurrentOptionIndex--;
                        OnOptionChanged();
                    }
                    break;

                case ConsoleKey.DownArrow:
                    if (CurrentOptionIndex < (Header.Lines.Count - 1) - FirstOptionIndex)
                    {
                        CurrentOptionIndex++;
                        OnOptionChanged();
                    }
                    break;

                case ConsoleKey.Spacebar:
                case ConsoleKey.Enter:
                case ConsoleKey.RightArrow:
                    gameController.ResetKey();
                    return OnOptionChosen();
                case ConsoleKey.LeftArrow:
                    OnGoBack();
                    break;
                //For tests only, remove
                case ConsoleKey.NumPad0:
                    gameController.ResetKey();
                    var levelType = gameController.TutorialsList[^1];
                    gameController.Play((Level)Activator.CreateInstance(levelType)!);
                    break;
            }
            gameController.ResetKey();
            return false;
        }
    }
}
