using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Nonograms
{
    public class GameMaster
    {
        /// <summary>
        ///     Główna plansza, na której będzie zaznaczane rozwiązanie.
        /// </summary>
        public Nonogram origin;



        /// <summary>
        ///     Użytkownik wskazuje ścieżkę do pliku, którego zawartość program próbuje sparsować na początkową planszę
        /// </summary>
        /// <returns>
        ///     Początkową planszę do rozwiązania, jeśli się udało wczytać prawidłowo planszę
        ///     null w przeciwnym wypadku
        /// </returns>
        /// <remarks>
        ///     Nazwa pliku
        ///     {height}x{width}x{paramHeight}x{paramWidth}
        ///     Rzuca wyjątkami
        /// </remarks>
        public void LoadBoardFromFile()
        {
            string path;
            int height = 0, width = 0, paramHeight = 0, paramWidth = 0;

            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = @"Open text file containing board to solve";
                ofd.FileName = string.Empty;
                ofd.Filter = @"Text files (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;
                path = ofd.FileName;
                var boardsize = ofd.FileName.Split('\\');
                boardsize = boardsize[boardsize.Length - 1].Split('.');
                boardsize = boardsize[0].Split('x');
                height = int.Parse(boardsize[0]);
                width = int.Parse(boardsize[1]);
                paramHeight = int.Parse(boardsize[2]);
                paramWidth = int.Parse(boardsize[3]);
            }
            string content;
            using (var sr = new StreamReader(path))
            {
                content = sr.ReadToEnd();
            }
            var row = new List<Line>[height];
            var column = new List<Line>[width];
            for (var i = 0; i < height; i++)
                row[i] = new List<Line>();
            for (var i = 0; i < width; i++)
                column[i] = new List<Line>();

            if (content.Contains("\t"))
            {
                var tmp = content.Split('\t');
                content = tmp[1];
            }
            var logs = content.Replace("\r\n", "x");
            var limit = paramHeight*width;
            const char empty = ' ';
            const char sep = 'x';
            var counter = 0; // do przechodzenia po stringu
            var chars = "";
            for (var i = 0; i < limit;)
                switch (logs[counter])
                {
                    case empty:
                        i++;
                        goto case sep;
                    case sep:
                        counter++;
                        break;
                    default:
                        chars = "";
                        while (logs[counter] != sep)
                            chars += logs[counter++];
                        column[i++%width].Add(new Line(int.Parse(chars)));
                        break;
                }

            limit = paramWidth*height;
            for (var j = 0; j < limit;)
                switch (logs[counter])
                {
                    case empty:
                        j++;
                        goto case sep;
                    case sep:
                        counter++;
                        break;
                    default:
                        chars = "";
                        while (logs[counter] != sep)
                            chars += logs[counter++];
                        row[j++/paramWidth].Add(new Line(int.Parse(chars)));
                        break;
                }

            origin = new Nonogram(row, column);
        }
    }
}