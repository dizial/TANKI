using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TANKI_client
{
	public class Cell : NotifyPropertyChanged
	{
		private CellState _state;

		public CellState State
		{
			get { return _state; }
			set
			{
				_state = value;
				OnPropertyChanged();
			}
		}
	}

	public enum CellState
	{
		Empty,
		Wall,
		TankUp,
		TankDown,
		TankLeft,
		TankRight,
		BulletUp,
		BulletDown,
		BulletLeft,
		BulletRight
	}
}
