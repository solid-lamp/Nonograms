using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;

namespace Nonograms
{
    public class Nonogram
    {
        /// <summary>
        ///     Macierz przedstawiająca planszę:
        ///     true - w tym miejscu jest czarne
        ///     false - to miejsce jest na pewno puste (krzyżyk)
        ///     null - to miejsce jest puste
        /// </summary>
        public bool?[,] board;

        /// <summary>
        ///     Tablica, która przechowuje wektory, które z kolei trzymają w sobie ile odcinków jakich długości jest w danej
        ///     kolumnie
        /// </summary>
        public List<Line>[] column;

        /// <summary>
        ///     Wysokość planszy
        /// </summary>
        public int height;

        /// <summary>
        ///     Tablica, która przechowuje wektory, które z kolei trzymają w sobie ile odcinków jakich długości jest w danym
        ///     wierszu
        /// </summary>
        public List<Line>[] row;

        /// <summary>
        ///     Szerokość planszy
        /// </summary>
        public int width;

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

        public bool UpdateRow(int index)
        {
            int nElem = row[index].Count;
            var LeftToRight = new int[width];
            var RightToLeft = new int[width];
            var counterltr = 0;
            var counterrtl = width - 1;
           
            for (int i = 0 ; i < nElem ; i++)
            {
                var lewa = row[index][i].Vector.Count;
                var prawa = row[index][nElem - i - 1].Vector.Count;
                while (lewa--!=0)
                {
                    LeftToRight[counterltr++] = i+1;
                }
                counterltr++;
                while (prawa-- != 0)
                {
                    RightToLeft[counterrtl--] = nElem - i;
                }
                counterrtl--;
            }
            var result = false;
            for (int i = 0 ; i < width ; i++)
            {
                if (LeftToRight[i] == RightToLeft[i] && RightToLeft[i] != 0)
                {
                    board[index, i] = result = true;
                }
            }
            return result;
            //int nElem = row[index].Count;
            //if (nElem < 1)
            //    return false;
            //int sLength = 0;
            //int mLength = -1;
            //for (int i = 0; i < nElem; i++)
            //{
            //    sLength += row[index][i].Vector.Count;
            //    mLength = mLength < row[index][i].Vector.Count
            //        ? row[index][i].Vector.Count
            //        : mLength;
            //}
            //sLength += (nElem - 1);
            //if (2*sLength > width && )

        }
        /// <summary>
        /// Zwraca listę wolnych linii w wierszu
        /// </summary>
        /// <param name="index">indeks wiersza</param>
        /// <returns>Lista wolnych miejsc</returns>
        public List<Line> GetEmptyLinesInRow(int index)
        {
            var it = -1;
            var prev_it = -1;
            var list = new List<Line>();
            while (++it < width)
            {
                if (board[index, it] != false)
                    continue;
                if (it - prev_it > 1)
                    list.Add(new Line(it-prev_it-1));
                prev_it = it;
            }
            if (it - prev_it > 1)
                list.Add(new Line(it - prev_it - 1));
            return list;
        }
    }
}