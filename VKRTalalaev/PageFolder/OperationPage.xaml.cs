﻿using LiveCharts.Wpf;
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
using System.Runtime.Remoting.Contexts;

namespace VKRTalalaev.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для OperationPage.xaml
    /// </summary>
    public partial class OperationPage : Page
    {
        public OperationPage()
        {


            InitializeComponent();
            LoadPieChartData();
            DBEntities.ResetContext();
            OperationsDataGrid.ItemsSource = DBEntities.GetContext().Operation.
        ToList().OrderBy(u => u.IdOperation);
            DBEntities.ResetContext();
            LoadComboBoxData();
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
            NazvanieCMB.ItemsSource = DBEntities.GetContext().Operation.Select(o => o.NameOperation).Distinct().ToList();
            DataCMB.ItemsSource = DBEntities.GetContext().Operation.Select(o => o.DateOperation).Distinct().ToList();

            CounterpartyCMB.ItemsSource = DBEntities.GetContext().Counterparty.Select(o => o.CounterpartyName).ToList();
            using (var context = DBEntities.GetContext())
            {
                StatusCMB.ItemsSource = context.Status
                    .Join(context.Status,
                          o => o.IdStatus,
                          c => c.IdStatus,
                          (o, c) => c.StatusName)
                    .Distinct()
                    .ToList();
            }
            //StatusCMB.ItemsSource = DBEntities.GetContext().Operation.Select(o => o.IdStatus).Distinct().ToList();
        }

        private void LoadDataGridData()
        {
            DBEntities.ResetContext();
            var operations = DBEntities.GetContext().Operation.AsQueryable();

            if (NazvanieCMB.SelectedItem != null)
            {

                string selectedName = NazvanieCMB.SelectedItem.ToString();
                operations = operations.Where(o => o.NameOperation == selectedName);
            }

            if (DataCMB.SelectedItem != null)
            {
                DateTime selectedData = Convert.ToDateTime(DataCMB.SelectedItem);
                operations = operations.Where(o => o.DateOperation == selectedData);
            }

            if (CounterpartyCMB.SelectedItem != null)
            {
                //int selectedCounterpartyName = Convert.ToInt32(CounterpartyCMB.SelectedValue);
                //operations = operations.Where(o => o.IdCounterParty == selectedCounterpartyName);


                string selectedCounterpartyName = (string)CounterpartyCMB.SelectedItem;
                int selectedCounterpartyId = 0;
                var status = (from c in DBEntities.GetContext().Counterparty
                              where c.CounterpartyName == selectedCounterpartyName
                              select c.IdCounterparty).FirstOrDefault();

                if (status != 0)
                {
                    selectedCounterpartyId = status;
                }

                operations = operations.Where(o => o.IdCounterParty == selectedCounterpartyId);
            }

            if (StatusCMB.SelectedItem != null)
            {
                string selectedCounterpartyName = (string)StatusCMB.SelectedItem;
                int selectedCounterpartyId = 0;
                var status = (from c in DBEntities.GetContext().Status
                              where c.StatusName == selectedCounterpartyName
                              select c.IdStatus).FirstOrDefault();

                if (status != 0)
                {
                    selectedCounterpartyId = status;
                }

                operations = operations.Where(o => o.IdStatus == selectedCounterpartyId);
                
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
            DataCMB.SelectedItem = null;
            CounterpartyCMB.SelectedItem = null;
            StatusCMB.SelectedItem = null;

            LoadDataGridData();
        }

        private void SearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchTb.Text.ToLower();
            var customers = DBEntities.GetContext().Operation.ToList();
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
            }).OrderBy(u => u.NameOperation).ToList();

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
            NavigationService.Navigate(new AddOperationPage());
        }

        private void modifyIt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditOperationPage(OperationsDataGrid.SelectedItem as Operation));
        }


        private void LoadEllipseImage()
        {
            
                DBEntities.ResetContext();
                var employer = DBEntities.GetContext().Employer
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
            DBEntities.ResetContext();
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

        private void FullInfo_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FullInfoOperationPage(OperationsDataGrid.SelectedItem as Operation));
        }

        private void DeleteIt_Click(object sender, RoutedEventArgs e)
        {
            if (OperationsDataGrid.SelectedItem == null)
                return;
            
            Operation selectedResource = OperationsDataGrid.SelectedItem as Operation;
            if (selectedResource == null)
                return;

            if (MBClass.QuestionMB("Удалить операцию " + $"{selectedResource.NameOperation}?"))
            {
                DBEntities.ResetContext();
                var context = DBEntities.GetContext();

                var resource = context.Operation.Find(selectedResource.IdOperation); 

                if (resource != null)
                {
                    context.Operation.Remove(resource);

                    context.SaveChanges();

                    MBClass.InfoMB("Операция удалена");
                    OperationsDataGrid.ItemsSource = context.Operation.ToList().OrderBy(u => u.NameOperation);
                }
                else
                {
                    MBClass.ErrorMB("Операция не найдена в базе данных.");
                }
            }

        }
    }
}
