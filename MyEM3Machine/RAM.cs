using System;
using System.Collections.Generic;

namespace MyEM3Machine
{
	public class MemoryCell
	{
		public MemoryCell()
		{
			address = 0;
			// commandInt = 0;
			commandStr = "ПЕР";
			a1 = 0;
			a2 = 0;
			a3 = 0;
		}
		public MemoryCell(int address)
		{
			this.address = address;
			// commandInt = 0;
			commandStr = "ПЕР";
			a1 = 0;
			a2 = 0;
			a3 = 0;
		}
		public MemoryCell(int address, /* int commandInt,*/ String commandStr, int a1, int a2, int a3)
		{
			this.address = address;
			// this.commandInt = commandInt;
			this.commandStr = commandStr;
			this.a1 = a1;
			this.a2 = a2;
			this.a3 = a3;
		}
		//public String[] GetWithIntCommand()
		//{
		//	return new String[] {
		//		address.ToString(),
		//		commandInt.ToString(),
		//		a1.ToString(),
		//		a2.ToString(),
		//		a3.ToString()
		//	};
		//}
		public String[] GetData()
		{
			return new String[] {
					address.ToString("D3"),
					commandStr,
					a1.ToString("D3"),
					a2.ToString("D3"),
					a3.ToString("D3")
				};
		}
		public void UpdateData(string[] memoryCellData)
		{
			commandStr = memoryCellData[(int)CELL.COM];
			a1 = Convert.ToInt32(memoryCellData[(int)CELL.A1]);
			a2 = Convert.ToInt32(memoryCellData[(int)CELL.A2]);
			a3 = Convert.ToInt32(memoryCellData[(int)CELL.A3]);
		}
		public bool isEqual(string[] memoryCellData)
		{
			if (this.GetData() == memoryCellData)
				return true;
			return false;
		}
		public int address { get; protected set; }
		// public int commandInt { get; set; }
		public String commandStr { get; protected set; }
		public int a1 { get; protected set; }
		public int a2 { get; protected set; }
		public int a3 { get; protected set; }
		
		
	}
	public class Registers
	{
		public Registers()
		{
			r1 = new MemoryCell();
			r2 = new MemoryCell();
			s = new MemoryCell();
			rk = new MemoryCell();
		}

