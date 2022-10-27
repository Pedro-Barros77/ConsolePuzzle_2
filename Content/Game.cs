using static ConsolePuzzle_2.Utility.ConsoleUtility;
using ConsolePuzzle_2.Content.Game_Objects;
using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.Enums;
using ConsolePuzzle_2.Content.Game_Objects.Basics;
using ConsolePuzzle_2.Content.Game_Objects.Interactable;
using System.Collections.ObjectModel;

namespace ConsolePuzzle_2.Content
{
    public class Game
    {
        #region Properties
        /// <summary>
        /// The level of this game.
        /// </summary>
        public Level ActiveLevel { get; set; }
        /// <summary>
        /// The game board, containing all game objects.
        /// </summary>
        public List<List<GameObject>> BoardObjects { get; private set; }
        /// <summary>
        /// A dictionary containing all enemies, with their names as keys.
        /// </summary>
        public Dictionary<string, Enemy> BoardEnemies { get; private set; }
        /// <summary>
        /// The BoardBuilder of the game.
        /// </summary>
        public BoardBuilder Board { get; private set; }
        /// <summary>
        /// Gets the length of the columns in the lines of the game board.
        /// </summary>
        public int BoardWidth => BoardObjects[0].Count;
        /// <summary>
        /// Gets the length of the rows of the game board.
        /// </summary>
        public int BoardHeight => BoardObjects.Count;
        /// <summary>
        /// The X position of the Player 1.
        /// </summary>
        public int PlayerXPos { get; private set; }
        /// <summary>
        /// The Y position of the Player 1.
        /// </summary>
        public int PlayerYPos { get; private set; }
        /// <summary>
        /// The range which Player 1 can see in the X axis.
        /// </summary>
        public int P1_XField { get; set; }
        /// <summary>
        /// The range which Player 1 can see in the Y axis.
        /// </summary>
        public int P1_YField { get; set; }
        /// <summary>
        /// The X position of the Player 2.
        /// </summary>
        public int Player2XPos { get; private set; }
        /// <summary>
        /// The Y position of the Player 2.
        /// </summary>
        public int Player2YPos { get; private set; }
        /// <summary>
        /// The range which Player 2 can see in the X axis.
        /// </summary>
        public int P2_XField { get; set; }
        /// <summary>
        /// The range which Player 2 can see in the Y axis.
        /// </summary>
        public int P2_YField { get; set; }
        /// <summary>
        /// If current level has more then one player.
        /// </summary>
        public bool IsMultiplayer { get { return BoardObjects.Any(r => r.Any(o => o.ObjectType.Equals(ObjectTypes.Player2))); } }
        /// <summary>
        /// If this level is finished by the player.
        /// </summary>
        public bool LevelCompleted { get; private set; }
        /// <summary>
        /// The title of this level.
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// The header of the game page, containing the instructions of the level.
        /// </summary>
        public UIBlock Header { get; set; }
        /// <summary>
        /// The footer of the game page, containing the instructions of gameplay.
        /// </summary>
        public UIBlock Footer { get; set; }
        /// <summary>
        /// Gets the length of the header.
        /// </summary>
        public int HeaderRows => Header.Lines.Count;
        /// <summary>
        /// Gets the length of the footer.
        /// </summary>
        public int FooterRows => Footer.Lines.Count;
        /// <summary>
        /// Gets X and Y position of player 1.
        /// </summary>
        public Coord P1_Pos => new(PlayerXPos, PlayerYPos);
        /// <summary>
        /// Gets X and Y position of player 2.
        /// </summary>
        public Coord P2_Pos => new(Player2XPos, Player2YPos);
        #endregion

        #region Level Data
        /// <summary>
        /// The number of coins collected by the player in this level.
        /// </summary>
        public int CollectedCoins { get; set; }
        /// <summary>
        /// The list of keys collected by the player in this level.
        /// </summary>
        public ObservableCollection<char> CollectedKeys { get; set; }
        /// <summary>
        /// The list of buttons pressed by the player in this level.
        /// </summary>
        public ObservableCollection<int> PressedButtons { get; set; }
        /// <summary>
        /// The list of portals in thos level, containing it's character and position.
        /// </summary>
        public List<Tuple<string, int, int>> BoardPortals { get; private set; }
        #endregion

