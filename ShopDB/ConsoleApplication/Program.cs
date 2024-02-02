using System.Xml.Linq;
using System.Data.SqlClient;
using System.Globalization;

namespace ConsoleApplication
{
    class Program
    {
        private static string _filePath = "C:\\Users\\Godli\\OneDrive\\Desktop\\Projects\\ShopDB\\ConsoleApplication\\inputData.xml";
        static void Main(string[] args)
        {
            SqlConnection sqlConnection = DataBaseConnection.Connect();
            try
            {
                XDocument xmlDocument = XDocument.Load(_filePath);

                var orders = xmlDocument.Root.Elements("order");
                foreach (var order in orders)
                {
                    //user data
                    var user = order.Element("user");
                    string username = user.Element("fio").Value;
                    string email = user.Element("email").Value;

                    //check if user already exists

                    string query = $"IF NOT EXISTS (SELECT 1 FROM Users WHERE username = @username AND email = @email)" +
                                   $"INSERT INTO Users (username, email) VALUES (@username, @email)";
                    SqlCommand command = new SqlCommand(query, sqlConnection);
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@email", email);
                    command.ExecuteNonQuery();

                    //get userId
                    query = $"SELECT user_id FROM Users WHERE username = N'{username}' AND email = '{email}'";
                    command = new SqlCommand(query, sqlConnection);
                    int userId = (int)(command.ExecuteScalar());

                    //purchase data
                    int purchaseId = int.Parse(order.Element("no").Value);
                    string purchaseDate = DateTime.ParseExact(order.Element("reg_date").Value, "yyyy.MM.dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
                    decimal purchaseSum = decimal.Parse(order.Element("sum").Value, CultureInfo.InvariantCulture);

                    query = "SET IDENTITY_INSERT Purchases ON";
                    command = new SqlCommand(query, sqlConnection);
                    command.ExecuteNonQuery();

                    query = $"IF NOT EXISTS (SELECT 1 FROM Purchases WHERE purchase_id = @purchaseId)" +
                            $"INSERT INTO Purchases (purchase_id, user_id, purchase_date, purchase_sum) VALUES (@purchaseId, @userId, @purchaseDate, @purchaseSum)";
                    command = new SqlCommand (query, sqlConnection);
                    command.Parameters.AddWithValue("@purchaseId", purchaseId);
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@purchaseDate", purchaseDate);
                    command.Parameters.AddWithValue("@purchaseSum", purchaseSum);
                    command.ExecuteNonQuery();

                    query = "SET IDENTITY_INSERT Purchases OFF";
                    command = new SqlCommand(query, sqlConnection);
                    command.ExecuteNonQuery();

                    //products and purchaseDetails data
                    var products = order.Elements("product");
                    foreach (var product in products)
                    {
                        //product data
                        string name = product.Element("name").Value;
                        decimal price = decimal.Parse(product.Element("price").Value, CultureInfo.InvariantCulture);
                        query = $"IF NOT EXISTS (SELECT 1 FROM Products WHERE name = @name AND price = @price)" +
                                $"INSERT INTO Products (name, price) VALUES (@name, @price)";
                        command = new SqlCommand(query, sqlConnection);
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@price", price);
                        command.ExecuteNonQuery();

                        //get productId
                        query = $"SELECT product_id FROM Products WHERE name = '{name}'";
                        command = new SqlCommand(query, sqlConnection);
                        int productId = (int)(command.ExecuteScalar());

                        //purchase details
                        int quantity = int.Parse(product.Element("quantity").Value);
                        query = $"IF NOT EXISTS (SELECT 1 FROM PurchaseDetails WHERE product_id = @productId AND quantity = @quantity)" +
                                $"INSERT INTO PurchaseDetails (purchase_id, product_id, quantity) VALUES (@purchaseId, @productId, @quantity)";
                        command = new SqlCommand(query, sqlConnection);
                        command.Parameters.AddWithValue("@purchaseId", purchaseId);
                        command.Parameters.AddWithValue("@productId", productId);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.ExecuteNonQuery();
                    }
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Data uploaded successfully!");
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Ошибка при чтении XML-файла: {ex.Message}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            DataBaseConnection.Close();
        }
    }
}
