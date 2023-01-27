using RestaurantSystem.Services;

namespace RestaurantSystem
{
    internal class Program
    {
        static void Main()
        {
            DefaultSqlService DefaultDatabase = new DefaultSqlService();
            DefaultDatabase.CreateDefaultSqlDatabase();

            SystemMenu MainMenu = new SystemMenu();
            MainMenu.GetPrimaryMenu();
        }
    }
}