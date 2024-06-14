using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
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
using VKRTalalaev.ClassFolder;
using VKRTalalaev.DataFolder;
using System.Globalization;
using Type = System.Type;

namespace VKRTalalaev.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для UserList.xaml
    /// </summary>
    public partial class UserList : Page
    {
        public UserList()
        {
            InitializeComponent();
            LoadDataGridData();
            LoadPieChartData();
            LoadComboBoxData();
            LoadEllipseImage();
            DBEntities.ResetContext();
            OperationsDataGrid.ItemsSource = DBEntities.GetContext().User.
        ToList().OrderBy(u => u.IdUser);
            DBEntities.ResetContext();
            using (var context = DBEntities.GetContext())
            {
                var employer = context.Employer
                                      .Where(e => e.IdUser == VariableClass.CurentUser)
                                      .Select(e => new { e.Name, e.Surname })
                                      .FirstOrDefault();

                if (employer != null)
                {
                    NameUserTb.Text = $"{employer.Name} {employer.Surname}";
                }
                else
                {
                    NameUserTb.Text = "User not found";
                }
            }
        }

        private void LoadComboBoxData()
        {
            DBEntities.ResetContext();
            NazvanieCMB.ItemsSource = DBEntities.GetContext().User.Select(o => o.Login).Distinct().ToList();
            ArticulCMB.ItemsSource = DBEntities.GetContext().Role.Select(o => o.RoleName).Distinct().ToList();
            PriceCMB.ItemsSource = DBEntities.GetContext().User.Select(o => o.IsBanned).Distinct().ToList();
        }

        private void LoadDataGridData()
        {
            DBEntities.ResetContext();
            var operations = DBEntities.GetContext().User.AsQueryable();

            if (NazvanieCMB.SelectedItem != null)
            {
                string selectedName = NazvanieCMB.SelectedItem.ToString();
                operations = operations.Where(o => o.Login == selectedName);
            }

            if (ArticulCMB.SelectedItem != null)
            {
                string selectedCounterpartyName = (string)ArticulCMB.SelectedItem;
                int selectedCounterpartyId = 0;
                var status = (from c in DBEntities.GetContext().Role
                              where c.RoleName == selectedCounterpartyName
                              select c.IdRole).FirstOrDefault();

                if (status != 0)
                {
                    selectedCounterpartyId = status;
                }

                operations = operations.Where(o => o.IdRole == selectedCounterpartyId);
            }

            
            if (PriceCMB.SelectedItem != null)
            {
                bool selectedStatus = (bool)PriceCMB.SelectedItem;
                operations = operations.Where(o => o.IsBanned == selectedStatus);
            }

            OperationsDataGrid.ItemsSource = operations.ToList();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDataGridData();
        }
        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            NazvanieCMB.SelectedItem = null;
            PriceCMB.SelectedItem = null;
            
            ArticulCMB.SelectedItem = null;

            LoadDataGridData();
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTb.Text.ToLower();
            var customers = DBEntities.GetContext().User.ToList();
            var filteredCustomers = customers.Where(customer =>
            {
                foreach (PropertyInfo property in customer.GetType().GetProperties())
                {
                    var value = property.GetValue(customer)?.ToString().ToLower();
                    if (!string.IsNullOrEmpty(value) && value.Contains(searchText))
                    {
                        return true;
                    }
                }
                return false;
            }).OrderBy(u => u.Login).ToList();

            OperationsDataGrid.ItemsSource = filteredCustomers;

            if (filteredCustomers.Count <= 0)
            {
                MBClass.ShowErrorPopup("Данные не найдены", Application.Current.MainWindow);
            }
        }

        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isBanned)
            {
                return isBanned ? "Заблокирован" : "активен";
            }
            return "Неизвестно";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private void CMB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDataGridData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddUserWindow());
        }

        private void modifyIt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditUser(OperationsDataGrid.SelectedItem as User));
        }

        private void FullInfo_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LoadEllipseImage()
        {
            using (var context = DBEntities.GetContext())
            {
                var employer = context.Employer
                                      .Where(e => e.IdUser == VariableClass.CurentUser)
                                      .Select(e => e.Photo)
                                      .FirstOrDefault();

                if (employer != null)
                {
                    ImageBrush imageBrush = new ImageBrush();
                    imageBrush.ImageSource = LoadImage(employer);
                    MyElipse.Fill = imageBrush;
                }
                else
                {
                    MyElipse.Fill = new SolidColorBrush(Colors.Gray);
                }
            }
        }

        private BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;

            BitmapImage image = new BitmapImage();
            using (MemoryStream mem = new MemoryStream(imageData))
            {
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }


        private void LoadPieChartData()
        {
            using (var context = DBEntities.GetContext())
            {

                var totalCounterparties = context.Counterparty.Count();
                var totalEmployers = context.Employer.Count();
                var totalGoods = context.Goods.Count();
                var totalOperations = context.Operation.Count();
                var totalUsers = context.User.Count();

                SeriesCollection seriesCollection = new SeriesCollection
                {
                    new PieSeries { Title = "Контрагенты", Values = new ChartValues<int> { totalCounterparties } },
                    new PieSeries { Title = "Сотрудники", Values = new ChartValues<int> { totalEmployers } },
                    new PieSeries { Title = "Товары", Values = new ChartValues<int> { totalGoods } },
                    new PieSeries { Title = "Операции", Values = new ChartValues<int> { totalOperations } },
                    new PieSeries { Title = "Пользователи", Values = new ChartValues<int> { totalUsers } },
                };

                pieChart.Series = seriesCollection;
            }
        }

        private void DeleteIt_Click(object sender, RoutedEventArgs e)
        {
            if (OperationsDataGrid.SelectedItem == null)
                return;

            Goods selectedResource = OperationsDataGrid.SelectedItem as Goods;
            if (selectedResource == null)
                return;

            if (MBClass.QuestionMB("Удалить товар " + $"{selectedResource.Name}?"))
            {
                DBEntities.ResetContext();
                var context = DBEntities.GetContext();


                var resource = DBEntities.GetContext().Employer.Find(selectedResource.IdGoods);

                if (resource != null)
                {

                    DBEntities.GetContext().Employer.Remove(resource);


                    DBEntities.GetContext().SaveChanges();


                    MBClass.InfoMB("Товар удалён");
                    OperationsDataGrid.ItemsSource = DBEntities.GetContext().Goods.ToList().OrderBy(u => u.IdGoods);
                }
                else
                {
                    MBClass.ErrorMB("Товар не найден в базе данных.");
                }
            }
        }
    }
}
