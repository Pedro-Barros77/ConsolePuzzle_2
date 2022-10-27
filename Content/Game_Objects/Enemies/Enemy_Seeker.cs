using ConsolePuzzle_2.Services.Models;
using ConsolePuzzle_2.Utility;
using System;
using System.Collections.Generic;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Enemies
{
    internal class Enemy_Seeker : Enemy
    {
        private Target target = Target.None;
        private Coord targetCoord;

        private Coord enemyPos;
        private Coord P1_pos;
        private Coord P2_pos;

        public Enemy_Seeker(int x, int y, string name, Game gameLvl) : base(name, x, y, ObjectTypes.Enemy_Seeker, gameLvl)
        {
            UpdateUI();
            targetCoord = new(0, 0);
            enemyPos = new(0, 0);
            P1_pos = new(0, 0);
            P2_pos = new(0, 0);
        }

        public override void UpdateUI()
        {
            if (!target.Equals(Target.None))
            {
                //sets horizontal pupil
                if (targetCoord.X < XPos)
                    Brackets = "0";
                else if (targetCoord.X > XPos)
                    Brackets = "2";
                else
                    Brackets = "1";

                //sets vertical pupil
                if (targetCoord.Y < YPos)
                    Content = "\'";
                else if (targetCoord.Y > YPos)
                    Content = ".";
                else
                    Content = "-";
            }
            else
            {
                Content = "?";
                Brackets = "1";
            }

            BracketsBgColor = ConsoleColor.DarkRed;
            BracketsFgColor = ConsoleColor.Black;
            BgColor = ConsoleColor.Red;
            FgColor = ConsoleColor.Black;
        }

        public override GameObject NewCopy()
        {
            return new Enemy_Seeker(XPos, YPos, this.Name, this.GameLvl)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Value = this.Value,
                Dir = this.Dir
            };
        }

        public override void Run()
        {
            enemyPos = new Coord(XPos, YPos);
            P1_pos = new Coord(GameLvl.PlayerXPos, GameLvl.PlayerYPos);
            P2_pos = new Coord(GameLvl.Player2XPos, GameLvl.Player2YPos);

            if (target.Equals(Target.None))
            {
                if (MathUtility.IsInRadius(P1_pos, enemyPos, SightRadius))
                {
                    target = Target.Player1;
                    targetCoord = P1_pos;
                }
                else if (MathUtility.IsInRadius(P2_pos, enemyPos, SightRadius))
                {
                    target = Target.Player2;
                    targetCoord = P2_pos;
                }
            }

            switch (target)
            {
                case Target.Player1:
                    targetCoord = P1_pos;
                    break;
                case Target.Player2:
                    targetCoord = P2_pos;
                    break;
                default:
                    return;
            }

            UpdateUI();

            if (delayToMove == null)
            {
                SW.Start();
            }

            delayToMove = DateTime.Now.AddMilliseconds(100 / Speed) - DateTime.Now;

            if (SW.Elapsed >= delayToMove)
            {
                Coord movementDir = GetDirection();
                Move(this, movementDir.X, movementDir.Y);

                SW.Stop();
                SW.Reset();
                delayToMove = null;
            }
            if ((XPos == GameLvl.PlayerXPos && YPos == GameLvl.PlayerYPos) || (XPos == GameLvl.Player2XPos && YPos == GameLvl.Player2YPos && GameLvl.IsMultiplayer && GameLvl.IsMultiplayer))
            {
                gameController.KillPlayer(GameLvl);
            }
        }

        private Coord GetDirection()
        {
            Coord _distance = new(Math.Abs(XPos - targetCoord.X), Math.Abs(YPos - targetCoord.Y));
            bool movehorizontal = false;

            Coord movementDir = new(0, 0);

            if (_distance.X < _distance.Y)
            {
                movehorizontal = true;

                int xInc = targetCoord.X > XPos ? 1 : -1;
                if (GameLvl.IsInBoardBounds(XPos + xInc, YPos))
                {
                    GameObject _destination = GameLvl.BoardObjects[YPos][XPos + xInc];
                    if (!gameController.EnemiesContainers.Contains(_destination.ObjectType) || !GameLvl.CanEnterGate(_destination, false) || _distance.X == 0)
                    {
                        movehorizontal = false;
                    }
                }
                else
                {
                    movehorizontal = false;
                }
            }
            else if (_distance.X > _distance.Y)
            {
                movehorizontal = false;

                int yInc = targetCoord.Y > YPos ? 1 : -1;
                if (GameLvl.IsInBoardBounds(XPos, YPos + yInc))
                {
                    GameObject _destination = GameLvl.BoardObjects[YPos + yInc][XPos];
                    if (!gameController.EnemiesContainers.Contains(_destination.ObjectType) || !GameLvl.CanEnterGate(_destination, false) || _distance.Y == 0)
                    {
                        movehorizontal = true;
                    }
                }
                else
                {
                    movehorizontal = true;
                }
            }


            if (movehorizontal)
            {
                if (targetCoord.X > XPos)
                {
                    movementDir.X = 1;
                }
                else if (targetCoord.X < XPos)
                {
                    movementDir.X = -1;
                }
            }
            else
            {
                if (targetCoord.Y > YPos)
                {
                    movementDir.Y = 1;
                }
                else if (targetCoord.Y < YPos)
                {
                    movementDir.Y = -1;
                }
            }

            return movementDir;
        }

        public (string value, ConsoleColor fg, ConsoleColor bg) GetPrintingValues(int i)
        {
            string _value = "";
            ConsoleColor _fg = ConsoleColor.Black, _bg = ConsoleColor.Black;

            switch (i)
            {
                case 0:
                    if (Brackets == "0")
                    {
                        _bg = BgColor;
                        _value = Content;
                    }
                    else
                    {
                        _bg = BracketsBgColor;
                        _value = " ";
                    }
                    break;

                case 1:
                    if (Brackets == "1")
                    {
                        _bg = BgColor;
                        _value = Content;
                    }
                    else
                    {
                        _bg = BracketsBgColor;
                        _value = " ";
                    }
                    break;

                case 2:
                    if (Brackets == "2")
                    {
                        _bg = BgColor;
                        _value = Content;
                    }
                    else
                    {
                        _bg = BracketsBgColor;
                        _value = " ";
                    }
                    break;
            }

            return (_value, _fg, _bg);
        }
    }
}
