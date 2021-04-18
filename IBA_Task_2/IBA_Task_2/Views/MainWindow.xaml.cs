using IBA_Task_2.CsvSerializer;
using IBA_Task_2.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using Microsoft.Win32;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using IBA_Task_2.ExcelSerializer;
using IBA_Task_2.JsonSerializer;
using IBA_Task_1.Abstract;
using System.IO;
using System.Data.Common;
using System.Data.Entity;

namespace IBA_Task_2.Views
{
    delegate void UpdateProgressBarDelegate(DependencyProperty dp, object value);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ProgressBar _progressBar;
        private string _filePathExcel = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Data\", "ExcelFile.xlsx");
        private string _filePathJson = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\Data\", "File.json");
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SurName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Date { get; set; }
        public List<User> Users { get; set; }
        public List<UserViewModel> UserViewModels { get; set; }
        public Action action { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Menu handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuOpen_Click(object sender, EventArgs e)
        {
            worker_ProgressChanged(sender, e);

            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;

            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Open a new progress window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void worker_ProgressChanged(object sender, EventArgs e)
        {
            _progressBar = new ProgressBar();
            _progressBar.Owner = this;
        }

        /// <summary>
        /// Сalling a method in a parallel thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ReadAndSaveFile(sender);
        }

        /// <summary>
        /// parameters for ProgressBar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progressBar.prgBar.Maximum = Users.Count;
            _progressBar.prgBar.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Reading and saving a file in csv format
        /// </summary>
        /// <param name="sender"></param>
        private void ReadAndSaveFile(object sender)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = true;
            openFile.Filter = "Users|*.csv|All Files|*.*";
            openFile.DefaultExt = ".csv";
            
            if (openFile.ShowDialog() == true)
            {
                Application.Current.Dispatcher.Invoke(() => _progressBar.Show());

                using (UserContext db = new UserContext())
                {
                    CsvReader csvReader = new CsvReader();

                    var fileNames = "";

                    foreach (var fileName in openFile.FileNames)
                    {
                        fileNames += ";" + fileName;
                    }

                    fileNames = fileNames.Substring(1);

                    Users = csvReader.ReadFromFile(fileNames);

                    for (int i = 0; i < Users.Count - 1; i++)
                    {
                        db.Users.Add(Users[i]);
                        (sender as BackgroundWorker).ReportProgress(i + 1);
                    }

                    (sender as BackgroundWorker).ReportProgress(Users.Count);

                    db.SaveChanges();

                    Application.Current.Dispatcher.Invoke(() => _progressBar.Close() );

                    MessageBox.Show("Opening the file was successful");
                }
            }
        }

        /// <summary>
        /// User Search button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchUserButton_Click(object sender, RoutedEventArgs e)
        {
            GetSpecificUsers(sender, e);
        }

        /// <summary>
        /// Show all users button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenAllUsersButton_Click(object sender, RoutedEventArgs e)
        {
            GetAllUsers(sender, e);
        }

        /// <summary>
        /// Clean handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CleanButton_Click(object sender, RoutedEventArgs e)
        {
            UsersGrid.ItemsSource = null;
            ClearProperties();
            UserViewModels.Clear();
        }

        /// <summary>
        /// Retrieves users by specific parameters from the database and shows them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetSpecificUsers(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                var firstNameParameter = new SqlParameter("@FirstName", FirstName);
                var lastNameParameter = new SqlParameter("@LastName", LastName);
                var surNameParameter = new SqlParameter("@SurName", SurName);
                var countryParameter = new SqlParameter("@Country", Country);
                var cityParameter = new SqlParameter("@City",  City);
                var dateParameter = new SqlParameter("@Date", datePicker.Text);

                DataTable dt = DataTable(db, "sp_GetUsers @FirstName, @LastName, @SurName, @Country, @City, @Date", firstNameParameter, lastNameParameter,
                    surNameParameter, countryParameter, cityParameter, dateParameter);

                UsersGrid.ItemsSource = dt.DefaultView;
                GetUsersFromDataTable(dt);

