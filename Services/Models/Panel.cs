using System.Text;
using ConsolePuzzle_2.Utility.Extensions;
using static ConsolePuzzle_2.Utility.Enums;

namespace ConsolePuzzle_2.Services.Models
{

    public class ConsolePanel
    {
        #region Properties
        /// <summary>
        /// The raw text to be displayed.
        /// </summary>
        public string Text { get; private set; }
        /// <summary>
        /// The time in milliseconds to wait before next screen update.
        /// </summary>
        public float Delay { get; private set; }
        /// <summary>
        /// The alignment of the text on the screen.
        /// </summary>
        public Alignment TextAlign { get; private set; }
        /// <summary>
        /// If this panel should slide the text as an animation or not.
        /// </summary>
        public bool Slide { get; private set; }
        /// <summary>
        /// The color of the pixels that represents the letters.
        /// </summary>
        public ConsoleColor TextColor { get; private set; }
        /// <summary>
        /// The color of the pixels that represents the background of the panel.
        /// </summary>
        public ConsoleColor BgColor { get; private set; }
        /// <summary>
        /// The last position of the cursor when the printing finished.
        /// </summary>
        public int LastCursorTop { get; private set; } = 5;
        #endregion

        #region Control Variables
        private readonly string Display;
        private readonly int Width;
        private readonly int textAlignSpaces;
        private int firstColumnOffset = 0;
        private DateTime nextUpdateTime;
        #endregion


        /// <summary>
        /// Creates a new custom panel.
        /// </summary>
        /// <param name="text">The text to be displayed as 3x5 pixel letters.</param>
        /// <param name="delay">The time between each movement, if slide set to true.</param>
        /// <param name="textAlign">The starting position of the text.</param>
        /// <param name="slide">Should the text has sliding animation? Set false to static.</param>
        /// <param name="textColor">The color of the pixels that represent the letters.</param>
        /// <param name="bgColor">The color of the pixels that represent the background of the letters.</param>
        public ConsolePanel(string text, int delay = 0, Alignment textAlign = Alignment.Center, bool slide = true, ConsoleColor textColor = ConsoleColor.White, ConsoleColor bgColor = ConsoleColor.Black)
        {
            Text = text.RemoveDiacritics().ToUpper();
            Delay = delay;
            TextAlign = textAlign;
            Slide = slide;
            TextColor = textColor;
            BgColor = bgColor;
            nextUpdateTime = DateTime.Now.AddMilliseconds(Delay);

            Width = ((Console.BufferWidth / 3) - Text.Length) - 1;

            StringBuilder sb = new();
            for (int row = 0; row < 5; row++)
            {
                for (int letter = 0; letter < Text.Length; letter++)
                {
                    if (PanelLetters.ContainsKey(Text[letter]))
                    {
                        string? pattern = PanelLetters[Text[letter]];
                        sb.Append(pattern[0 + (row * 3)]);
                        sb.Append(pattern[1 + (row * 3)]);
                        sb.Append(pattern[2 + (row * 3)]);
                    }

                    if (letter != Text.Length - 1)
                    {
                        sb.Append(' ');
                    }
                }
            }
            Display = sb.ToString();

            int contentLength = Text.Length * 3 + (Text.Length - 1);

            switch (TextAlign)
            {
                case Alignment.Center:
                    textAlignSpaces = ((Console.BufferWidth - contentLength) / 2) - 1;
                    break;
                case Alignment.Left:
                    textAlignSpaces = 0;
                    break;
                case Alignment.Right:
                    textAlignSpaces = (Console.BufferWidth - contentLength) - 1;
                    break;
                case Alignment.Hidden:
                    textAlignSpaces = Console.BufferWidth - 1;
                    break;
            }

            Display = Display.Insert(contentLength * 4, new string(';', textAlignSpaces));
            Display = Display.Insert(contentLength * 3, new string(';', textAlignSpaces));
            Display = Display.Insert(contentLength * 2, new string(';', textAlignSpaces));
            Display = Display.Insert(contentLength * 1, new string(';', textAlignSpaces));
            Display = Display.Insert(contentLength * 0, new string(';', textAlignSpaces));
        }

