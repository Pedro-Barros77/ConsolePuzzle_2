using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Basics
{
    internal class Player : GameObject
    {
        public Player(int x, int y) : base(ObjectTypes.Player, x, y)
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
            FgColor = ConsoleColor.Magenta;
        }

        public override GameObject NewCopy()
        {
            return new Player(XPos, YPos)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Value = this.Value,
                Dir = this.Dir
            };
        }
    }
}
