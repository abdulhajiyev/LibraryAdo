using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Configuration;

namespace LibraryAdo
{
    public partial class MainWindow : Window
    {
        public readonly string ConnectionString = ConfigurationManager.ConnectionStrings["DbConnect"].ConnectionString;
        public MainWindow()
        {
            //var connectionString = ConfigurationManager.ConnectionStrings["DbConnect"].ConnectionString;
            InitializeComponent();
            Table.SelectedIndex = 0;
            //LoadToCombo();
        }

        public void FillDataGrid()
        {
            SqlConnection sqlConnection;
            using (sqlConnection = new SqlConnection(ConnectionString))
            {
                ComboBoxItem cbi = (ComboBoxItem)Table.SelectedItem;

                var cmdString = $"SELECT * FROM {cbi.Content}";

                SqlCommand sqlCommand = new SqlCommand(cmdString, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable(cbi.Content.ToString());

                sqlDataAdapter.Fill(dataTable);

                DataGrd.ItemsSource = dataTable.DefaultView;
                MWindow.Title = $"SELECT * FROM {cbi.Content}";
            }
        }

        public void LoadAuthorToCombo()
        {
            CatAut.Items.Clear();
            SqlConnection sqlConnection;
            using (sqlConnection = new SqlConnection(ConnectionString))
            {

                var cmdString = "SELECT CONCAT(FirstName, LastName) AS 'Author' FROM Authors";
                SqlCommand sqlCommand = new SqlCommand(cmdString, sqlConnection);
                sqlConnection.Open();
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CatAut.Items.Add(reader[0].ToString());
                    }
                    sqlConnection.Close();
                }
            }
        }

        public void FillAuthorDataGrid()
        {

            /*SELECT CONCAT(FirstName, LastName) AS 'Author', Books.Name AS 'BookName' FROM Authors JOIN Books ON Books.Id_Author = Authors.Id GROUP BY CONCAT(FirstName, LastName), Books.Name HAVING CONCAT(FirstName, LastName) = 'Boris Carpov'*/

            SqlConnection sqlConnection;
            using (sqlConnection = new SqlConnection(ConnectionString))
            {
                var cmdString = $@"SELECT CONCAT(FirstName, LastName) AS 'Author', Books.Name AS 'BookName' FROM Authors JOIN Books ON Books.Id_Author = Authors.Id WHERE CONCAT(FirstName, LastName) = '{CatAut.SelectedItem}'";

                SqlCommand sqlCommand = new SqlCommand(cmdString, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable(CatAut.SelectedItem.ToString());

                sqlDataAdapter.Fill(dataTable);

                DataGrd.ItemsSource = dataTable.DefaultView;
                MWindow.Title = $"Author: {CatAut.SelectedItem}";
            }
        }

        public void LoadCategoriesToCombo()
        {
            CatAut.Items.Clear();
            SqlConnection sqlConnection;
            using (sqlConnection = new SqlConnection(ConnectionString))
            {

                var cmdString = "SELECT DISTINCT Categories.Name AS 'Category' FROM Categories JOIN Books ON Books.Id_Category = Categories.Id";
                SqlCommand sqlCommand = new SqlCommand(cmdString, sqlConnection);
                sqlConnection.Open();
                using (SqlDataReader reader = sqlCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CatAut.Items.Add(reader[0].ToString());
                    }
                    sqlConnection.Close();
                }

                CatAut.SelectedIndex = 0;
            }
        }

        public void FillCategoryDataGrid()
        {
            SqlConnection sqlConnection;
            using (sqlConnection = new SqlConnection(ConnectionString))
            {
                var cmdString = $@"SELECT Categories.Name AS 'Category', Books.Name AS 'Book' FROM Categories JOIN Books ON Books.Id_Category = Categories.Id WHERE Categories.Name = '{CatAut.SelectedItem}'";
                //CatAut.SelectedIndex = 0;
                SqlCommand sqlCommand = new SqlCommand(cmdString, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable(CatAut.SelectedItem.ToString());


                sqlDataAdapter.Fill(dataTable);

                DataGrd.ItemsSource = dataTable.DefaultView;
                MWindow.Title = $"Category: {CatAut.SelectedItem}";
            }
        }



        private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Table.SelectedIndex == 0)
            {
                CatAut.Items.Clear();
                LoadAuthorToCombo();

            }
            else if (Table.SelectedIndex == 1)
            {
                CatAut.Items.Clear();
                LoadCategoriesToCombo();
            }

        }

        private void CatAut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (Table.SelectedIndex == 0)
            {
                FillAuthorDataGrid();

            }
            else if (Table.SelectedIndex == 1)
            {
                FillCategoryDataGrid();
            }*/
            //FillAuthorDataGrid();
            FillCategoryDataGrid();


            

        }
    }
}
