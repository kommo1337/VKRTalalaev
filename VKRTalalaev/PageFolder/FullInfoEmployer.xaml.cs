using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
    /// Логика взаимодействия для FullInfoEmployer.xaml
    /// </summary>
    public partial class FullInfoEmployer : Page
    {
        private byte[] FotoImageData;
        private byte[] FotoImageData2;
        Employer Emp;

        public FullInfoEmployer(Employer Emp)
        {
            InitializeComponent();
            DataContext = Emp;
            this.Emp = Emp;
            LoadCounterpartyPhoto(Emp.IdEmployer);
            LoadCounterpartyPhotoPasport(Emp.IdEmployer);
            DisplayAddress(Emp.IdEmployer);

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DBEntities.ResetContext();
            NavigationService.Navigate(new EmployerPage());
            DBEntities.ResetContext();
        }

        private void PhotoPasportBtn_Click(object sender, RoutedEventArgs e)
        {
            PhotoPasport.Visibility = Visibility.Visible;
            
        }

        private void LoadCounterpartyPhoto(int counterpartyId)
        {
            DBEntities.ResetContext();
            using (var context = DBEntities.GetContext())
            {
                try
                {
                    var counterparty = context.Employer
                                               .FirstOrDefault(c => c.IdEmployer == counterpartyId);
                    if (counterparty != null && counterparty.Photo != null)
                    {
                        BitmapImage image = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(counterparty.Photo))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = stream;
                            image.EndInit();
                        }
                        Photo.Source = image;
                        FotoImageData = counterparty.Photo;
                    }
                    else
                    {
                        
                    }
                }
                catch (Exception ex)
                {
                    ClassFolder.MBClass.ErrorMB("Ошибка при загрузке фото: " + ex.Message);
                }
            }
        }

        private void LoadCounterpartyPhotoPasport(int counterpartyId) //Не воркает 
        {
            DBEntities.ResetContext();
            using (var context = DBEntities.GetContext())
            {
                try
                {
                    var counterparty = context.Employer
                                               .FirstOrDefault(c => c.IdEmployer == counterpartyId);
                    if (counterparty != null && counterparty.Photo != null)
                    {
                        BitmapImage image = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(counterparty.Photo))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = stream;
                            image.EndInit();
                        }
                        Photo.Source = image;
                        FotoImageData = counterparty.Photo;
                        DBEntities.ResetContext();
                    }
                    else
                    {
                        
                    }
                }
                catch (Exception ex)
                {
                    ClassFolder.MBClass.ErrorMB("Ошибка при загрузке фото: " + ex.Message);
                }
            }
        }

        private void DisplayAddress(int addressId) //TODO не воркает 
        {
            DBEntities.ResetContext();
            using (var context = DBEntities.GetContext())
            {
                var adress = context.Adress
                    .Where(a => a.IdAdress == addressId) 
                    .FirstOrDefault();

                if (adress != null)
                {
                    
                    string adressFormat = $"{adress.IdCity}, {adress.IdStreet}, {adress.House}, {adress.Appartment}";
                    AdresTb.Text = adressFormat;
                }
            }
        }
    }
}
