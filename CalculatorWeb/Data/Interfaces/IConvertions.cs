namespace CalculatorWeb.Data.Interfaces
{
    public interface IConvertions
    {
        string NegDecimalToBinary(double d);
        object[] DecimalToBinary(double d);
        object[] DecimalToHexa(double d);
        string NegDecimalToHexa(double d);
        string DecimalToOctal(double d);
        double ConvertToDouble(string s);
    }
}
