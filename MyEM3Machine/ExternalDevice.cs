using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ElectricalComputingSystem;

namespace MyEM3Machine
{

	public partial class ExternalDevice : Form
	{
		/// <summary>
		/// Electrical Computing System
		/// ExternalDevice include input-output and storage devices:
		///		#External Keyboard,
		///		#External Screen,
		///		#External Storage Device
		/// </summary>
		public ExternalDevice()
		{
			InitializeComponent();


			model = new Model();
			model.RegisterIOHandler(new InputDataHandler(GetEditorData), new OutputDataHandler(UpdateScreenData));
			model.Execute(COMMAND.Start);
		}
		public void UpdateScreenData(DataPresentation dataPresentation)
		{
			// Очистка строк в редакторе
			dgvEditor.Rows.Clear();
			// Вывод данных в редактор
			List<String[]> cellsData = dataPresentation.GetCellsData();
			cellsData.RemoveAt(0);
			foreach (var cellData in cellsData)
			{
				dgvEditor.Rows.Add(cellData);
			}
			// Очистка строк в регистре
			dgvRegisters.Rows.Clear();
			// Вывод данных в регистры
			foreach (var registersCellData in dataPresentation.GetRegistersData())
			{
				dgvRegisters.Rows.Add(registersCellData);
			}
			// Вывод данных в регистровою информацию
			String[] tmpRegisterInfo = dataPresentation.GetRegistersInfoData();
			lbRAValue.Text = tmpRegisterInfo[(int)REGISTER_INFO.RA];
			lbWValue.Text = tmpRegisterInfo[(int)REGISTER_INFO.W];
			lbErrValue.Text = tmpRegisterInfo[(int)REGISTER_INFO.Err];
			// Вывод данных в окно "ВЫВОД"
			lbOutputValue.Text = dataPresentation.output;
			// Вывод данных в окно "ТЕКУЩИЙ ФАЙЛ"
			lbCurrentFileValue.Text = dataPresentation.currentFile;
			// Вывод данных в окно "ДИАГНОСТИКА"
			lbDiagnosticValue.Text = dataPresentation.diagnostic;
			// Вывод данных в окно "ДОПОЛНИТЕЛЬНО"
			lbAdditionalValue.Text = dataPresentation.additional;
		}
		public List<String[]> GetEditorData()
		{
			List<String[]> result = new List<String[]>(DataPresentation.REGISTERS_CELL_QUANTITY);
			foreach (DataGridViewRow row in dgvEditor.Rows)
			{
				result.Add(new String[]
				{
					row.Cells[(int)CELL.ADR].Value.ToString(),
					row.Cells[(int)CELL.COM].Value.ToString(),
					row.Cells[(int)CELL.A1].Value.ToString(),
					row.Cells[(int)CELL.A2].Value.ToString(),
					row.Cells[(int)CELL.A3].Value.ToString(),
				});
			}
			return result;
		}
		private void ExternalDevice_KeyUp_1(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				// Показать команды
				case Keys.F1:
					Commands commandsForm = new Commands();
					commandsForm.Show();
					break;
				// Сохранить
				case Keys.F2:
					model.Execute(COMMAND.SaveCode);
					break;
				// Открыть файл
				case Keys.F3:
					MessageBox.Show("===Открытие файла===");
					model.Execute(COMMAND.OpenFileCode);
					break;
				// Мнемонический режим
				case Keys.F6:
					break;
				// Запустить код
				case Keys.F9:
					MessageBox.Show("===Запуск кода===");
					model.Execute(COMMAND.RunCode);
					break;
				// Выход
				case Keys.F10:
					model.Execute(COMMAND.Exit);
					Close();
					break;
				default:
					break;
			}
	}
		private Model model;
	}
}
