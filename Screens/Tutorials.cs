using System;
using System.Collections.Generic;
using static ConsolePuzzle_2.Utility.Enums;
using static ConsolePuzzle_2.Utility.ConsoleUtility;
using ConsolePuzzle_2.Services.Models;
using ConsolePuzzle_2.Content;

namespace ConsolePuzzle_2.Screens
{
    public class Tutorials : Page
    {
        public Tutorials() : base(Pages.Tutorials)
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
            Header.Fill(14);
            FirstOptionIndex = 2;
            OptionsDescriptions = GenerateDescriptions();

            Header.Lines[0].Add(new LineItem(CenterText(OptionDescription, bothSides: true), ConsoleColor.DarkMagenta, ConsoleColor.White));
            Header.Lines[1].Add(new LineItem(CenterText("", true, true)));
            Header.Lines[2].Add(new LineItem("01- " + menuController.GetString("Movement"), centerLine: true, alignRows: true, active: true));
            Header.Lines[3].Add(new LineItem("02- " + menuController.GetString("Walls"), centerLine: true, alignRows: true, active: gameController.CompletedTutorials.Count >= 2));
            Header.Lines[4].Add(new LineItem("03- " + menuController.GetString("Coins"), centerLine: true, alignRows: true, active: gameController.CompletedTutorials.Count >= 3));
            Header.Lines[5].Add(new LineItem("04- " + menuController.GetString("Keys"), centerLine: true, alignRows: true, active: gameController.CompletedTutorials.Count >= 4));
            Header.Lines[6].Add(new LineItem("05- " + menuController.GetString("One-Ways"), centerLine: true, alignRows: true, active: gameController.CompletedTutorials.Count >= 5));
            Header.Lines[7].Add(new LineItem("06- " + menuController.GetString("Portals"), centerLine: true, alignRows: true, active: gameController.CompletedTutorials.Count >= 6));
            Header.Lines[8].Add(new LineItem("07- " + menuController.GetString("Boxes"), centerLine: true, alignRows: true, active: gameController.CompletedTutorials.Count >= 7));
            Header.Lines[9].Add(new LineItem("08- " + menuController.GetString("The Guards"), centerLine: true, alignRows: true, active: gameController.CompletedTutorials.Count >= 8));
            Header.Lines[10].Add(new LineItem("09- " + menuController.GetString("The Cannons"), centerLine: true, alignRows: true, active: gameController.CompletedTutorials.Count >= 9 || true));
            Header.Lines[11].Add(new LineItem("10- " + menuController.GetString("The Seekers"), centerLine: true, alignRows: true, active: gameController.CompletedTutorials.Count >= 10));
            Header.Lines[12].Add(new LineItem("11- " + menuController.GetString("Multiplayer"), centerLine: true, alignRows: true, active: gameController.CompletedTutorials.Count >= 11));
            Header.Lines[13].Add(new LineItem("12- " + menuController.GetString("Field of View"), centerLine: true, alignRows: true, active: gameController.CompletedTutorials.Count >= 11));


            DefaultFooter();

            PagePanel = new(menuController.GetString("Tutorials"),  slide: false);
        }

        protected override bool OnOptionChosen()
        {
            gameController.ResetKey();
            //If there is a tutorial for the current index and if it's active
            if (gameController.TutorialsList.Length - 1 >= CurrentOptionIndex && Header.Lines[CurrentOptionIndex + FirstOptionIndex][0].Active)
            {
                Type? levelType = gameController.TutorialsList[CurrentOptionIndex];
                if(levelType == null)
                    return false;
                Level chosenTutorial = (Level)Activator.CreateInstance(levelType)!;
                if (chosenTutorial == null) return false;
                gameController.Play(chosenTutorial);
                return true;
            }
            return false;
        }

        protected override string[] GenerateDescriptions()
        {
            return Enumerable.Range(1, Header.Lines.Count - FirstOptionIndex + 1).Select(i => $"{menuController.GetString("Start Tutorial")} {i.ToString().PadLeft(2, '0')}").ToArray();
        }
    }
}
