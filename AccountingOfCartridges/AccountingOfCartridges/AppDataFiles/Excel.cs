using Microsoft.Office.Interop.Excel;
using System;
using System.Linq;
using System.Windows;

namespace AccountingOfCartridges.AppDataFiles
{
    internal class Excel
    {
        /// <summary>
        /// Формирует и открывает Excel файл
        /// </summary>
        public static void GetExcelFile()
        {
            try
            {
                var device = ConnectOdb.conObj.Device.Where(x => x.StatusId == 4).ToList();

                System.Windows.Forms.DataGridView dataGridView = new System.Windows.Forms.DataGridView();

                dataGridView.Columns.Add("column-1", "Header column - 1");
                dataGridView.Columns.Add("column-2", "Header column - 1");
                dataGridView.Columns.Add("column-3", "Header column - 1");
                dataGridView.Columns.Add("column-4", "Header column - 1");
                dataGridView.Columns.Add("column-5", "Header column - 1");
                dataGridView.Columns.Add("column-6", "Header column - 1");
                dataGridView.Columns.Add("column-7", "Header column - 1");
                dataGridView.Columns.Add("column-8", "Header column - 1");

                dataGridView.Rows.Add("id", "Название", "Тип устройства", "Количество", "Кабинет", "Дата принятия", "Серийный номер", "ФИО преподавателя");

                for (int i = 0; i < device.Count; i++)
                {
                    dataGridView.Rows.Add(device[i].id, device[i].Title, device[i].StringType, device[i].Count,
                        device[i].Сabinet, device[i].DateAcceptance.ToString(), device[i].SerialNumber, device[i].FIO);
                }

                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Workbook ExcelWorkBook;
                Worksheet ExcelWorkSheet;

                ExcelWorkBook = ExcelApp.Workbooks.Add(System.Reflection.Missing.Value);

                ExcelWorkSheet = (Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView.ColumnCount; j++)
                    {
                        ExcelApp.Cells[i + 1, j + 1] = dataGridView.Rows[i].Cells[j].Value;
                    }
                }

            ((Range)ExcelWorkSheet.get_Range("A1", $"H{device.Count + 1}")).Cells.Borders.LineStyle = XlLineStyle.xlContinuous;

                ExcelApp.Visible = true;
                ExcelApp.UserControl = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Отсутствует Excel");
            }
        }
    }
}
