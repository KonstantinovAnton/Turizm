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

namespace Turizm
{
    /// <summary>
    /// Логика взаимодействия для WindowEditHotel.xaml
    /// </summary>
    public partial class WindowEditHotel : Window
    {
        Hotel hotelToEdit;

        public WindowEditHotel()
        {
            InitializeComponent();

            if (GlobalValues.isNewHotel)
            {
                buttonSave.Content = "Добавить";
                addHotel();
            }
            else
                fillHotel();
            
        }


        private void fillHotel()
        {
            hotelToEdit = Base.EM.Hotel.First(x => x.Id == GlobalValues.idHotel);

            textBoxHotelName.Text = hotelToEdit.Name;
            comboBoxStars.SelectedIndex = hotelToEdit.CountOfStars;
            textBoxDescription.Text =  hotelToEdit.Description;

            defCountries();

            comboBoxCountries.SelectedItem = hotelToEdit.Country;



        }

        private void defCountries()
        {
            comboBoxCountries.ItemsSource = Base.EM.Country.ToList();

            comboBoxCountries.DisplayMemberPath = "Name";
            comboBoxCountries.SelectedValuePath = "Code";
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (!(GlobalValues.isNewHotel))
            {
                if (textBoxHotelName.Text == "" || textBoxHotelName.Text == " ")
                {
                    MessageBox.Show("Дайте название отелю", "Изменение отеля", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (textBoxDescription.Text == "" || textBoxDescription.Text == " ")
                {
                    MessageBox.Show("Добавьте описание отеля", "Изменение отеля", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                hotelToEdit.Name = textBoxHotelName.Text;
                hotelToEdit.Description = textBoxDescription.Text;
                hotelToEdit.CountOfStars = comboBoxStars.SelectedIndex;
                hotelToEdit.CountryCode = comboBoxCountries.SelectedValue.ToString();

                try
                {
                    Base.EM.SaveChanges();
                    MessageBox.Show("Данные успешно отредактированы", "Изменение отеля", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Проблема с соединением. Попробуйте позже", "Изменение отеля", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                if (textBoxHotelName.Text == "" || textBoxHotelName.Text == " ")
                {
                    MessageBox.Show("Дайте название отелю", "Добавление отеля", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (textBoxDescription.Text == "" || textBoxDescription.Text == " ")
                {
                    MessageBox.Show("Добавьте описание отеля", "Добавление отеля", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if(comboBoxCountries.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите страну", "Добавление отеля", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (comboBoxStars.SelectedIndex == -1)
                {
                    MessageBox.Show("Выберите кол-во звезд", "Добавление отеля", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Hotel hotel = new Hotel();
                hotel.Name = textBoxHotelName.Text;
                hotel.Description = textBoxDescription.Text;
                hotel.CountOfStars = comboBoxStars.SelectedIndex;
                hotel.CountryCode = comboBoxCountries.SelectedValue.ToString();

                try
                {
                    Base.EM.Hotel.Add(hotel);
                    Base.EM.SaveChanges();

                    MessageBox.Show("Отель успешно добавлен", "Добавление отеля", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Отель не добавлен. Попробуйте позже", "Добавление отеля", MessageBoxButton.OK, MessageBoxImage.Warning);
                }


            }
            MainWindow mainWindow = new MainWindow();
            mainWindow.frm.Navigate(new PageHotels());
            mainWindow.Show();

            this.Close();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.frm.Navigate(new PageHotels());
            mainWindow.Show();

            this.Close();
        }

        private void addHotel()
        {
            defCountries();
        }
    }
}
