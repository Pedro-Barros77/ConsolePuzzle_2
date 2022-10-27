namespace ConsolePuzzle_2.Services.Models
{
    public class UIBlock
    {
        public List<List<LineItem>> Lines { get; set; } = new();

        /// <summary>
        /// Fills the Lines list with lists of LineItem, according to rowsCount.
        /// </summary>
        /// <param name="rowsCount">The number of rows to generate.</param>
        public void Fill(int rowsCount)
        {
            Lines = Enumerable.Range(1, rowsCount).Select(x => new List<LineItem>()).ToList();
        }
    }
}
