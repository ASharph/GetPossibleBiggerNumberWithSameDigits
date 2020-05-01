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

        private static long NextNumber(long number, int currentIndex = -1, int previousIndex = -1, List<int> pastNumbers = null)
        {
            // When there is no more numbers ahead.
            if (previousIndex < 0 && currentIndex == 0)
            {
                return number;
            }

            var numbers = number
                .ToString()
                .ToCharArray()
                .Select(d => (int)char.GetNumericValue(d))
                .ToList();

            currentIndex = currentIndex == -1 ? numbers.Count - 1 : currentIndex;
            previousIndex = previousIndex == -1 ? currentIndex - 1 : previousIndex;

            var currentDigit = numbers[currentIndex];
            var previousDigit = numbers[previousIndex];

            pastNumbers ??= new List<int>();

            if(currentDigit > previousDigit)
            {
                pastNumbers.Add(currentDigit);
                currentDigit = GetMinNumberAndOrderPreviousNumbers(previousDigit, ref pastNumbers) ?? currentDigit;

                // Move current digit ahead.
                numbers.RemoveAt(currentIndex);
                numbers.Insert(previousIndex, currentDigit);

                // Remove unordered end with ordered one.
                if(pastNumbers.Count > 1)
                {
                    numbers.RemoveRange(currentIndex, pastNumbers.Count);
                    numbers.AddRange(pastNumbers);
                }

                var updatedNumber = GetLong(numbers);
                if(updatedNumber > number)
                {
                    return updatedNumber;
                }

                pastNumbers.Add(currentDigit);

                return NextNumber(number, --currentIndex, --previousIndex, pastNumbers);
            }

            pastNumbers.Add(currentDigit);

            return NextNumber(number, --currentIndex, --previousIndex, pastNumbers);
        }

        private static long GetLong(IList<int> numbers)
        {
            var stringBuilder = new StringBuilder();
            foreach (var number in numbers)
            {
                stringBuilder.Append(number);
            }

            return long.Parse(stringBuilder.ToString());
        }

        private static int? GetMinNumberAndOrderPreviousNumbers(int previousNumber, ref List<int> numbers)
        {
            if(numbers.Count > 1)
            {
                var minSequence = numbers.Where(d => d > previousNumber).ToList();                

                if(minSequence.Count > 0)
                {
                    var minNumber = minSequence.Min();

                    numbers.Add(previousNumber);
                    numbers.Remove(minNumber);
                    numbers = numbers.OrderBy(d => d).ToList();

                    return minNumber;
                }

                numbers = numbers.OrderBy(d => d).ToList();
            }

            return null;
        }
    }
}
