using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Basics
{
    internal class Blank : GameObject
    {
        public Blank(int x, int y) : base(ObjectTypes.Blank, x, y)
        {
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "()";
            Content = " ";
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.DarkGray;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.DarkGray;
        }

        public override GameObject NewCopy()
        {
            return new Blank(XPos, YPos)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Value = this.Value,
                Dir = this.Dir
            };
        }
    }
}
