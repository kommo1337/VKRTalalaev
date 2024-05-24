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
            OperationsDataGrid.ItemsSource = DBEntities.GetContext().Operation.
        ToList().OrderBy(u => u.IdOperation);
            LoadComboBoxData();
        }
        private void LoadComboBoxData()
        {
            NazvanieCMB.ItemsSource = DBEntities.GetContext().Operation.Select(o => o.NameOperation).Distinct().ToList();
            DataCMB.ItemsSource = DBEntities.GetContext().Operation.Select(o => o.DateOperation).Distinct().ToList();
            CounterpartyCMB.ItemsSource = DBEntities.GetContext().Operation.Select(o => o.IdCounterParty).Distinct().ToList();
            StatusCMB.ItemsSource = DBEntities.GetContext().Operation.Select(o => o.IdStatus).Distinct().ToList();
        }

        private void LoadDataGridData()
        {
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
                int selectedCounterparty = (int)CounterpartyCMB.SelectedItem;
                operations = operations.Where(o => o.IdCounterParty == selectedCounterparty);
            }

            if (StatusCMB.SelectedItem != null)
            {
                int selectedStatus = (int)StatusCMB.SelectedItem;
                operations = operations.Where(o => o.IdStatus == selectedStatus);
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
    }
}
