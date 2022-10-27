using System.Diagnostics;
using ConsolePuzzle_2.Content;
using ConsolePuzzle_2.Content.Game_Objects;
using ConsolePuzzle_2.Content.Game_Objects.Basics;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2
{
    public abstract class Enemy : GameObject
    {
        /// <summary>
        /// The name/id of this enemy so it can be found later.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The radius distance that a enemy can see the player.
        /// </summary>
        public int SightRadius { get; set; }
        /// <summary>
        /// The movement speed of bullets shot by the enemy.
        /// </summary>
        public float BulletSpeed { get; set; }
        /// <summary>
        /// The movement speed of the enemy.
        /// </summary>
        public float Speed { get; set; }
        /// <summary>
        /// The time in milliseconds the enemy will wait to change direction after hiting the end of it's course.
        /// </summary>
        public float Delay { get; set; }
        /// <summary>
        /// The time in milliseconds the enemy will wait before starting to move/shoot.
        /// </summary>
        public float StartDelay { get; set; }
        /// <summary>
        /// The rate wich the enemy shoots bullets.
        /// </summary>
        public float RateOfFire { get; set; }

        public float StartBulletSpeed { get; set; }
        public float StartRateOfFire { get; set; }


        protected Game GameLvl;
        protected TimeSpan? delayToMove = null;
        protected readonly Stopwatch SW;

        protected Enemy(string name, int x, int y, ObjectTypes objectType, Game gameLvl) : base(objectType, x, y)
        {
            Name = name;
            GameLvl = gameLvl;

            SW = new();

            //Default values
            SightRadius = 2;
            BulletSpeed = 1;
            Speed = 0.3f;
            Delay = 0;
            StartDelay = 0;
            RateOfFire = 0.5f;
        }

        public abstract override GameObject NewCopy();

        /// <summary>
        /// Moves the enemy x/y steps in each direction.
        /// </summary>
        /// <param name="enemy">The enemy to be moved.</param>
        /// <param name="x">The x step.</param>
        /// <param name="y">The y step.</param>
        /// <returns>True if moved successfully, otherwise, false.</returns>
        public bool Move(Enemy enemy, int x, int y)
        {
            if (!GameLvl.IsInBoardBounds(enemy.XPos + x, enemy.YPos + y))
                return false;

            GameObject _destination = GameLvl.BoardObjects[enemy.YPos + y][enemy.XPos + x];
            GameObject _originalObj = GameLvl.BoardObjects[enemy.YPos][enemy.XPos].ObjectTypeUnderThis ?? new Blank(enemy.XPos, enemy.YPos);

            if (_destination.ObjectType.Equals(ObjectTypes.OneWay))
            {
                if (!GameLvl.IsInBoardBounds(enemy.XPos + x*2, enemy.YPos + y*2))
                    return false;

                if ((_destination.Dir == Direction.Up && y < 0) || (_destination.Dir == Direction.Down && y > 0) ||
                    (_destination.Dir == Direction.Left && x < 0) || (_destination.Dir == Direction.Right && x > 0))
                {
                    x *= 2;
                    y *= 2;
                    _destination = GameLvl.BoardObjects[enemy.YPos + y][enemy.XPos + x];
                }
                else
                    return false;
            }

            if (!gameController.EnemiesContainers.Contains(_destination.ObjectType))
                return false;

            if (!GameLvl.CanEnterGate(_destination, true))
                return false;


            GameObject _destinationOriginalObj = _destination?.NewCopy() ?? new Blank(_destination!.XPos, _destination.YPos);

            //sets enemy to destination
            GameLvl.BoardObjects[_destination.YPos][_destination.XPos] = GameLvl.BoardObjects[enemy.YPos][enemy.XPos];
            GameLvl.BoardObjects[_destination.YPos][_destination.XPos].XPos += x;
            GameLvl.BoardObjects[_destination.YPos][_destination.XPos].YPos += y;

            //moves destination to background
            GameLvl.BoardObjects[_destination.YPos][_destination.XPos].ObjectTypeUnderThis = _destinationOriginalObj;

            if (!enemy.ObjectType.Equals(ObjectTypes.Enemy_Seeker))
            {
                GameObject _destObj = GameLvl.BoardObjects[_destination.YPos][_destination.XPos];
                _destObj.Brackets = _destinationOriginalObj.Brackets;
                _destObj.BracketsBgColor = _destinationOriginalObj.BracketsBgColor;

                if (GameLvl.CanEnterGate(_destinationOriginalObj, false))
                {
                    _destObj.Brackets = "{}";
                }


                if (!_destObj.ObjectType.Equals(ObjectTypes.StandingButton))
                    _destObj.BracketsFgColor = _destinationOriginalObj.BracketsFgColor;
                _destObj.BgColor = _destinationOriginalObj.BgColor;
            }

            //sets enemy old position to old object
            GameLvl.BoardObjects[enemy.YPos + (-y)][enemy.XPos + (-x)] = _originalObj.NewCopy();
            GameLvl.BoardObjects[enemy.YPos + (-y)][enemy.XPos + (-x)].XPos = enemy.XPos + (-x);
            GameLvl.BoardObjects[enemy.YPos + (-y)][enemy.XPos + (-x)].YPos = enemy.YPos + (-y);

            if (enemy.ObjectTypeUnderThis.ObjectType.Equals(ObjectTypes.StandingButton))
            {
                if (!GameLvl.PressedButtons.Contains(int.Parse(enemy.ObjectTypeUnderThis.Value)))
                    GameLvl.PressedButtons.Add(int.Parse(enemy.ObjectTypeUnderThis.Value));

                GameLvl.BoardObjects[enemy.YPos][enemy.XPos].BracketsFgColor = ConsoleColor.Green;
                GameLvl.BoardObjects[enemy.YPos][enemy.XPos].BgColor = enemy.ObjectTypeUnderThis.BgColor;
            }

            if (GameLvl.IsInBoardBounds(_originalObj.XPos, _originalObj.YPos) && _originalObj.ObjectType.Equals(ObjectTypes.StandingButton))
            {
                GameLvl.BoardObjects[_originalObj.YPos][_originalObj.XPos].UpdateUI();

                if (GameLvl.PressedButtons.Contains(int.Parse(_originalObj.Value)))
                    GameLvl.PressedButtons.Remove(int.Parse(_originalObj.Value));
            }

            return true;
        }

        public override abstract void UpdateUI();

        /// <summary>
        /// Runs the enemy script once.
        /// </summary>
        public abstract void Run();
    }
}
