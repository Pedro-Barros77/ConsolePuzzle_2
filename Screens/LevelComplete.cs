using static ConsolePuzzle_2.Utility.Enums;
using static ConsolePuzzle_2.Utility.ConsoleUtility;
using ConsolePuzzle_2.Services.Models;
using ConsolePuzzle_2.Content;

namespace ConsolePuzzle_2.Screens
{
    public class LevelComplete : Page
    {
        private readonly Level gameLevel;
        private readonly bool isTutorial;
        public LevelComplete(Level gameLvl) : base(Pages.CompletedLevel)
        {
            gameLevel = gameLvl;
            string gameClass = gameLevel.ToString() ?? "";
            isTutorial = gameClass.Contains("Tutorials");
            OptionsDescriptions = GenerateDescriptions();

            CurrentOptionIndex = 0;
            Console.Clear();
            BuildPage();
        }

        public override void Startup()
        {
            if (isTutorial)
            {
                if (!gameController.CompletedTutorials.Contains(gameLevel.LevelNumber))
                    gameController.CompletedTutorials.Add(gameLevel.LevelNumber);
            }
            else
            {
                if (!gameController.CompletedLevels.Contains(gameLevel.LevelNumber))
                    gameController.CompletedLevels.Add(gameLevel.LevelNumber);
            }

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
            string select = isTutorial ? menuController.GetString("Select Tutorial") : menuController.GetString("Select Level");

            Header.Fill(6);
            Header.Lines[0].Add(new LineItem(CenterText(OptionDescription, bothSides: true), ConsoleColor.DarkMagenta, ConsoleColor.White));
            Header.Lines[1].Add(new LineItem(CenterText("", true, true)));
            Header.Lines[2].Add(new LineItem(menuController.GetString("Next Level"), centerLine: true, alignRows: true));
            Header.Lines[3].Add(new LineItem(menuController.GetString("Restart"), centerLine: true, alignRows: true));
            Header.Lines[4].Add(new LineItem(select, centerLine: true, alignRows: true));
            Header.Lines[5].Add(new LineItem(menuController.GetString("Main Menu"), centerLine: true, alignRows: true));

            FirstOptionIndex = 2;

            DefaultFooter();

            PagePanel = new(menuController.GetString("Level Completed"), slide: false);
        }

        protected override bool OnOptionChosen()
        {
            gameController.ResetKey();
            switch (CurrentOptionIndex)
            {
                case 0:
                    if (gameController.TutorialsList.Length > gameLevel.LevelNumber && gameController.TutorialsList.Length - 1 >= gameController.CompletedTutorials.Count && Header.Lines[CurrentOptionIndex + FirstOptionIndex][0].Active)
                    {
                        Type? levelType = gameController.TutorialsList[gameLevel.LevelNumber];
                        if (levelType == null)
                            return false;
                        Level chosenTutorial = (Level)Activator.CreateInstance(levelType)!;
                        if (chosenTutorial == null) return false;
                        gameController.Play(chosenTutorial);
                        return true;
                    }
                    break;
                case 1:
                    gameController.RestartLevel(gameLevel);
                    break;
                case 2:
                    menuController.ResetPagesAndScheduleNew(Pages.Tutorials, gameLevel);
                    break;
                case 3:
                    menuController.ResetPagesAndScheduleNew(Pages.MainMenu, gameLevel);
                    break;
            }

            return false;
        }

        protected override string[] GenerateDescriptions()
        {
            string levelType = isTutorial ? menuController.GetString("Tutorial") : menuController.GetString("Level");

            return new string[]
            {
                menuController.GetString("Start") + " " + gameLevel.ActiveGame.Title[0..^2].Replace("_"," ") + (gameLevel.LevelNumber +1).ToString().PadLeft(2, '0'),
                menuController.GetString("Restart") + " " + gameLevel.ActiveGame.Title.Replace("_"," "),
                menuController.GetString("Select another") + $" {levelType} " + menuController.GetString("to Start"),
                menuController.GetString("Go back to Main Menu")
            };
        }
    }
}