        /// <summary>
        /// Plays the next slide animation of the panel if it has one, otherwise just prints the static text.
        /// </summary>
        public void SlidePanel()
        {
            if (DateTime.Compare(DateTime.Now, nextUpdateTime) >= 0)
            {
                nextUpdateTime = DateTime.Now.AddMilliseconds(Delay);
                WriteOnPanel(Text);
                if (Slide)
                {
                    firstColumnOffset++;
                    if (Width == (Console.BufferWidth / 3) - Text.Length - 1)
                    {
                        if (firstColumnOffset >= Console.BufferWidth - 1 + (Text.Length * 3 + (Text.Length - 1)))
                        {
                            firstColumnOffset = 0;
                        }
                    }
                    else
                    {
                        if (firstColumnOffset >= Width + (Text.Length * 3 + (Text.Length - 1)))
                        {
                            firstColumnOffset = 0;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Writes the text on the panel as colored pixels.
        /// </summary>
        /// <param name="text">The text to be displayed.</param>
        private void WriteOnPanel(string text)
        {
            int currentRow = 0;
            int contentLength = text.Length * 3 + (text.Length - 1);

            for (int currentPixel = 0; currentPixel < Display.Length; currentPixel++)
            {
                bool printPixel = true;

                //If pixel's position is somehow greater than the total number of pixels in the current row
                if (currentPixel < firstColumnOffset + (currentRow * (text.Length * 4 - 1 + textAlignSpaces)))
                    printPixel = false;

                //If pixel is outside of the screen
                if (TextAlign == Alignment.Hidden && currentPixel - ((contentLength + textAlignSpaces) * currentRow) >= textAlignSpaces + firstColumnOffset)
                    printPixel = false;

                if (printPixel)
                {
                    if (Display[currentPixel] == '1')
                    {
                        Console.BackgroundColor = TextColor;
                        Console.Write(" ");
                    }
                    else if (Display[currentPixel] == '0' || Display[currentPixel] == ' ')
                    {
                        Console.BackgroundColor = BgColor;
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write(" ");
                    }
                }

                //If it's the last pixel of any row
                if (currentPixel == (contentLength + textAlignSpaces) * 1 - 1 ||
                    currentPixel == (contentLength + textAlignSpaces) * 2 - 1 ||
                    currentPixel == (contentLength + textAlignSpaces) * 3 - 1 ||
                    currentPixel == (contentLength + textAlignSpaces) * 4 - 1 ||
                    currentPixel == (contentLength + textAlignSpaces) * 5 - 1)
                {
                    currentRow++;

                    Console.BackgroundColor = BgColor;
                    Console.Write(new string(' ', (Console.WindowWidth - Console.CursorLeft) - 1) + "\n");

                    if (currentRow == 5)
                    {
                        LastCursorTop = Console.CursorTop;
                    }
                }
            }
        }

        /// <summary>
        /// Pixel mapping of characters.
        /// </summary>
        private readonly Dictionary<char, string> PanelLetters = new()
        {
            { 'A', "010101111101101" },
            { 'B', "110101110101110" },
            { 'C', "111100100100111" },
            { 'D', "110101101101110" },
            { 'E', "111100110100111" },
            { 'F', "111100110100100" },
            { 'G', "011100100101011" },
            { 'H', "101101111101101" },
            { 'I', "111010010010111" },
            { 'J', "111010010010110" },
            { 'K', "101101110101101" },
            { 'L', "100100100100111" },
            { 'M', "101111111101101" },
            { 'N', "010101101101101" },
            { 'O', "010101101101010" },
            { 'P', "111101111100100" },
            { 'Q', "010101101111011" },
            { 'R', "111101110101101" },
            { 'S', "011100010001110" },
            { 'T', "111010010010010" },
            { 'U', "101101101101111" },
            { 'V', "101101101101010" },
            { 'W', "101101111111101" },
            { 'X', "101101010101101" },
            { 'Y', "101101010010010" },
            { 'Z', "111001010100111" },
            { ' ', "000000000000000" },
            { '1', "010110010010010" },
            { '2', "111001111100111" },
            { '3', "111001011001111" },
            { '4', "101101111001001" },
            { '5', "111100111001111" },
            { '6', "111100111101111" },
            { '7', "111001001001001" },
            { '8', "111101111101111" },
            { '9', "111101111001111" },
            { '0', "111101101101111" },
            { '?', "111001010000010" },
            { '!', "010010010000010" },
            { ':', "000010000010000" },
            { ',', "000010000010000" },
            { '-', "000000111000000" }
        };
    }
}
