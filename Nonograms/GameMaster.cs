//#define PISZ
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Nonograms
{
    internal class GameMaster
    {
        /// <summary>
        /// Główna plansza, na której będzie zaznaczane rozwiązanie.
        /// </summary>
        private Nonogram origin;
        /// <summary>
        ///     Użytkownik wskazuje ścieżkę do pliku, którego zawartość program próbuje sparsować na początkową planszę
        /// </summary>
        /// <returns>
        ///     Początkową planszę do rozwiązania, jeśli się udało wczytać prawidłowo planszę
        ///     null w przeciwnym wypadku  
        /// </returns>
        /// <remarks>Nazwa pliku
        /// {height}x{width}x{paramHeight}x{paramWidth}
        /// </remarks>
        public void LoadBoardFromFile()
        {
            string path;
            int height=0, width=0,paramHeight = 0, paramWidth = 0;

            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = @"Open text file containing board to solve";
                ofd.FileName = string.Empty;
                ofd.Filter = @"Text files (*.txt)|*.txt";
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    path = ofd.FileName;
                    var boardsize = ofd.FileName.Split('\\');
                    boardsize = boardsize[boardsize.Length - 1].Split('.');
                    boardsize = boardsize[0].Split('x');
                    try
                    {
                        height = int.Parse(boardsize[0]);
                        width = int.Parse(boardsize[1]);
                        paramHeight = int.Parse(boardsize[2]);
                        paramWidth = int.Parse(boardsize[3]);
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                else return;
            }
            string content;
            using (var sr = new StreamReader(path))
            {
                try
                {
                    content = sr.ReadToEnd();
                }
                catch (Exception)
                {
                    return;
                }
            }
            var row = new List<Line>[height];
            var column = new List<Line>[width];
            int i;
            for (i = 0; i < height; i++)
                row[i] = new List<Line>();
            for (i = 0 ; i < width ; i++)
                column[i] = new List<Line>();

            if (content.Contains("\t"))
            {
                var tmp = content.Split('\t');
                content = tmp[1];
            }

            var logs = content.Replace("\r\n", "");
            var limit = paramHeight*width;
            const char sep = ' ';
#if PISZ
            Console.WriteLine("Columnsy buduje z:\n");
#endif
            for (i = 0 ; i < limit ; ++i)
            {
#if PISZ
                Console.Write(logs[i]);
#endif
                if(logs[i]!=sep)
                    column[i%width].Add(new Line(int.Parse(logs[i].ToString())));
#if PISZ
                if (i % width == width-1)
                    Console.WriteLine();
#endif
            }
            limit = paramWidth*height;
#if PISZ
            Console.WriteLine("Columnsy buduje z:\n");
#endif
            for ( int j=0 ; j < limit ; ++j)
            {
#if PISZ
                Console.Write(logs[j+i]);
#endif
                if (logs[j+i] !=sep)
                    row[j / paramWidth].Add(new Line(int.Parse(logs[j+i].ToString())));
#if PISZ
                if (j%paramWidth==paramWidth-1)
                    Console.WriteLine();
#endif
            }
            origin = new Nonogram(row,column);
    }
    }
}