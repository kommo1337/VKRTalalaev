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
using static MaterialDesignThemes.Wpf.Theme;

namespace VKRTalalaev.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для EmployerPage.xaml
    /// </summary>
    public partial class EmployerPage : Page
    {
        public EmployerPage()
        {
            InitializeComponent();
            LoadComboBoxData();
            LoadPieChartData();
            LoadDataGridData();
            LoadEllipseImage();
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
            try
            {
                DBEntities.ResetContext();
                using (var context = DBEntities.GetContext())
                {
                    var fullNames = context.Employer
                                          .Select(e => e.Surname + " " + e.Name + " " + e.Therdname)
                   .ToList();

                    NazvanieCMB.ItemsSource = fullNames;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            DBEntities.ResetContext();
            DataCMB.ItemsSource = DBEntities.GetContext().Employer.Select(o => o.Phone).Distinct().ToList();
            CounterpartyCMB.ItemsSource = DBEntities.GetContext().Employer.Select(o => o.IdGender).Distinct().ToList();
            StatusCMB.ItemsSource = DBEntities.GetContext().Employer.Select(o => o.Email).Distinct().ToList();
        }

        private void LoadDataGridData()
        {
            DBEntities.ResetContext();
            var operations = DBEntities.GetContext().Employer.AsQueryable();

            if (NazvanieCMB.SelectedItem != null)
            {
                string selectedName = NazvanieCMB.SelectedItem.ToString();
                operations = operations.Where(o => o.Name == selectedName); //TODO посик по ФИО
            }

            if (DataCMB.SelectedItem != null)
            {
                string selectedData = DataCMB.SelectedItem.ToString();
                operations = operations.Where(o => o.Phone == selectedData);
            }

            if (CounterpartyCMB.SelectedItem != null)
            {
                int selectedCounterparty = (int)CounterpartyCMB.SelectedItem;
                operations = operations.Where(o => o.IdGender == selectedCounterparty);
            }

            if (StatusCMB.SelectedItem != null)
            {
                string selectedStatus = StatusCMB.SelectedItem.ToString();
                operations = operations.Where(o => o.Email == selectedStatus);
            }

            ListBox_Resource.ItemsSource = operations.ToList();
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDataGridData();
        }
        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            NazvanieCMB.SelectedItem = null;
            DataCMB.SelectedItem = null;
            CounterpartyCMB.SelectedItem = null;
            StatusCMB.SelectedItem = null;

            LoadDataGridData();
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTb.Text.ToLower();
            var customers = DBEntities.GetContext().Employer.ToList();
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
            }).OrderBy(u => u.Name).ToList();

            ListBox_Resource.ItemsSource = filteredCustomers;

            if (filteredCustomers.Count <= 0)
            {
                MBClass.ShowErrorPopup("Данные не найдены", Application.Current.MainWindow);
            }
        }

        private void CMB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDataGridData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEmployerPage());
        }

        private void modifyIt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditEmployer(ListBox_Resource.SelectedItem as Employer));
        }

        private void FullInfo_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FullInfoEmployer(ListBox_Resource.SelectedItem as Employer));
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

        private void ListBox_Resource_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (ListBox_Resource.SelectedItem is null)
                ListBox_Resource_ContextMenu.Visibility = Visibility.Hidden;
            else
                ListBox_Resource_ContextMenu.Visibility = Visibility.Visible;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditEmployer(ListBox_Resource.SelectedItem as Employer));
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FullInfoEmployer(ListBox_Resource.SelectedItem as Employer));
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

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
    }
}
