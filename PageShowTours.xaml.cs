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
    /// Логика взаимодействия для PageShowTours.xaml
    /// </summary>
    public partial class PageShowTours : Page
    {
        public PageShowTours()
        {
            InitializeComponent();

            

            defTypes();
            defListView();
        }


        private void defTypes()
        {
            List<Type> data = new List<Type>();

            Type type = new Type();
            type.Id = 0;
            type.Name = "Все типы";

            data.Add(type);

            List<Type> listTypes = Base.EM.Type.ToList();
           

            foreach (var item in listTypes)
            {
                data.Add(item);
            }

       
        

            comboBoxTypes.ItemsSource = data.ToList();

            comboBoxTypes.DisplayMemberPath = "Name";
            comboBoxTypes.SelectedValuePath = "Id";

            comboBoxTypes.SelectedIndex = 0;
        }

        private void defListView()
        {
            List<Tour> listTours = Base.EM.Tour.ToList();

          
            listView.ItemsSource = listTours;
        }

        private void textBoxNameTour_TextChanged(object sender, TextChangedEventArgs e)
        {
            sortTours();
        }

        private void comboBoxTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            sortTours();  
        }

        List<Tour> globalListTours = new List<Tour>();

        private void sortTours()
        {

            int idType = Convert.ToInt32(comboBoxTypes.SelectedValue.ToString());

            if (idType != 0)
            {

                List<TypeOfTour> listTypeOFTour = Base.EM.TypeOfTour.Where(x => x.TypeId == idType).ToList();

                List<Tour> listTours = new List<Tour>();

                foreach (var item in listTypeOFTour)
                {
                    if (item.TypeId == idType)
                    {
                        listTours.Add(Base.EM.Tour.FirstOrDefault(x => x.Id == item.TourId));
                    }
                }

                if (textBoxNameTour.Text != "" && textBoxNameTour.Text != " ")
                    listTours = listTours.Where(x => x.Name.ToLower().Contains(textBoxNameTour.Text.ToLower())).ToList();

                if ((bool)checkBoxIsActual.IsChecked)
                    listTours = listTours.Where(x => x.IsActual == true).ToList();


                if ((bool)radioButtonDesc.IsChecked)
                {
                    listTours = listTours.OrderByDescending(x => x.Price).ToList();
                }
                else
                {
                    listTours = listTours.OrderBy (x => x.Price).ToList();
                }

                listView.ItemsSource = listTours;
                globalListTours = listTours;

                decimal ageSum = listTours.Sum(x => x.Price * x.TicketCount);
                textBlockTotalPrice.Text = ("Общая стоимость: " + string.Format("{0:F}", ageSum) + " руб.");

            }
            else
            {
                List<Tour> listTours = Base.EM.Tour.ToList();


                if ((bool)checkBoxIsActual.IsChecked)
                    listTours = listTours.Where(x => x.IsActual == true).ToList();
               

                if (textBoxNameTour.Text != "" && textBoxNameTour.Text != " ")
                    listTours = listTours.Where(x => x.Name.ToLower().Contains(textBoxNameTour.Text.ToLower())).ToList();


                if((bool)radioButtonDesc.IsChecked)
                {
                    listTours = listTours.OrderByDescending(x => x.Price).ToList();
                }
                else
                {
                    listTours = listTours.OrderBy(x => x.Price).ToList();
                }

                listView.ItemsSource = listTours;
                globalListTours = listTours;

                decimal ageSum = listTours.Sum(x => x.Price * x.TicketCount);
             
                
                
                
               textBlockTotalPrice.Text = ("Общая стоимость: " + string.Format("{0:F}", ageSum) + " руб.");
            }



        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            sortTours();
           
        }

        private void radioButtonDesc_Checked(object sender, RoutedEventArgs e)
        {
            List<Tour> listTour  = globalListTours.OrderByDescending(x => x.Price).ToList();
            listView.ItemsSource = listTour;
        }

        private void radioButtonAsc_Checked(object sender, RoutedEventArgs e)
        {
          /* List<Tour> listTour = globalListTours.OrderBy(x => x.Price).ToList();
            listView.ItemsSource = listTour;*/
         
        }

       
    }

}
