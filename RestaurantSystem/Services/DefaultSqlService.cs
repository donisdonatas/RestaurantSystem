using RestaurantSystem.Models;
using RestaurantSystem.Extentions;
using RestaurantSystem.Enumbers;
using System.Data.SQLite;

namespace RestaurantSystem.Services
{
    public class DefaultSqlService
    {
        private List<Table> Tables = new List<Table>() {new Table(2),
                                                         new Table(2),
                                                         new Table(4),
                                                         new Table(4),
                                                         new Table(4),
                                                         new Table(6),
                                                         new Table(6),
                                                         new Table(8)};

        private List<Meal> FoodMenu = new List<Meal>() {new Meal(MealTypes.Užkandžiai, "Rinkinys prie vyno", 9.50m),
                                                         new Meal(MealTypes.Užkandžiai, "Silkės tartaras su baravykais", 6.50m),
                                                         new Meal(MealTypes.Užkandžiai, "Kepta duona su sūrio padažu", 4.50m),
                                                         new Meal(MealTypes.Salotos, "Cezario salotos su vištiena", 8.50m),
                                                         new Meal(MealTypes.Salotos, "Cezario salotos su krevetėmis", 9.50m),
                                                         new Meal(MealTypes.Karšti, "Dienos sriuba", 3.00m),
                                                         new Meal(MealTypes.Karšti, "Šaltibaršciai su bulvemis", 4.00m),
                                                         new Meal(MealTypes.Karšti, "Bulviniai blynai su sūdyta lašiša", 7.50m),
                                                         new Meal(MealTypes.Karšti, "Mėsainis su plėšyta kiauliena", 8.00m),
                                                         new Meal(MealTypes.Karšti, "Mėsainis su vištiena", 7.00m),
                                                         new Meal(MealTypes.Karšti, "Kiaulienos išpjovos kepsneliai", 9.50m),
                                                         new Meal(MealTypes.Karšti, "Vištienos kepsneliai", 8.50m),
                                                         new Meal(MealTypes.Karšti, "Starkio užkepėlė", 10.50m),
                                                         new Meal(MealTypes.Desertai, "Karštas obuolių pyragas su ledais", 4.00m),
                                                         new Meal(MealTypes.Desertai, "Vaniliniai ledai su padažu", 4.00m)};


        private List<Meal> DrinksMenu = new List<Meal>() {new Meal(MealTypes.Gėrimai, "Spanguolių arbata", 2.50m),
                                                           new Meal(MealTypes.Gėrimai, "Kapučinas", 2.50m),
                                                           new Meal(MealTypes.Gėrimai, "Kava su likeriu", 4.80m),
                                                           new Meal(MealTypes.Gėrimai, "Sultys", 2.80m),
                                                           new Meal(MealTypes.Gėrimai, "Alus", 4.50m),
                                                           new Meal(MealTypes.Gėrimai, "Vynas", 2.80m)};

        private void CreateTablesTable()
        {
            using SQLiteConnection ConnectionToDatabase = SqlService.CreateConnection();
            using SQLiteCommand SqlCommand = ConnectionToDatabase.CreateCommand();
            SqlCommand.CommandText = $"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='tables';";
            bool isTableExist = Convert.ToBoolean(SqlCommand.ExecuteScalar());
            if (!isTableExist)
            {
                SqlCommand.CommandText = $"CREATE TABLE tables (" +
                                                        $"TableID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"Seats INTEGER, " +
                                                        $"OccupiedSeats INTEGER DEFAULT 0, " +
                                                        $"isReserved TEXT DEFAULT 'false', " +
                                                        $"isOrderAccepted TEXT DEFAULT 'false');";
                SqlCommand.ExecuteNonQuery();

                foreach (Table table in Tables)
                {
                    SqlCommand.CommandText = $"INSERT INTO Tables (Seats) VALUES ({table.Seats});";
                    SqlCommand.ExecuteNonQuery();
                }
            }
        }

