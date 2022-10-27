﻿using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.ConsoleUtility;
using ConsolePuzzle_2.Utility.Extensions;

namespace ConsolePuzzle_2.Content.Tutorials
{
    public class Tutorial_12 : Level
    {
        public override void Startup()
        {
            string json = @"
                {
                  'mapping': [
                    '(P)&[ ]&( )&( )&( )&( )&( )&( )&( )&[ ]&( )&[ ]',
                    '( )&[ ]&( )&[ ]&[ ]&[ ]&[ ]&[ ]&( )&[ ]&( )&( )',
                    '( )&( )&( )&[ ]&( )&( )&( )&[ ]&[ ]&[ ]&[ ]&( )',
                    '( )&[ ]&( )&( )&( )&[ ]&( )&( )&( )&( )&( )&( )',
                    '( )&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&( )',
                    '( )&( )&( )&( )&( )&( )&( )&( )&( )&( )&[ ]&( )',
                    '[ ]&[ ]&( )&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&( )&[ ]&( )',
                    '( )&[ ]&( )&( )&( )&( )&( )&( )&( )&( )&[ ]&( )',
                    '( )&[ ]&( )&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&[ ]&( )',
                    '( )&( )&( )&[ ]&( )&( )&( )&[ ]&($)&[ ]&( )&( )',
                    '( )&[ ]&[ ]&[ ]&( )&[ ]&( )&[ ]&( )&[ ]&[1]&[ ]',
                    '( )&( )&( )&( )&( )&[ ]&( )&( )&( )&[ ]&( )&(@)'
                  ],
                  'config': {
                    'enemiesData': []
                  }
                }
            ".CompactJSON();

            ActiveGame = new Game(json, this)
            {
                P1_XField = 3,
                P1_YField = 3
            };

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
            Header.Fill(4);

            Header.Lines[0].Add(new LineItem(CenterText(ActiveGame.Title.Replace('_', ' '), bothSides: true), ConsoleColor.DarkMagenta, ConsoleColor.White));
            Header.Lines[1].Add(new LineItem(""));
            Header.Lines[2].Add(new LineItem(menuController.GetString("Sometimes you can't see everything arround you") + ".", centerLine: true));
            Header.Lines[3].Add(new LineItem(menuController.GetString("Caution!"), centerLine: true));

            DefaultGameFooter(ActiveGame);

            ActiveGame.Header = Header;
            ActiveGame.Footer = Footer;
        }

        //Not used in game-pages
        protected override bool OnOptionChosen() { return false; }
        protected override string[] GenerateDescriptions() { return Array.Empty<string>(); }
    }
}
