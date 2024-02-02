using System.Data.SqlClient;
using System.Data;

namespace ConsoleApplication
{
    public class DataBaseConnection
    {
        private const string _connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\GitHub\\xml_to_database\\ShopDB\\ConsoleApplication\\Database.mdf;Integrated Security=True";
        private static SqlConnection _sqlConnection = null;
        public static SqlConnection Connect()
        {
            _sqlConnection = new SqlConnection(_connectionString);
            _sqlConnection.Open();
            if (_sqlConnection.State == ConnectionState.Open)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Connetion established!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            return _sqlConnection;
        }
        public static void Close()
        {
            _sqlConnection.Close();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Connection closed by user!");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
