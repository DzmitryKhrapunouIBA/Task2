using IBA_Task_2.Models;
using IBA_Task_2.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Data;
using System.Data.Entity;

namespace IBA_Task_2.Services
{
    public class DbService
    {
        /// <summary>
        /// Retrieves users by specific parameters from the database and shows them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public List<UserViewModel> GetSpecificUsers(string firstName, string lastName, string surName, string country, string city, string date)
        {
            using (UserContext db = new UserContext())
            {
                var _date = TryParse(date);
                var _city = db.Cities.Where(c => c.CityName == city).FirstOrDefault();
                var cityId = _city?.Id;
                var _country = db.Сountries.Where(c => c.CountryName == country).FirstOrDefault();
                var countryId = _country?.Id;

                var users = db.Users.Include(c => c.Country).Include(t => t.City)
                    .Where(u => (lastName != null ? u.LastName == lastName : true)
                             && (firstName != null ? u.FirstName == firstName : true)
                             && (surName != null ? u.SurName == surName : true)
                             && (date != null ? u.Date == _date : true)
                             && (city != null ? u.CityId == cityId : true)
                             && (country != null ? u.CountryId == countryId : true)
                             ).ToList();

                List<UserViewModel> UserViewModels = new List<UserViewModel>();
                foreach (var user in users)
                {
                    UserViewModels.Add(new UserViewModel(user));
                }

                return UserViewModels;
            }
        }

        /// <summary>
        /// Retrieves all users from the database and shows them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public List<UserViewModel> GetAllUsers(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                var users = db.Users.Include(c => c.Country).Include(t => t.City).ToList();

                List<UserViewModel> UserViewModels = new List<UserViewModel>();
                foreach (var user in users)
                {
                    UserViewModels.Add(new UserViewModel(user));
                }

                return UserViewModels;
            }
        }

        /// <summary>
        /// Covert text to date
        /// </summary>
        /// <param name="text"></param>
        /// <returns>DateTime</returns>
        private DateTime? TryParse(string text)
        {
            DateTime date;
            return DateTime.TryParse(text, out date) ? date : (DateTime?)null;
        }
    }
}
