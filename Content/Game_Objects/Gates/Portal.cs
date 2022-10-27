using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Gates
{
    internal class Portal : GameObject
    {
        public Portal(int x, int y, char value) : base(ObjectTypes.Portal, x, y)
        {
            Value = value.ToString();
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "||";
            Content = Value;
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.DarkBlue;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.Blue;
        }

        public override GameObject NewCopy()
        {
            return new Portal(XPos, YPos, this.Value[0])
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Dir = this.Dir
            };
        }
    }
}
