using RestaurantSystem.Utilities;
using RestaurantSystem.Strukts;
using RestaurantSystem.Extentions;
using System.Net.Mail;
using RestaurantSystem.Reports;

namespace RestaurantSystem.Services
{
    public class CustomerCheckoutService
    {
        private BillInfo _billInfo = new BillInfo();
        public void InitializeCheckout()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Atsiskaitymo priėmimas");
            Console.WriteLine("Pasirinkite staliuką užsakymo atsiskaitymui:");
            int TableId = SelectOrderedTable();
            if (TableId != 0)
            {
                CompleteCheckout(TableId);
            }
            Console.ReadLine();
            SystemMenu Menu = new SystemMenu();
            Menu.BackToMainMenu();
        }

        private int SelectOrderedTable()
        {
            List<sTable> Tables = SqlService.RetrieveTableList();
            IEnumerable<sTable> OrderedTables = Tables.Where(table => table.isOrdered);
            if (OrderedTables.Any())
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine("-----------------");
                int KeyboardKey = 0;
                foreach (sTable table in OrderedTables)
                {
                    ++KeyboardKey;
                    Console.WriteLine($"[{KeyboardKey}] Staliukas Nr. {table.TableID}. Laukia {table.OccupiedSeats} {(table.OccupiedSeats > 1 ? " žmonės" : " žmogus")}.");
                }
                Console.WriteLine("-----------------");
                int Input = InputValidation.ValidateInput(OrderedTables.Count());
                _billInfo.TableId = OrderedTables.ElementAt(Input - 1).TableID;
                _billInfo.Seats = OrderedTables.ElementAt(Input - 1).Seats;
                _billInfo.OccupiedSeats = OrderedTables.ElementAt(Input - 1).OccupiedSeats;
                return OrderedTables.ElementAt(Input - 1).TableID;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("Nėra staliukų laukiančių atsiskaitymo");
                return 0;
            }
        }

        private void CompleteCheckout(int tableId)
        {
            _billInfo.Time = DateTime.Now.ToString("HH:mm");
            _billInfo.TotalValue = GetTotalPaidValue(tableId);
            AccountingInfo AccInfo = GetAccountingInfo(tableId);
            _billInfo.AccountingId = AccInfo.AccountingId;
            _billInfo.Date = AccInfo.AccountingDate;
            string OrdersUpdateString = $"UPDATE orders SET isPaid='true' WHERE TableID={tableId} AND isPaid='false';";
            string TablesUpdateString = $"UPDATE tables SET isReserved='false', OccupiedSeats=0, isOrderAccepted='false' WHERE TableID={tableId};";
            string AccountingUpdateString = $"UPDATE accounting SET Time='{_billInfo.Time}', " +
                                            $"Value={Converter.ConvertDecimalToReal(_billInfo.TotalValue)}, " +
                                            $"ClientEmail='{(IsCheckNeed() ? AskMailInfo() : '-')}' " +
                                            $"WHERE AccountingID={_billInfo.AccountingId};";
            SqlService.UpdateSqlTable(OrdersUpdateString);
            SqlService.UpdateSqlTable(TablesUpdateString);
            SqlService.UpdateSqlTable(AccountingUpdateString);
        }

        private decimal GetTotalPaidValue(int tableId)
        {
            List<MenuItem> Order = SqlService.GetOrderBillList(tableId);
            _billInfo.MenuItems = Order;
            decimal TotalPaid = 0.00m;
            foreach (MenuItem item in Order)
            {
                TotalPaid += item.MealPrice;
            }
            return TotalPaid;
        }

        private bool IsCheckNeed()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ar reikia atsiųsti sąskaitos išrašą?");
            Console.WriteLine("[1] Taip");
            Console.WriteLine("[2] Ne");
            int KeyboardKey = InputValidation.ValidateInput(2);
            return OpenMenuByChoice(KeyboardKey);
        }

        private bool OpenMenuByChoice(int key)
        {
            switch(key)
            {
                case 1:
                    return true;
                case 2:
                    return false;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Sistemos klaida: GetClientEmail");
                    return false;
            }
        }

        private MailAddress? AskMailInfo()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Įveskite email:");
            MailAddress? ClientEmail = null;
            bool isValidEmail = false;
            Exception? err = null;
            while (!isValidEmail)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    ClientEmail = new MailAddress(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    err = ex;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Neteisingai įvestas el. pašto adresas. Pakartokite.");
                }
                finally
                {
                    if (err == null)
                    {
                        isValidEmail = true;
                        SendBillByMail(ClientEmail);
                    }
                }
            }
            return ClientEmail;
        }

        private AccountingInfo GetAccountingInfo(int tableId)
        {
            AccountingInfo Accounting = SqlService.GetOrderAccountingID(tableId);
            return Accounting;
        }

        private void SendBillByMail(MailAddress email)
        {
            HtmlBill GenerateHtmlBill = new HtmlBill();
            EmailService SendMail = new EmailService();
            SendMail.SendEmail(GenerateHtmlBill.generateHTMLraport(_billInfo), email);
        }
    }
}
