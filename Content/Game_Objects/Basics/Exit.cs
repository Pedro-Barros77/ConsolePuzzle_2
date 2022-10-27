using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Basics
{
    internal class Exit : GameObject
    {
        public Exit(int x, int y) : base(ObjectTypes.Exit, x, y)
        {
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "()";
            Content = "@";
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.DarkGray;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.Green;
        }

        public override GameObject NewCopy()
        {
            return new Exit(XPos, YPos)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Value = this.Value,
                Dir = this.Dir
            };
        }
    }
}
