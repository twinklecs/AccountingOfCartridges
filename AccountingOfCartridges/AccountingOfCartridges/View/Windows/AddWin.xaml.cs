using AccountingOfCartridges.AppDataFiles;
using System;
using System.Linq;
using System.Windows;

namespace AccountingOfCartridges.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddWin.xaml
    /// </summary>
    public partial class AddWin : Window
    {
        public AddWin()
        {
            InitializeComponent();

            dateTB.Text = DateTime.Now.ToString();
            typeCB.ItemsSource = ConnectOdb.conObj.Type.ToList();
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (titleTB.Text != "" && countTB.Text != "" && cabinetTB.Text != ""
                && dateTB.Text != "" && SerialNumberTB.Text != "" && fioTB.Text != "")
            {
                var type = typeCB.SelectedItem as AppDataFiles.Type;

                int typeId = 0;
                typeId = typeCB.SelectedIndex;
                typeId += 1;

                Device device = new Device();

                device.Title = titleTB.Text;
                if (int.TryParse(countTB.Text, out var parsedCount))
                {
                    device.Count = parsedCount;
                }
                else { 
                    MessageBox.Show("В строке \"Количество\" имеются буквы!"); 
                    return;
                }
                device.Сabinet = cabinetTB.Text;
                if (DateTime.TryParse(dateTB.Text, out var parsedDate))
                {
                    device.DateAcceptance = parsedDate;
                }
                else
                {
                    MessageBox.Show("В строке \"Дата принятия\" имеются буквы!");
                    return;
                }
                device.SerialNumber = SerialNumberTB.Text;
                if(typeId != 0)
                    device.TypeId = typeId;
                else
                    device.TypeId = 1;
                device.StatusId = 1;
                device.FIO = fioTB.Text;


                ConnectOdb.conObj.Device.Add(device);
                ConnectOdb.conObj.SaveChanges();

                MessageBox.Show("Сохранено!");
                Close();
            }
            else
                MessageBox.Show("Пустые строки!");
        }

        private void clearBtn_Click(object sender, RoutedEventArgs e)
        {
            titleTB.Text = "";
            countTB.Text = "1";
            cabinetTB.Text = "";
            dateTB.Text = DateTime.Now.ToString();
            SerialNumberTB.Text = "";
            fioTB.Text = "";
        }
    }
}
