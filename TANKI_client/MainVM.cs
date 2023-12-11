using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace TANKI_client
{
	public class MainVM : NotifyPropertyChanged
	{
		private List<List<Cell>> _battlefield;
		private ICommand _Command;
		private string _commandToSend;
		private bool newCommand;
		private int width = 11;
		private int height = 11;
		public MainVM()
		{
			FillBF();
		}
		public List<List<Cell>> BattleField
		{
			get => _battlefield;
			set
			{
				_battlefield = value;
				OnPropertyChanged();
			}
		}

		public ICommand Command => _Command ?? new RelayCommand(parameter =>
		{
			if (Enum.TryParse(parameter.ToString(), out Commands result))
			{
				_commandToSend = result.ToString();
				newCommand = true;
			}
		});

		public string GetCommandToSend()
		{
			if (newCommand)
			{
				newCommand = false;
				return _commandToSend;
			}
			return null;
		}

		public void ClearMap()
		{
			for (int i = 0; i < width; i++)
			{
				for(int j = 0; j < height; j++)
				{
					if (BattleField[i][j].State == CellState.TankUp ||
						BattleField[i][j].State == CellState.TankDown ||
						BattleField[i][j].State == CellState.TankRight ||
						BattleField[i][j].State == CellState.TankLeft ||
						BattleField[i][j].State == CellState.BulletUp ||
						BattleField[i][j].State == CellState.BulletDown ||
						BattleField[i][j].State == CellState.BulletLeft ||
						BattleField[i][j].State == CellState.BulletRight)
					{
						BattleField[i][j].State = CellState.Empty;
					}
				}
			}
		}

		public void RemoveObj(int X, int Y)
		{
			//_battlefield[X][Y].State = CellState.Empty;
			BattleField[X][Y].State = CellState.Empty;
			//OnPropertyChanged();
		}

		public void AddObj(int X, int Y, CellState state)
		{
			//_battlefield[X][Y].State = state;
			//OnPropertyChanged();
			BattleField[X][Y].State = state;
		}

		public void FillBF()
		{
			_battlefield = new List<List<Cell>>();
			for (int i = 0; i < height; i++)
			{
				List<Cell> row = new List<Cell>();
				for (int j = 0; j < width; j++)
				{
					row.Add(new Cell());
				}
				_battlefield.Add(row);
			}
			var map = "11111111111\n10000000001\n10000000001\n10000100001\n10000100001\n10011111001\n10000100001\n10000100001\n10000000001\n10000000001\n11111111111\n";
			int MapColumn = 0, MapRow = 0;
			foreach(var cellState in map)
			{
				switch (cellState)
				{
					case '1':
						_battlefield[MapColumn][MapRow].State = CellState.Wall;
						MapColumn++;
						break;
					case '0':
						_battlefield[MapColumn][MapRow].State = CellState.Empty;
						MapColumn++;
						break;
					case '\n':
						MapRow++;
						MapColumn = 0;
						break;
				}
			}
		}
	}
}
