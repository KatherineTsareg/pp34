using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace SimpleSocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                SendMessageFromSocket(11000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
            }
        }
        static void SendMessageFromSocket(int port)
        {
            // Буфер для входящих данных
            byte[] bytes = new byte[4];

            // Соединяемся с удаленным устройством
            // Устанавливаем удаленную точку для сокета
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);

            Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Соединяем сокет с удаленной точкой
            sender.Connect(ipEndPoint);
            
            //Получаем от сервера количество итераций для подсчета
            int bytesRec = sender.Receive(bytes);
            Array.Reverse(bytes);
            int iterationsNumber = BitConverter.ToInt32(bytes, 0);
            //Получаем количество вхождений точек в окружность
            Stopwatch sWatch = new Stopwatch();
            sWatch.Start();

            int hitsNumber = Pi.GetHitsCount(iterationsNumber);

            sWatch.Stop();
            TimeSpan tSpan;
            tSpan = sWatch.Elapsed;
            int ms = tSpan.Milliseconds;

            var outByteList = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(hitsNumber)).ToList();
            var timeBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(ms));
            outByteList.AddRange(timeBytes);
            // Отправляем данные через сокет
            var outBytes = outByteList.ToArray();
            sender.Send(outBytes);

            // Освобождаем сокет
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
        }
    }
}
