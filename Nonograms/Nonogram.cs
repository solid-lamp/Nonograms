using System.Collections.Generic;
using System.Data.Common;
using System.Deployment.Application;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
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
        /// Czy dana kolumna została już wpisana do końca
        /// </summary>
        public bool[] columns;
        /// <summary>
        /// Czy dany wiersz został już wpisany do końca
        /// </summary>
        public bool[] rows;
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
            columns = new bool[width];
            rows = new bool[height];
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
            var lines = GetEmptyLinesInRow(index);
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
        }
        public bool UpdateBorders()
        {
            var result = false;
            // znaleźć pierwszy nullowaty element
            // jesli jest on bezposrednio przy sciance to nic nie da rady zrobic
            // jesli nad nim jest false to tez nie da rady
            // jesli nad nim jest true to uaktualnic 
            // wszystkie wpisane 'Placed' nad nim
            // cofnac sie na sam poczatek lini ktora mamy dokonczyc
            // dociagnac nastepna linie do konca 
            // wpisac false o ile sie da
            for (int i = 0 ; i < width ; i++)
            {
                // gora
                for (int j = 0 ; j < height ; j++)
                {
                    if (board[j,i] != null)
                        continue;
                    if (j == 0)
                        break;
                    if (board[j - 1, i] == false)
                        break;
                    var tmp = 0;
                    var vect = 0;
                    for (; tmp < j; )
                    {
                        if (board[tmp, i] == true)
                        {
                            tmp += column[i][vect].Vector.Count;
                            column[i][vect++] = new Line(column[i][vect].Vector.Count, true);
                        }
                        tmp++;
                    }
                    while (board[j, i] != false)
                        j--;
                    column[i][vect] = new Line(column[i][vect].Vector.Count, true);
                    int k;
                    for (k = 0 ; k < column[i][vect].Vector.Count ; k++)
                    {
                        board[j, i + k] = true;
                    }
                    if (k != height)
                        board[j, i + k] = false;
                    break;
                }
                // dol

            }
            for (int i = 0 ; i < height ; i++)
            {
                for (int j = 0 ; j < width ; j++)
                {

                }
            }
            return result;
        }
        /// <summary>
        /// Zwraca listę wektorów boolowskich wolnych albo nie zapełnionych linii w wierszu
        /// </summary>
        /// <param name="index">indeks wiersza</param>
        /// <returns>Lista wolnych miejsc</returns>
        public List<bool?[]> GetEmptyLinesInRow(int index)
        {
            var it = -1;
            var prevIt = -1;
            var lines = new List<bool?[]>();
            bool hitEmpty = false; // w pasku jest wolne miejsce
            while (++it < width) 
            {
                if (board[index, it] != false)
                {
                    if (board[index, it] == null)
                        hitEmpty = true;
                    continue;
                }
                if (it - prevIt > 1 && hitEmpty)
                {
                    hitEmpty = false;
                    lines.Add(new bool?[(it - prevIt - 1)]);
                    for (int i = it - prevIt - 2; i >= 0; --i)
                        lines[lines.Count - 1][i] = board[index, prevIt + i+1];
                }
                prevIt = it;
            }
            if (it - prevIt > 1 && hitEmpty)
            {
                lines.Add(new bool?[(it - prevIt - 1)]);
                for (int i = it - prevIt - 2 ; i >= 0 ; --i)
                    lines[lines.Count - 1][i] = board[index, prevIt + i + 1];
            }
            return lines;
        }
    }
}