using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSocketClient
{
    class Pi
    {
        static public int GetHitsCount(int iterationsNumber)
        {
            Random rand = new Random();
            int count = 0;
            double x, y;
            for (int i = 0; i < iterationsNumber; i++)
            {
                x = (double)rand.Next(-10000, 10000) / 10000;
                y = (double)rand.Next(-10000, 10000) / 10000;
                if ((x * x + y * y) <= 1) ++count;
            }
            return count;
        }
        static public double Get(int hitsNumber, int iterationsNumber)
        {
            return (double)hitsNumber / iterationsNumber * 4;
        }
    }
}
