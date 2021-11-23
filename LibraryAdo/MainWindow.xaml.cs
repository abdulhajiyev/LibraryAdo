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
            }
        }

        public void Foo()
        {

        }

        private void Table_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            FillDataGrid();
        }
    }
}
