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
        public bool Placed;
        /// <summary>
        /// Tworzy pasek o długości length i ustawia wartości kolejnych pól na false, chyba ze podano wartosc domyslna
        /// </summary>
        /// <param name="length">długość wektora</param>
        public Line(int length, bool value= false)
        {
            if (value == false)
                Vector = new List<bool>(new bool[length]);
            else
            {
                Vector = new List<bool>(length);
                for (int i = 0 ; i < length ; i++)
                {
                    Vector[i] = true;
                }
            }
            Placed = value;
        }
    }
}
