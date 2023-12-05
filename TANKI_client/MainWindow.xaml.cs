using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace TANKI_client
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private int angle = 0;
		DispatcherTimer timer = new DispatcherTimer();
		private int speed, velocityX, velocityY;

		public MainWindow()
		{
			InitializeComponent();
			Canvas.SetLeft(Tank, 100);
			//Tank.RenderTransform = new RotateTransform(450);

			Rectangle tank2 = new Rectangle();
			tank2.Width = 25;
			tank2.Height = 55;
			tank2.RadiusX = 10;
			tank2.RadiusY = 10;
			tank2.Fill = Brushes.Black;
			Canvas.SetLeft(tank2, 150);
			Canvas.SetTop(tank2, 100);
			Field.Children.Add(tank2);

			timer.Tick += TimerEvent;
			timer.Interval = TimeSpan.FromMilliseconds(20);
			timer.Start();

			Field.Focus();
		}

		private void TimerEvent(object sender, EventArgs e)
		{
			Canvas.SetLeft(Tank, Canvas.GetLeft(Tank) + velocityX);
			Canvas.SetTop(Tank, Canvas.GetTop(Tank) + velocityY);
		}

		private void KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.A)
			{
				angle--;
				if(angle < 0)
					angle = 359;
				Tank.RenderTransform = new RotateTransform(angle, Tank.Width/2, Tank.Height/2);
			}

			if (e.Key == Key.D)
			{
				angle++;
				if (angle > 359)
					angle = 0;
				Tank.RenderTransform = new RotateTransform(angle, Tank.Width / 2, Tank.Height / 2);
			}

			if (e.Key == Key.W)
			{
				if(speed < 10)
					speed++;
				this.velocityX = Convert.ToInt32(speed*Math.Cos(angle));
				this.velocityY = Convert.ToInt32(speed*Math.Sin(angle));
			}

		}

		private void KeyUp(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.W)
			{
				speed = 0;
				this.velocityX = 0;
				this.velocityY = 0;
			}
		}
	}
}
