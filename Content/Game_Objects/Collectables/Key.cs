using System;
using System.Collections.Generic;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Collectables
{
    internal class Key : GameObject
    {
        public Key(int x, int y, char keyLetter) : base(ObjectTypes.Key, x, y)
        {
            Content = keyLetter.ToString();
            Value = keyLetter.ToString();
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "()";
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.DarkGray;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.Cyan;
        }

        public override GameObject NewCopy()
        {
            return new Key(XPos, YPos, this.Value[0])
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Dir = this.Dir
            };
        }
    }
}
