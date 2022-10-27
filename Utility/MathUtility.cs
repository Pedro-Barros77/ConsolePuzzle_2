using ConsolePuzzle_2.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePuzzle_2.Utility
{
    public static class MathUtility
    {
        /// <summary>
        /// Checks if an integer is between a pair of integers.
        /// </summary>
        /// <param name="value">Value to be checked.</param>
        /// <param name="interval">The pair of integers as the interval.</param>
        /// <returns>True if the value is between, otherwise, false.</returns>
        public static bool IsBetween(int value, Coord interval)
        {
            return value >= interval.X && value <= interval.Y;
        }
        /// <summary>
        /// Checks if a coordinate is within the range of another position.
        /// </summary>
        /// <param name="value">The coordinates to be checked.</param>
        /// <param name="position">The coordinates of the object with a range.</param>
        /// <param name="radius">The radius of the object.</param>
        /// <returns></returns>
        public static bool IsInRadius(Coord value, Coord position, int radius)
        {
            return IsBetween(value.X, new Coord(position.X - radius, position.X + radius)) &&
                   IsBetween(value.Y, new Coord(position.Y - radius, position.Y + radius));
        }
    }
}
