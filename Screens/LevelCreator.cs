using static ConsolePuzzle_2.Utility.Enums;
using static ConsolePuzzle_2.Utility.ConsoleUtility;
using ConsolePuzzle_2.Services.Models;
using System.Diagnostics;
using ConsolePuzzle_2.Content.Tutorials;
using ConsolePuzzle_2.Utility.Extensions;

namespace ConsolePuzzle_2.Screens
{
    public class LevelCreator : Page
    {
        public LevelCreator() : base(Pages.LevelCreator)
        {
            CurrentOptionIndex = 0;
            Console.Clear();
            BuildPage();
        }

        public override void Startup()
        {
            OnOptionChanged();
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
            Header.Fill(4);
            FirstOptionIndex = 2;
            OptionsDescriptions = GenerateDescriptions();

            Header.Lines[0].Add(new LineItem(CenterText(OptionDescription, bothSides: true), ConsoleColor.DarkMagenta, ConsoleColor.White));
            Header.Lines[1].Add(new LineItem(CenterText("", true, true)));
            Header.Lines[2].Add(new LineItem(menuController.GetString("Open creator (HTML file)"), centerLine: true, alignRows: true, active: true));
            Header.Lines[3].Add(new LineItem(menuController.GetString("Paste map JSON value"), centerLine: true, alignRows: true, active: true));


            DefaultFooter();

            PagePanel = new(menuController.GetString("Level Creator"), slide: false);
        }

        protected override bool OnOptionChosen()
        {
            gameController.ResetKey();
            switch (CurrentOptionIndex)
            {
                case 0:
                    string html = LevelEditorHTML.HtmlText;
                    string fileName = "CP2_LevelEditor.html";
                    string filePath = Path.Combine(menuController.ProjectRootPath, fileName);
                    
                    if (!File.Exists(filePath))
                        File.WriteAllText(filePath, html);

                    ProcessStartInfo pInfo = new(filePath)
                    {
                        UseShellExecute = true
                    };
                    Process.Start(pInfo);
                    break;
                case 1:
                    string value = ReadClipboard().CompactJSON();
                    gameController.ValidateBoardJson(value);
                    gameController.Play(new UserLevel(value));
                    break;
            }
            return false;
        }

        protected override string[] GenerateDescriptions()
        {
            return new[]
            {
                menuController.GetString("Open Level Creator HTML file on your browser"),
                menuController.GetString("Paste map JSON (from clipboard) to load and play the level")
            };
        }
    }
}
