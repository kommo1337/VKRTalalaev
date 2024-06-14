using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для PasportPhotoCounterparty.xaml
    /// </summary>
    public partial class PasportPhotoCounterparty : Page
    {
        Counterparty counterparty;
        private byte[] FotoImageData;
        public PasportPhotoCounterparty(Counterparty counterparty)
        {
            this.counterparty = counterparty;
            DataContext = counterparty;
            InitializeComponent();
            LoadCounterpartyPhotoPasport(counterparty.IdCounterparty);
        }

        private void LoadCounterpartyPhotoPasport(int counterpartyId)
        {
            DBEntities.ResetContext();
            using (var context = DBEntities.GetContext())
            {
                try
                {
                    var counterparty = context.Counterparty
                                               .FirstOrDefault(c => c.IdCounterparty == counterpartyId);
                    if (counterparty != null && counterparty.PasportPhoto != null)
                    {
                        BitmapImage image = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(counterparty.PasportPhoto))
                        {
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.StreamSource = stream;
                            image.EndInit();
                        }
                        PasportPhotoIMG.Source = image;
                        FotoImageData = counterparty.PasportPhoto;
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