		/*
		public String[,] GetRegistersData()
		{
			String[,] result = new String[REGISTERS_CELL_QUANTITY, REGISTERS_CELL_QUANTITY];
			for (int row = 0; row < REGISTERS_CELL_QUANTITY; row++)
			{
				String[] tmpMemoryCellData = r1.GetMemoryCellData();
				result[row, (int)REGISTER.COM] = tmpMemoryCellData[(int)REGISTER.COM];
				result[row, (int)REGISTER.A1] = tmpMemoryCellData[(int)REGISTER.A1];
				result[row, (int)REGISTER.A2] = tmpMemoryCellData[(int)REGISTER.A2];
				result[row, (int)REGISTER.A3] = tmpMemoryCellData[(int)REGISTER.A3];
			}
			return result;
		}
		*/
		public List<String[]> GetRegistersData()
		{
			List<String[]> result = new List<string[]>(DataPresentation.REGISTERS_CELL_QUANTITY);
			result.Add(r1.GetData());
			result.Add(r2.GetData());
			result.Add(s.GetData());
			result.Add(rk.GetData());
			return result;
		}
		private MemoryCell r1;
		private MemoryCell r2;
		private MemoryCell s;
		private MemoryCell rk;
	}
	/// <summary>
	/// The presentation of data in Electrical Computing System
	/// </summary>
	public class DataPresentation
	{
		public static int MEMORY_CELL_QUANTITY = 512;
		public static int REGISTERS_CELL_QUANTITY = 4;
		public static int REGISTERS_INFO_QUANTITY = 3;
		public DataPresentation()
		{
			memoryCells = new List<MemoryCell>(MEMORY_CELL_QUANTITY);
			for (int i = 0; i < MEMORY_CELL_QUANTITY; i++)
			{
				memoryCells.Add(new MemoryCell(i));
			}
			registers = new Registers();
			ra = 0;
			w = 0;
			err = 0;
			input = "ВВЕДИТЕ НЕ ТРЕБУЕТСЯ...";
			output = "ВЫВОД ОТСУТСВУЕТ...";
			currentFile = "ФАЙЛ НЕ СОХРАНЕН...";
			diagnostic = "ДИАГНОСТИКА ОТСУТСТВУЕТ...";
			additional = "ДОПОЛНИТЕЛЬНАЯ ИНФОРМАЦИЯ ОТСУТСТВУЕТ...";
		}
		public MemoryCell GetMemoryCellAt(int address)
		{
			return memoryCells[address];
		}
		public List<String[]> GetCellsData()
		{
			List<String[]> result = new List<String[]>(REGISTERS_CELL_QUANTITY);
			foreach (var item in memoryCells)
			{
				result.Add(item.GetData());
			}
			return result;
		}
		public void UpdateMemoryCells(List<String[]> updated_CellsData)
		{
			foreach (var cell in updated_CellsData)
			{
				int cell_adr = Convert.ToInt32(cell[(int)CELL.ADR]);
				if (!memoryCells[cell_adr].isEqual(cell))
				{
					memoryCells[cell_adr].UpdateData(cell);
				}
			}
		}
		public List<String[]> GetRegistersData()
		{
			return registers.GetRegistersData();
		}
		/*
		public String[,] GetMemoryCellsData()
		{
			int editor_column = 5;
			String[,] result = new String[MEMORY_CELL_QUANTITY, editor_column];
			for (int memoryCell = 0; memoryCell < MEMORY_CELL_QUANTITY; memoryCell++)
			{
				String[] tmpMemoryCell = memoryCells[memoryCell].GetMemoryCellData();

				result[memoryCell, (int)CELL.ADR] = tmpMemoryCell[(int)CELL.ADR];
				result[memoryCell, (int)CELL.COM] = tmpMemoryCell[(int)CELL.COM];
				result[memoryCell, (int)CELL.A1] = tmpMemoryCell[(int)CELL.A1];
				result[memoryCell, (int)CELL.A2] = tmpMemoryCell[(int)CELL.A2];
				result[memoryCell, (int)CELL.A3] = tmpMemoryCell[(int)CELL.A3];
			}
			return result;
		}
		public String[,] GetRegistersData()
		{
			return registers.GetRegistersData();
		}
		*/
		public String[] GetRegistersInfoData()
		{
			String[] result = new String[REGISTERS_INFO_QUANTITY];
			result[(int)REGISTER_INFO.RA] = ra.ToString("D3");
			result[(int)REGISTER_INFO.W] = ra.ToString("D3");
			result[(int)REGISTER_INFO.Err] = ra.ToString("D3");
			return result;
		}
		public String input { get; set; }
		public String output { get; set; }
		public String currentFile { get; set; }
		public String diagnostic { get; set; }
		public String additional { get; set; }

		public List<MemoryCell> memoryCells;
		public Registers registers;
		public int ra;
		public int w;
		public int err;
	}
	/// <summary>
	/// RAM in Electrical Computing System
	/// </summary>
	public class RAM : DataPresentation
	{
		public RAM() : base()
		{
			cellValues = new List<double>(MEMORY_CELL_QUANTITY) { 0.0 };
		}
		public void ClearCellValues()
		{
			cellValues = new List<double>(MEMORY_CELL_QUANTITY) { 0.0 };
		}
		public double GetCellValue (int address)
		{
			return cellValues[address];
		}
		public void SetCellValue(int address, double value)
		{
			cellValues[address] = value;
		}
		public List<double> cellValues { get; protected set; }
	}
}
