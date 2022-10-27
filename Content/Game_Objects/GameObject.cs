using ConsolePuzzle_2.Content.Game_Objects.Basics;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Content.Game_Objects
{
    public abstract class GameObject
    {
        /// <summary>
        /// The type of the GameObject
        /// </summary>
        public ObjectTypes ObjectType { get; set; }
        /// <summary>
        /// The X position.
        /// </summary>
        public int XPos { get; set; }
        /// <summary>
        /// The Y Position
        /// </summary>
        public int YPos { get; set; }
        /// <summary>
        /// The color of the content foreground.
        /// </summary>
        public ConsoleColor FgColor { get; set; }
        /// <summary>
        /// The color of the content background.
        /// </summary>
        public ConsoleColor BgColor { get; set; }
        /// <summary>
        /// The color of the brackets background.
        /// </summary>
        public ConsoleColor BracketsBgColor { get; set; }
        /// <summary>
        /// The color of the brackets foreground.
        /// </summary>
        public ConsoleColor BracketsFgColor { get; set; }
        /// <summary>
        /// The text of the brackets.
        /// </summary>
        public string Brackets { get; set; }
        /// <summary>
        /// The text inside the brackets.
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// The value of it's object (such as door numbers and keys).
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// The direction of the object, if applies.
        /// </summary>
        public Direction Dir { get; set; }

        private GameObject _objectTypeUnderThis;
        /// <summary>
        /// The object under this object (such as a door under the player)
        /// </summary>
        public GameObject ObjectTypeUnderThis
        {
            get
            {
                return _objectTypeUnderThis ?? new Blank(XPos, YPos);
            }
            set
            {
                _objectTypeUnderThis = value;
                _objectTypeUnderThis.XPos = XPos;
                _objectTypeUnderThis.YPos = YPos;
            }
        }
        protected readonly GameController gameController;

        protected GameObject(ObjectTypes Type, int Xpos, int Ypos, string value = " ")
        {
            gameController = GameController.GetInstance();
            ObjectType = Type;

            if (ObjectType.Equals(ObjectTypes.Blank))
                _objectTypeUnderThis = null!;
            else
            {
                _objectTypeUnderThis = new Blank(Xpos, Ypos);
                ObjectTypeUnderThis.UpdateUI();
            }

            XPos = Xpos;
            YPos = Ypos;
            Value = value;
            Brackets = "()";
            Content = " ";
        }

        /// <summary>
        /// Updates the object content, brackets and colors according to game data.
        /// </summary>
        public abstract void UpdateUI();
        /// <summary>
        /// Creates a new copy of the game object with the exact same values.
        /// </summary>
        /// <returns>The created copy.</returns>
        public abstract GameObject NewCopy();
    }
}
