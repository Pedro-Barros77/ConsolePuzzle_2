using ConsolePuzzle_2.Services.Models;
using ConsolePuzzle_2.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Enemies
{
    internal class Enemy_Guard : Enemy
    {
        private bool waitingDelay = false;
        private bool firstMove = true;
        private int x = 0, y = 0;
        public Enemy_Guard(int x, int y, string name, Game gameLvl) : base(name, x, y, ObjectTypes.Enemy_Guard, gameLvl)
        {
            UpdateUI();
        }

        public override void UpdateUI()
        {
            Brackets = "()";
            Content = "X";
            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.DarkGray;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.Red;
        }

        public override GameObject NewCopy()
        {
            return new Enemy_Guard(XPos, YPos, this.Name, this.GameLvl)
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
                if (firstMove)
                {
                    delayToMove = DateTime.Now.AddMilliseconds(StartDelay) - DateTime.Now;
                    firstMove = false;
                }
                else
                {
                    delayToMove = DateTime.Now.AddMilliseconds(100 / Speed) - DateTime.Now;
                }
            }

            if (SW.Elapsed >= TimeSpan.FromMilliseconds(Delay))
                waitingDelay = false;

            Coord moveDir = new Coord(0, 0).MapDirection(Dir);
            y = moveDir.Y;
            x = moveDir.X;

            if (SW.Elapsed >= delayToMove && !waitingDelay)
            {
                bool canMove;

                canMove = Move(this, x, y);

                if (!canMove)
                {
                    waitingDelay = true;
                    Dir = Dir switch
                    {
                        Direction.Up => Direction.Down,
                        Direction.Right => Direction.Left,
                        Direction.Down => Direction.Up,
                        Direction.Left => Direction.Right,
                        _ => Direction.Left
                    };

                    x = x < 0 ? x * x : x * -x;
                    y = y < 0 ? y * y : y * -y;
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
