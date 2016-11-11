using System.Collections.Generic;

namespace Nonograms
{
    internal class Nonogram
    {
        /// <summary>
        ///     Macierz przedstawiająca planszę:
        ///     true - w tym miejscu jest czarne
        ///     false - to miejsce jest na pewno puste (krzyżyk)
        ///     null - to miejsce jest puste
        /// </summary>
        protected bool?[,] board;

        /// <summary>
        ///     Tablica, która przechowuje wektory, które z kolei trzymają w sobie ile odcinków jakich długości jest w danej
        ///     kolumnie
        /// </summary>
        protected readonly List<Line>[] column;

        /// <summary>
        ///     Wysokość planszy
        /// </summary>
        protected readonly int height;

        /// <summary>
        ///     Tablica, która przechowuje wektory, które z kolei trzymają w sobie ile odcinków jakich długości jest w danym
        ///     wierszu
        /// </summary>
        private readonly List<Line>[] row;

        /// <summary>
        ///     Szerokość planszy
        /// </summary>
        private readonly int width;

        /// <summary>
        ///     Tworzy pustą planszę
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        public Nonogram(List<Line>[] row, List<Line>[] column)
        {
            if ((row == null) || (column == null)) return;
            this.row = row;
            this.column = column;
            height = row.Length;
            width = column.Length;
            board = new bool?[height, width];
        }
        public Nonogram(Nonogram n)
        {
            row = new List<Line>[height];
            column = new List<Line>[width];
            for (var i = 0 ; i < height ; i++)
                row[i] = new List<Line>(n.row[i]);
            for (var i = 0 ; i < width ; i++)
                column[i] = new List<Line>(n.column[i]);
            height = n.height;
            width = n.width;
            board = new bool?[height, width];
            for (var i =0;i<width;++i)
                for (var j = 0; j < height; ++j)
                    board[i, j] = n.board[i, j];
            
        }

        public override string ToString()
        {
            return $"Nonogram - board {height}x{width}";
        }

        public bool? CheckRow(int index)
        {
            if ((index <= 0) || (height <= index) || (row == null) || (column == null))
                return null;


            return true;
        }

        public bool IsValid()
        {
            return true;
        }

        public bool UpdateRow()
        {
            return true;
        }
    }
}