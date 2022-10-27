using ConsolePuzzle_2.Services.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects.Enemies
{
    internal class Enemy_Cannon : Enemy
    {
        private int bulletsCounter;
        private Coord bulletOffset = new(0, 0);
        private bool firstShot = true;
        public Enemy_Cannon(int x, int y, string name, Direction dir, Game gameLvl) : base(name, x, y, ObjectTypes.Enemy_Cannon, gameLvl)
        {
            Dir = dir;
            UpdateUI();
        }

        public override void UpdateUI()
        {
            switch (Dir)
            {
                case Direction.Up:
                    Brackets = @"/\";
                    Content = "'";
                    break;
                case Direction.Right:
                    Brackets = "[=";
                    Content = ")";
                    break;
                case Direction.Down:
                    Brackets = @"\/";
                    Content = ".";
                    break;
                case Direction.Left:
                    Brackets = "=]";
                    Content = "(";
                    break;
            }

            BracketsBgColor = ConsoleColor.Black;
            BracketsFgColor = ConsoleColor.Red;
            BgColor = ConsoleColor.Black;
            FgColor = ConsoleColor.Red;
            bulletOffset = new Coord(0, 0).MapDirection(Dir);
        }

        public override GameObject NewCopy()
        {
            return new Enemy_Cannon(XPos, YPos, this.Name, this.Dir, this.GameLvl)
            {
                ObjectTypeUnderThis = this.ObjectTypeUnderThis,
                Value = this.Value
            };
        }

        public override void Run()
        {
            if (delayToMove == null)
            {
                SW.Start();
                if (firstShot)
                {
                    delayToMove = DateTime.Now.AddMilliseconds(StartDelay) - DateTime.Now;
                }
                else
                {
                    delayToMove = DateTime.Now.AddMilliseconds(1000 / RateOfFire) - DateTime.Now;
                }
            }

            if (SW.Elapsed >= delayToMove || (firstShot && StartDelay == 0))
            {
                SW.Stop();
                SW.Reset();
                delayToMove = null;
                firstShot = false;

                Coord bulletPos = new(XPos + bulletOffset.X, YPos + bulletOffset.Y);

                if (!GameLvl.IsInBoardBounds(bulletPos.X, bulletPos.Y))
                    return;

                GameObject spawnContainer = GameLvl.BoardObjects[bulletPos.Y][bulletPos.X];

                if (!gameController.EnemiesContainers.Contains(spawnContainer.ObjectType) || !gameController.EnemiesContainers.Contains(spawnContainer.ObjectTypeUnderThis.ObjectType))
                    return;

                if (!GameLvl.CanEnterGate(spawnContainer, true))
                    return;

                switch (spawnContainer.ObjectType)
                {
                    case ObjectTypes.OneWay:
                        if (spawnContainer.Dir != Dir)
                            return;
                        bulletPos += bulletOffset;
                        spawnContainer = GameLvl.BoardObjects[bulletPos.Y][bulletPos.X];
                        if (!GameLvl.CanEnterGate(spawnContainer, true))
                            return;
                        break;
                    case ObjectTypes.StandingButton:
                        if (!GameLvl.PressedButtons.Contains(int.Parse(spawnContainer.Value)))
                            GameLvl.PressedButtons.Add(int.Parse(spawnContainer.Value));
                        break;
                    case ObjectTypes.Player:
                    case ObjectTypes.Player2:
                        gameController.KillPlayer(GameLvl);
                        break;
                }

                var bullet = new Enemy_CannonBullet(bulletPos.X, bulletPos.Y, Name + "_" + bulletsCounter, Dir, GameLvl)
                {
                    BulletSpeed = this.BulletSpeed,
                    ObjectTypeUnderThis = spawnContainer
                };

                if (GameLvl.CanEnterGate(spawnContainer, false))
                {
                    bullet.Brackets = "{}";
                    bullet.BracketsFgColor = spawnContainer.BracketsFgColor;
                }

                if (spawnContainer.ObjectType.Equals(ObjectTypes.StandingButton))
                {
                    bullet.BracketsFgColor = ConsoleColor.Green;
                    bullet.BgColor = spawnContainer.BgColor;
                }

                GameLvl.BoardObjects[bulletPos.Y][bulletPos.X] = bullet;
                GameLvl.BoardEnemies.Add(bullet.Name, bullet);
                bulletsCounter++;
            }
        }
    }
}
