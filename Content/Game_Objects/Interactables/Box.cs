using ConsolePuzzle_2.Content.Game_Objects.Basics;
using System;
using System.Collections.Generic;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Interactable
{
    internal class Box : GameObject
    {
        public Box(int x, int y) : base(ObjectTypes.Box, x, y)
        {
            this.ObjectTypeUnderThis = new Blank(x, y);
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "()";
            Content = "#";
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.DarkGray;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.DarkYellow;
        }

        public override GameObject NewCopy()
        {
            return new Box(XPos, YPos)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Value = this.Value,
                Dir = this.Dir
            };
        }
    }
}
