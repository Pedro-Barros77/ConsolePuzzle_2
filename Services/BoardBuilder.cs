using ConsolePuzzle_2.Content;
using ConsolePuzzle_2.Content.Game_Objects;
using ConsolePuzzle_2.Content.Game_Objects.Enemies;
using ConsolePuzzle_2.Services.Models;
using static ConsolePuzzle_2.Utility.ConsoleUtility;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2
{
    public class BoardBuilder
    {

        /// <summary>
        /// The left distance of the first character of the board.
        /// </summary>
        public int BoardCursorLeft { get; set; }
        /// <summary>
        /// The top distance of the first character of the board.
        /// </summary>
        public int BoardCursorTop { get; set; }

        /// <summary>
        /// Game board height.
        /// </summary>
        private int Height => GameLvl.BoardHeight;
        /// <summary>
        /// Game board width.
        /// </summary>
        private int Width => GameLvl.BoardWidth;
        /// <summary>
        /// The vertical size of the board buffer.
        /// </summary>
        private int BoardVerticalField { get; set; }
        /// <summary>
        /// The horizontal size of the board buffer.
        /// </summary>
        private int BoardHorizontalField { get; set; }
        /// <summary>
        /// If the coin content is a '$' or a '|', changes over time for animation.
        /// </summary>
        private bool CoinState { get; set; }

        public bool EnableRenderBLock { get; set; } = true;

        /// <summary>
        /// The color board's border.
        /// </summary>
        private readonly ConsoleColor BorderColor = ConsoleColor.DarkGray;

        private readonly Game GameLvl;

        public BoardBuilder(Game GameLvl)
        {
            this.GameLvl = GameLvl;

            System.Timers.Timer _coinTimer = new(250);
            _coinTimer.Elapsed += ToggleCoin;
            _coinTimer.Start();
        }

        /// <summary>
        /// Called automatically by a timer, toggles the CoinState.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event.</param>
        private void ToggleCoin(object? sender, System.Timers.ElapsedEventArgs e)
        {
            CoinState = !CoinState;
        }

        /// <summary>
        /// Writes the game board on screen.
        /// </summary>
        /// <param name="GameLvl">The level containing the board to be built.</param>
        public void Build()
        {
            int _renderStartingX, _renderStartingY;
            int _renderEndingX, _renderEndingY;

            Console.SetCursorPosition(BoardCursorLeft, BoardCursorTop);
            BoardVerticalField = Console.WindowHeight - GameLvl.HeaderRows - GameLvl.FooterRows - 3;
            BoardHorizontalField = (Console.WindowWidth / 3) - 1;

            #region Vertical Render Limiter

            int _minVrange = GameLvl.PlayerYPos - ((BoardVerticalField / 2) - 1);
            int _maxVrange = Height - (Height - BoardVerticalField) - 1;
            //If there's some spare space for max range, add it to min range.
            if (_maxVrange - (BoardVerticalField / 2) - (Height - GameLvl.PlayerYPos) > 0)
                _minVrange -= _maxVrange - (BoardVerticalField / 2) - (Height - GameLvl.PlayerYPos);

            _renderStartingY = Math.Clamp(_minVrange, 0, Height);
            _renderEndingY = Math.Clamp(_maxVrange, 0, Height);

            #endregion

            #region Horizontal Render Limiter

            int _minHrange = GameLvl.PlayerXPos - ((BoardHorizontalField / 2) - 1);
            int _maxHrange = Width - (Width - BoardHorizontalField);
            //If there's some spare space for max range, add it to min range.
            if (_maxHrange - (BoardHorizontalField / 2) - (Width - GameLvl.PlayerXPos) > 0)
                _minHrange -= _maxHrange - (BoardHorizontalField / 2) - (Width - GameLvl.PlayerXPos);

            _renderStartingX = Math.Clamp(_minHrange, 0, Width);
            _renderEndingX = Math.Clamp(_maxHrange, 0, Width);

            #endregion

            int _boardCursorLeft = 0;

            for (int y = _renderStartingY; y < _renderEndingY + (y >= Height ? 0 : _renderStartingY); y++)
            {
                bool visible;
                for (int x = _renderStartingX; x < _renderEndingX + (x >= Width ? 0 : _renderStartingX); x++)
                {
                    //If is in player's field of view.
                    visible = (y + 1 < BoardVerticalField + _renderStartingY) &&
                              (x + 1 < BoardHorizontalField + _renderStartingX) &&
                              IsInPlayerFOV(x, y, GameLvl.P1_Pos, new Coord(GameLvl.P1_XField, GameLvl.P1_YField)) ||
                              (GameLvl.IsMultiplayer &&
                              IsInPlayerFOV(x, y, GameLvl.P2_Pos, new Coord(GameLvl.P2_XField, GameLvl.P2_YField)));


                    if (x == _renderStartingX)
                    {
                        #region Top Border
                        if (y == 0)
                        {
                            //If all board is visible horizontally, center the board.
                            if (BoardHorizontalField >= Width)
                                Console.CursorLeft = CenterText(new string(' ', Width * 3 + 4), spacesOnly: true).Length;

                            Console.BackgroundColor = BorderColor;
                            //Left border
                            Console.Write("  ");
                            //Top border
                            Console.Write(new string(' ', (BoardHorizontalField <= Width ? (BoardHorizontalField * 3) - 5 : Width * 3)));
                            //Right border
                            Console.Write("  ");

                            Console.ResetColor();
                            Console.Write("\n");
                        }
                        #endregion

                        #region Left Border
                        //If all board is visible horizontally, center the board.
                        if (BoardHorizontalField >= Width)
                            Console.Write(CenterText(new string(' ', (Width * 3 + 4)), spacesOnly: true));

                        //If the border is visible.
                        if (y + 1 < BoardVerticalField + _renderStartingY)
                            Console.BackgroundColor = BorderColor;
                        else
                            Console.BackgroundColor = ConsoleColor.Black;

                        _boardCursorLeft = Console.CursorLeft;

                        if (GameLvl.PlayerXPos < (BoardHorizontalField - 1) / 2
                            || BoardHorizontalField >= Width)
                            Console.Write("  ");
                        Console.ResetColor();
                        #endregion
                    }

                    GameObject _currentObj = GameLvl.BoardObjects[y][x];
                    string _content = _currentObj.Content;

                    switch (_currentObj.ObjectType)
                    {
                        case ObjectTypes.Coin:
                            {
                                if (CoinState)
                                {
                                    if (y % 2 == 0)
                                        _content = x % 2 == 0 ? "$" : "|";
                                    else
                                        _content = x % 2 == 0 ? "|" : "$";
                                }
                                else
                                {
                                    if (y % 2 == 0)
                                        _content = x % 2 == 0 ? "|" : "$";
                                    else
                                        _content = x % 2 == 0 ? "$" : "|";
                                }
                                break;
                            }

                        case ObjectTypes.CoinDoor:
                            {
                                if (GameLvl.CanEnterGate(_currentObj, false))
                                {
                                    _currentObj.Brackets = ("{}");
                                    _content = " ";
                                }
                                else
                                    _content = (int.Parse(_currentObj.Value) - GameLvl.CollectedCoins).ToString();
                                break;
                            }

                        case ObjectTypes.KeyDoor:
                            {
                                if (GameLvl.CanEnterGate(_currentObj, false) || _currentObj.Content == "P" || _currentObj.Content == "#")
                                {
                                    _currentObj.Brackets = "{}";
                                    _content = " ";
                                }
                                else
                                    _currentObj.Brackets = "[]";
                                break;
                            }

                        case ObjectTypes.CodeDoor:
                            {
                                if ((GameLvl.CanEnterGate(_currentObj, false)) || _currentObj.Content == "P" || _currentObj.Content == "#")
                                {
                                    _currentObj.Brackets = "{}";
                                    _content = " ";
                                }
                                else
                                {
                                    _currentObj.Brackets = "[]";
                                    _content = "-";
                                }
                                break;
                            }
                    }
                    _currentObj.Content = _content;

                    bool _isSeeker = _currentObj.ObjectType.Equals(ObjectTypes.Enemy_Seeker);

                    if (visible)
                    {
                        #region Left Brackets
                        if (_isSeeker)
                        {
                            (var _value, var _fg, var _bg) = ((Enemy_Seeker)_currentObj).GetPrintingValues(0);
                            Console.ForegroundColor = _fg;
                            Console.BackgroundColor = _bg;
                            Console.Write(_value);
                        }
                        else
                        {
                            Console.BackgroundColor = _currentObj.BracketsBgColor;
                            Console.ForegroundColor = _currentObj.BracketsFgColor;
                            Console.Write(_currentObj.Brackets[0]);
                        }
                        #endregion

                        #region Content
                        if (_isSeeker)
                        {
                            (var _value, var _fg, var _bg) = ((Enemy_Seeker)_currentObj).GetPrintingValues(1);
                            Console.ForegroundColor = _fg;
                            Console.BackgroundColor = _bg;
                            Console.Write(_value);
                        }
                        else
                        {
                            Console.BackgroundColor = _currentObj.BgColor;
                            Console.ForegroundColor = _currentObj.FgColor;
                            Console.Write(_content);
                        }
                        #endregion

                        #region Right bracket
                        if (_isSeeker)
                        {
                            (var _value, var _fg, var _bg) = ((Enemy_Seeker)_currentObj).GetPrintingValues(2);
                            Console.ForegroundColor = _fg;
                            Console.BackgroundColor = _bg;
                            Console.Write(_value);
                        }
                        else
                        {
                            Console.BackgroundColor = _currentObj.BracketsBgColor;
                            Console.ForegroundColor = _currentObj.BracketsFgColor;
                            Console.Write(_currentObj.Brackets[1]);
                        }
                        #endregion
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        if (y + 1 < BoardVerticalField + _renderStartingY &&
                            x + 1 < BoardHorizontalField + _renderStartingX)
                            Console.Write("   ");
                    }

                    #region Right Border
                    if (x == Width - 1 || x == _renderStartingX + BoardHorizontalField + 1)
                    {
                        //If border is visible
                        if (x + 1 < BoardHorizontalField + _renderStartingX && y + 1 < BoardVerticalField + _renderStartingY)
                            Console.BackgroundColor = BorderColor;
                        else
                            Console.BackgroundColor = ConsoleColor.Black;

                        Console.Write("  ");
                        Console.ResetColor();
                        Console.Write(new string(' ', (Console.WindowWidth - Console.CursorLeft) - 1));

                    }
                    #endregion

                    //Cleaner
                    if (GameLvl.PlayerXPos < (BoardHorizontalField / 2) - 1 && BoardHorizontalField < Width)
                    {
                        int lastCursorLeft = Console.CursorLeft;
                        Console.CursorLeft = (BoardHorizontalField * 3) - 1;
                        Console.Write(" ");
                        Console.CursorLeft = lastCursorLeft;
                    }

                    Console.ResetColor();
                }

                #region Bottom Border
                if (y + 1 < BoardVerticalField + _renderStartingY && y == Height - 1)
                {
                    Console.Write("\n");
                    //If all board is visible horizontally, center the board.
                    if (BoardHorizontalField >= Width)
                        Console.Write(CenterText(new string(' ', (Width * 3 + 4)), spacesOnly: true));

                    //If border is visible.
                    if (y + 1 < BoardVerticalField + _renderStartingY)
                        Console.BackgroundColor = BorderColor;
                    else
                        Console.BackgroundColor = ConsoleColor.Black;

                    //Left border
                    Console.Write("  ");
                    //Bottom Border
                    Console.Write(new string(' ', (BoardHorizontalField <= Width ? (BoardHorizontalField * 3) - 5 : (Width * 3))));
                    //Right Border
                    Console.Write("  ");

                    Console.ResetColor();
                    //Cleaner
                    Console.Write(new string(' ', (Console.WindowWidth - Console.CursorLeft) - 1));
                }
                #endregion

                Console.ResetColor();
                //Cleaner
                if (y == BoardVerticalField - 1 + _renderStartingY || y == Height)
                {
                    int previousCursorLeft = Console.CursorLeft;
                    Console.CursorLeft = _boardCursorLeft;
                    Console.Write(new string(' ', (Width * 3 + 4) - 1));
                    Console.CursorLeft = previousCursorLeft;
                }
                Console.Write("\n");
            }
        }

        /// <summary>
        /// Checks if a board object is within player Field of View.
        /// </summary>
        /// <param name="x">Object X position.</param>
        /// <param name="y">Object Y position.</param>
        /// <param name="playerPos">Player coordinates.</param>
        /// <param name="playerFOV">Player FOV x and y values.</param>
        /// <returns>True if the object is inside the player's FOV.</returns>
        private bool IsInPlayerFOV(int x, int y, Coord playerPos, Coord playerFOV)
        {
            return ((playerFOV.Y == 0 || y + 1 <= playerFOV.Y + playerPos.Y) &&
                   (playerFOV.Y == 0 || y > -playerFOV.Y + playerPos.Y))
                   &&
                   ((playerFOV.X == 0 || x + 1 <= playerFOV.X + playerPos.X) &&
                   (playerFOV.X == 0 || x > -playerFOV.X + playerPos.X));
        }
    }
}
