using IBA_Task_2.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace IBA_Task_2.CsvSerializer
{
    /// <summary>
    /// Csv Reader
    /// </summary>
    public class CsvReader
    {
        /// <summary>
        /// Reads data from file
        /// </summary>
        /// <param name="path">path to file</param>
        /// <returns>List of users</returns>
        public List<User> ReadFromFile(string path)
        {
            if (File.Exists(path))
            {
                var list = new List<User>();
                var lines = File.ReadAllLines(path);

                foreach (var line in lines)
                {
                    var cells = line.Split(';');
                    list.Add(ReadFromCsv(cells));
                }
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Сreating of user object
        /// </summary>
        /// <param name="values">String with data</param>
        /// <returns>User</returns>
        private User ReadFromCsv(string[] values)
        {
            var date = Convert.ToDateTime(values[0]);
            var firstName = Convert.ToString(values[1]);
            var lastName = Convert.ToString(values[2]);
            var surName = Convert.ToString(values[3]);
            var city = Convert.ToString(values[4]);
            var country = Convert.ToString(values[5]);
            var id = 0;

            User user = new User(id, firstName, lastName, surName, country, city, date);

            return user;
        }
    }
}
