using System;

namespace IBA_Task_2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public Country Country { get; set; }
        public City City { get; set; }
        public DateTime Date { get; set; }

        public User(int id, string firstName, string lastName, string surName, string countryName, string cityName, DateTime date)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            SurName = surName;
            Country = new Country(countryName);
            City = new City(cityName);
            Date = date;
        }
    }
}
