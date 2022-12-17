using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turizm
{
    public  partial class Hotel
    {
        public int ToursHotel
        {
            get
            {
                List<HotelOfTour> list = Base.EM.HotelOfTour.Where(x => x.HotelId == Id).ToList();
                int i = 0;
                foreach (HotelOfTour hotelOfTour in list)
                {
                    i += hotelOfTour.Tour.TicketCount;
                }
                return i;
            }
        }
    }
}
