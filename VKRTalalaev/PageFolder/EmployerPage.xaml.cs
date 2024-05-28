using System;
using System.Collections.Generic;
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
            LoadDataGridData();
        }

        private  void LoadComboBoxData()
        {
            try
            {
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
            NavigationService.Navigate(new AddEmployerPage());
        }

        private void modifyIt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditEmployer(OperationsDataGrid.SelectedItem as Employer));
        }

        private void FullInfo_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FullInfoEmployer(OperationsDataGrid.SelectedItem as Employer));
        }
    }
}
