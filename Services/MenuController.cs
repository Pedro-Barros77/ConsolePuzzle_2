using ConsolePuzzle_2.Screens;
using System.Reflection;
using System.Resources;
using static ConsolePuzzle_2.Utility.Enums;
using ConsolePuzzle_2.Services;
using System.Text.RegularExpressions;
using ConsolePuzzle_2.Content;
using System.Text;


namespace ConsolePuzzle_2
{
    /// <summary>
    /// This is a singleton which provides its methods as a service. It controls user interface such as screens and configs.
    /// </summary>
    public sealed class MenuController
    {
        #region Singleton
        private MenuController(){}
        private static MenuController? _instance;

        public static MenuController GetInstance()
        {
            if (_instance == null)
            {
                _instance = new MenuController();
            }
            return _instance;
        }
        #endregion

        #region Properties
        /// <summary>
        /// The opened pages in order.
        /// </summary>
        public Stack<Page> PagesHistory { get; } = new();
        /// <summary>
        /// The page currently active/running.
        /// </summary>
        public Page CurrentPage { get { return PagesHistory.Peek(); } }
        /// <summary>
        /// If keyboard input reading should be disabled or not. Set to true for in-game typing.
        /// </summary>
        public bool DisableGameInput { get; set; }
        /// <summary>
        /// Gets the path of the project root (...\\ConsolePuzzle_2).
        /// </summary>
        public string ProjectRootPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        public float FPS { get; set; } = 0;

        public System.Timers.Timer FPS_timer { get; private set; } = new(250);
        #endregion

        #region Private Properties
        private ResourceManager Resources = new("ConsolePuzzle_2.Resources.pt-BR_local", Assembly.GetExecutingAssembly());

        private readonly Dictionary<Languages, string> LanguageFile = new()
        {
            { Languages.Portuguese, "pt-BR_local" }
        };

        private string ConsoleTitle = "";

        private Tuple<Pages, Level?>? ScheduledPage { get; set; }
        #endregion

        #region Control Variables
        private Languages? _lastLanguage = null;
        private int FPS_Count = 0;
        #endregion

        private readonly bool DISABLE_TITLE_UPDATING = false;

        /// <summary>
        /// Opens the first page (main menu) to start the game.
        /// </summary>
        public void Startup()
        {
            FPS_timer.Elapsed += (sender, e) => HandleFPSQuarter();
            FPS_timer.Start();

            OpenPage(Pages.MainMenu);
        }

        /// <summary>
        /// Opens a new page on the screen and adds it to the history.
        /// </summary>
        /// <param name="pageToOpen">The new page to open.</param>
        /// <param name="pageToClose">The previous page that will stop running.</param>
        /// <param name="gameLvl">The level assossiated to the page, if exists.</param>
        public void OpenPage(Pages pageToOpen, Level? gameLvl = null)
        {
            Page? _newPage;
            switch (pageToOpen)
            {
                case Pages.MainMenu:
                    _newPage = new MainMenu();
                    break;

                case Pages.Tutorials:
                    _newPage = new Tutorials();
                    break;

                case Pages.Options:
                    _newPage = new Options();
                    break;

                case Pages.Level:
                    if (gameLvl == null) return;
                    _newPage = gameLvl;
                    break;

                case Pages.Pause:
                    _newPage = new Pause(gameLvl!);
                    break;

                case Pages.CompletedLevel:
                    _newPage = new LevelComplete(gameLvl!);
                    break;

                case Pages.LevelCreator:
                    _newPage = new LevelCreator();
                    break;
                default:
                    return;
            }
            if (!PushPageInHistory(_newPage)) return;

            RunPage();
        }

        /// <summary>
        /// Goes back one page in the history.
        /// </summary>
        /// <returns>True if returned successfully, otherwise, false.</returns>
        public bool ReturnPage()
        {
            if (PagesHistory.Count <= 1) return false;
            PagesHistory.Pop();
            GenerateConsoleTitle(CurrentPage);
            CurrentPage.UpdateUI();
            return true;
        }

        /// <summary>
        /// Opens pause page.
        /// </summary>
        /// <param name="gameLvl">Current playing game to be paused.</param>
        public void PauseGame(Level? gameLvl)
        {
            OpenPage(Pages.Pause, gameLvl);
        }

