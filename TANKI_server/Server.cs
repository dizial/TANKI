using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;


namespace TANKI_server
{
    class Server
    {
        
        public Dictionary<EndPoint, (string, int)> Clients = new Dictionary<EndPoint, (string, int)>();
        // Dictionary<string, EndPoint> Names = new Dictionary<string, EndPoint>();
        string data = null;
        string datatosend = null;
        byte[] bytes = new byte[1024];
        byte[] bytestosend = new byte[1024];
        Socket socket;
        EndPoint ipEndPoint;
        IPAddress address = null;
        Mappy mappy;
        Mutex mutex, mutexMappy;



        public void Socketinit()
        {
            //string data = null;
            // byte[] bytes = new byte[1024];
            mutex = new Mutex();           
            mappy = new Mappy();
            mutexMappy = new Mutex(); 
            mappy.LoadFromFile();
            address = new IPAddress(new byte[] { 0, 0, 0, 0 });  //
            ipEndPoint = new IPEndPoint(address, 8000);
            socket = new Socket(address.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                socket.Bind(ipEndPoint);
                //socket.Listen(10);
           

                Thread acceptThrd = new Thread(new ParameterizedThreadStart(acceptfunc)); //Создаем новый объект потока (Thread)
                acceptThrd.Start(socket); //запускаем поток
                while (true)
                {
                    mutexMappy.WaitOne();
                    try
                    {
                       
                        mappy.WorldStep();
                        datatosend = mappy.GetMapData();
                        // mutexMappy.ReleaseMutex();
                        // mutex.WaitOne();
                        var ClientsToRemove = Clients.ToArray();
                        foreach (var client in ClientsToRemove)
                        {
                            //MessageBox.Show(""+ datatosend);
                            if (mappy.GetTankByID(client.Value.Item2) == null) 
                            {
                                Clients.Remove(client.Key);
                                continue;
                            }
                               

                            SendData(datatosend + mappy.GetScore(client.Value.Item2) + mappy.GetHPOfClient(client.Value.Item2), client.Key);
                            
                        }
                       
                    }
                    catch
                    {
                    }
                    mutexMappy.ReleaseMutex();
                    Thread.Sleep(100);
                }
            }
            catch
            {
				Console.Write("нет соединения/ ошибка");
            }
        }
        public void acceptfunc(object parametr)
        {
            
            Socket handler = (Socket)parametr;
            int ID = 0;
            int bytesRec = 0;
            
            while (true)
            {
                try
                {
                    bytesRec = handler.ReceiveFrom(bytes,  ref ipEndPoint);
                }
                catch(Exception E)
                {
                   Console.Write(E.Message);
                }
               
                if (bytesRec > 0)
                {
                    data = Encoding.UTF8.GetString(bytes, 0, bytesRec);
                    Console.WriteLine("Пришел пакет: " + data + "\n\n");
           
                    switch (data) 
                    {
                        /*
                        case "GibMeTonk":

                            string map;
                            map = mappy.GetMapAndID(ID);
                            SendData(map, ipEndPoint);
                            mutex.WaitOne();
                            if (!Clients.ContainsKey(ipEndPoint)) 
                            {
                              //  Clients.Add(ipEndPoint, ID);
                                mappy.CreateTank(ID);
                                ID++;
                            }
                            mutex.ReleaseMutex();
                            
                            //надо еще заспавнить танк по спавн координатам

                            //вызов функции отправки данных и отправляем карту и айди
                            break;
                        */
                        case "MoveForward":
                            //int CurrentID = Clients[ipEndPoint];
                            // int CurrentID = Clients.Where(m => m.Value.Item1 == ipEndPoint).First().Value.Item2;
                            // Tank tank = mappy.GetTankByID(CurrentID);
                            mutexMappy.WaitOne();
                            try
                            {
                                int CureentId = Clients[ipEndPoint].Item2;
                                Tank tank = mappy.GetTankByID(CureentId);
                                tank.LastActionTime = DateTime.Now;
                                if (tank != null)
                                {

                                    if (tank.Direction == 2)
                                        mappy.Move(tank);
                                    else
                                        tank.TargetDirection = 2;

                                }
                                else Clients.Remove(ipEndPoint);
                            }
                            catch { Console.WriteLine("быдло"); }
                            mutexMappy.ReleaseMutex();
                            
                            //if (mappy.GetTankByID(Clients.Where(m => m.Value.Item1 == ipEndPoint).First().Value.Item2) == null)
                            // Clients.Remove(data.Substring(5));

                            break;
                        case "MoveBackward":
                            mutexMappy.WaitOne();
                            try
                            {
                                int CureentId = Clients[ipEndPoint].Item2;
                                Tank tank = mappy.GetTankByID(CureentId);
                                tank.LastActionTime = DateTime.Now;
                                if (tank != null)
                                    if (tank != null)
                                    {

                                        //  mappy.TurnBackward(mappy.GetTankByID(ID));
                                        if (tank.Direction == 0)
                                            mappy.Move(tank);
                                        else
                                            tank.TargetDirection = 0;

                                    }
                            }
                            catch { Console.WriteLine("быдло"); }
                            mutexMappy.ReleaseMutex();
                            break;
                        case "TurnRight":
                            mutexMappy.WaitOne();
                            try
                            {
                                int CureentId = Clients[ipEndPoint].Item2;
                                Tank tank = mappy.GetTankByID(CureentId);
                                tank.LastActionTime = DateTime.Now;
                                if (tank != null)
                                {
                                    // mutexMappy.WaitOne();
                                    if (tank.Direction == 3)
                                        mappy.Move(tank);
                                    else
                                        tank.TargetDirection = 3;
                                    //  mutexMappy.ReleaseMutex();
                                }
                            }
                            catch { Console.WriteLine("быдло"); }
                            mutexMappy.ReleaseMutex();
                            break;
                        case"TurnLeft":
                            try
                            {
                                mutexMappy.WaitOne();
                                int CureentId = Clients[ipEndPoint].Item2;
                                Tank tank = mappy.GetTankByID(CureentId);
                                tank.LastActionTime = DateTime.Now;
                                if (tank != null)
                                {
                                   
                                    if (tank.Direction == 1)
                                        mappy.Move(tank);
                                    else
                                        tank.TargetDirection = 1;
                                    
                                }
                            }
                            catch { Console.WriteLine("быдло"); }
                            mutexMappy.ReleaseMutex();
                            break;
                        case "Fire":
                            try
                            {
                                mutexMappy.WaitOne();
                                int CureentId = Clients[ipEndPoint].Item2;
                                Tank tank = mappy.GetTankByID(CureentId);
                                tank.LastActionTime = DateTime.Now;
                                if (tank != null)
                                {

                                    tank.Fire(mappy);

                                }
                                mutexMappy.ReleaseMutex();
                            }
                            catch { Console.WriteLine("быдло"); }
                            break;
                      /*  case "Disconnect":
                            CurrentID = Clients.Where(m => m.Value.Item1 == ipEndPoint).First().Value.Item2;
                           
                            mappy.DeleteTank(CurrentID);
                            Clients.Remove(ipEndPoint);
                            
                            break;*/
                        default:

                            if (Clients.Where(m => m.Value.Item1 == data.Substring(5)).Count() == 0)
                            {
                                Clients.Add(ipEndPoint, (data.Substring(5), ID));
                                string map;
                                map = mappy.GetMapAndID(ID);
                                SendData(map, ipEndPoint);
                                mutex.WaitOne();
                             //   if (!Clients.ContainsKey(data.Substring(5)))
                             //   {
                                    //  Clients.Add(ipEndPoint, ID);
                                    mappy.CreateTank(ID);
                                    ID++;
                            //    }
                                mutex.ReleaseMutex();
                            }
                            else SendData("!", ipEndPoint);


                            break;
                    }

                    
                    

                }
                else
                {
                    break;
                }
            }

        }
        public void SendData(string datatosend, EndPoint IP)
        {
           
            bytestosend = Encoding.UTF8.GetBytes(datatosend);
            socket.SendTo(bytestosend, IP);
        }
       

    } 
}