        private void CreateMenuTables()
        {
            string FoodTableName = "menuOfFood";
            CreateMenuTable(FoodTableName, FoodMenu);
            string DrinksTableName = "menuOfDrinks";
            CreateMenuTable(DrinksTableName, DrinksMenu);
        }

        private void CreateMenuTable(string tableName, List<Meal> menu)
        {
            using SQLiteConnection ConnectionToDatabase = SqlService.CreateConnection();
            using SQLiteCommand SqlCommand = ConnectionToDatabase.CreateCommand();
            SqlCommand.CommandText = $"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='{tableName}';";
            bool isTableExist = Convert.ToBoolean(SqlCommand.ExecuteScalar());
            if (!isTableExist)
            {
                SqlCommand.CommandText = $"CREATE TABLE {tableName} (" +
                                                        $"MealID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"MealType TEXT(20), " +
                                                        $"MealName TEXT(100), " +
                                                        $"MealPrice REAL);";
                SqlCommand.ExecuteNonQuery();

                foreach (Meal meal in menu)
                {
                    string PriceAsRealNumber = Converter.ConvertDecimalToReal(meal.MealPrice);
                    string? MealType = Enum.GetName(typeof(MealTypes), meal.MealType);
                    SqlCommand.CommandText = $"INSERT INTO {tableName} (MealType, MealName, MealPrice) VALUES ('{MealType}', '{meal.MealName}', {PriceAsRealNumber});";
                    SqlCommand.ExecuteNonQuery();
                }
            }
        }

        private void CreateAccountingTable()
        {
            using SQLiteConnection ConnectionToDatabase = SqlService.CreateConnection();
            using SQLiteCommand SqlCommand = ConnectionToDatabase.CreateCommand();
            SqlCommand.CommandText = $"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='accounting';";
            bool isTableExist = Convert.ToBoolean(SqlCommand.ExecuteScalar());
            if (!isTableExist)
            {
                SqlCommand.CommandText = $"CREATE TABLE accounting (" +
                                                        $"AccountingID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"Date TEXT(10), " +
                                                        $"Time TEXT(5), " +
                                                        $"Value REAL, " +
                                                        $"ClientEmail TEXT);";
                SqlCommand.ExecuteNonQuery();
            }
        }

        private void CreateClientsDataTable()
        {
            using SQLiteConnection ConnectionToDatabase = SqlService.CreateConnection();
            using SQLiteCommand SqlCommand = ConnectionToDatabase.CreateCommand();
            SqlCommand.CommandText = $"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='clientsData';";
            bool isTableExist = Convert.ToBoolean(SqlCommand.ExecuteScalar());
            if (!isTableExist)
            {
                SqlCommand.CommandText = $"CREATE TABLE clientsData (" +
                                                        $"ClientID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"Mail TEXT);";
                SqlCommand.ExecuteNonQuery();
            }
        }

        private void CreateOrdersTable()
        {
            using SQLiteConnection ConnectionToDatabase = SqlService.CreateConnection();
            using SQLiteCommand SqlCommand = ConnectionToDatabase.CreateCommand();
            SqlCommand.CommandText = $"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='orders';";
            bool isTableExist = Convert.ToBoolean(SqlCommand.ExecuteScalar());
            if (!isTableExist)
            {
                SqlCommand.CommandText = $"CREATE TABLE orders (" +
                                                        $"OrderID INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                                        $"AccountingID INTEGER, " +
                                                        $"TableID INTEGER, " +
                                                        $"MealName TEXT, " +
                                                        $"MealPrice REAL, " +
                                                        $"isPaid TEXT DEFAULT 'false');";
                SqlCommand.ExecuteNonQuery();
            }
        }

        public void CreateDefaultSqlDatabase()
        {
            CreateTablesTable();
            CreateMenuTables();
            CreateAccountingTable();
            CreateOrdersTable();
        }
    }
}
