using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Gates
{
    internal class PlayerDoor : GameObject
    {
        public PlayerDoor(int x, int y, int playerNumber) : base(ObjectTypes.PlayerDoor, x, y)
        {
            Value = playerNumber.ToString();
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "[]";
            Content = "p";
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = Value == "1" ? ConsoleColor.Magenta : ConsoleColor.Blue;
            BgColor = ConsoleColor.Black;
            FgColor = Value == "1" ? ConsoleColor.DarkMagenta : ConsoleColor.DarkBlue;
        }

        public override GameObject NewCopy()
        {
            return new PlayerDoor(XPos, YPos, int.Parse(this.Value))
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Dir = this.Dir
            };
        }
    }
}
