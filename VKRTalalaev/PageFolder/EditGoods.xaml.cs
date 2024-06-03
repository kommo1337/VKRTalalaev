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
    /// Логика взаимодействия для EditGoods.xaml
    /// </summary>
    public partial class EditGoods : Page
    {
        Goods goods;
        public EditGoods(Goods goods)
        {
            InitializeComponent();
            DataContext = goods;
            this.goods = goods;
            DBEntities.ResetContext();
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
                int index = CounterpartyCb.SelectedIndex + 1;
              
                goods = DBEntities.GetContext().Goods
                    .FirstOrDefault(u => u.IdGoods == goods.IdGoods);
                goods.IdCounterparty = index;

                goods.Name = NameTb.Text;
                goods.Price =int.Parse( PriceTb.Text);
                goods.Articul = ArticulTb.Text;
                
                DBEntities.GetContext().SaveChanges();
                MBClass.ShowMesagePopup("Успешно", Application.Current.MainWindow);

            }
            catch (Exception ex)
            {
                MBClass.ShowErrorPopup(ex.Message, Application.Current.MainWindow);
            }
        }
    }
}
