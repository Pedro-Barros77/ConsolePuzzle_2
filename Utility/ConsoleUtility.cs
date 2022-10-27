using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ConsolePuzzle_2.Utility
{
    internal static class ConsoleUtility
    {
        #region Console and kernel configs
        //Configuration to maximize console window
        private static readonly IntPtr ThisConsole = GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int MAXIMIZE = 3;

        //Configuration to prevent user from minimizing and resizing console window
        private const int MF_BYCOMMAND = 0x00000000;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_SIZE = 0xF000;
        [DllImport("user32.dll")]
        public static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32.dll")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        //Configuration to prevent user from stopping program by clicking on the console (edit mode)
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(IntPtr hConsoleHandle, int ioMode);
        const int QuickEditMode = 64;
        const int ExtendedFlags = 128;
        public const int STD_INPUT_HANDLE = -10;
        [DllImport("Kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int nStdHandle);
        #endregion

        #region Console Config Methods
        public static void MaximizeWindow()
        {
            //Maximize console window
            ShowWindow(ThisConsole, MAXIMIZE);

            //Deletes options to minimize and resize console window
            IntPtr handle = GetConsoleWindow();
            IntPtr sysMenu = GetSystemMenu(handle, false);
            if (handle != IntPtr.Zero)
            {
                _ = DeleteMenu(sysMenu, SC_MINIMIZE, MF_BYCOMMAND);
                _ = DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            }

            //Disable console edit mode
            DisableQuickEdit();
        }

        static void DisableQuickEdit()
        {
            IntPtr conHandle = GetStdHandle(STD_INPUT_HANDLE);

            if (!GetConsoleMode(conHandle, out int mode))
            {
                // error getting the console mode. Exit.
                return;
            }

            mode &= ~(QuickEditMode | ExtendedFlags);

            if (!SetConsoleMode(conHandle, mode))
            {
                // error setting console mode.
            }
        }
        #endregion

        public static readonly int MAX_BUFFER_WIDTH = Console.LargestWindowWidth,
                                   MAX_BUFFER_HEIGHT = Console.LargestWindowHeight;

        /// <summary>
        /// Sets initial console window config such as cursor visibillity, window size and buffer, maximized mode and locked dimensions.
        /// </summary>
        public static void SetConsoleWindowConfig()
        {
            Console.CursorVisible = false;
            Console.TreatControlCAsInput = true;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.SetWindowSize(MAX_BUFFER_WIDTH, MAX_BUFFER_HEIGHT);
                Console.SetBufferSize(MAX_BUFFER_WIDTH, MAX_BUFFER_HEIGHT);
            }
            MaximizeWindow();
        }

        /// <summary>
        /// Centers given text on the screen, based on it's length and window width.
        /// </summary>
        /// <param name="textToCenter">The text to be centered.</param>
        /// <param name="bothSides">Should spaces be added to the right as well as to the left?</param>
        /// <param name="spacesOnly">Should the text be omitted but it's length accounted?</param>
        /// <returns></returns>
        public static string CenterText(string textToCenter, bool bothSides = false, bool spacesOnly = false)
        {
            string _halfSpaces = new(' ', Math.Clamp(((Console.WindowWidth - textToCenter.Length) / 2) - 1,0, Console.WindowWidth));

            if (spacesOnly)
                return bothSides ? _halfSpaces + " " + _halfSpaces + " " : _halfSpaces;
            else
            {
                return bothSides ? _halfSpaces + textToCenter + _halfSpaces + " " : _halfSpaces + textToCenter;
            }
        }

        /// <summary>
        /// Gets the content of the user's clipboard.
        /// </summary>
        /// <returns>A string containing the text in clipboard.</returns>
        public static string ReadClipboard()
        {
            return Clipboard.GetText();
        }
    }
}
