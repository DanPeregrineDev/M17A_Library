using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M17A_Library
{
    internal class Print
    {
        int nLines, nPage = 1;

        public void PrintGrid(PrintDocument printDocument1, System.Drawing.Printing.PrintPageEventArgs e, DataGridView grid)
        {
            Graphics printer = e.Graphics;
            Font letterType = new Font("Arial", 10);
            Font largeLetterType = new Font("Arial", 12, FontStyle.Bold);
            Brush color = Brushes.Black;
            float marginLeft, marginRight, marginUp, marginDown, line, width;
            Pen pen = new Pen(color, 2);

            marginLeft = printDocument1.DefaultPageSettings.Margins.Left;
            marginRight = printDocument1.DefaultPageSettings.Bounds.Right - marginLeft;
            marginUp = printDocument1.DefaultPageSettings.Margins.Top;
            marginDown = printDocument1.DefaultPageSettings.Bounds.Height - marginUp;

            width = marginRight - marginLeft;

            float[] columns = new float[grid.Columns.Count];
            float gridWidth = 0;

            for (int i = 0; i < grid.Columns.Count; i++)
            {
                gridWidth += grid.Columns[i].Width;
            }
                
            columns[0] = marginLeft;
            float total = marginLeft, columnWidth;

            for (int i = 0; i < grid.Columns.Count - 1; i++)
            {
                columnWidth = grid.Columns[i].Width / gridWidth;
                columns[i + 1] = columnWidth * width + total;
                total = columns[i + 1];
            }

            for (int i = 0; i < grid.Columns.Count; i++)
            {
                printer.DrawString(grid.Columns[i].HeaderText, largeLetterType, color, columns[i], marginUp);
            }

            line = marginUp + largeLetterType.Height;

            int l;

            for (l = nLines; l < grid.Rows.Count; l++)
            {
                printer.DrawLine(pen, marginLeft, line, marginRight, line);

                for (int c = 0; c < grid.Columns.Count; c++)
                {
                    printer.DrawString(grid.Rows[l].Cells[c].Value.ToString(),
                       
                    letterType, color, columns[c], line);
                }

                line = line + letterType.Height;

                if (line + letterType.Height > marginDown)
                {
                    break;
                }

            }

            if (l < grid.Rows.Count)
            {
                nLines = l + 1;
                e.HasMorePages = true;
            }

            printer.DrawString("Página " + nPage.ToString(), letterType, color, marginLeft, marginDown);

            nPage++;

            printer.DrawLine(pen, marginLeft, marginUp, marginRight, marginUp);

            printer.DrawLine(pen, marginLeft, line, marginRight, line);

            for (int c = 0; c < columns.Length; c++)
            {
                printer.DrawLine(pen, columns[c], marginUp, columns[c], line);
            }

            printer.DrawLine(pen, marginRight, marginUp, marginRight, line);
        }
    }
}
