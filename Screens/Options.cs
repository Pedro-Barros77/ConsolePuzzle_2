using ConsolePuzzle_2.Services;
using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.Enums;
using static ConsolePuzzle_2.Utility.ConsoleUtility;
using ConsolePuzzle_2.Utility;

namespace ConsolePuzzle_2.Screens
{
    public class Options : Page
    {
        private readonly int LANG_COUNT;
        public Options() : base(Pages.Options)
        {
            OptionsDescriptions = GenerateDescriptions();
            CurrentOptionIndex = 0;
            Console.Clear();
            BuildPage();
            LANG_COUNT = Enum.GetNames(typeof(Languages)).Length;
        }
        
        public override void Startup()
        {
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Console.Write(new string(' ', Console.WindowWidth - 1));
        }

        public override void Run()
        {
            Console.CursorTop = 1;

            PagePanel?.SlidePanel();

            CursorTop = PagePanel?.LastCursorTop ?? 1;

            WriteHeader();
            WriteFooter();

            Console.SetCursorPosition(0, 0);
            bool changedPage = GetInput();
            if (changedPage) Startup();
        }

        protected override void BuildPage()
        {
            Header.Fill(6);

            Header.Lines[0].Add(new LineItem(CenterText(OptionDescription, bothSides: true), ConsoleColor.DarkMagenta, ConsoleColor.White));
            Header.Lines[1].Add(new LineItem(CenterText("", true, true)));

            Header.Lines[2].Add(new LineItem(menuController.GetString("Language") + ": ", centerLine: true, alignRows: true));
            Header.Lines[2].Add(new LineItem(menuController.GetString(ConfigManager.Language.ToString()), fgColor: ConsoleColor.Green, centerLine: false, alignRows: true));


            Header.Lines[3].Add(new LineItem(menuController.GetString("Sound Effects") + ": ", centerLine: true, alignRows: true));
            if (ConfigManager.HasSoundFX)
                Header.Lines[3].Add(new LineItem(menuController.GetString("On"), fgColor: ConsoleColor.Green, centerLine: false, alignRows: true));
            else
                Header.Lines[3].Add(new LineItem(menuController.GetString("Off"), fgColor: ConsoleColor.Red, centerLine: false, alignRows: true));


            Header.Lines[4].Add(new LineItem(menuController.GetString("Music") + ": ", centerLine: true, alignRows: true));
            if (ConfigManager.HasMusic)
                Header.Lines[4].Add(new LineItem(menuController.GetString("On"), fgColor: ConsoleColor.Green, centerLine: false, alignRows: true));
            else
                Header.Lines[4].Add(new LineItem(menuController.GetString("Off"), fgColor: ConsoleColor.Red, centerLine: false, alignRows: true));


            Header.Lines[5].Add(new LineItem(menuController.GetString("Dev Code") + ": ", centerLine: true, alignRows: true));

            FirstOptionIndex = 2;

            DefaultFooter();

            PagePanel = new ConsolePanel(menuController.GetString("Options"), slide: false);
            OnOptionChanged();
        }

        protected override bool OnOptionChosen()
        {
            switch (CurrentOptionIndex)
            {
                case 0:
                    SetLanguage();
                    menuController.GenerateConsoleTitle(menuController.CurrentPage);
                    OnOptionChanged();
                    break;
                case 1:
                    ConfigManager.SetSoundFX(!ConfigManager.HasSoundFX);
                    break;
                case 3:
                    HandleCheats();
                    break;
            }
            UpdateUI();
            return false;
        }

        private void SetLanguage()
        {
            int currentLanguage = (int)ConfigManager.Language;
            if (LANG_COUNT > currentLanguage + 1)
                currentLanguage++;
            else
                currentLanguage = 0;
            ConfigManager.SetLanguage((Languages)currentLanguage);
        }

        private void HandleCheats()
        {
            string devCode = ReadClipboard();

            switch (devCode)
            {
                case "ulckall":
                    int a = gameController.TutorialsList.Length - 1;
                    for (int i = 0; i < a; i++)
                    {
                        //gameController.CompletedTutorials.Add(new ConsolePuzzleGame.Tutorials.Tutorial_01());
                    }
                    Console.SetCursorPosition((Console.WindowWidth / 2), FirstOptionIndex + CurrentOptionIndex + 6);
                    //if (gameController.CompletedTutorials.Count == gameController.TutorialsList.Count)
                    //{
                    //    Console.Write("Níveis Desbloqueados!");
                    //}
                    Thread.Sleep(500);
                    break;
            }
        }

        protected override string[] GenerateDescriptions()
        {
            return new string[]
            {
                menuController.GetString("Change game language"),
                menuController.GetString("Enable/Disable game sound effects"),
                menuController.GetString("Enable/Disable game music"),
                menuController.GetString("Execute special commands (developer only)")
            };
        }
    }
}
