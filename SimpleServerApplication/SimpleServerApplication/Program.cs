using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SimpleSocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int iterationCount = Convert.ToInt32(args[0]);
            int clientsCount = Convert.ToInt32(args[1]);
            int hitsCount = 0;
            byte[] bytes = new byte[4];
            // Устанавливаем для сокета локальную конечную точку
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 11000);

            // Создаем сокет Tcp
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            List<Socket> sockets = new List<Socket>();
            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(clientsCount);
                Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);
                // Начинаем слушать соединения
                while (true)
                {
                    // Программа приостанавливается, ожидая входящее соединение
                    //Socket handler = sListener.Accept();
                    sockets.Add(sListener.Accept());
                    Console.Write("Подключено " + sockets.Count + " клиентов.\n");

                    if (sockets.Count == clientsCount)
                    {
                        foreach (var handler in sockets)
                        {
                            //Отправляем ответ клиенту в виде числа итераций
                            byte[] msg = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(iterationCount / clientsCount));
                            handler.Send(msg);
                        }
                        for (int i=0;i<sockets.Count;i++)
                        {
                            byte[] outbytes = new byte[8];
                            //Принимаем ответ от клиента
                            sockets[i].Receive(outbytes);
                            Array.Reverse(outbytes);
                            int time = BitConverter.ToInt32(outbytes, 0);
                            int res = BitConverter.ToInt32(outbytes, 4);
                            hitsCount += res;
                            // Показываем данные в консоли
                            Console.Write("Клиент #"+ i + ": \nвернул значение, равное " + res + "\n");
                            Console.Write("за " + time + " миллисекунд.\n\n");

                            sockets[i].Shutdown(SocketShutdown.Both);
                            sockets[i].Close();
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                var pi = (double)hitsCount / iterationCount * 4;
                Console.Write("\n\nPi равен " + Convert.ToString(pi) + "\n");
            }
        }
    }
}
