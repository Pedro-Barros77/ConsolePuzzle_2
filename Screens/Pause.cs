using System;
using System.Collections.Generic;
using ConsolePuzzle_2.Content;
using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.Enums;
using static ConsolePuzzle_2.Utility.ConsoleUtility;

namespace ConsolePuzzle_2.Screens
{
    public class Pause : Page
    {
        private readonly Level gameLevel;
        private readonly bool isTutorial;
        private readonly string LevelName;

        public Pause(Level gameLvl) : base(Pages.Pause)
        {
            gameLevel = gameLvl;
            string gameClass = gameLevel.ToString() ?? "";
            isTutorial = gameClass.Contains("Tutorials");
            LevelName = gameClass.Split('.').Last();
            OptionsDescriptions = GenerateDescriptions();
            LevelNumber = gameLevel.LevelNumber;

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
            Header.Fill(7);

            string select = isTutorial ? menuController.GetString("Select Tutorial") : menuController.GetString("Select Level");

            Header.Lines[0].Add(new LineItem(CenterText(OptionDescription, bothSides: true), ConsoleColor.DarkMagenta, ConsoleColor.White));
            Header.Lines[1].Add(new LineItem(CenterText("", true, true)));
            Header.Lines[2].Add(new LineItem(menuController.GetString("Continue"), centerLine: true, alignRows: true));
            Header.Lines[3].Add(new LineItem(menuController.GetString("Restart"), centerLine: true, alignRows: true));
            Header.Lines[4].Add(new LineItem(select, centerLine: true, alignRows: true));
            Header.Lines[5].Add(new LineItem(menuController.GetString("Options"), centerLine: true, alignRows: true));
            Header.Lines[6].Add(new LineItem(menuController.GetString("Main Menu"), centerLine: true, alignRows: true));

            FirstOptionIndex = 2;
            DefaultFooter();

            PagePanel = new ConsolePanel(menuController.GetString("Game Paused"), slide: false);
        }

        protected override bool OnOptionChosen()
        {
            switch (CurrentOptionIndex)
            {
                case 0:
                    CurrentOptionIndex = 0;
                    gameController.ContinueGame(gameLevel);
                    break;
                case 1:
                    gameController.RestartLevel(gameLevel);
                    break;
                case 2:
                    menuController.ResetPagesAndScheduleNew(Pages.Tutorials);
                    return true;
                case 3:
                    gameController.Playing = false;
                    menuController.OpenPage(Pages.Options, gameLevel);
                    return true;
                case 4:
                    menuController.ResetPagesAndScheduleNew(Pages.MainMenu);
                    return true;
            }
            return false;
        }

        protected override void OnGoBack()
        {
            base.OnGoBack();
            gameLevel.WriteGameUI();
            gameController.Playing = true;
        }

        protected override string[] GenerateDescriptions()
        {
            string levelType = isTutorial ? menuController.GetString("Tutorial") : menuController.GetString("Level");

            return new string[]
            {
                menuController.GetString("Continue") + " " + LevelName,
                menuController.GetString("Restart") + " " + LevelName,
                menuController.GetString("Select another") + $" {levelType} " + menuController.GetString("to Start"),
                menuController.GetString("Change Options and Settings"),
                menuController.GetString("Go back to Main Menu")
            };
        }
    }
}
