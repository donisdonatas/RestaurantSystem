namespace RestaurantSystem.Extentions
{
    public static class Converter
    {
        public static string ConvertDecimalToReal(decimal num)
        {
            return Convert.ToString(num).Replace(",", ".").Replace("m", "");
        }
    }
}
