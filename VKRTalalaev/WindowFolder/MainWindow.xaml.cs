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
using System.Windows.Shapes;
using VKRTalalaev.PageFolder;
using static VKRTalalaev.ClassFolder.MBClass;

namespace VKRTalalaev.WindowFolder
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new OperationPage());
            this.Closing += MainWindow_Closing;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new OperationPage());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CounterParty());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new EmployerPage());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Goodspage());
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Используем общий метод для обработки закрытия
            HandleClosing(e);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // Создаем фиктивное событие CancelEventArgs для передачи в HandleClosing
            var cancelEventArgs = new System.ComponentModel.CancelEventArgs();
            HandleClosing(cancelEventArgs);
        }

        private void HandleClosing(System.ComponentModel.CancelEventArgs e) //TODO
        {
            
            var result = LogoutOrCloseMessageBox.Show();

            switch (result)
            {
                case MessageBoxResult.Yes:
                    
                    Logout();
                    e.Cancel = true;
                    break;
                case MessageBoxResult.No:
                    
                    Application.Current.Shutdown();
                    break;
                case MessageBoxResult.Cancel:
                    
                    e.Cancel = true;
                    break;
            }
        }

        private void Logout()
        {
        
            this.Hide();
           
            var authWindow = new AutorisationWindow();
            authWindow.Show();
            
            this.Close();
        }
    }

    public static class LogoutOrCloseMessageBox
    {
        public static MessageBoxResult Show()
        {
            return MessageBox.Show(
                "Выберите действие:\n\nYes - Выйти из аккаунта\nNo - Закрыть приложение\nCancel - Отмена",
                "Выйти из аккаунта или закрыть приложение",
                MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);
        }
    }
}
