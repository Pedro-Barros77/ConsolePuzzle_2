using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using ConsolePuzzle_2.Utility;

namespace ConsolePuzzle_2
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            try
            {
                var menu = MenuController.GetInstance();
                var gameController = GameController.GetInstance();

                ConsoleUtility.SetConsoleWindowConfig();
                gameController.StartReadingInput();

                menu.Startup();
            }
            catch (Exception ex)
            {
                Console.Clear();
                Console.ResetColor();
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Console.BufferHeight = 500;
                Console.WriteLine("Exception: " + ex.Message);
                Console.WriteLine("Inner: " + ex.InnerException?.Message);
                Console.WriteLine("StackTrace: " + ex.StackTrace);
                Console.ReadKey();
            }
        }
    }
}