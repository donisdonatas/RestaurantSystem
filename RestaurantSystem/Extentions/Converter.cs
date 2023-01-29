namespace RestaurantSystem.Extentions
{
    public static class Converter
    {
        public static string ConvertDecimalToReal(decimal num)
        {
            return Convert.ToString(num).Replace(",", ".").Replace("m", "");
        }

        public static bool ConvertToBool(string boolString)
        {
            return boolString == "true" ? true : false;
        }

        public static bool ConvertToBool(int answer, int valForTrue)
        {
            return answer == valForTrue;
        }
    }
}
