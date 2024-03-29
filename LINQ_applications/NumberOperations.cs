﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQ_applications
{
    public static class NumberOperations
    {
        public static IEnumerable<IEnumerable<int>> GenerateSubsets(IEnumerable<int> array, int maxSum = int.MinValue)
        {
            ThrowNullException(array);

            int length = array.Count();
            return array.SelectMany((_, startPos) => array.GetSubSet(startPos)
                                    .Select((number, secIndex)
                                    => array.GetSubSet(startPos, length - secIndex - startPos)))
                        .Where(x => CheckSumOfElements(x, maxSum));
        }

        public static IEnumerable<IEnumerable<int>> GetPythagoreanNumbers(IEnumerable<int> array)
        {
            ThrowNullException(array);

            return array.SelectSkip((a, inner) =>
                inner.SelectSkip((b, result) =>
                    result.SelectMany(c => GetTriplePermutations(a, b, c))));
        }

        public static IEnumerable<string> GetSumCombinations(int maxNumber, int numberToCheck)
        {
            return GetAllCombinations(maxNumber, numberToCheck);
        }

        private static int CalculateSum(string source)
        {
            return Enumerable.Range(0, source.Length)
                .Aggregate(0, (a, b) => source[b] == '+' ? a + 1 + b : a - 1 - b);
        }

        private static bool CheckSumOfElements(IEnumerable<int> array, int sum)
        {
            return array.Sum() <= sum;
        }

        private static IEnumerable<string> GetAllCombinations(int maxNumber, int toReach)
        {
            return Enumerable.Range(1, maxNumber)
                 .Aggregate((IEnumerable<string>)new[] { "" }, (x, _) =>
                     x.SelectMany(result => new[] { result + "+", result + "-" }))
               .Where(a => CalculateSum(a) == toReach)
                   .Select(element => string.Concat(element.Select((letter, i) => letter.ToString() + (i + 1)))
                   + string.Concat(" = ", toReach.ToString()));
        }

        private static IEnumerable<int> GetSubSet(this IEnumerable<int> array, int startingPosition, int numbersToTake)
        {
            return GetSubSet(array, startingPosition).Take(numbersToTake);
        }

        private static IEnumerable<int> GetSubSet(this IEnumerable<int> array, int startingPosition)
        {
            return array.Skip(startingPosition);
        }

        private static IEnumerable<IEnumerable<int>> GetTriplePermutations(int first, int second, int third)
        {
            return PermuteElements(first, second, third)
                   .Where(x => IsTriplePythagorean(x.First(), x.ElementAt(1), x.Last()));
        }

        private static bool IsTriplePythagorean(int firstElement, int secondElement, int thirdElement)
        {
            return (firstElement * firstElement) + (secondElement * secondElement)
                     == thirdElement * thirdElement;
        }

        private static IEnumerable<IEnumerable<int>> MakeEnum(int first, int second, int third)
        {
            return new[]
            {
                new[] { first, second, third }, new[] { first, third, second },
                new[] { second, first, third }, new[] { second, third, first },
                new[] { third, first, second }, new[] { third, second, first }
            };
        }

        private static IEnumerable<IEnumerable<int>> PermuteElements(int first, int second, int third)
        {
            return first != second
                ? MakeEnum(first, second, third)
                : new[] { new[] { 0, 0, 0 } };
        }

        private static IEnumerable<T> SelectSkip<T>(
            this IEnumerable<int> source,
            Func<int, IEnumerable<int>, IEnumerable<T>> func)
        {
            return source.SelectMany((a, i) =>
            {
                var inner = source.Skip(i + 1);
                return func(a, inner);
            });
        }

        private static void ThrowNullException(IEnumerable<int> array)
        {
            if (array != null)
            {
                return;
            }

            throw new ArgumentNullException(nameof(array));
        }
    }
}