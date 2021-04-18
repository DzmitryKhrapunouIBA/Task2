using IBA_Task_1.Abstract;
using IBA_Task_2.Views;
using Microsoft.Office.Interop.Excel;
using System.Collections;
using System.Collections.Generic;

namespace IBA_Task_2.ExcelSerializer
{
    public class ExcelSaver : ISaver
    {
        // Start line for writing titles
        const int StartTitles = 1;

        // Start line for writing data
        const int StartData = 2;

        // Number of columns
        const int Quantity = 6;

        /// <summary>
        /// Save the users to excel file
        /// </summary>
        /// <param name="users">users to save</param>
        /// <param name="path">path to file</param>
        public void SaveToFile(List<UserViewModel> users, string path)
        {
            Application excelApp = new Application();
            Workbook workBook = excelApp.Workbooks.Add();
            Worksheet workSheet = workBook.ActiveSheet;

            ArrayList titles = new ArrayList();

            titles.Add("FirstName");
            titles.Add("LastName");
            titles.Add("SurName");
            titles.Add("Country");
            titles.Add("City");
            titles.Add("Date");

            for (int i = 1; i <= titles.Count; i++)
            {
                workSheet.Cells[StartTitles, i] = titles[i-1];
            }

            for (int i = StartData; i <= StartData + users.Count - 1; i++)
            {
                var user = AddUserToList(users[i - StartData]);

                for (int j = 1; j <= Quantity; j++)
                    workSheet.Cells[i, j] = user[j-1];
            }

            workSheet.Columns.EntireColumn.AutoFit();
            workBook.Close(true, path);
            excelApp.Quit();
        }

        /// <summary>
        /// Add user to list
        /// </summary>
        /// <param name="user"> UserViewModel </param>
        /// <returns>List of user attrebutes</returns>
        private ArrayList AddUserToList(UserViewModel user)
        {
            ArrayList userAttributs = new ArrayList();

            userAttributs.Add(user.FirstName);
            userAttributs.Add(user.LastName);
            userAttributs.Add(user.SurName);
            userAttributs.Add(user.Country);
            userAttributs.Add(user.City);
            userAttributs.Add(user.Date);

            return userAttributs;
        }
    }
}
