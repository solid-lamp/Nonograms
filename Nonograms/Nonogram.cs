using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Deployment.Application;
using System.Linq;
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
        /// <summary>
        /// Kopia głęboka planszy
        /// </summary>
        /// <param name="n">plansza do skopiowania</param>
        public Nonogram(Nonogram n)
        {
            // kopia głęboka
            // ograniczyć używanie
            height = n.height;
            width = n.width;
            row = new List<Line>[height];
            rows = new bool[height];
            column = new List<Line>[width];
            columns = new bool[width];
            board = new bool?[height, width];
            for (var i = 0; i < height; i++)
            {
                row[i] = new List<Line>(n.row[i]);
                rows[i] = n.rows[i];
            }
            for (var j = 0; j < width; j++)
            {
                column[j] = new List<Line>(n.column[j]);
                columns[j] = n.columns[j];
            }
            for (var i =0;i<height;++i)
                for (var j = 0; j < width; ++j)
                    board[i, j] = n.board[i, j];
        }
        public override string ToString()
        {
            // w debugerze ładnie wygląda :)
            return $"Nonogram - board {height}x{width}";
        }
        /// <summary>
        /// Sprawdza czy plansza jest jeszcze poprawna
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid()
        {
            // Nonogram ma tylko metody logiczne do rozwiązywania, więc jakby inaczej miałobyć:
            // TODO: a jeśli plansza nie ma rozwiązania
            return true;
        }

        public bool UpdateRow(int index)
        {
            // TODO: algorytm
            // wymyslic algorytm naiwnego wstawiania z obu stron, biorac pod uwage 'na pewno nie' i 'na pewno tak' wartosci
            var nElem = row[index].Count;
            var LeftToRight = new int[width];
            var RightToLeft = new int[width];
            var counterltr = 0;
            var counterrtl = width - 1;
            var lines = GetLinesInRow(index);
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
        /// <summary>
        /// Sprawdza czy na planszy nie ma czasem 'pustych' wierszy albo kolumn
        /// Jeżeli są to krzyżykuje i oznacza dany/ą wiersz/kolumnę jako ukończony/ą
        /// Powinna być wywołana raz dla planszy.
        /// </summary>
        /// <returns>true, jeżeli coś się udało oznaczyć na planszy</returns>
        public bool CheckForEmptyEntries()
        {
            var result = false;
            for (var i = 0 ; i < height ; ++i)
            {
                if (rows[i] || row[i].Count != 0) continue;
                for (var j = 0; j < width; ++j)
                    board[i, j] = false;
                rows[i] = true;
                result = true;
            }
            for (var j = 0 ; j < width ; ++j)
            {
                if (columns[j] || column[j].Count != 0)
                    continue;
                for (var i = 0; i < height; ++i)
                    board[i, j] = false;
                columns[j] = true;
                result = true;
            }
            return result;
        }
        /// <summary>
        /// Sprawdza czy dana wiersz jest ukończony
        /// </summary>
        /// <param name="index">który wiersz</param>
        /// <returns>true, jeżeli jest uzupełniony</returns>
        public bool CheckRow(int index)
        {
            // ReSharper disable once InvertIf
            if (!rows[index])
            {
                var j = 0;
                while (j++ < width)
                {
                    if (board[index, j] == null)
                        break;
                }
                if (j == width)
                    rows[index] = true;
            }
            return rows[index];
        }
        /// <summary>
        /// Sprawdza czy dana kolumna jest ukończona
        /// </summary>
        /// <param name="index">która kolumna</param>
        /// <returns>true, jeżeli jest uzupełniona</returns>
        public bool CheckColumn(int index)
        {
            // ReSharper disable once InvertIf
            if (!columns[index])
            {
                var i = 0;
                while (i++ < height)
                {
                    if (board[i, index] == null)
                        break;
                }
                if (i == width)
                    columns[index] = true;
            }
            return columns[index];
        }
        /// <summary>
        /// Sprawdza czy nie ma rozwiązanych i nieoznaczonych za rozwiązane kolumn/wierszy
        /// </summary>
        /// <returns>zwraca true, jeżeli oznaczył jaką kolumnę/wiersz za ukończony/ą</returns>
        public bool CheckBoard()
        {
            var result = false;
            for (var i = 0; i < height; ++i)
                result |= CheckRow(i);
            for (var j = 0; j < width; ++j)
                result |= CheckColumn(j);
            return result;
        }
        /// <summary>
         /// Dociąga paski od krawędzi
         /// </summary>
         /// <returns>true, jeżeli się udało coś dopisać, false w przeciwnym wypadku</returns>
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
            // TODO: styl
            // poprawić indeksacji aby była bardziej jednolita

            for (var i = 0 ; i < width ; i++)
            {
                if (columns[i])
                    continue;

                // gora
                for (var j = 0 ; j < height ; j++)
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
                            // a co jak to jest pusty wektor? 
                            // sprawdzaj czy kolumna nie jest juz wypelniona
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
                    else
                        columns[i] = true; // to byl ostatni dociagniety pasek
                    break;
                }
                // krok wyzej dopisalismy cala kolumne
                if (columns[i])
                    continue;
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
        /// Zwraca listę pasków w wierszu oddzielonych między sobą 'krzyżykiem/kami' 
        /// </summary>
        /// <param name="index">w którym wierszu</param>
        /// <returns>Lista pasków z zaznaczonymi wartościami</returns>
        public List<bool?[]> GetLinesInRow(int index)
        {
            // na chwile obecna zwracamy wszystkie paski, nawet te uzupełnione całkowicie
            var it = -1;
            var prevIt = -1;
            var lines = new List<bool?[]>();
            //var hitEmpty = false; // w pasku jest wolne miejsce
            /* 
             * x - na pewno tak
             * | - na pewno nie
             * . - puste
             * 
             * Mając sytuację
             * w do wpisania [2|2] i planszę -> [...|xx|...]      
             * algorytm wpisując naiwnie mając informacje tylko 
             * paskach [...] i [...] wpisałby
             *         [.x.] i [.x.] co by prowadziło 
             * do [2|2] [.x.|xx|.x.]
             * niby jest opcja sprawdzania 'Placed' ale w takich przypadkach nie wiadomo, która to była dwójka
             */

            while (++it < width) 
            {
                if (board[index, it] != false)
                {

                    /*
                    if (board[index, it] == null)
                        hitEmpty = true;
                        */
                    continue;
                }
                if (it - prevIt > 1/* && hitEmpty*/)
                {
                    /*hitEmpty = false;*/
                    lines.Add(new bool?[(it - prevIt - 1)]);
                    for (var i = it - prevIt - 2; i >= 0; --i)
                        lines[lines.Count - 1][i] = board[index, prevIt + i+1];
                }
                prevIt = it;
            }
            // ReSharper disable once InvertIf
            if (it - prevIt > 1/* && hitEmpty*/)
            {
                lines.Add(new bool?[(it - prevIt - 1)]);
                for (var i = it - prevIt - 2 ; i >= 0 ; --i)
                    lines[lines.Count - 1][i] = board[index, prevIt + i + 1];
            }
            return lines;
        }
        /// <summary>
        /// Zwraca listę pasków w kolumnie oddzielonych między sobą 'krzyżykiem/kami'
        /// </summary>
        /// <param name="index">w której kolumnie</param>
        /// <returns>Lista pasków z zaznaczonymi wartościami</returns>
        public List<bool?[]> GetLinesInColumn(int index)
        {
            // Patrz GetLinesInRow
            
            var it = -1;
            var prevIt = -1;
            var lines = new List<bool?[]>();
            while (++it < height)
            {
                if (board[index, it] != false)
                    continue;
                if (it - prevIt > 1)
                {
                    lines.Add(new bool?[(it - prevIt - 1)]);
                    for (var i = it - prevIt - 2 ; i >= 0 ; --i)
                        lines[lines.Count - 1][i] = board[index, prevIt + i + 1];
                }
                prevIt = it;
            }
            // ReSharper disable once InvertIf
            if (it - prevIt > 1)
            {
                lines.Add(new bool?[(it - prevIt - 1)]);
                for (var i = it - prevIt - 2 ; i >= 0 ; --i)
                    lines[lines.Count - 1][i] = board[index, prevIt + i + 1];
            }
            return lines;
        }
    }
}