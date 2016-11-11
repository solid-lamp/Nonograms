using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms
{
    /// <summary>
    /// Struktura paska
    /// </summary>
    public struct Line
    {
        public List<bool> Vector;
        /// <summary>
        /// Czy pasek został już umieszczony całkowicie
        /// </summary>
        public bool Placed { get; set; }
        /// <summary>
        /// Tworzy pasek o długości length i ustawia wartości kolejnych pól na false
        /// </summary>
        /// <param name="length">długość wektora</param>
        public Line(int length)
        {
            Vector = new List<bool>(new bool[length]);
            Placed = false;
        }
    }
}
