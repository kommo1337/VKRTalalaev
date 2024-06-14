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

namespace VKRTalalaev.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для Goodspage.xaml
    /// </summary>
    public partial class Goodspage : Page
    {
        public Goodspage()
        {
            InitializeComponent();
            LoadDataGridData();
            LoadPieChartData();
            LoadComboBoxData();
            LoadEllipseImage();
            DBEntities.ResetContext();
            OperationsDataGrid.ItemsSource = DBEntities.GetContext().Goods.
        ToList().OrderBy(u => u.IdGoods);
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
            NazvanieCMB.ItemsSource = DBEntities.GetContext().Goods.Select(o => o.Name).Distinct().ToList();
            ArticulCMB.ItemsSource = DBEntities.GetContext().Goods.Select(o => o.Articul).Distinct().ToList();
            
                CounterpartyCMB.ItemsSource = DBEntities.GetContext().Operation
                    .Join(DBEntities.GetContext().Counterparty,
                          o => o.IdCounterParty,
                          c => c.IdCounterparty,
                          (o, c) => c.CounterpartyName)
                    .Distinct()
                    .ToList();
            
            PriceCMB.ItemsSource = DBEntities.GetContext().Goods.Select(o => o.Price).Distinct().ToList();
        }

        private void LoadDataGridData()
        {
            DBEntities.ResetContext();
            var operations = DBEntities.GetContext().Goods.AsQueryable();

            if (NazvanieCMB.SelectedItem != null)
            {
                string selectedName = NazvanieCMB.SelectedItem.ToString();
                operations = operations.Where(o => o.Name == selectedName);
            }

            if (ArticulCMB.SelectedItem != null)
            {
                string selectedData = ArticulCMB.SelectedItem.ToString();
                operations = operations.Where(o => o.Articul == selectedData);
            }

            if (CounterpartyCMB.SelectedItem != null)
            {
                string selectedCounterpartyName = (string)CounterpartyCMB.SelectedItem;
                
                    int selectedCounterpartyId = DBEntities.GetContext().Counterparty
                        .Where(c => c.CounterpartyName == selectedCounterpartyName)
                        .Select(c => c.IdCounterparty)
                        .FirstOrDefault();

                    operations = operations.Where(o => o.IdCounterparty == selectedCounterpartyId);
                
            }

            if (PriceCMB.SelectedItem != null)
            {
                int selectedStatus = (int)PriceCMB.SelectedItem;
                operations = operations.Where(o => o.Price == selectedStatus);
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
            CounterpartyCMB.SelectedItem = null;
            ArticulCMB.SelectedItem = null;

            LoadDataGridData();
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTb.Text.ToLower();
            var customers = DBEntities.GetContext().Goods.ToList();
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

            OperationsDataGrid.ItemsSource = filteredCustomers;

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
            NavigationService.Navigate(new AddGoods());
        }

        private void modifyIt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditGoods(OperationsDataGrid.SelectedItem as Goods));
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

