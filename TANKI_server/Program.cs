// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Collections.Specialized.BitVector32;
using System.Numerics;



namespace TANKI_server
{
    public class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddr = new IPAddress(new byte[] { 127, 0, 0, 100 });  //iphost.addresslist[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 8000);

            Socket tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                tcpListener.Bind(ipEndPoint);
                tcpListener.Listen();    // запускаем сервер
                Console.WriteLine("Сервер запущен. Ожидание подключений... ");

                // Начинаем слушать соединения
                while (true)
                {
                    // получаем подключение в виде TcpClient
                    //var tcpClient = await sListener.AcceptAsync();
                    var tcpClient = tcpListener.AcceptAsync();

                    // создаем новую задачу для обслуживания нового клиента
                    Task.Run(async () => await ProcessClientAsync(tcpClient));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.ReadLine();
            }

            async Task ProcessClientAsync(Socket tcpClient)
            {
                string data = null;

                byte[] bytes = new byte[1024];
                int bytesRec = Convert.ToInt32(tcpClient.ReceiveAsync(bytes));

                data += Encoding.UTF8.GetString(bytes, 0, bytesRec);
                // Показываем данные на консоли
                Console.Write("Полученный текст: " + data + "\n");

                string nomber = null;
                string action = null;
                string reply = null;

                string[] words = data.Split(' ');

                if (words.Length == 2)
                {
                    nomber = words[0];
                    action = words[1];
                    Console.WriteLine($"{nomber} + {action}");
                    Tank droid = new Tank();
                    reply = "Спасибо за запрос в " + data.Length.ToString()
                        + " символов\n" + "";
                }
                else
                {
                    reply = "Спасибо за запрос в " + data.Length.ToString()
                        + " символов";
                }

                // Отправляем ответ клиенту\
                byte[] msg = Encoding.UTF8.GetBytes(reply);
                await tcpClient.SendAsync(msg);

                tcpClient.Shutdown(SocketShutdown.Both);
                tcpClient.Close();
            }
        }
    }
}