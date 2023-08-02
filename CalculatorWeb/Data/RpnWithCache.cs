using CalculatorWeb.Data.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;

namespace CalculatorWeb.Data
{
    public class RpnWithCache : IRpnWithCache
    {
        private readonly IMemoryCache _Cache;
        private readonly IRpnCalculate _calculate;
        private readonly IConvertions _convertions;
        public RpnWithCache(IMemoryCache memoryCache, IRpnCalculate calculate, IConvertions convertions)
        {
            _Cache = memoryCache;
            _calculate = calculate;
            _convertions = convertions;
        }
        public object CalculateRpmWithCache(string s)
        {
            string cacheKey = $"{s} ";

            if (_Cache.TryGetValue(cacheKey, out object cachedResult))
            {
                return cachedResult;
            }

            double result = _calculate.CalculateExpression(s);

            if (result >= 0)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _Cache.Set($"{cacheKey}Decimal", result, cacheEntryOptions);

                object[] result2 = _convertions.DecimalToBinary(result);
                string stringResult2 = string.Join("", result2);
                _Cache.Set($"{cacheKey}Binary", stringResult2, cacheEntryOptions);

                object[] result3 = _convertions.DecimalToHexa(result);
                string stringResult3 = string.Join("", result3);
                _Cache.Set($"{cacheKey}Hexadecimal", stringResult3, cacheEntryOptions);

                string result4 = _convertions.DecimalToOctal(result);
                _Cache.Set($"{cacheKey}Octal", result4, cacheEntryOptions);

                return new
                {
                    Result = result,
                    Result2 = stringResult2.ToString(),
                    Result3 = stringResult3.ToString(),
                    Result4 = result4,
                };
            }
            else
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                _Cache.Set($"{cacheKey}Decimal", result, cacheEntryOptions);

                string result2 = _convertions.NegDecimalToBinary(result);
                string stringResult2 = string.Join("", result2);
                _Cache.Set($"{cacheKey}Binary", stringResult2, cacheEntryOptions);

                string result3 = _convertions.NegDecimalToHexa(result);
                string stringResult3 = string.Join("", result3);
                _Cache.Set($"{cacheKey}Hexadecimal", stringResult3, cacheEntryOptions);

                string result4 = _convertions.DecimalToOctal(result);
                _Cache.Set($"{cacheKey}Octal", result4, cacheEntryOptions);

                return new
                {
                    Result = result,
                    Result2 = stringResult2.ToString(),
                    Result3 = stringResult3.ToString(),
                    Result4 = result4,
                };
            }
        }

        public IEnumerable<KeyValuePair<object, object>> GetAllCachedItems()
        {
            var cacheEntriesCollectionDefinition = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
            var cacheEntriesCollection = cacheEntriesCollectionDefinition.GetValue(_Cache) as dynamic;

            foreach (var cacheItem in cacheEntriesCollection)
            {
                var cacheItemValue = cacheItem.GetType().GetProperty("Value").GetValue(cacheItem);
                var key = cacheItemValue.GetType().GetProperty("Key").GetValue(cacheItemValue).ToString();
                var value = cacheItemValue.GetType().GetProperty("Value").GetValue(cacheItemValue);

                yield return new KeyValuePair<object, object>(key, value);
            }
        }
    }
}
