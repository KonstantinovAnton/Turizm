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

namespace Turizm
{
    /// <summary>
    /// Логика взаимодействия для PageHotels.xaml
    /// </summary>
    public partial class PageHotels : Page
    {
        public PageHotels()
        {
            InitializeComponent();

            List<Hotel> listHotels = Base.EM.Hotel.ToList();
            dataGridHotels.ItemsSource = listHotels;

             
        }

        private void buttonEditHotel_Click(object sender, RoutedEventArgs e)
        {
            GlobalValues.idHotel = (dataGridHotels.SelectedItem as Hotel).Id;
            GlobalValues.isNewHotel = false;

            WindowEditHotel windowEditHotel = new WindowEditHotel();

            windowEditHotel.Show();

            foreach (Window w in App.Current.Windows)
            {
                if (w != windowEditHotel)
                    w.Close();
            }
        }

        private void buttonDeleteHotel_Click(object sender, RoutedEventArgs e)
        {
            int idHotel = (dataGridHotels.SelectedItem as Hotel).Id;

            string nameHotel = (dataGridHotels.SelectedItem as Hotel).Name;

            List<HotelOfTour> hotelOfTours = Base.EM.HotelOfTour.Where(x => x.HotelId == idHotel).ToList();

            if(hotelOfTours.Count != 0)
            {
                foreach (var item in hotelOfTours)
                {
                    var result = Base.EM.Tour.Where(x => x.Id == item.TourId).Where(y => y.IsActual == true).ToList();
                    if(result.Count > 0)
                    {
                        MessageBox.Show("Удаление невозможно. Для данного отеля существует актуальный тур", "Удаление отеля", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

               if(MessageBox.Show("Вы точно хотите удалить отель "+ nameHotel + "?", "Удаление отеля", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes){
                    deleteHotel(idHotel);
                    NavigationService.Navigate(new PageHotels());
                }
                else
                {
                    return;
                }

               
            }

            if (MessageBox.Show("Вы точно хотите удалить отель " + nameHotel + "?", "Удаление отеля", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                deleteHotel(idHotel);
                NavigationService.Navigate(new PageHotels());
            }
            else
            {
                return;
            }
        }

        public void deleteHotel(int idHotel)
        {
            List<HotelImage> hotelImages = Base.EM.HotelImage.Where(x => x.HotelId == idHotel).ToList();

            try
            {
                foreach (var item in hotelImages)
                {
                    Base.EM.HotelImage.Remove(item);
                }

                Hotel hotel = Base.EM.Hotel.First(x => x.Id == idHotel);
                Base.EM.Hotel.Remove(hotel);

                Base.EM.SaveChanges();

                MessageBox.Show("Отель успешно удален", "Удаление отеля", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Отель не удален. Попробуйте позже", "Удаление отеля", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void buttonAddHotel_Click(object sender, RoutedEventArgs e)
        {
            GlobalValues.isNewHotel = true;

            WindowEditHotel windowEditHotel = new WindowEditHotel();
            windowEditHotel.Show();

            foreach (Window w in App.Current.Windows)
            {
                if (w != windowEditHotel)
                    w.Close();
            }
        }
    }
}
