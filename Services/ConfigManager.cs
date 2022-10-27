using System;
using static ConsolePuzzle_2.Utility.Enums;


namespace ConsolePuzzle_2.Services
{
    internal static class ConfigManager
    {
        /// <summary>
        /// The active language of the game.
        /// </summary>
        public static Languages Language { get; private set; } = Languages.English;
        /// <summary>
        /// If sound effects are enabled or not.
        /// </summary>
        public static bool HasSoundFX { get; private set; } = true;
        /// <summary>
        /// If music is enabled or not.
        /// </summary>
        public static bool HasMusic { get; private set; } = true;

        /// <summary>
        /// Sets the language of the game interface.
        /// </summary>
        /// <param name="value">The language to be set according to available options.</param>
        public static void SetLanguage(Languages value)
        {
            Language = value;
        }

        /// <summary>
        /// Activates/Deactivates sound effects.
        /// </summary>
        /// <param name="isActive">Should the application play sound effects?</param>
        public static void SetSoundFX(bool isActive)
        {
            HasSoundFX = isActive;
        }
    }
}