                ClearProperties();
            }
        }

        /// <summary>
        /// Retrieves all users from the database and shows them
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetAllUsers(object sender, RoutedEventArgs e)
        {
            using (UserContext db = new UserContext())
            {
                DataTable dt = DataTable(db, "sp_GetAllUsers");

                UsersGrid.ItemsSource = dt.DefaultView;
                GetUsersFromDataTable(dt);

                ClearProperties();
            }
        }

        /// <summary>
        /// Get users from DataTable
        /// </summary>
        /// <param name="dt">Data table</param>
        private void GetUsersFromDataTable(DataTable dt)
        {
            UserViewModels = new List<UserViewModel>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var id = Convert.ToInt32(dt.Rows[i]["Id"]);
                var firstName = Convert.ToString(dt.Rows[i]["FirstName"]);
                var lastName = Convert.ToString(dt.Rows[i]["LastName"]);
                var surName = Convert.ToString(dt.Rows[i]["SurName"]);
                var city = Convert.ToString(dt.Rows[i]["CityName"]);
                var country = Convert.ToString(dt.Rows[i]["CountryName"]);
                var date = Convert.ToDateTime(dt.Rows[i]["Date"]);

                User user = new User(id, firstName, lastName, surName, country, city, date);
                UserViewModel userViewModel = new UserViewModel(user);
                UserViewModels.Add(userViewModel);
            }
        }

        /// <summary>
        /// DbContext object extension method to create a new data table
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sqlQuery"></param>
        /// <param name="parameters"></param>
        /// <returns>Data table</returns>
        DataTable DataTable(DbContext context, string sqlQuery, params DbParameter[] parameters)
        {
            DataTable dataTable = new DataTable();
            DbConnection connection = context.Database.Connection;
            DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;

                foreach (var item in parameters)
                {
                    if (item.Value == null || item.Value.Equals(""))
                    {
                        item.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(item);
                }

                using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        /// <summary>
        /// FirstName TextBox handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbxFirst_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (IsWordCharactersValid(textBox.Text))
            {
                FirstName = textBox.Text;
            }
            else
            {
                if (textBox.Text != "")
                {
                    MessageBox.Show("Invalid input");
                }
            }
        }

        /// <summary>
        /// LastName TextBox handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbxLast_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (IsWordCharactersValid(textBox.Text))
            {
                LastName = textBox.Text;
            }
            else
            {
                if (textBox.Text != "")
                {
                    MessageBox.Show("Invalid input");
                }
            }
        }

        /// <summary>
        /// SurName TextBox handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbxSur_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (IsWordCharactersValid(textBox.Text))
            {
                SurName = textBox.Text;
            }
            else
            {
                if (textBox.Text != "")
                {
                    MessageBox.Show("Invalid input");
                }
            }
        }

        /// <summary>
        /// Country TextBox handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbxCountry_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (IsWordCharactersValid(textBox.Text))
            {
                Country = textBox.Text;
            }
            else
            {
                if (textBox.Text != "")
                {
                    MessageBox.Show("Invalid input");
                }
            }
        }

        /// <summary>
        /// City TextBox handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TbxCity_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (IsWordCharactersValid(textBox.Text))
            {
                City = textBox.Text;
            }
            else
            {
                if (textBox.Text != "")
                {
                    MessageBox.Show("Invalid input");
                }
            }
        }

        /// <summary>
        /// Check if the value characters valid
        /// </summary>
        /// <param name="value">the player word</param>
        /// <returns>true if the value characters valid</returns>
        private bool IsWordCharactersValid(string value)
        {
            return Regex.Match(value, "^[a-zA-Z-а-яА-Я]+$").Success;
        }

        /// <summary>
        /// Clear properties
        /// </summary>
        private void ClearProperties()
        {
            FirstName = null;
            LastName = null;
            SurName = null;
            Country = null;
            City = null;
            Date = null;
        }

        /// <summary>
        /// Save to Json file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToJson_Click(object sender, RoutedEventArgs e)
        {
            ISaver jsonSaver = new JsonSaver();
            jsonSaver.SaveToFile(UserViewModels, _filePathJson);
        }

        /// <summary>
        /// Save to Excel file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            ISaver excelSaver = new ExcelSaver();
            excelSaver.SaveToFile(UserViewModels, _filePathExcel);
        }
    }
}
