﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms
{
    internal class NonogramWorker : Nonogram
    {
        private bool assume;
        private int row;
        private int column;
        /// <summary>
        /// Tworzy kopie planszy, do metody rekurencyjnej
        /// Pamięta założoną wartość i gdzie była zakładana
        /// </summary>
        /// <param name="n">Plansza do skopiowania</param>
        /// <param name="row">w którym wierszu</param>
        /// <param name="column">w której kolumnie</param>
        /// <param name="value">jaką wartość zakładamy</param>
        public NonogramWorker(Nonogram n, int row, int column, bool value) : base(n)
        {
            this.row = row;
            this.column = column;
            assume = value;
        }
        /// <summary>
        /// Metoda, która próbuje rozwiązać rekurencyjnie (maksymalnie 10 zejść)
        /// </summary>
        /// <returns>
        ///     true, jeżeli rozwiązano planszę
        ///     null, jeżeli po 10 założeniach do niczego się nie doszło
        ///     false, wykryto sprzeczność
        /// </returns>
        public bool? RSolve()
        {
            return null;
        }


    }
}