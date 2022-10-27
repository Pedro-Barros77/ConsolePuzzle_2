using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Gates
{
    internal class CodeDoor : GameObject
    {
        private readonly Game gameLvl;
        public CodeDoor(int x, int y, int code, Game gameLvl) : base(ObjectTypes.CodeDoor, x, y)
        {
            Value = code.ToString();
            this.gameLvl = gameLvl;
            UpdateUI();
        }

        public override void UpdateUI()
        {
            if (gameLvl.PressedButtons != null)
                Brackets = gameLvl.PressedButtons.Contains(Value[0]) ? "{}" : "[]";
            else
                Brackets = "[]";
            Content = Value.Length == 1 ? Value : "-";
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.DarkCyan;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.Cyan;
        }

        public override GameObject NewCopy()
        {
            return new CodeDoor(XPos, YPos, int.Parse(this.Value), this.gameLvl)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Dir = this.Dir
            };
        }
    }
}
