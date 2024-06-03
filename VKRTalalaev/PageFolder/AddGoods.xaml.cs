using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Логика взаимодействия для AddGoods.xaml
    /// </summary>
    public partial class AddGoods : Page
    {
        public AddGoods()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            DBEntities.ResetContext();
            CounterpartyCb.ItemsSource = DBEntities.GetContext().Counterparty.ToList();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBEntities.GetContext().Goods.Add(new Goods()
                {
                    IdCounterparty = Int32.Parse(CounterpartyCb.SelectedValue.ToString()),
                    Name = NameTb.Text,
                    Articul = ArticulTb.Text,
                    Price = int.Parse(PriceTb.Text)
                   
                });
                DBEntities.GetContext().SaveChanges();
                MBClass.ShowMesagePopup("Успешно", Application.Current.MainWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении операции: {ex.Message}");
            }
        }
    }
}
