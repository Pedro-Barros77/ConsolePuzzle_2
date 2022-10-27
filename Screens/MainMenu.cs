using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.ConsoleUtility;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Screens
{
    public class MainMenu : Page
    {
        public MainMenu() : base(Pages.MainMenu)
        {
            OptionsDescriptions = GenerateDescriptions();
            CurrentOptionIndex = 0;
            Console.Clear();
            BuildPage();
        }

        public override void Startup()
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write(new string(' ', Console.WindowWidth - 1));
        }

        public override void Run()
        {
            Console.CursorTop = 1;
            try
            {
                PagePanel?.SlidePanel();
            }
            catch
            {
                Console.SetCursorPosition(0, 1);
                PagePanel = new ConsolePanel("Console Puzzle Game", 0, slide: false);
                PagePanel.SlidePanel();
            }
            CursorTop = PagePanel?.LastCursorTop ?? 1;

            WriteHeader();
            WriteFooter();

            Console.SetCursorPosition(0, 0);
            bool changedPage = GetInput();
            if (changedPage) Startup();
        }

        protected override void BuildPage()
        {
            Header.Fill(7);

            Header.Lines[0].Add(new LineItem(CenterText(OptionDescription, bothSides: true), ConsoleColor.DarkMagenta, ConsoleColor.White));
            Header.Lines[1].Add(new LineItem(CenterText("", true, true)));
            Header.Lines[2].Add(new LineItem(menuController.GetString("Tutorials"), centerLine: true, alignRows: true));
            Header.Lines[3].Add(new LineItem(menuController.GetString("Levels"), centerLine: true, alignRows: true, active: false));
            Header.Lines[4].Add(new LineItem(menuController.GetString("Survival Mode"), centerLine: true, alignRows: true, active: false));
            Header.Lines[5].Add(new LineItem(menuController.GetString("Level Creator"), centerLine: true, alignRows: true, active: true));
            Header.Lines[6].Add(new LineItem(menuController.GetString("Options"), centerLine: true, alignRows: true));

            FirstOptionIndex = 2;

            DefaultFooter();

            PagePanel = new ConsolePanel(menuController.GetString("welcome to console puzzle game!"), delay: 50, textAlign: Alignment.Hidden);
            OnOptionChanged();
        }

        protected override bool OnOptionChosen()
        {
            switch (CurrentOptionIndex)
            {
                case 0:
                    menuController.OpenPage(Pages.Tutorials);
                    return true;

                case 3:
                    menuController.OpenPage(Pages.LevelCreator);
                    return true;

                case 4:
                    menuController.OpenPage(Pages.Options);
                    return true;
            }
            return false;
        }

        protected override string[] GenerateDescriptions()
        {
            return new string[]
            {
                menuController.GetString("Open Tutorials List"),
                menuController.GetString("Open Levels List"),
                menuController.GetString("Start Survival Mode"),
                menuController.GetString("Create Custom Levels"),
                menuController.GetString("Change Options and Settings")
            };
        }
    }
}