        /// <summary>
        /// Clears page history and keep only MainMenu, then schedule new page to be open after code stack returns to the start.
        /// </summary>
        /// <param name="pageToOpen">The page to be scheduled.</param>
        /// <param name="gameLvl">The active level, if current page is one.</param>
        public void ResetPagesAndScheduleNew(Pages pageToOpen, Level? gameLvl = null)
        {
            Page? mainMenu = PagesHistory.ToArray().LastOrDefault();
            PagesHistory.Clear();
            PagesHistory.Push(mainMenu ?? new MainMenu());
            ScheduledPage = new(pageToOpen, gameLvl);
        }

        /// <summary>
        /// Gets the string assossiated to the chosen language.
        /// </summary>
        /// <param name="en_version">The original string in english version.</param>
        /// <returns>The translated version of the string.</returns>
        public string GetString(string en_version)
        {
            if (ConfigManager.Language == Languages.English) return en_version;
            if (ConfigManager.Language != _lastLanguage)
            {
                _lastLanguage = ConfigManager.Language;
                string fileName = LanguageFile[ConfigManager.Language];
                Resources = new ResourceManager($"ConsolePuzzle_2.Resources.{fileName}", Assembly.GetExecutingAssembly());
            }

            string? result = Resources.GetString(en_version);

            if (result == null) return en_version;
            return result;
        }

        /// <summary>
        /// Set's the title of the game console.
        /// </summary>
        /// <param name="text"></param>
        public void SetTitle(string text)
        {
            if (DISABLE_TITLE_UPDATING) return;
            Console.Title = $"{text} (FPS: {FPS})";
        }

        /// <summary>
        /// Creates a title for the console, based on current page type.
        /// </summary>
        /// <param name="page">The page currently on screen.</param>
        /// <returns>The string added to the console title.</returns>
        public string GenerateConsoleTitle(Page page)
        {
            string pageName = Regex.Replace(page.PageType.ToString(), "([A-Z])", " $1").Trim();
            string title;
            switch (page.PageType)
            {
                case Pages.Level:
                    string gameClass = page.ToString() ?? "";
                    bool isTutorial = gameClass.Contains("Tutorials");
                    title = $"Console Puzzle Game 2 - {GetString(isTutorial ? "Tutorial" :"Level")} {page.LevelNumber}";
                    break;
                case Pages.CompletedLevel:
                case Pages.Pause:
                    title = $"Console Puzzle Game 2 - {pageName} ({GetString("Tutorial")} {page.LevelNumber})";
                    break;
                default:
                    title = "Console Puzzle Game 2 - " + GetString(pageName);
                    break;
            }
            ConsoleTitle = title;
            SetTitle(ConsoleTitle);
            return ConsoleTitle;
        }

        /// <summary>
        /// Runs the current opened page.
        /// </summary>
        private void RunPage()
        {
            CurrentPage.Startup();
            GenerateConsoleTitle(CurrentPage);
            Pages _runningPage = CurrentPage.PageType;
            while (CurrentPage.PageType == _runningPage)
            {
                if (ScheduledPage != null)
                {
                    Tuple<Pages, Level?>? toOpen = ScheduledPage;
                    ScheduledPage = null;
                    if (toOpen.Item1 == CurrentPage.PageType)
                    {
                        CurrentPage.UpdateUI();
                        CurrentPage.Startup();
                        GenerateConsoleTitle(CurrentPage);
                    }
                    else
                        OpenPage(toOpen.Item1, toOpen.Item2);
                }

                CurrentPage.Run();

                //For debug only, remove
                if (DISABLE_TITLE_UPDATING)
                {
                    //Title Debug Here
                }
                FPS_Count++;
            }
        }

        /// <summary>
        /// Adds a new page to the history, if not already in.
        /// </summary>
        /// <param name="page">The page to be added.</param>
        /// <returns>Whether it was added successfully or not.</returns>
        private bool PushPageInHistory(Page page)
        {
            if (!PagesHistory.Any() || !PagesHistory.Select(x => x.PageType).Contains(page.PageType))
            {
                PagesHistory.Push(page);
                GenerateConsoleTitle(CurrentPage);
                return true;
            }
            return false;
        }

        private void HandleFPSQuarter()
        {
            FPS = ((float)FPS_Count * 4);
            FPS_Count = 0;
            SetTitle(ConsoleTitle);
        }
    }
}
