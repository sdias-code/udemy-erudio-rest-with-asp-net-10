using RestWithAspNet10_Scaffold.Utils;

namespace RestWithAspNet10_Scaffold.Services
{
    public class NumberService
    {
        private bool TryGetTwoNumbers(string a, string b, out decimal n1, out decimal n2)
        {
            n1 = 0;
            n2 = 0;

            var number1 = NumberUtils.ConvertToDecimal(a);
            var number2 = NumberUtils.ConvertToDecimal(b);

            if (!number1.HasValue || !number2.HasValue)
                return false;

            n1 = number1.Value;
            n2 = number2.Value;
            return true;
        }

        public decimal? Sum(string a, string b)
            => TryGetTwoNumbers(a, b, out var n1, out var n2) ? n1 + n2 : null;


        public decimal? Subtract(string firstNumber, string secondNumber)
        {
            if (TryGetTwoNumbers(firstNumber, secondNumber, out var n1, out var n2))
                return n1 - n2;

            return null;
        }

        public decimal? Multiply(string firstNumber, string secondNumber)
        {
            if (TryGetTwoNumbers(firstNumber, secondNumber, out var n1, out var n2))
                return n1 * n2;

            return null;
        }

        public decimal? Divide(string firstNumber, string secondNumber)
        {
            if (TryGetTwoNumbers(firstNumber, secondNumber, out var n1, out var n2) && n2 != 0)
                return n1 / n2;

            return null;
        }

        public decimal? Mean(string firstNumber, string secondNumber)
        {
            if (TryGetTwoNumbers(firstNumber, secondNumber, out var n1, out var n2))
                return (n1 + n2) / 2;

            return null;
        }

        public decimal? SquareRoot(string number)
        {
            var num = NumberUtils.ConvertToDecimal(number);

            if (num <= 0) throw new System.Exception("Cannot calculate square root of negative numbers");

            if (num.HasValue && num.Value >= 0)
                return (decimal?)Math.Sqrt((double)num.Value);
            return null;
        }

    }
}
