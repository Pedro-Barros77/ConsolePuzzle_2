using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Services.Models
{
    /// <summary>
    /// A simple class to represent a pair of integers
    /// </summary>
    public class Coord
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Coord(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Maps a new Coord with 1/-1 added to x/y depending on the direction.
        /// </summary>
        /// <param name="dir">The direction to be mapped.</param>
        /// <returns>A new Coord with maped values.</returns>
        public Coord MapDirection(Direction dir)
        {
            return dir switch
            {
                Direction.Up => new(X, Y - 1),
                Direction.Right => new(X + 1, Y),
                Direction.Down => new(X, Y + 1),
                Direction.Left => new(X - 1, Y),
                _ => new(X - 1, Y),
            };
        }

        /// <summary>
        /// Maps a new Coord with 1/-1 added to x/y depending on the key pressed.
        /// </summary>
        /// <param name="dir">The direction to be mapped.</param>
        /// <returns>A new Coord with maped values.</returns>
        public Coord MapDirection(ConsoleKey dir)
        {
            return dir switch
            {
                ConsoleKey.UpArrow => new(X, Y - 1),
                ConsoleKey.RightArrow => new(X + 1, Y),
                ConsoleKey.DownArrow => new(X, Y + 1),
                ConsoleKey.LeftArrow => new(X - 1, Y),

                ConsoleKey.W => new(X, Y - 1),
                ConsoleKey.D => new(X + 1, Y),
                ConsoleKey.S => new(X, Y + 1),
                ConsoleKey.A => new(X - 1, Y),
                _ => new(X - 1, Y),
            };
        }

        public static Coord operator *(Coord a, Coord b)
        => new(a.X * b.X, a.Y * b.Y);

        public static Coord operator *(Coord a, int value)
        => new(a.X * value, a.Y * value);

        public static Coord operator +(Coord a, Coord b)
        => new(a.X + b.X, a.Y + b.Y);
    }
}
