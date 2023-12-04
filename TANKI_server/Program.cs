// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace TANKI_server
{
    public class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddr = new IPAddress(new byte[] { 127, 0, 0, 100 });  //iphost.addresslist[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, 8000);

            // Создаем сокет Tcp/Ip
            Socket sListener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Назначаем сокет локальной конечной точке и слушаем входящие сокеты
            try
            {
                sListener.Bind(ipEndPoint);
                sListener.Listen(10);

                // Начинаем слушать соединения
                while (true)
                {
                    Console.WriteLine("Ожидаем соединение через порт {0}", ipEndPoint);

                    // Программа приостанавливается, ожидая входящее соединение
                    Socket handler = sListener.Accept();
                    string data = null;

                    // Мы дождались клиента, пытающегося с нами соединиться

                    byte[] bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    data += Encoding.UTF8.GetString(bytes, 0, bytesRec);

                    string nomber = null;
                    string action = null;
                    string reply = null;

                    // Показываем данные на консоли
                    Console.Write("Полученный текст: " + data + "\n");

                    string[] words = data.Split(' ');

                    if (words.Length == 2)
                    {
                        nomber = words[0];
                        action = words[1];
                        Console.WriteLine($"{nomber} + {action}");
                        Tank droid = new Tank();
                        /*reply = "Спасибо за запрос в " + data.Length.ToString()
                            + " символов\n" + droid.Tell();*/
                    }
                    else
                    {
                        reply = "Спасибо за запрос в " + data.Length.ToString()
                            + " символов";
                    }

                    // Отправляем ответ клиенту\
                    byte[] msg = Encoding.UTF8.GetBytes(reply);
                    handler.Send(msg);

                    if (data.IndexOf("<TheEnd>") > -1)
                    {
                        Console.WriteLine("Сервер завершил соединение с клиентом.");
                        break;
                    }

                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}