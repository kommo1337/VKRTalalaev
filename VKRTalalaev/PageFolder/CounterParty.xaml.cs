using Microsoft.Win32;
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
    /// Логика взаимодействия для CounterParty.xaml
    /// </summary>
    public partial class CounterParty : Page
    {

        private bool IsFotoLoaded;
        private byte[] FotoImageData;

        public CounterParty()
        {
            InitializeComponent();
            DBEntities.ResetContext();
            LoadDataGridData();
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
            NazvanieCMB.ItemsSource = DBEntities.GetContext().Counterparty.Select(o => o.CounterpartyName).Distinct().ToList();
            DataCMB.ItemsSource = DBEntities.GetContext().Counterparty.Select(o => o.INN).Distinct().ToList();
            CounterpartyCMB.ItemsSource = DBEntities.GetContext().Counterparty.Select(o => o.KPP).Distinct().ToList();
            StatusCMB.ItemsSource = DBEntities.GetContext().Counterparty.Select(o => o.FinancialAccaunt).Distinct().ToList();
        }

        private void LoadDataGridData()
        {
            var operations = DBEntities.GetContext().Counterparty.AsQueryable();

            if (NazvanieCMB.SelectedItem != null)
            {
                string selectedName = NazvanieCMB.SelectedItem.ToString();
                operations = operations.Where(o => o.CounterpartyName == selectedName);
            }

            if (DataCMB.SelectedItem != null)
            {
                int selectedData = (int)DataCMB.SelectedItem;
                operations = operations.Where(o => o.INN == selectedData);
            }

            if (CounterpartyCMB.SelectedItem != null)
            {
                int selectedCounterparty = (int)CounterpartyCMB.SelectedItem;
                operations = operations.Where(o => o.KPP == selectedCounterparty);
            }

            if (StatusCMB.SelectedItem != null)
            {
                int selectedStatus = (int)StatusCMB.SelectedItem;
                operations = operations.Where(o => o.FinancialAccaunt == selectedStatus);
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
            var customers = DBEntities.GetContext().Counterparty.ToList();
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
            }).OrderBy(u => u.CounterpartyName).ToList();

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
            NavigationService.Navigate(new AddCounterparty());
        }

        private void modifyIt_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditCounterparty(ListBox_Resource.SelectedItem as Counterparty));
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditCounterparty(ListBox_Resource.SelectedItem as Counterparty));
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new FullInfoEmployer(ListBox_Resource.SelectedItem as Employer));
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_Resource_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (ListBox_Resource.SelectedItem is null)
                ListBox_Resource_ContextMenu.Visibility = Visibility.Hidden;
            else
                ListBox_Resource_ContextMenu.Visibility = Visibility.Visible;
        }
    }

}

