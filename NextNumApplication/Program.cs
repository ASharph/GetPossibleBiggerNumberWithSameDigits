using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NextNumApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            long l = 18603;

            for (var i = 0; i < 25; i++)
            {
                l = NextNumber(l);
                Console.WriteLine(l);
            }
        }

        private static long NextNumber(long number, int currentIndex = -1, int previousIndex = -1, List<byte> pastDigits = null)
        {
            // When there is no more digits ahead.
            if (previousIndex < 0 && currentIndex == 0)
            {
                return number;
            }

            var digits = number
                .ToString()
                .ToCharArray()
                .Select(d => (byte)char.GetNumericValue(d))
                .ToList();

            currentIndex = currentIndex == -1 ? digits.Count - 1 : currentIndex;
            previousIndex = previousIndex == -1 ? currentIndex - 1 : previousIndex;

            var currentDigit = digits[currentIndex];
            var previousDigit = digits[previousIndex];

            pastDigits ??= new List<byte>();

            if(currentDigit > previousDigit)
            {
                pastDigits.Add(currentDigit);
                currentDigit = GetMinDigitAndOrderPreviousDigits(previousDigit, ref pastDigits) ?? currentDigit;

                // Move current digit ahead.
                digits.RemoveAt(currentIndex);
                digits.Insert(previousIndex, currentDigit);

                // Replace unordered end with ordered one.
                if(pastDigits.Count > 1)
                {                    
                    digits.RemoveRange(currentIndex, pastDigits.Count);
                    digits.AddRange(pastDigits);
                }

                var updatedNumber = GetLong(digits);
                if(updatedNumber > number)
                {
                    return updatedNumber;
                }

                pastDigits.Add(currentDigit);

                return NextNumber(number, --currentIndex, --previousIndex, pastDigits);
            }

            pastDigits.Add(currentDigit);

            return NextNumber(number, --currentIndex, --previousIndex, pastDigits);
        }

        private static long GetLong(IList<byte> numbers)
        {
            var stringBuilder = new StringBuilder();
            foreach (var number in numbers)
            {
                stringBuilder.Append(number);
            }

            return long.Parse(stringBuilder.ToString());
        }

        private static byte? GetMinDigitAndOrderPreviousDigits(byte previousNumber, ref List<byte> digits)
        {
            if(digits.Count > 1)
            {
                var minSequence = digits.Where(d => d > previousNumber).ToList();                

                if(minSequence.Count > 0)
                {
                    var minNumber = minSequence.Min();

                    digits.Add(previousNumber);
                    digits.Remove(minNumber);
                    digits = digits.OrderBy(d => d).ToList();

                    return minNumber;
                }

                digits = digits.OrderBy(d => d).ToList();
            }

            return null;
        }
    }
}
