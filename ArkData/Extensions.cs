using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkData
{
    internal static class Extensions
    {
        static readonly int[] Empty = new int[0];

        public static int LocateFirst(this byte[] self, byte[] candidate, int offset = 0)
        {
            if (IsEmptyLocate(self, candidate, offset))
                return -1;

            for (int i = offset; i < self.Length; i++)
            {
                if (!IsMatch(self, i, candidate))
                    continue;

                return i;
            }

            return -1;
        }

        public static int[] Locate(this byte[] self, byte[] candidate)
        {
            if (IsEmptyLocate(self, candidate, 0))
                return Empty;

            var list = new List<int>();

            for (int i = 0; i < self.Length; i++)
            {
                if (!IsMatch(self, i, candidate))
                    continue;

                list.Add(i);
            }

            return list.Count == 0 ? Empty : list.ToArray();
        }

        private static bool IsMatch(byte[] array, int position, byte[] candidate)
        {
            if (candidate.Length > (array.Length - position))
                return false;

            for (int i = 0; i < candidate.Length; i++)
                if (array[position + i] != candidate[i])
                    return false;

            return true;
        }

        private static bool IsEmptyLocate(byte[] array, byte[] candidate, int offset)
        {
            return array == null
                || candidate == null
                || array.Length == 0
                || candidate.Length == 0
                || candidate.Length > array.Length
                || offset == -1
                || offset > array.Length;
        }

        public static IEnumerable<IEnumerable<TSource>> Split<TSource>(this IEnumerable<TSource> elements, long groupCount)
        {
            long i = 0;

            return elements.GroupBy(e => (long)Math.Floor((decimal)(i++ / groupCount))).Select(g => g.ToArray()).ToArray();
        }

        public static IEnumerable<string> SplitJoin<TSource>(this IEnumerable<TSource> elements, Func<TSource, string> selector, long groupCount)
        {
            return elements.SplitJoin(selector, groupCount, ",");
        }

        public static IEnumerable<string> SplitJoin<TSource>(this IEnumerable<TSource> elements, Func<TSource, string> selector, long groupCount, string separator)
        {
            return elements.Split(groupCount).Select(g => string.Join(separator, g.Select(selector))).ToArray();
        }

        public static async Task<IEnumerable<TResult>> SelectManyAsync<TSource, TResult>(this IEnumerable<TSource> enumeration, Func<TSource, Task<IEnumerable<TResult>>> func)
        {
            return (await Task.WhenAll(enumeration.Select(func))).SelectMany(s => s);
        }
    }
}
