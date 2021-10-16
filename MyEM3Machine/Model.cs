using System;
using System.Collections.Generic;

namespace MyEM3Machine
{
	public enum CELL { ADR, COM, A1, A2, A3 }
	public enum REGISTER { COM, A1, A2, A3 }
	public enum REGISTER_INFO { RA, W, Err }
	public enum COMMAND { Start, ShowCommands, SaveCode, OpenFileCode, MnemonicMode, RunCode, Exit }
	public delegate void OutputDataHandler(DataPresentation dataPresentation);
	public delegate List<String[]> InputDataHandler();

	class Model
	{
		public Model()
		{
			cpu = new CPU();
			ram = new RAM();
		}
		public void RegisterIOHandler(InputDataHandler inputData, OutputDataHandler otputData)
		{
			cpu.controlDevice.inputDataDelegate = inputData;
			cpu.controlDevice.outputDataDelegate = otputData;
		}
		public void UpdateMemoryCells()
		{
			if (cpu.controlDevice.inputDataDelegate != null)
				ram.UpdateMemoryCells(cpu.controlDevice.inputDataDelegate());
		}
		public void UpdateExternalScreen()
		{
			if (cpu.controlDevice.outputDataDelegate != null)
				cpu.controlDevice.outputDataDelegate(ram);
		}
		public void Execute(COMMAND command)
		{
			switch (command)
			{
				case COMMAND.Start:
					UpdateExternalScreen();
					break;
				case COMMAND.ShowCommands:
					break;
				case COMMAND.SaveCode:
					break;
				case COMMAND.OpenFileCode:
					break;
				case COMMAND.MnemonicMode:
					break;
				case COMMAND.RunCode:
					UpdateMemoryCells();
					cpu.controlDevice.ExecuteProgram(ram);
					UpdateExternalScreen();
					break;
				case COMMAND.Exit:
					break;
				default:
					break;
			}
		}

		private CPU cpu;
		private RAM ram;

	}
}
