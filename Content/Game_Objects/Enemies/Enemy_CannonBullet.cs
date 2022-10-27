using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Enemies
{
    internal class Enemy_CannonBullet : Enemy
    {
        public Enemy_CannonBullet(int x, int y, string name, Direction dir, Game gameLvl) : base(name, x, y, ObjectTypes.Enemy_CannonBullet, gameLvl)
        {
            Dir = dir;
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "()";
            Content = "o";
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.DarkGray;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.Red;
        }

        public override GameObject NewCopy()
        {
            return new Enemy_CannonBullet(XPos, YPos, this.Name, this.Dir, this.GameLvl)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Value = this.Value,
                Dir = this.Dir
            };
        }

        public override void Run()
        {
            if (delayToMove == null)
            {
                SW.Start();
                delayToMove = DateTime.Now.AddMilliseconds(100 / BulletSpeed) - DateTime.Now;
            }

            Coord moveDir = new Coord(0, 0).MapDirection(Dir);

            if (SW.Elapsed >= delayToMove)
            {
                bool moved = Move(this, moveDir.X, moveDir.Y);

                if (!moved)
                {
                    GameLvl.BoardEnemies.Remove(Name);
                    GameLvl.BoardObjects[YPos][XPos] = GameLvl.BoardObjects[YPos][XPos].ObjectTypeUnderThis.NewCopy();
                    GameLvl.BoardObjects[YPos][XPos].UpdateUI();
                }

                SW.Stop();
                SW.Reset();
                delayToMove = null;
            }
            if ((XPos == GameLvl.PlayerXPos && YPos == GameLvl.PlayerYPos) || (XPos == GameLvl.Player2XPos && YPos == GameLvl.Player2YPos && GameLvl.IsMultiplayer))
            {
                gameController.KillPlayer(GameLvl);
            }
        }
    }
}
