using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolePuzzle_2.Services.Models
{
    public class LineItem
    {
        public string Text { get; set; }
        public ConsoleColor BgColor { get; set; }
        public ConsoleColor FgColor { get; set; }
        public bool CenterLine { get; set; }
        public bool AlignRows { get; set; }
        public bool Active { get; set; }

        /// <summary>
        /// An item to be displayed in a line (could be a letter, a word, a sentence).
        /// </summary>
        /// <param name="text">Text to be shown.</param>
        /// <param name="bgColor">Console background color.</param>
        /// <param name="fgColor">Console foreground color.</param>
        /// <param name="centerLine">Should this text receive spaces to be centered on screen?</param>
        /// <param name="alignRows">Should this text be aligned with items from other lines?</param>
        /// <param name="active">If this is a menu option, is it active? (clickable).</param>
        public LineItem(string text, ConsoleColor bgColor = ConsoleColor.Black, ConsoleColor fgColor = ConsoleColor.Gray, bool centerLine = false, bool alignRows = false, bool active = true)
        {
            Text = text;
            BgColor = bgColor;
            FgColor = fgColor;
            CenterLine = centerLine;
            AlignRows = alignRows;
            Active = active;
        }
    }
}
