using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePuzzle_2.Utility
{
    public static class Enums
    {
        /// <summary>
        /// Available pages (screens) to open.
        /// </summary>
        public enum Pages
        {
            MainMenu,
            Tutorials,
            Levels,
            SurvivalMode,
            LevelCreator,
            Options,
            Pause,
            CompletedLevel,
            Level
        }

        /// <summary>
        /// Available languages to select.
        /// </summary>
        public enum Languages
        {
            Portuguese,
            English
        }

        /// <summary>
        /// Available options to align text.
        /// </summary>
        public enum Alignment
        {
            /// <summary>
            /// Aligns text to the left of screen
            /// </summary>
            Left,
            /// <summary>
            /// Aligns text to the center of screen
            /// </summary>
            Center,
            /// <summary>
            /// Aligns text to the right of screen
            /// </summary>
            Right,
            /// <summary>
            /// Screen width is added to the left of the text. It means it will start hidden.
            /// </summary>
            Hidden
        }

        /// <summary>
        /// Available directions.
        /// </summary>
        public enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        /// <summary>
        /// Game object types.
        /// </summary>
        public enum ObjectTypes
        {
            //Main
            NullObject,
            Blank,
            Player,
            Player2,
            Exit,
            Wall,

            //Collectables
            Coin,
            Key,

            //Gates
            CoinDoor,
            KeyDoor,
            CodeDoor,
            OneWay,
            Portal,
            PlayerDoor,

            //Interactables
            Box,
            StandingButton,

            //Enemies
            Enemy_Guard,
            Enemy_Cannon,
            Enemy_CannonBullet,
            Enemy_Seeker
        }

        //ammo + bullet
        //power-up (armor, slow motion etc)
        //bomb

        /// <summary>
        /// Available targets for enemies.
        /// </summary>
        public enum Target
        {
            Player1,
            Player2,
            None
        }
    }
}
