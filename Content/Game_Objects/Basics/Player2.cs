using System;
using System.Collections.Generic;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Basics
{
    internal class Player2: GameObject
    {
        public Player2(int x, int y) : base(ObjectTypes.Player2, x, y)
        {
            this.ObjectTypeUnderThis = new Blank(x, y);
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "()";
            Content = "P";
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.DarkGray;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.DarkBlue;
        }

        public override GameObject NewCopy()
        {
            return new Player2(XPos, YPos)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Value = this.Value,
                Dir = this.Dir
            };
        }
    }
}
