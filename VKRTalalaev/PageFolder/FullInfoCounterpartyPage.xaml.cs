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
    /// Логика взаимодействия для FullInfoCounterpartyPage.xaml
    /// </summary>
    public partial class FullInfoCounterpartyPage : Page
    {
        Counterparty party;
        public FullInfoCounterpartyPage(Counterparty party)
        {
            DataContext = party;
            this.party = party;
            
            InitializeComponent();
            AdresTb.Text = GetFullAddress(party.IdAdress);
        }

        public static string GetFullAddress(int addressId)
        {

             DBEntities.ResetContext();
                var address = DBEntities.GetContext().Adress
                                     .Where(a => a.IdAdress == addressId)
                                     .Select(a => new
                                     {
                                         a.IdCity,
                                         a.IdStreet,
                                         a.House,
                                         a.Appartment
                                     })
                                     .FirstOrDefault();

                if (address == null)
                {
                    return "Address not found";
                }

                var city = DBEntities.GetContext().City
                                  .Where(c => c.IdCity == address.IdCity)
                                  .Select(c => c.CityName)
                                  .FirstOrDefault();

                var street = DBEntities.GetContext().Street
                                    .Where(s => s.IdStreet == address.IdStreet)
                                    .Select(s => s.StreetName)
                                    .FirstOrDefault();

                return $"{city}, улица {street}, дом {address.House}, квартира {address.Appartment}";
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void PhotoPasportBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PasportPhotoCounterparty(party));
        }
    }
}
