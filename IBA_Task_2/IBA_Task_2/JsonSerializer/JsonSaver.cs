using IBA_Task_1.Abstract;
using IBA_Task_2.Views;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace IBA_Task_2.JsonSerializer
{
    public class JsonSaver : ISaver
    {
        /// <summary>
        /// Saves an object
        /// </summary>
        /// <param name="data">data to save</param>
        /// <param name="path">path to file</param>
        public void SaveToFile(List<UserViewModel> users, string path)
        {
            string output = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(path, output);
        }
    }
}