using System;
using System.Collections.Generic;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Gates
{
    internal class CoinDoor : GameObject
    {
        private readonly Game gameLvl;
        public CoinDoor(int x, int y, int requiredCoins, Game gameLvl) : base(ObjectTypes.CoinDoor, x, y)
        {
            Value = requiredCoins.ToString();
            this.gameLvl = gameLvl;
            UpdateUI();
        }

        public override void UpdateUI()
        {
            if (Value == " " || Value.Length < 1)
                Value = "0";
            Brackets = int.Parse(Value) - gameLvl.CollectedCoins <= 0 ? "{}" : "[]";
            Content = Value;
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.DarkYellow;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.Yellow;
        }

        public override GameObject NewCopy()
        {
            return new CoinDoor(XPos, YPos, int.Parse(this.Value), this.gameLvl)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Dir = this.Dir
            };
        }
    }
}
