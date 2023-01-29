using RestaurantSystem.Utilities;
using RestaurantSystem.Strukts;
using RestaurantSystem.Extentions;

namespace RestaurantSystem.Services
{
    public class CustomerOrderService
    {
        public List<int> Order = new List<int>();

        public void InitializeOrder()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Užsakymų priėmimas");
            Console.WriteLine("Pasirinkite staliuką užsakymo priėmimui:");
            int TableId = SelectReservedTables();
            if (TableId != 0)
            {
                GiveCustomersMenu();
                CompleteOrder(TableId);
            }
            Console.ReadLine();
            SystemMenu Menu = new SystemMenu();
            Menu.BackToMainMenu();
        }

        private int SelectReservedTables()
        {
            List<sTable> Tables = SqlService.RetrieveTableList();
            IEnumerable<sTable> ReservedTables = Tables.Where(table => table.isReserved && !table.isOrdered);
            if (ReservedTables.Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("-----------------");
                int KeyboardKey = 0;
                foreach (sTable table in ReservedTables)
                {
                    ++KeyboardKey;
                    Console.WriteLine($"[{KeyboardKey}] Staliukas Nr. {table.TableID}. Laukia {table.OccupiedSeats} {(table.OccupiedSeats > 1 ? " žmonės" : " žmogus")}.");
                }
                Console.WriteLine("-----------------");
                int Input = InputValidation.ValidateInput(ReservedTables.Count());
                return ReservedTables.ElementAt(Input - 1).TableID;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Nėra staliukų laukiančių užsakymo priėmimo");
                return 0;
            }
        }

        private void GiveCustomersMenu()
        {
            List<MenuItem> Menu = GetMenu();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("MENU");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("-----------------");
            int NumericKey = 0;
            string MealType = "";
            foreach (MenuItem menuLine in Menu)
            {
                if (MealType != menuLine.MealType)
                {
                    MealType = menuLine.MealType;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{menuLine.MealType}:");
                    ++NumericKey;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"[{NumericKey}] {menuLine.MealName} - {menuLine.MealPrice}");
                }
                else
                {
                    ++NumericKey;
                    Console.WriteLine($"[{NumericKey}] {menuLine.MealName} - {menuLine.MealPrice}");
                }
            }
            Console.WriteLine("-----------------");
            SelectMenuItems();
        }

        public List<MenuItem> GetMenu()
        {
            List<MenuItem> Menu = new List<MenuItem>();

            string FoodSqlString = $"SELECT * FROM menuOfFood;";
            List<MenuItem> FoodMenu = SqlService.RetreveMenu(FoodSqlString);
            Menu.AddRange(FoodMenu);

            string DrinksSqlString = $"SELECT * FROM menuOfDrinks;";
            List<MenuItem> DrinksMenu = SqlService.RetreveMenu(DrinksSqlString);
            Menu.AddRange(DrinksMenu);
           
            return Menu;
        }

        private void SelectMenuItems()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sudarykite užsakymą iš pateikto meniu:");
            Console.WriteLine("Užsakymą, galite vesti į vieną eitutę, reikšmes atskirant kableliu \",\":");
            string? InputLine;
            bool IsOrderComplete = false;
            List<string> OrderList = new List<string>();
            while (!IsOrderComplete)
            {
                Console.ForegroundColor = ConsoleColor.White;
                InputLine = Console.ReadLine();
                OrderList.Add(InputLine);
                object[] FullOrder = InputValidation.ConvertListStringToListInt(OrderList);
                ValidateOrder(FullOrder);
                IsOrderComplete = !AddAnotherLine();
            }
        }

        protected void ValidateOrder(object[] Lists)
        {
            List<int> GoodList = (List<int>)Lists[0];
            List<string> FailedList = (List<string>)Lists[1];
            Order.AddRange(GoodList);
            if (FailedList.Any())
            {
                foreach (string str in FailedList)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Blogai įvesta reikšmė: {str}");
                }
            }
        }

        private bool AddAnotherLine()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ar norite papildyti užsakymą?");
            Console.WriteLine("[1] Taip");
            Console.WriteLine("[2] Ne");
            int NumericKey = InputValidation.ValidateInput(2);
            return Converter.ConvertToBool(NumericKey, 1);
        }

        private void CompleteOrder(int tableId)
        {
            SqlService.WriteOrderToSql(tableId, GetOrderItems());
            string AcceptOrder = $"UPDATE tables SET isOrderAccepted='true' WHERE TableID={tableId};";
            SqlService.UpdateSqlTable(AcceptOrder);
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Užsakymas priimtas.");
        }

        private List<MenuItem> GetOrderItems()
        {
            List<MenuItem> OrderItems = new List<MenuItem>();
            List<MenuItem> Menu = GetMenu();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Užsakymas:");
            for (var i = 0; i < Order.Count; i++)
            {
                MenuItem OrderItem = new MenuItem();
                OrderItem.MealName = Menu[Order[i] - 1].MealName;
                OrderItem.MealPrice = Menu[Order[i] - 1].MealPrice;
                OrderItems.Add(OrderItem);
                Console.WriteLine($"{OrderItem.MealName} - {OrderItem.MealPrice}Eur");
            }
            return OrderItems;
        }
    }
}
