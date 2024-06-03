using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
    /// Логика взаимодействия для EditOperationPage.xaml
    /// </summary>
    public partial class EditOperationPage : Page
    {
        Operation user = new Operation();
        public EditOperationPage(Operation user)
        {
            InitializeComponent();
            DataContext = user;

            this.user.IdOperation = user.IdOperation;

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
                int index = TypeOperationCB.SelectedIndex + 1;
                int index2 = CounterpartyCb.SelectedIndex + 1;
                int index3 = StatusCb.SelectedIndex + 1;
                int index4 = ProjectCb.SelectedIndex + 1;
                int index5 = TypeCb.SelectedIndex + 1;
                int index6 = GoodsCb.SelectedIndex + 1;
                user = DBEntities.GetContext().Operation
                    .FirstOrDefault(u => u.IdOperation == user.IdOperation);
                user.IdTypeOfOperation = index;
                user.IdCounterParty = index2;
                user.IdStatus = index3;
                user.IdProject = index4;
                user.IdType = index5;
                user.IdGoods = index6;
                user.NameOperation = NameOperationTb.Text;
                user.DateOperation = (DateTime)BTHDatePick.SelectedDate;
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
