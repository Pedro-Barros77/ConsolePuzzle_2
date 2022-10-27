using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Gates
{
    internal class KeyDoor : GameObject
    {
        private readonly Game gameLvl;
        public KeyDoor(int x, int y, char keyLetter, Game gameLvl) : base(ObjectTypes.KeyDoor, x, y)
        {
            Value = keyLetter.ToString();
            this.gameLvl = gameLvl;
            UpdateUI();
        }

        public override void UpdateUI()
        {
            if (gameLvl.CollectedKeys != null)
                Brackets = gameLvl.CollectedKeys.Contains(Value[0]) ? "{}" : "[]";
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
            return new KeyDoor(XPos, YPos, this.Value[0], this.gameLvl)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Dir = this.Dir
            };
        }
    }
}
