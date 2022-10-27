using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Collectables
{
    internal class Coin : GameObject
    {
        public Coin(int x, int y) : base(ObjectTypes.Coin, x, y)
        {
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "()";
            Content = "$";
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.DarkGray;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.Yellow;
        }

        public override GameObject NewCopy()
        {
            return new Coin(XPos, YPos)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Value = this.Value,
                Dir = this.Dir
            };
        }
    }
}