        #region Private Variables
        private readonly GameController gameController;
        private readonly MenuController menuController;
        private GameObject Player_1 => BoardObjects[PlayerYPos][PlayerXPos];
        private GameObject Player_2 => BoardObjects[Player2YPos][Player2XPos];
        #endregion

        public Game(string boardJSON, Level activeLevel)
        {
            gameController = GameController.GetInstance();
            menuController = MenuController.GetInstance();

            BoardEnemies = new();
            CollectedKeys = new();
            CollectedCoins = new();
            PressedButtons = new();
            BoardPortals = new();
            Header = new();
            Footer = new();
            Title = "";

            BoardObjects = gameController.BuildBoardObjects(boardJSON, this);
            Board = new(this);

            PlayerYPos = BoardObjects.FindIndex(r => r.Any(c => c.ObjectType.Equals(ObjectTypes.Player)));
            PlayerXPos = BoardObjects[PlayerYPos].FindIndex(c => c.ObjectType.Equals(ObjectTypes.Player));

            if (IsMultiplayer)
            {
                Player2YPos = BoardObjects.FindIndex(r => r.Any(c => c.ObjectType.Equals(ObjectTypes.Player2)));
                Player2XPos = BoardObjects[Player2YPos].FindIndex(c => c.ObjectType.Equals(ObjectTypes.Player2));
            }

            gameController.Playing = true;
            ActiveLevel = activeLevel;

            CollectedKeys.CollectionChanged += (s, e) => OnUIBarChanged();
            PressedButtons.CollectionChanged += (s, e) => OnUIBarChanged();
        }

        /// <summary>
        /// Call it when the info on UI Info bar has changed (collected coins, keys, etc).
        /// </summary>
        public void OnUIBarChanged()
        {
            ActiveLevel.InfoBarChanged = true;
        }

        /// <summary>
        /// Compares the key pressed by the user and runs it's action.
        /// </summary>
        /// <param name="pressedKey">The key pressed by the user.</param>
        public void HandleInput(ConsoleKey pressedKey)
        {
            if (pressedKey == ConsoleKey.K)
                return;

            GameObject P1 = Player_1.NewCopy();
            GameObject P2 = Player_2.NewCopy();

            Coord moveDir = new Coord(0, 0).MapDirection(pressedKey);

            switch (pressedKey)
            {
                case ConsoleKey.UpArrow:
                case ConsoleKey.RightArrow:
                case ConsoleKey.DownArrow:
                case ConsoleKey.LeftArrow:
                    ValidateDestination(moveDir.X, moveDir.Y, P1);
                    break;

                case ConsoleKey.W:
                case ConsoleKey.A:
                case ConsoleKey.S:
                case ConsoleKey.D:
                    ValidateDestination(moveDir.X, moveDir.Y, IsMultiplayer ? P2 : P1);
                    break;


                case ConsoleKey.R:
                    gameController.ResetKey();
                    gameController.RestartLevel(ActiveLevel);
                    break;

                case ConsoleKey.P:
                    gameController.ResetKey();
                    gameController.Playing = false;
                    menuController.PauseGame(ActiveLevel);
                    break;

                case ConsoleKey.H:
                    Board.EnableRenderBLock = !Board.EnableRenderBLock;
                    Console.Beep(2500, 100);
                    break;
            }
            gameController.ResetKey();
        }

