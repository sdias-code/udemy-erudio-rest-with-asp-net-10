namespace RestWithAspNet10_Scaffold.Utils
{
    public static class NumberUtils
    { 

        public static decimal? ConvertToDecimal(string strNumber)
        {
            if (decimal.TryParse(
                strNumber,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out decimal value))
            {
                return value;
            }

            return null;
        }
    }
}
