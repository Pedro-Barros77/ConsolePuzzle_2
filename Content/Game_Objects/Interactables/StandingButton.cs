using System;
using System.Collections.Generic;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Interactable
{
    internal class StandingButton : GameObject
    {
        public StandingButton(int x, int y, int code) : base(ObjectTypes.StandingButton, x, y)
        {
            Value = code.ToString();
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "()";
            Content = "-";
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.White;
            BgColor = ConsoleColor.DarkGray;
            FgColor = ConsoleColor.Gray;
        }

        public override GameObject NewCopy()
        {
            return new StandingButton(XPos, YPos, int.Parse(this.Value))
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Value = this.Value,
                Dir = this.Dir
            };
        }
    }
}