        /// <summary>
        /// Checks the direction the player wants to move and runs the movement if it's valid.
        /// </summary>
        /// <param name="x">The X step of the movement.</param>
        /// <param name="y">The Y step of the movement.</param>
        /// <param name="player">The GameObject of player that is moving (player 1 or player 2).</param>
        void ValidateDestination(int x, int y, GameObject player)
        {
            if (!IsInBoardBounds(player.XPos + x, player.YPos + y)) return;

            GameObject _destination = BoardObjects[player.YPos + y][player.XPos + x];

            bool _canPushBox = true;
            bool _canPullBox = true;
            bool _isPlayer1 = player.ObjectType.Equals(ObjectTypes.Player);

            GameObject? _objAfterDirection = null;
            if (IsInBoardBounds(player.XPos + (x * 2), player.YPos + (y * 2)))
            {
                _objAfterDirection = BoardObjects[player.YPos + (y * 2)][player.XPos + (x * 2)];
            }

            //Check if object in front of box is empty/open so player can push onto it.
            if (_objAfterDirection != null)
            {
                _canPushBox = CanEnterGate(_objAfterDirection, _canPushBox);

                if (_objAfterDirection.ObjectType.Equals(ObjectTypes.PlayerDoor))
                    _canPushBox = false;
            }

            //Check if object where player is going is empty/open so them can pull the box onto it
            _canPullBox = CanEnterGate(_destination, _canPullBox) || _destination.ObjectType.Equals(ObjectTypes.Box);

            GameObject? _objBehindPlayer = null;
            if (IsInBoardBounds(player.XPos + (-x), player.YPos + (-y)))
            {
                _objBehindPlayer = BoardObjects[player.YPos + (-y)][player.XPos + (-x)];
            }

            switch (_destination.ObjectType)
            {
                case ObjectTypes.Blank:
                    Move(x, y, player);
                    break;

                case ObjectTypes.Exit:
                    Move(x, y, player);
                    LevelCompleted = true;
                    gameController.Playing = false;
                    break;

                case ObjectTypes.Coin:
                    Move(x, y, player);
                    gameController.CollectCoin(this);
                    break;

                case ObjectTypes.CoinDoor:
                    if (CanEnterGate(_destination, true))
                        Move(x, y, player);
                    break;

                case ObjectTypes.Key:
                    if (!CollectedKeys.Contains(_destination.Value[0]))
                        CollectedKeys.Add(_destination.Value[0]);
                    Move(x, y, player);
                    break;

                case ObjectTypes.KeyDoor:
                    if (CanEnterGate(_destination, true))
                        Move(x, y, player);
                    break;
                case ObjectTypes.CodeDoor:
                    if (CanEnterGate(_destination, true))
                        Move(x, y, player);
                    break;

                case ObjectTypes.OneWay:
                    {
                        if (_objAfterDirection != null && IsInBoardBounds(_objAfterDirection.XPos, _objAfterDirection.YPos))
                        {
                            bool canJump;

                            canJump = !gameController.NotJumpableObjs.Contains(_objAfterDirection.ObjectType);
                            canJump = CanEnterGate(_objAfterDirection, canJump);

                            //If obj after one-way is a player door but it's not the correct player
                            if (_objAfterDirection.ObjectType.Equals(ObjectTypes.PlayerDoor) && (_isPlayer1 && _objAfterDirection.Value != "1" || !_isPlayer1 && _objAfterDirection.Value != "2"))
                                canJump = false;

                            if (!canJump) break;

                            //If player is going to the same direction that one-way points to
                            if ((_destination.Dir == Direction.Up && y == -1) || (_destination.Dir == Direction.Right && x == 1) || (_destination.Dir == Direction.Down && y == 1) || (_destination.Dir == Direction.Left && x == -1))
                            {
                                switch (_objAfterDirection.ObjectType)
                                {
                                    case ObjectTypes.Coin:
                                        gameController.CollectCoin(this);
                                        break;
                                    case ObjectTypes.Key:
                                        if (!CollectedKeys.Contains(_objAfterDirection.Value[0]))
                                            CollectedKeys.Add(_objAfterDirection.Value[0]);
                                        break;
                                }

                                Move(x * 2, y * 2, player);
                            }
                        }
                    }
                    break;

                case ObjectTypes.Portal:
                    {
                        bool hasDestination = false;
                        Coord destPortalPos = new(0, 0);

                        foreach (Tuple<string, int, int> t in BoardPortals)
                        {
                            if (char.IsUpper(_destination.Value[0]))
                            {
                                if (t.Item1 == _destination.Value.ToLower())
                                {
                                    hasDestination = true;
                                    destPortalPos = new(t.Item2, t.Item3);
                                }
                            }
                            else
                            {
                                if (t.Item1 == _destination.Value.ToUpper())
                                {
                                    hasDestination = true;
                                    destPortalPos = new(t.Item2, t.Item3);
                                }
                            }
                        }

                        GameObject destinationPortal = BoardObjects[destPortalPos.Y][destPortalPos.X];
                        //If there is another portal connected and there are no players on the other portal
                        if (hasDestination && !destinationPortal.ObjectType.Equals(ObjectTypes.Player) && !destinationPortal.ObjectType.Equals(ObjectTypes.Player2))
                        {
                            Move(destPortalPos.X - player.XPos, destPortalPos.Y - player.YPos, player);
                        }
                    }
                    break;

                case ObjectTypes.Box:
                    {
                        if (_objAfterDirection != null && IsInBoardBounds(_objAfterDirection.XPos, _objAfterDirection.YPos))
                        {
                            if (!gameController.BoxContainers.Contains(_objAfterDirection.ObjectType) || !_canPushBox)
                                break;

                            //creates a template of the object that the box will cover
                            GameObject objUnderBoxTemplate = BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].NewCopy();

                            //sets box to it's destination
                            BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos] = _destination.NewCopy();
                            BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].XPos = _objAfterDirection.XPos;
                            BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].YPos = _objAfterDirection.YPos;

