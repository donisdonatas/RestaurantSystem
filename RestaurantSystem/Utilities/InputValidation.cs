using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantSystem.Utilities
{
    public static class InputValidation
    {
        public static int ValidateInput()
        {
            int Number = -1;
            bool isImputValid = false;
            while (!isImputValid)
            {
                Exception? err = null;
                try
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Number = int.Parse(Console.ReadLine());
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                    err = ex;
                }
                catch (NullReferenceException ex)
                {
                    Console.WriteLine(ex.Message);
                    err = ex;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    err = ex;
                }
                finally
                {
                    if (err == null && Number >= 0)
                    {
                        isImputValid = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Įvesta netinkama reikšmė. Bandykite dar kartą.");
                    }
                }
            }
            return Number;
        }

        public static int ValidateInput(int maxValue)
        {
            int Number = -1;
            bool isImputValid = false;
            while (!isImputValid)
            {
                Exception? err = null;
                try
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Number = int.Parse(Console.ReadLine());
                }
                catch (FormatException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    err = ex;
                }
                catch (NullReferenceException ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    err = ex;
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    err = ex;
                }
                finally
                {
                    if (err == null && Number >= 0 && Number <= maxValue)
                    {
                        isImputValid = true;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Blogai įvestas pasirinkimas. Bandykite dar kartą.");
                    }
                }
            }
            return Number;
        }

        public static object[] ConvertListStringToListInt(List<string> strLst)
        {
            List<int> MenuOrder = new List<int>();
            List<string> FailedInputs = new List<string>();
            foreach (string str in strLst)
            {
                IEnumerable<string> strArr = str.Split(",").Select(s => s.Trim());
                foreach (string str2 in strArr)
                {
                    int MenuID;
                    bool success = int.TryParse(str2, out MenuID);
                    if (success && MenuID <= 21) // TODO: Čia 21 turėtų būti pakeistas į eilučių skaičių esančių meniu.
                    {
                        MenuOrder.Add(MenuID);
                    }
                    else
                    {
                        FailedInputs.Add(str2);
                    }
                }
            }
            object[] ConvertResults = new object[2] { MenuOrder, FailedInputs };
            return ConvertResults;
        }
    }
}
