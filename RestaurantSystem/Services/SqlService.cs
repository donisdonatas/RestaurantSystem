using RestaurantSystem.Strukts;
using System.Data.SQLite;

namespace RestaurantSystem.Services
{
    public class SqlService
    {
        public static SQLiteConnection CreateConnection()
        {
            SQLiteConnection SqlConnection = new SQLiteConnection("Data Source = restaurant.db; Version = 3; New = True; Compress = True;");
            try
            {
                SqlConnection.Open();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"There was error connecting to database. Error message: {ex.Message}");
            }
            return SqlConnection;
        }

        public static List<sTable> GetListOfTables()
        {
            using SQLiteConnection ConnectToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectToDatabase.CreateCommand();

            List<sTable> FreeTableSeats = new List<sTable>();
            SQLiteDataReader SqlReader;
            SQLCommand.CommandText = $"SELECT * FROM tables;";
            SqlReader = SQLCommand.ExecuteReader();

            while (SqlReader.Read())
            {
                sTable availableTable = new sTable();
                availableTable.TableID = Convert.ToInt32(SqlReader[0]);
                availableTable.Seats = Convert.ToInt32(SqlReader[1]);
                FreeTableSeats.Add(availableTable);
            }

            return FreeTableSeats;
        }

        public static List<sTable> RetrieveTableList()
        {
            using SQLiteConnection ConnectToDatabase = CreateConnection();
            using SQLiteCommand SQLCommand = ConnectToDatabase.CreateCommand();

            List<sTable> TableList = new List<sTable>();
            SQLiteDataReader SqlReader;
            SQLCommand.CommandText = $"SELECT * FROM tables;";
            SqlReader = SQLCommand.ExecuteReader();
            while (SqlReader.Read())
            {
                sTable Table = new sTable();
                Table.TableID = Convert.ToInt32(SqlReader[0]);
                Table.Seats = Convert.ToInt32(SqlReader[1]);
                Table.isReserved = Convert.ToBoolean(SqlReader[2]);
                Table.isOrdered = Convert.ToBoolean(SqlReader[3]);
                TableList.Add(Table);
            }
            return TableList;
        }

        public static void UpdateSqlTable(string sqlString)
        {
            using SQLiteConnection ConnectToDatabase = CreateConnection();
            using SQLiteCommand SqlCommand = ConnectToDatabase.CreateCommand();

            SqlCommand.CommandText = sqlString;
            SqlCommand.ExecuteNonQuery();
        }
    }
}
