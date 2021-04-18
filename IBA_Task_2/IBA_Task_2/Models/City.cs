using System;

namespace IBA_Task_2.Models
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }

        public City(string cityName)
        {
            CityName = cityName;
        }
    }
}
