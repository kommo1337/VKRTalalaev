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
using Type = VKRTalalaev.DataFolder.Type;

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
                MBClass.ShowMesagePopup("Успешно", Application.Current.MainWindow);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при добавлении операции: {ex.Message}");
            }
        }

        //private void AddStatusButton_Click(object sender, RoutedEventArgs e)
        //{
        //    string newStatus = StatusCb.Text;

        //    if (!string.IsNullOrEmpty(newStatus))
        //    {
        //        using (var context = new DBEntities())
        //        {

        //            if (!context.Status.Any(s => s.StatusName == newStatus))
        //            {

        //                var newStatusEntry = new Status { StatusName = newStatus };
        //                context.Status.Add(newStatusEntry);
        //                context.SaveChanges();


        //                StatusCb.ItemsSource = context.Status.ToList();
        //                StatusCb.Text = string.Empty; 
        //                MessageBox.Show("Status added successfully!");
        //            }
        //            else
        //            {
        //                MessageBox.Show("This status already exists.");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Please enter a status.");
        //    }
        //}

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string comboBoxType = button.Tag.ToString();
            string newValue = string.Empty;

            switch (comboBoxType)
            {
                case "Status":
                    newValue = StatusCb.Text;
                    AddNewItemToDatabase(newValue, "Status");
                    break;
                case "Project":
                    newValue = ProjectCb.Text;
                    AddNewItemToDatabase(newValue, "Project");
                    break;
                case "Type":
                    newValue = TypeCb.Text;
                    AddNewItemToDatabase(newValue, "Type");
                    break;
            }
        }

        private void AddNewItemToDatabase(string newValue, string itemType)
        {
            if (!string.IsNullOrEmpty(newValue))
            {
                using (var context = new DBEntities())
                {
                    switch (itemType)
                    {
                        case "Status":
                            if (!context.Status.Any(s => s.StatusName == newValue))
                            {
                                var newStatusEntry = new Status { StatusName = newValue };
                                context.Status.Add(newStatusEntry);
                                context.SaveChanges();
                                StatusCb.ItemsSource = context.Status.ToList();
                                StatusCb.Text = string.Empty;
                                MessageBox.Show("Успешно добавленно");
                            }
                            else
                            {
                                MessageBox.Show("Такой статус уже существует");
                            }
                            break;

                        case "Project":
                            if (!context.Project.Any(p => p.ProjectName == newValue))
                            {
                                var newProjectEntry = new Project { ProjectName = newValue };
                                context.Project.Add(newProjectEntry);
                                context.SaveChanges();
                                ProjectCb.ItemsSource = context.Project.ToList();
                                ProjectCb.Text = string.Empty;
                                MessageBox.Show("Успешно добавленно!");
                            }
                            else
                            {
                                MessageBox.Show("Такой проект уже существует.");
                            }
                            break;

                        case "Type":
                            if (!context.Type.Any(t => t.Name == newValue))
                            {
                                var newTypeEntry = new Type { Name = newValue };
                                context.Type.Add(newTypeEntry);
                                context.SaveChanges();
                                TypeCb.ItemsSource = context.Type.ToList();
                                TypeCb.Text = string.Empty;
                                MessageBox.Show("Успешно добавленно!");
                            }
                            else
                            {
                                MessageBox.Show("Такой тип уже существует.");
                            }
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show($"Пожалуйста введите {itemType.ToLower()}.");
            }
        }
    }

}

