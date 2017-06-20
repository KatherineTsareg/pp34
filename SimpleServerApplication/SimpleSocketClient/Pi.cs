using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SimpleSocketClient
{
    class Pi
    {
        static public int GetHitsCount(int iterationsNumber)
        {
            Stopwatch sWatch = new Stopwatch();
            sWatch.Start();

            Random rand = new Random();
            int count = 0;
            double x, y;
            for (int i = 0; i < iterationsNumber; i++)
            {
                x = (double)rand.Next(-10000, 10000) / 10000;
                y = (double)rand.Next(-10000, 10000) / 10000;
                if ((x * x + y * y) <= 1) ++count;
            }

            sWatch.Stop();
            TimeSpan tSpan;
            tSpan = sWatch.Elapsed;
            Console.WriteLine("Client working time: " + tSpan.ToString() + "\n");
            Console.WriteLine("Result of client working: " + count + "\n");
            return count;
        }
        static public double Get(int hitsNumber, int iterationsNumber)
        {
            return (double)hitsNumber / iterationsNumber * 4;
        }
    }
}
