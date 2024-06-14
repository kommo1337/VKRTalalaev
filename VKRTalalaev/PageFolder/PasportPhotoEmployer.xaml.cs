using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
    /// Логика взаимодействия для PasportPhotoEmployer.xaml
    /// </summary>
    public partial class PasportPhotoEmployer : Page
    {
        Employer employer;
        private byte[] FotoImageData;
        public PasportPhotoEmployer(Employer employer)
        {

            InitializeComponent();
            
            this.employer = employer;
            DataContext = employer;
            LoadCounterpartyPhotoPasport(employer.IdEmployer);
        }

        private void LoadCounterpartyPhotoPasport(int counterpartyId) 
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
                        PasportPhotoIMG.Source = image;
                        FotoImageData = counterparty.Photo;
                        DBEntities.ResetContext();
                    }
                    
                }
                catch (Exception ex)
                {
                    ClassFolder.MBClass.ErrorMB("Ошибка при загрузке фото: " + ex.Message);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
