using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nonograms
{
    class Nonogram
    {
        /// <summary>
        /// Macierz przedstawiająca planszę:
        ///     true - w tym miejscu jest czarne
        ///     false - to miejsce jest na pewno puste (krzyżyk)
        ///     null - to miejsce jest puste
        /// </summary>
        private bool?[,] board;
        /// <summary>
        /// Wysokość planszy
        /// </summary>
        private int height;
        /// <summary>
        /// Szerokość planszy
        /// </summary>
        private int width;
        /// <summary>
        /// Tablica, która przechowuje wektory, które z kolei trzymają w sobie ile odcinków jakich długości jest w danym wierszu
        /// </summary>
        private List<Line>[] row;
        /// <summary>
        /// Tablica, która przechowuje wektory, które z kolei trzymają w sobie ile odcinków jakich długości jest w danej kolumnie
        /// </summary>
        private List<Line>[] column;
        /// <summary>
        /// Tworzy pustą planszę
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public Nonogram(List<Line>[] row, List<Line>[] column)
        {
            if (row == null || column == null) return;
            this.row = row;
            this.column = column;
            height = row.Length;
            width = column.Length;
            board = new bool?[height,width];
        }

        public override string ToString()
        {
            return $"Nonogram - board {height}x{width}";
        }
    }
}
