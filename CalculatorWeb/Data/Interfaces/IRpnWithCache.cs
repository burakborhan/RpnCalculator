namespace CalculatorWeb.Data.Interfaces
{
    public interface IRpnWithCache
    {
        object CalculateRpmWithCache(string s);
        IEnumerable<KeyValuePair<object, object>> GetAllCachedItems();
    }
}
