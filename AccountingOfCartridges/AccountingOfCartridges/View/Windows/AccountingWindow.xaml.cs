using AccountingOfCartridges.AppDataFiles;
using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace AccountingOfCartridges.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для AccountingWindow.xaml
    /// </summary>
    public partial class AccountingWindow : System.Windows.Window
    {
        public AccountingWindow()
        {
            InitializeComponent();
            UpdateData();

            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval += TimeSpan.FromSeconds(3);
            dispatcherTimer.Tick += UpdateData;
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Обновление информации
        /// </summary>
        public void UpdateData()
        {
            var db = ConnectOdb.conObj;

            WaitLV.ItemsSource = db.Device.Where(x => x.StatusId == 1).ToList();
            AtWorkLV.ItemsSource = db.Device.Where(x => x.StatusId == 2).ToList();
            SuccessfulLV.ItemsSource = db.Device.Where(x => x.StatusId == 3).ToList();

            AppDataFiles.Type type = new AppDataFiles.Type { Title = "Все устройства" };
            var typeList = db.Type.ToList();
            typeList.Insert(0, type);
            typeFilterCB.ItemsSource = typeList;

            typeFilterCB.SelectedIndex = 0;
        }

        /// <summary>
        /// Обновление информации по таймеру
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UpdateData(object sender, object e)
        {
            var db = ConnectOdb.conObj;

            if (typeRB.IsChecked == true && typeFilterCB.SelectedIndex == 0)
            {
                WaitLV.ItemsSource = db.Device.Where(x => x.StatusId == 1).ToList();
                AtWorkLV.ItemsSource = db.Device.Where(x => x.StatusId == 2).ToList();
                SuccessfulLV.ItemsSource = db.Device.Where(x => x.StatusId == 3).ToList();
            }
        }

        private void btnAddRecord_Click(object sender, RoutedEventArgs e)
        {
            AddWin addWin = new AddWin();
            addWin.ShowDialog();

            UpdateData();
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClearInformation();

            var IsSelecteditem = sender as ListViewItem;
            if (IsSelecteditem != null && IsSelecteditem.IsSelected)
            {
                var item = WaitLV.SelectedItem as Device;
                idTB.Text = item.id.ToString();
                titleTB.Text = item.Title;
                typeCB.ItemsSource = ConnectOdb.conObj.Type.ToList();
                typeCB.SelectedIndex = item.TypeId - 1;
                countTB.Text = item.Count.ToString();
                cabinetTB.Text = item.Сabinet;
                dateTB.Text = item.DateAcceptance.ToString();
                SerialNumberTB.Text = item.SerialNumber;
                fioTB.Text = item.FIO;
            }
        }

        private void InWorkBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = WaitLV.SelectedItem as Device;

            if (item != null)
            {
                item.StatusId = 2;

                ConnectOdb.conObj.Entry(item).State = System.Data.Entity.EntityState.Modified;
                ConnectOdb.conObj.SaveChanges();

                UpdateData();
            }
            else
                MessageBox.Show("Не выбрано устройство\nиз списка \"В ожидании\"!");

            ClearInformation();
        }

        private void SuccessfulBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = AtWorkLV.SelectedItem as Device;

            if (item != null)
            {
                item.StatusId = 3;

                ConnectOdb.conObj.Entry(item).State = System.Data.Entity.EntityState.Modified;
                ConnectOdb.conObj.SaveChanges();

                UpdateData();
            }
            else
                MessageBox.Show("Не выбрано устройство\nиз списка \"В работе\"!");

            ClearInformation();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = AtWorkLV.SelectedItem as Device;
            if (item != null)
            {
                item.StatusId = 1;

                ConnectOdb.conObj.Entry(item).State = System.Data.Entity.EntityState.Modified;
                ConnectOdb.conObj.SaveChanges();

                UpdateData();
            }
            else
                MessageBox.Show("Не выбрано устройство\nиз списка \"В работе\"!");

            ClearInformation();
        }

        private void IssuedBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = SuccessfulLV.SelectedItem as Device;
            if (item != null)
            {
                item.StatusId = 4;

                ConnectOdb.conObj.Entry(item).State = System.Data.Entity.EntityState.Modified;
                ConnectOdb.conObj.SaveChanges();

                UpdateData();
            }
            else
                MessageBox.Show("Не выбрано устройство\nиз списка \"На выдачу\"!");

            ClearInformation();
        }

        /// <summary>
        /// Очистка информации по девайсу
        /// </summary>
        public void ClearInformation()
        {
            idTB.Text = "";
            titleTB.Text = "";
            countTB.Text = "";
            cabinetTB.Text = "";
            typeCB.Text = "";
            dateTB.Text = "";
            SerialNumberTB.Text = "";
            fioTB.Text = "";
        }

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            var item = WaitLV.SelectedItem as Device;

            if (item != null)
            {
                if (MessageBox.Show($"Вы уверены, что хотите удалить устройсво \"{item.Title}\" ?", "Подтверждение удаления", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    ConnectOdb.conObj.Device.Remove(item);
                    ConnectOdb.conObj.SaveChanges();

                    MessageBox.Show("Удалено!");

                    UpdateData();
                }
            }
            else
                MessageBox.Show("Не выбрано устройство\nиз списка \"В ожидании\"!");
        }

        private void ChangeInformationBtn_Click(object sender, RoutedEventArgs e)
        {
            if (titleTB.Text != "" && countTB.Text != "" && cabinetTB.Text != ""
                && dateTB.Text != "" && fioTB.Text != "")
            {
                var item = WaitLV.SelectedItem as Device;

                if (MessageBox.Show($"Вы уверены, что хотите изменить устройсво \"{item.Title}\" ?", "Подтверждение изменений", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    item.Title = titleTB.Text;
                    if (int.TryParse(countTB.Text, out var parsedCount))
                    {
                        item.Count = parsedCount;
                    }
                    else
                    {
                        MessageBox.Show("В строке \"Количество\" имеются буквы!");
                        return;
                    }
                    item.Сabinet = cabinetTB.Text;
                    if (DateTime.TryParse(dateTB.Text, out var parsedDate))
                    {
                        item.DateAcceptance = parsedDate;
                    }
                    else
                    {
                        MessageBox.Show("В строке \"Дата принятия\" имеются буквы!");
                        return;
                    }
                    item.SerialNumber = SerialNumberTB.Text;
                    item.TypeId = typeCB.SelectedIndex;
                    item.TypeId += 1;
                    item.FIO = fioTB.Text;

                    ConnectOdb.conObj.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    ConnectOdb.conObj.SaveChanges();

                    MessageBox.Show("Изменено!");

                    ClearInformation();

                    UpdateData();
                }
            }
            else
                MessageBox.Show("Пустые строки!");
        }

        private void SaveHistory_Click(object sender, RoutedEventArgs e)
        {
            Excel.GetExcelFile();
        }

        private void ShowInfo_Click(object sender, RoutedEventArgs e)
        {
            var item = SuccessfulLV.SelectedItem as Device;

            if (item != null)
            {
                MessageBox.Show($"Название: {item.Title}\n" +
                    $"Тип устройства: {item.StringType}\n" +
                    $"Количество: {item.Count}\n" +
                    $"Кабинет: {item.Сabinet}\n" +
                    $"Дата принятия {item.DateAcceptance}\n" +
                    $"Серийный номер: {item.SerialNumber}\n" +
                    $"ФИО: {item.FIO}");
            }
        }

        private void ClearFilterBtn_Click(object sender, RoutedEventArgs e)
        {
            cabinetFilterTB.Text = "";
            fioFilterTB.Text = "";
            typeFilterCB.SelectedIndex = 0;
            typeRB.IsChecked = true;

            UpdateData();
        }

        private void SelectionByConditionsBtn_Click(object sender, RoutedEventArgs e)
        {
            var db = ConnectOdb.conObj;

            if (typeRB.IsChecked == true)
            {
                if (typeFilterCB.SelectedIndex != 0)
                {
                    WaitLV.ItemsSource = db.Device.Where(x => x.StatusId == 1 && x.TypeId == typeFilterCB.SelectedIndex ).ToList();
                    AtWorkLV.ItemsSource = db.Device.Where(x => x.StatusId == 2 && x.TypeId == typeFilterCB.SelectedIndex ).ToList();
                    SuccessfulLV.ItemsSource = db.Device.Where(x => x.StatusId == 3 && x.TypeId == typeFilterCB.SelectedIndex).ToList();
                }
                else
                {
                    WaitLV.ItemsSource = db.Device.Where(x => x.StatusId == 1).ToList();
                    AtWorkLV.ItemsSource = db.Device.Where(x => x.StatusId == 2).ToList();
                    SuccessfulLV.ItemsSource = db.Device.Where(x => x.StatusId == 3).ToList();
                }
            }

            if (cabinetRB.IsChecked == true)
            {
                WaitLV.ItemsSource = db.Device.Where(x => x.StatusId == 1 && x.Сabinet.Contains(cabinetFilterTB.Text)).ToList();
                AtWorkLV.ItemsSource = db.Device.Where(x => x.StatusId == 2 && x.Сabinet.Contains(cabinetFilterTB.Text)).ToList();
                SuccessfulLV.ItemsSource = db.Device.Where(x => x.StatusId == 3 && x.Сabinet.Contains(cabinetFilterTB.Text)).ToList();
            }

            if (fioRB.IsChecked == true)
            {
                WaitLV.ItemsSource = db.Device.Where(x => x.StatusId == 1 && x.FIO.Contains(fioFilterTB.Text)).ToList();
                AtWorkLV.ItemsSource = db.Device.Where(x => x.StatusId == 2 && x.FIO.Contains(fioFilterTB.Text)).ToList();
                SuccessfulLV.ItemsSource = db.Device.Where(x => x.StatusId == 3 && x.FIO.Contains(fioFilterTB.Text)).ToList();
            }
        }

        private void OpenInstructions_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process txt = new System.Diagnostics.Process();
            txt.StartInfo.FileName = "notepad.exe";
            txt.StartInfo.Arguments = "Readme.txt";
            txt.Start();
        }
    }
}
