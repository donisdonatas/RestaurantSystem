using RestaurantSystem.Strukts;
using RestaurantSystem.Utilities;

namespace RestaurantSystem.Services
{
    public class CustomerReceptionService
    {
        public void InitializeReception()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Klientų priėmimas / staliuko rezervavimas");
            Console.WriteLine("Ar norite priimti klientus?");
            Console.WriteLine("[1] Taip. Staliuko rezervavimas.");
            Console.WriteLine("[2] Ne. Staliuko atlaisvinimas.");
            Console.WriteLine("[3] Grįžti į pagrindinį meniu.");

            int MenuChoice = InputValidation.ValidateInput(3);
            OpenMenuByChoice(MenuChoice);
        }

        private void OpenMenuByChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    StartTableReservation();
                    break;
                case 2:
                    CancelReservation();
                    break;
                case 3:
                    SystemMenu MainMenu = new SystemMenu();
                    MainMenu.GetPrimaryMenu();
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sistemos klaida: CustomerReceptionService");
                    break;
            }
        }

        private void StartTableReservation()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Įveskite žmonių skaičių:");
            int NumberOfClients = InputValidation.ValidateInput();

            IEnumerable<sTable> AvailableTables = GetAvailableTables();
            sTable Table = ChooseTable(NumberOfClients, AvailableTables);
            CreateTableReservation(Table, NumberOfClients);
        }

        private IEnumerable<sTable> GetAvailableTables()
        {
            List<sTable> Tables = SqlService.RetrieveTableList();
            IEnumerable<sTable> AvailableTables = Tables.Where(table => !table.isReserved);
            return AvailableTables;
        }

        private sTable ChooseTable(int numberOfCustomers, IEnumerable<sTable> availableTables)
        {
            IEnumerable<sTable> TablesForOrder = availableTables.Where(availableSeats => availableSeats.Seats - numberOfCustomers >= 0)
                                                                .OrderBy(availableSeats => availableSeats.Seats - numberOfCustomers >= 0);
            return TablesForOrder.FirstOrDefault();
        }

        private void CreateTableReservation(sTable table, int customers)
        {
            if (table.TableID != 0)
            {
                string SqlString = $"UPDATE tables SET isReserved='true', OccupiedSeats={customers} WHERE TableID={table.TableID};";
                SqlService.UpdateSqlTable(SqlString);
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Rezervuoto staliuko numeris: {table.TableID} ({table.Seats} vietos).");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Nėra laisvo staliuko, tokiam žmonių kiekiui.");
            }
            Console.ReadLine();
        }

        private void CancelReservation()
        {
            List<sTable> Tables = SqlService.RetrieveTableList();
            IEnumerable<sTable> ReservedTables = Tables.Where(table => table.isReserved);
            if (ReservedTables.Any())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Pasirinkite iš sąrašo, kurį staliuką norite atrezervuoti:");
                int KeyboardKey = 0;
                foreach (sTable table in ReservedTables)
                {
                    ++KeyboardKey;
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"[{KeyboardKey}] Staliuko numeris: {table.TableID} ({table.OccupiedSeats} {(table.OccupiedSeats > 1 ? " žmonės" : " žmogus")})");
                }
                int Input = InputValidation.ValidateInput(ReservedTables.Count());
                string SqlString = $"UPDATE tables SET isReserved='false', OccupiedSeats=0 WHERE TableID={ReservedTables.ElementAt(Input - 1).TableID};";
                SqlService.UpdateSqlTable(SqlString);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Staliukas Nr. {ReservedTables.ElementAt(Input - 1).TableID} atrezervuotas.");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Nėra galimų atlaisvinti staliukų.");
            }
            Console.ReadLine();
        }
    }
}
