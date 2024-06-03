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
using VKRTalalaev.DataFolder;

namespace VKRTalalaev.PageFolder
{
    /// <summary>
    /// Логика взаимодействия для AddOperationPage.xaml
    /// </summary>
    public partial class AddOperationPage : Page
    {
        public AddOperationPage()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        private void LoadComboBoxData()
        {
            DBEntities.ResetContext();
            TypeOperationCB.ItemsSource = DBEntities.GetContext().TypeOfOperation.ToList();
            CounterpartyCb.ItemsSource = DBEntities.GetContext().Counterparty.ToList();
            StatusCb.ItemsSource = DBEntities.GetContext().Status.ToList();
            ProjectCb.ItemsSource = DBEntities.GetContext().Project.ToList();
            TypeCb.ItemsSource = DBEntities.GetContext().Type.ToList();
            GoodsCb.ItemsSource = DBEntities.GetContext().Goods.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DBEntities.GetContext().Operation.Add(new Operation()
                {
                    IdTypeOfOperation = Int32.Parse(TypeOperationCB.SelectedValue.ToString()),
                    NameOperation = NameOpeonTb.Text,
                    DateOperation = DateTime.Parse(BTHDatePick.Text),
                    IdCounterParty = Int32.Parse(CounterpartyCb.SelectedValue.ToString()),
                    IdStatus = Int32.Parse(StatusCb.SelectedValue.ToString()),
                    IdProject = Int32.Parse(ProjectCb.SelectedValue.ToString()),
                    IdType = Int32.Parse(TypeCb.SelectedValue.ToString()),
                    IdGoods = Int32.Parse(GoodsCb.SelectedValue.ToString())
                });
                DBEntities.GetContext().SaveChanges();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении операции: {ex.Message}");
            }
        }
    }
}