                            BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].ObjectTypeUnderThis = objUnderBoxTemplate;

                            //keeps object under box configuration after setting the box
                            BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].BracketsBgColor = objUnderBoxTemplate.BracketsBgColor;
                            if (objUnderBoxTemplate.ObjectType.Equals(ObjectTypes.StandingButton))
                            {
                                BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].BracketsFgColor = ConsoleColor.Green;
                                int _value = int.Parse(BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].ObjectTypeUnderThis.Value);
                                if (!PressedButtons.Contains(_value))
                                {
                                    PressedButtons.Add(_value);
                                }
                            }
                            else
                            {
                                BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].BracketsFgColor = objUnderBoxTemplate.BracketsFgColor;
                            }

                            if (BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].ObjectTypeUnderThis.ObjectType.Equals(ObjectTypes.KeyDoor) ||
                                BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].ObjectTypeUnderThis.ObjectType.Equals(ObjectTypes.CoinDoor))
                            {
                                if (CanEnterGate(BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].ObjectTypeUnderThis, false))
                                    BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].Brackets = "{}";
                            }
                            else
                            {
                                BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].Brackets = objUnderBoxTemplate.Brackets;
                            }
                            BoardObjects[_objAfterDirection.YPos][_objAfterDirection.XPos].BgColor = objUnderBoxTemplate.BgColor;

                            BoardObjects[_destination.YPos][_destination.XPos] = BoardObjects[_destination.YPos][_destination.XPos].ObjectTypeUnderThis.NewCopy();

                            Move(x, y, player);
                        }
                    }
                    break;

                case ObjectTypes.StandingButton:
                    Move(x, y, player);
                    break;

                case ObjectTypes.PlayerDoor:
                    if ((_isPlayer1 && _destination.Value == "1") || (!_isPlayer1 && _destination.Value == "2"))
                        Move(x, y, player);
                    break;

                case ObjectTypes.Enemy_Guard:
                case ObjectTypes.Enemy_CannonBullet:
                case ObjectTypes.Enemy_Seeker:
                    {
                        BoardObjects[player.YPos][player.XPos].Content = " ";

                        Console.SetCursorPosition(Board.BoardCursorLeft, Board.BoardCursorTop);
                        Board.Build();
                        gameController.KillPlayer(this);
                    }
                    break;
            }

            //If there's nothing behid the player or it's not a box or it's not pulling it
            if (_objBehindPlayer == null || !gameController.CtrlKeyHeld || !_objBehindPlayer.ObjectType.Equals(ObjectTypes.Box))
                return;

            //If can't pull the box or player's new position wouldn't accept a box (like a portal or player door)
            if (!_canPullBox || !gameController.BoxContainers.Contains(BoardObjects[player.YPos][player.XPos].ObjectType))
                return;

            GameObject objUnderBox = BoardObjects[player.YPos][player.XPos];
            //If obj where box is being pulled to can't accpes a box
            if (!gameController.BoxContainers.Contains(objUnderBox.ObjectType))
                return;

            //Places the box to it's new destination
            BoardObjects[player.YPos][player.XPos] = new Box(player.XPos, player.YPos)
            {
                ObjectTypeUnderThis = objUnderBox,
                XPos = player.XPos,
                YPos = player.YPos,

                BracketsBgColor = objUnderBox.BracketsBgColor
            };
            //If box's new position is a standingButton
            if (BoardObjects[player.YPos][player.XPos].ObjectTypeUnderThis.ObjectType.Equals(ObjectTypes.StandingButton))
            {
                BoardObjects[player.YPos][player.XPos].BracketsFgColor = ConsoleColor.Green;
                int _value = int.Parse(BoardObjects[player.YPos][player.XPos].ObjectTypeUnderThis.Value);
                if (!PressedButtons.Contains(_value))
                {
                    PressedButtons.Add(_value);
                }
            }
            else
            {
                BoardObjects[player.YPos][player.XPos].BracketsFgColor = objUnderBox.BracketsFgColor;
                if (IsInBoardBounds(_objBehindPlayer.XPos, _objBehindPlayer.YPos))
                {
                    GameObject objBeforeBox = BoardObjects[_objBehindPlayer.YPos][_objBehindPlayer.XPos].ObjectTypeUnderThis;
                    //If box's old position was a standing button
                    if(objBeforeBox != null && objBeforeBox.ObjectType.Equals(ObjectTypes.StandingButton))
                    {
                        int _value = int.Parse(objBeforeBox.Value);
                        if (PressedButtons.Contains(_value))
                            PressedButtons.Remove(_value);
                    }
                }
            }
            if (BoardObjects[player.YPos][player.XPos].ObjectTypeUnderThis.ObjectType.Equals(ObjectTypes.KeyDoor) ||
                BoardObjects[player.YPos][player.XPos].ObjectTypeUnderThis.ObjectType.Equals(ObjectTypes.CoinDoor))
            {
                if (CanEnterGate(BoardObjects[player.YPos][player.XPos].ObjectTypeUnderThis, false))
                    BoardObjects[player.YPos][player.XPos].Brackets = "{}";
            }
            else
            {
                BoardObjects[player.YPos][player.XPos].Brackets = objUnderBox.Brackets;
            }
            BoardObjects[player.YPos][player.XPos].BgColor = objUnderBox.BgColor;

            BoardObjects[_objBehindPlayer.YPos][_objBehindPlayer.XPos] = _objBehindPlayer.ObjectTypeUnderThis.NewCopy();

            //If box's old position was a standing button
            if (_objBehindPlayer.ObjectType.Equals(ObjectTypes.StandingButton) && PressedButtons.Contains(int.Parse(_objBehindPlayer.Value)))
            {
                PressedButtons.Remove(int.Parse(_objBehindPlayer.Value));
            }
        }

        /// <summary>
        /// Moves the player according to the specified step directions.
        /// </summary>
        /// <param name="x">The X step of the movement.</param>
        /// <param name="y">The Y step of the movement.</param>
        /// <param name="player">The GameObject of the player to be moved (player 1 or player 2)</param>
        public void Move(int x, int y, GameObject player)
        {
            GameObject p = player.NewCopy();
            GameObject _destinationObject = BoardObjects[p.YPos + y][p.XPos + x];
            GameObject _originalObject = p.ObjectTypeUnderThis;

            //Moves player to destination
            BoardObjects[p.YPos + y][p.XPos + x] = BoardObjects[p.YPos][p.XPos].NewCopy();
            BoardObjects[p.YPos + y][p.XPos + x].XPos = p.XPos + x;
            BoardObjects[p.YPos + y][p.XPos + x].YPos = p.YPos + y;

            //Puts destination under player
            BoardObjects[p.YPos + y][p.XPos + x].ObjectTypeUnderThis = _destinationObject;

            //If there were content under player's old position, put it back
            if (gameController.ObjToKeepContent.Contains(_originalObject.ObjectType))
            {
                BoardObjects[p.YPos][p.XPos] = _originalObject.NewCopy();
                if (gameController.ObjectsToClear.Contains(_originalObject.ObjectType))
                {
                    BoardObjects[p.YPos][p.XPos].Content = " ";
                }
            }
            else
            {
                BoardObjects[p.YPos][p.XPos] = new Blank(p.XPos, p.YPos);
            }

            //If player's new position is on a container, change it's brackets back to original
            if (gameController.ObjToKeepContent.Contains(_destinationObject.ObjectType))
            {
                BoardObjects[p.YPos + y][p.XPos + x].Brackets = _destinationObject.Brackets;
            }

            //If player's new position is a standing button, light it green
            if (_destinationObject.ObjectType.Equals(ObjectTypes.StandingButton))
            {
                BoardObjects[_destinationObject.YPos][_destinationObject.XPos].BracketsFgColor = ConsoleColor.Green;
                int value = int.Parse(BoardObjects[_destinationObject.YPos][_destinationObject.XPos].ObjectTypeUnderThis.Value);
                if (!PressedButtons.Contains(value))
                    PressedButtons.Add(value);
            }
            else
            {
                BoardObjects[p.YPos + y][p.XPos + x].BracketsFgColor = _destinationObject.BracketsFgColor;
            }

            //If player's old position was a stading button, unlight it
            if (BoardObjects[p.YPos][p.XPos].ObjectType.Equals(ObjectTypes.StandingButton))
            {
                int value = int.Parse(BoardObjects[p.YPos][p.XPos].Value);
                if (PressedButtons.Contains(value))
                    PressedButtons.Remove(value);
            }

            //Paint's destination brackets to original color
            BoardObjects[p.YPos + y][p.XPos + x].BracketsBgColor = _destinationObject.BracketsBgColor;
            BoardObjects[p.YPos + y][p.XPos + x].BgColor = _destinationObject.BgColor;

            if (p.ObjectType == ObjectTypes.Player)
            {
                PlayerXPos += x;
                PlayerYPos += y;
            }
            else
            {
                Player2XPos += x;
                Player2YPos += y;
            }
        }

        /// <summary>
        /// Executes the 'Run' script of each enemy.
        /// </summary>
        public void RunEnemies()
        {
            for (int i = 0; i < BoardEnemies.Count; i++)
            {
                BoardEnemies.Values.ElementAt(i).Run();
            }
        }

        /// <summary>
        /// Checks if coordinates are inside of the bounds of the game board, to prevent exceptions.
        /// </summary>
        /// <param name="x">The X position.</param>
        /// <param name="y">The Y position.</param>
        /// <returns>True if the coordinates are a valid game object of the board.</returns>
        public bool IsInBoardBounds(int x, int y)
        {
            return x >= 0 && x < BoardWidth && y >= 0 && y < BoardHeight;
        }

        /// <summary>
        /// Checks if the player has the requirements to enter the specified gate.
        /// </summary>
        /// <param name="gate">The gate to check conditions.</param>
        /// <param name="defaultHandling">What to return if the specified object is not a gate.</param>
        /// <returns>True if the gate is opened, otherwise, false.</returns>
        public bool CanEnterGate(GameObject gate, bool defaultHandling = false)
        {
            switch (gate.ObjectType)
            {
                case ObjectTypes.CoinDoor:
                    return CollectedCoins >= int.Parse(gate.Value);
                case ObjectTypes.KeyDoor:
                    return CollectedKeys.Contains(gate.Value[0]);
                case ObjectTypes.CodeDoor:
                    return PressedButtons.Contains(int.Parse(gate.Value));
                default:
                    return defaultHandling;
            }
        }
        /// <summary>
        /// Writes the game page UI, containig header and footer.
        /// </summary>
        public void WriteGameUI()
        {
            WriteUI(Header); //Writes header
            Console.CursorTop += 2;
            WriteUI(Footer); //Writes instructions

            Board.BoardCursorLeft = Console.CursorLeft;
            Board.BoardCursorTop = Console.CursorTop;
        }

        /// <summary>
        /// Writes the specified UI on screen (header or footer).
        /// </summary>
        /// <param name="block"></param>
        public void WriteUI(UIBlock block)
        {
            for (int line = 0; line < block.Lines.Count; line++)
            {
                bool centerLine = false;
                for (int item = 0; item < block.Lines[line].Count; item++)
                {
                    int lineLength = 0;
                    if (block.Lines[line][item].CenterLine)
                    {
                        centerLine = true;
                        lineLength = block.Lines[line].Sum(x => x.Text.Length);
                    }
                    Console.BackgroundColor = block.Lines[line][item].BgColor;
                    Console.ForegroundColor = block.Lines[line][item].FgColor;
                    if (centerLine)
                    {
                        Console.Write(CenterText(new string(' ', lineLength), spacesOnly: true));
                        centerLine = false;
                    }
                    Console.Write(block.Lines[line][item].Text);
                }
                Console.Write("\n");
            }
        }
    }
}
