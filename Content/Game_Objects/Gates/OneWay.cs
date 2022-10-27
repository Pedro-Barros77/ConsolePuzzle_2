using System;
using System.Collections.Generic;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Gates
{
    internal class OneWay : GameObject
    {
        public OneWay(int x, int y, Direction dir) : base(ObjectTypes.OneWay, x, y)
        {
            Dir = dir;
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "{}";
            switch (Dir)
            {
                case Direction.Up:
                    Content = "↑";
                    break;
                case Direction.Right:
                    Content = ">";
                    break;
                case Direction.Down:
                    Content = "↓";
                    break;
                case Direction.Left:
                    Content = "<";
                    break;
            }
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.Gray;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.Gray;
        }

        public override GameObject NewCopy()
        {
            return new OneWay(XPos, YPos, this.Dir)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Value = this.Value
            };
        }
    }
}
