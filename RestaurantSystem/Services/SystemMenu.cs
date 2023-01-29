using RestaurantSystem.Utilities;

namespace RestaurantSystem.Services
{
    public class SystemMenu
    {
        public void GetPrimaryMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Sveiki, tai Restorano valdymo sistema.");
            Console.WriteLine("Galimi veiksmai:");

            Console.WriteLine("[1] Klientų priėmimas / staliuko rezervavimas");
            Console.WriteLine("[2] Užsakymo priėmimas");
            Console.WriteLine("[3] Atsiskaitymo priėmimas");
            Console.WriteLine("[0] Išeiti iš programos");

            int MenuChoice = InputValidation.ValidateInput(3);
            OpenMenuByPrimaryChoice(MenuChoice);
        }

        public void OpenMenuByPrimaryChoice(int choise)
        {
            while (true)
            {
                Console.Clear();
                switch (choise)
                {
                    case 1:
                        CustomerReceptionService OrderTable = new CustomerReceptionService();
                        OrderTable.InitializeReception();
                        break;
                    case 2:
                        CustomerOrderService Order = new CustomerOrderService();
                        Order.InitializeOrder();
                        break;
                    case 3:
                        CustomerCheckoutService Checkout = new CustomerCheckoutService();
                        Checkout.InitializeCheckout();
                        break;
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("Viso gero.");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Sistemos klaida: SystemMenu");
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public void BackToMainMenu()
        {
            GetPrimaryMenu();
        }
    }
}
