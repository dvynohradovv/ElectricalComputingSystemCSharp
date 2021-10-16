using System;
using System.Collections.Generic;


namespace MyEM3Machine
{
	/// <summary>
	/// CPU in Electrical Computing System
	/// </summary>
	public class CPU
	{
		public CPU()
		{
			controlDevice = new ControlDevice();
		}
		public class ControlDevice
		{
			private static Dictionary<String, int> COMMANDS = new Dictionary<String, int>
			{
				{ "ПЕР", 0 }, { "СЛВ", 1 },
				{ "ВЧВ", 2 }, { "УМВ", 3 },
				{ "ДЕВ", 4 }, { "ВВВ", 5 },
				{ "ВВЦ", 6 }, { "БЕЗ", 9 },
				{ "ЦЕЛ", 10 }, { "СЛЦ", 11 },
				{ "ВЧЦ", 12 }, { "УМЦ", 13 },
				{ "ДЕЦ", 14 }, { "ВЫВ", 15 },
				{ "ВЫЦ", 16 }, { "УСЛ", 19 },
				{ "ВЕЩ", 20 }, { "МОД", 24 },
				{ "ОСТ", 31 }
			};
			private String diagnosticResponse_OK = "ПРОГРАММА ЗАВЕРШИЛАСЬ УСПЕШНО...\n";
			// private String diagnosticResponse_LOGIC_ERR = "ПРОГРАММА ЗАВЕРШИЛАСЬ С ЛОГИЧЕСКОЙ ОШИБКОЙ...\n";
			private String diagnosticResponse_SYNTAX_ERR = "ПРОГРАММА ЗАВЕРШИЛАСЬ С СИНТАКСИЧЕСКОЙ ОШИБКОЙ...\n";
			private String diagnosticResponse_STOP_ERR = "ПРОГРАММА ЗАВЕРШИЛАСЬ из-за отсутствия комманды СТОП...\n";
			private String diagnosticResponse_UNKNOWN_ERR = "ПРОГРАММА ЗАВЕРШИЛАСЬ С НЕИЗВЕСТНОЙ ОШИБКОЙ...\n";
			private String additionalResponse_OK = "ОШИБОК В РЕГИСТРАХ НЕ ОБНАРУЖЕНО...\n";
			private String additionalResponse_ERR = "ОШИБКУ ВЫЗВАЛ РЕГИСТР ПАМЯТИ ПОД НОМЕРОМ: ";
			private String saveResponse_OK = "ПРОГРАММА СОХРАНЕНА";
			public void SaveCode()
			{

			}
			public void OpenFileCode()
			{

			}
			public void ExecuteProgram(RAM ram)
			{
				ram.ClearCellValues();
				int stopAddress;
				ConstInitialize(ram, out stopAddress);
				// A1 - result, A2 - operand1, A3 - operand2
				if (ram.err == 0)
					CodeRun(ram, stopAddress);
			}
			private void ConstInitialize(RAM ram, out int stopAddress)
			{
				stopAddress = 0;
				try
				{
					int registerAddress;
					bool isStopCommand = false;
					for (registerAddress = 0; registerAddress < RAM.MEMORY_CELL_QUANTITY; registerAddress++)
					{
						if (!isStopCommand)
						{
							isStopCommand = ram.GetMemoryCellAt(registerAddress).commandStr.ToUpper() == "ОСТ";
							stopAddress = registerAddress + 1;
						}
						double cellValue = ram.GetMemoryCellAt(registerAddress).a3;
						ram.SetCellValue(registerAddress, cellValue);
					}
				}
				catch (Exception)
				{
					ram.err = 1;
					ram.diagnostic = diagnosticResponse_STOP_ERR;
					ram.additional = additionalResponse_ERR + " " + registerAddress;
				}
			}
			private void CodeRun(RAM ram, int stopAddress)
			{
				try
				{
					// Выполнение
					for (registerAddress = 1; registerAddress <= stopAddress; registerAddress++)
					{
						ram.ra = registerAddress;
						int command = 0;
						MemoryCell registerCommand = ram.GetMemoryCellAt(registerAddress);
						bool commandExist = COMMANDS.TryGetValue(registerCommand.commandStr.ToUpper(), out command);
						if (!commandExist)
						{
							ram.diagnostic = diagnosticResponse_SYNTAX_ERR;
							ram.additional = additionalResponse_ERR;
							break;
						}
						bool isLogicCommand = ArithmeticLogicUnit.LOGIC_COMMAND.Contains(command);
						int a1Address = registerCommand.a1;
						int a2Address = registerCommand.a2;
						int a3Address = registerCommand.a3;
						if (isLogicCommand)
						{
							double a1Value = ram.GetCellValue(a1Address);
							double a2Value = ram.GetCellValue(a2Address);
							double a3Value = ram.GetCellValue(a3Address);
							switch (command)
							{
								case 1:
									ram.SetCellValue(a1Address, ArithmeticLogicUnit.Addition(a2Value, a3Value));
									break;
								case 2:
									ram.SetCellValue(a1Address, ArithmeticLogicUnit.Subtraction(a2Value, a3Value));
									break;
								case 3:
									ram.SetCellValue(a1Address, ArithmeticLogicUnit.Multiplication(a2Value, a3Value));
									break;
								case 4:
									ram.SetCellValue(a1Address, ArithmeticLogicUnit.Division(a2Value, a3Value));
									break;
								case 11:
									ram.SetCellValue(a1Address, ArithmeticLogicUnit.Addition(a2Value, a3Value));
									break;
								case 12:
									ram.SetCellValue(a1Address, ArithmeticLogicUnit.Subtraction(a2Value, a3Value));
									break;
								case 13:
									ram.SetCellValue(a1Address, ArithmeticLogicUnit.Multiplication(a2Value, a3Value));
									break;
								case 14:
									ram.SetCellValue(a1Address, ArithmeticLogicUnit.Division(a2Value, a3Value));
									break;
								case 24:
									ram.SetCellValue(a1Address, ArithmeticLogicUnit.Mod(a2Value, a3Value));
									break;
							}
						}
						else
						{
							switch (command)
							{
								case 0:
									break;
								case 5:
									break;
								case 6:
									break;
								case 9:
									break;
								case 10:
									break;
								case 15:
									for (int i = 1; i <= a2Address; i++)
									{
										ram.output += " " + ram.GetCellValue(a1Address + i - 1);
									}
									ram.output += "\n";
									break;
								case 16:
									for (int i = 1; i <= a2Address; i++)
									{
										ram.output += " " + ram.GetCellValue(a1Address + i - 1);
									}
									ram.output += "\n";
									break;
								case 19:
									break;
								case 20:
									break;
								case 31:
									ram.err = 0;
									ram.diagnostic = diagnosticResponse_OK;
									ram.additional = additionalResponse_OK;
									break;
								default:
									break;
							}
						}
					}
				}
				catch (Exception)
				{
					ram.diagnostic = diagnosticResponse_UNKNOWN_ERR;
					ram.additional = additionalResponse_ERR + " " + registerAddress;
					ram.err = 1;
				}
			}
			public OutputDataHandler outputDataDelegate;
			public InputDataHandler inputDataDelegate;
			private int registerAddress;
		}
		public static class ArithmeticLogicUnit
		{
			public static List<int> LOGIC_COMMAND = new List<int> { 1, 2, 3, 4, 11, 12, 13, 14, 24 };
			public static double Addition(double a2, double a3)
			{
				return a2 + a3;
			}
			public static double Subtraction(double a2, double a3)
			{
				return a2 - a3;
			}
			public static double Multiplication(double a2, double a3)
			{
				return a2 * a3;
			}
			public static double Division(double a2, double a3)
			{
				return a2 / a3;
			}
			public static double Mod(double a2, double a3)
			{
				return a2 % a3;
			}
		}
		public ControlDevice controlDevice { get; private set; }
	}
}
