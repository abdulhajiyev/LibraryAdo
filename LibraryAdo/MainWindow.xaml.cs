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
            InitializeComponent();
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
            }
        }

        public void FillCategoryDataGrid()
        {
            SqlConnection sqlConnection;
            using (sqlConnection = new SqlConnection(ConnectionString))
            {
                var cmdString = $@"SELECT Categories.Name AS 'Category', Books.Name AS 'Book' FROM Categories JOIN Books ON Books.Id_Category = Categories.Id WHERE Categories.Name = '{CatAut.SelectedItem}'";

                SqlCommand sqlCommand = new SqlCommand(cmdString, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable(CatAut.SelectedItem.ToString());


                sqlDataAdapter.Fill(dataTable);

                DataGrd.ItemsSource = dataTable.DefaultView;
                MWindow.Title = $"Category: {CatAut.SelectedItem}";
            }
        }

        public void FillCustomDataGrid()
        {
            SqlConnection sqlConnection;
            using (sqlConnection = new SqlConnection(ConnectionString))
            {
                ComboBoxItem cbi = (ComboBoxItem)Table.SelectedItem;

                var cmdString = "SELECT * FROM Books";

                SqlCommand sqlCommand = new SqlCommand(cmdString, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTable = new DataTable(cbi.Content.ToString());

                sqlDataAdapter.Fill(dataTable);

                DataGrd.ItemsSource = dataTable.DefaultView;
            }
        }

        private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Table.SelectedIndex == 0)
            {
                CatAut.IsEnabled = true;
                TbSearch.IsEnabled = false;

                LoadAuthorToCombo();
            }
            else if (Table.SelectedIndex == 1)
            {
                CatAut.IsEnabled = true;
                TbSearch.IsEnabled = false;

                LoadCategoriesToCombo();
            }
            else if (Table.SelectedIndex == 2)
            {
                FillCustomDataGrid();
                CatAut.Items.Clear();
                CatAut.IsEnabled = false;
                TbSearch.IsEnabled = true;
            }
        }

        private void CatAut_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CatAut.SelectedItem is null) return;
            if (Table.SelectedIndex == 0)
            {
                FillAuthorDataGrid();
            }
            else if (Table.SelectedIndex == 1)
            {
                FillCategoryDataGrid();
            }
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TbSearch.Text != "")
            {
                SqlConnection sqlConnection;
                using (sqlConnection = new SqlConnection(ConnectionString))
                {
                    ComboBoxItem cbi = (ComboBoxItem)Table.SelectedItem;

                    var cmdString = $"SELECT * FROM Books WHERE Name LIKE '%{TbSearch.Text}%'";

                    SqlCommand sqlCommand = new SqlCommand(cmdString, sqlConnection);
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                    DataTable dataTable = new DataTable(cbi.Content.ToString());

                    sqlDataAdapter.Fill(dataTable);

                    DataGrd.ItemsSource = dataTable.DefaultView;
                }
            }
            else if (TbSearch.Text == "")
            {
                FillCustomDataGrid();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)DataGrd.SelectedItem;
            row.Delete();

            SqlConnection sqlConnection;
            using (sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand($"DELETE FROM Books WHERE Name = '{row["Name"]}'", sqlConnection);
                sqlConnection.Open();
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
