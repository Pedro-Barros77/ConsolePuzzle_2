using System;
using System.Collections.Generic;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Basics
{
    internal class Wall : GameObject
    {
        public Wall(int x, int y) : base(ObjectTypes.Wall, x, y)
        {
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "[]";
            Content = Value;
            BracketsBgColor = ConsoleColor.DarkGray;
            BracketsFgColor = ConsoleColor.Black;
            BgColor = ConsoleColor.DarkGray;
            FgColor = ConsoleColor.Black;
        }

        public override GameObject NewCopy()
        {
            return new Wall(XPos, YPos)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Value = this.Value,
                Dir = this.Dir
            };
        }
    }
}
