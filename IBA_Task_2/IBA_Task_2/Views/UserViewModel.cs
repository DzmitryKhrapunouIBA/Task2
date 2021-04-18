using IBA_Task_2.Models;
using System;

namespace IBA_Task_2.Views
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }

        public UserViewModel(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            SurName = user.SurName;
            Country = user.Country.CountryName;
            City = user.City.CityName;
            Date = user.Date;
        }
    }
}
