using System.Drawing;
using System.Windows.Forms;

namespace EmployeeRecords
{
    // The app's colors (Starbucks-style palette) and the shared look of every data grid.
    public static class Theme
    {
        public static readonly Color Green = Color.FromArgb(0, 112, 74);
        public static readonly Color DarkGreen = Color.FromArgb(30, 57, 50);
        public static readonly Color Background = Color.White;
        public static readonly Color LightGrey = Color.FromArgb(232, 232, 232);
        public static readonly Color RowShade = Color.FromArgb(244, 244, 244);

        public static void StyleGrid(DataGridView grid)
        {
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 36;
            grid.ColumnHeadersDefaultCellStyle.BackColor = DarkGreen;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = LightGrey;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = DarkGreen;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.ForeColor = DarkGreen;
            grid.DefaultCellStyle.SelectionBackColor = Green;
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = RowShade;
            grid.RowTemplate.Height = 30;
            grid.GridColor = LightGrey;
        }
    }
}
