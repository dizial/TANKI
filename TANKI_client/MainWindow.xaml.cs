using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Windows.Markup;
using System.Threading;


namespace TANKI_client
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainVM vm = new MainVM();

		private Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		private IPAddress localAddress;
		private int port;
		private int myID;
		private bool initMessage = true;
		public MainWindow()
		{

			DataContext = vm;
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			ConnectButton.IsEnabled = false;
			localAddress = IPAddress.Parse(textBoxIp.Text);
			port = int.Parse(textBoxPort.Text);
			Reg();
			Thread listener = new Thread(() => StartGame());
			listener.Start();
		}

		public void StartGame() //"t0:10x10x3;b0:2x1x1;s0:0;h0:100"
		{
			
			while(true) 
			{
				var endPoint = new IPEndPoint(localAddress, port);
				var message = vm.GetCommandToSend();
				if (message != null)
				{
					byte[] data = Encoding.UTF8.GetBytes(message);	
					socket.SendTo(data, endPoint);
				}
				

				byte[] bytes = new byte[1024];
				socket.Receive(bytes);

				vm.ClearMap();

				string InformationFromServer = Encoding.UTF8.GetString(bytes);
				if (initMessage)
				{
					myID = Convert.ToInt32(InformationFromServer[0].ToString());
					string newMap = InformationFromServer.Substring(2);
					if (newMap != null)
						vm.SetBF(newMap);
					initMessage = false;
				}
				string[] IFSparse = InformationFromServer.Split(';');

				int indexOfDD;
				int count;

				string tmpStringCoordX;
				string tmpStringCoordY;
				string tmpStringHp;
				string tmpStringScore;
				int tmpDirection;
				CellState tmpCellState;
				
				foreach (var s in IFSparse) 
				{
					tmpStringCoordX = "";
					tmpStringCoordY = "";
					//score
					if (s[0] == 's')
					{
						if (Convert.ToInt32(s[1].ToString()) == myID)
						{
							tmpStringScore = "";
							count = 3;
							while (count != s.Length)
							{
								tmpStringScore += s[count];
								count++;
								
							}
							vm.SetScore(Convert.ToInt32(tmpStringScore));
						}
					}
					//hp
					if (s[0] == 'h')
					{
						if (Convert.ToInt32(s[1].ToString()) == myID)
						{
							tmpStringHp = "";
							count = 3;
							while (count != s.Length)
							{
								tmpStringHp += s[count];
								count++;
							}
							vm.SetHealth(Convert.ToInt32(tmpStringHp));
						}
					}
					//bullet
					if (s[0] == 'b')
					{
						indexOfDD = s.IndexOf(':');
						count = indexOfDD + 1;
						while (s[count] != 'x')
						{
							tmpStringCoordX += s[count].ToString();
							count++;
						}
						count++;
						while (s[count] != 'x')
						{
							tmpStringCoordY += s[count].ToString();
							count++;
						}
						count++;
						tmpDirection = Convert.ToInt32(s[count].ToString());
						switch (tmpDirection)
						{
							case 0:
								tmpCellState = CellState.BulletDown;
								break;
							case 1:
								tmpCellState = CellState.BulletLeft;
								break;
							case 2:
								tmpCellState = CellState.BulletUp;
								break;
							case 3:
								tmpCellState = CellState.BulletRight;
								break;
							default:
								tmpCellState = CellState.BulletDown;
								break;
						}
						vm.AddObj(Convert.ToInt32(tmpStringCoordY), Convert.ToInt32(tmpStringCoordX), tmpCellState);
					}
					//tank
					if (s[0] == 't')
					{
						indexOfDD = s.IndexOf(':');
						count = indexOfDD + 1;
						while (s[count] != 'x')
						{
							tmpStringCoordX += s[count].ToString();
							count++;
						}
						count++;
						while (s[count] != 'x')
						{
							tmpStringCoordY += s[count].ToString();
							count++;
						}
						count++;
						tmpDirection = Convert.ToInt32(s[count].ToString());
						switch (tmpDirection)
						{
							case 0:
								tmpCellState = CellState.TankDown;
								break;
							case 1:
								tmpCellState = CellState.TankLeft;
								break;
							case 2:
								tmpCellState = CellState.TankUp;
								break;
							case 3:
								tmpCellState = CellState.TankRight;
								break;
							default:
								tmpCellState = CellState.TankDown;
								break;
					}
					vm.AddObj(Convert.ToInt32(tmpStringCoordY), Convert.ToInt32(tmpStringCoordX), tmpCellState);
						
					}
				
				}
			}
		}

		public void Reg() // регистрация на сервере
		{
			var endPoint = new IPEndPoint(localAddress, port);
			var message = "Name:" + playerName.Text;

			byte[] data = Encoding.UTF8.GetBytes(message);
			socket.SendTo(data, endPoint);
		}

	}

}
