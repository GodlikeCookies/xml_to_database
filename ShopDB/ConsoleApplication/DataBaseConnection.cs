using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class DataBaseConnection
    {
        private const string _connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Godli\\OneDrive\\Desktop\\Projects\\ShopDB\\ConsoleApplication\\Database.mdf;Integrated Security=True";
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
