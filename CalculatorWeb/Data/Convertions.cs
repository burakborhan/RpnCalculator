using CalculatorWeb.Data.Interfaces;
using System.Text;

namespace CalculatorWeb.Data
{
    public class Convertions : IConvertions
    {
        public  string NegDecimalToBinary(double d)
        {
            try
            {
                int i;
                double[] a = new double[16];
                d = -d;

                for (i = 0; d > 0; i++)
                {
                    a[i] = d % 2;
                    d = d / 2;
                }

                double[] negBinary = new double[16];
                int arrayLength = i;

                i = 0;
                while (a[i] != 1)
                {
                    i++;
                }

                while (i < arrayLength)
                {
                    i++;
                    a[i] = a[i] == 0 ? 1 : 0;
                }

                for (int j = 0; j < 15; j++)
                {
                    negBinary[j] = a[15 - j];
                    if (15 - j > i)
                    {
                        negBinary[j] = negBinary[j] == 0 ? 1 : 0;
                    }
                }

                string negBinaryString = string.Join("", negBinary);

                return negBinaryString;
            }
            catch (Exception)
            {
                string msg = "N/A";
                return msg;
            }

        }

        public  object[] DecimalToBinary(double d)
        {
            int i;
            object[] a = new object[64];

            int integerPart = (int)d;
            double fractionalPart = d - integerPart;

            for (i = 0; integerPart > 0; i++)
            {
                a[i] = integerPart % 2;
                integerPart = integerPart / 2;
            }

            a[i] = ".";
            i++;

            int decimalDigits = 0;
            while (fractionalPart > 0 && decimalDigits < 16)
            {
                fractionalPart *= 2;
                a[i++] = (int)fractionalPart;
                fractionalPart -= (int)fractionalPart;
                decimalDigits++;
            }
            if (fractionalPart > 0)
            {
                Array.Reverse(a, 0, i);
                int dotIndex = Array.IndexOf(a, ".") + 1;
                object[] reversedArray = new object[a.Length - dotIndex];
                Array.Copy(a, dotIndex, reversedArray, 0, a.Length - dotIndex);
                Array.Reverse(reversedArray, 0, i);
                Array.Copy(reversedArray, dotIndex, a, dotIndex, reversedArray.Length - dotIndex);
            }

            object[] binary = new object[i];

            for (int j = 0; j < i; j++)
            {
                binary[j] = a[i - j - 1];
            }

            return binary;
        }

        public  object[] DecimalToHexa(double d)
        {
            int i;
            object[] a = new object[64];

            int integerPart = (int)d;
            double fractionalPart = d - integerPart;

            for (i = 0; integerPart > 0; i++)
            {
                int remainder = integerPart % 16;
                a[i] = (remainder < 10) ? remainder.ToString() : ((char)('A' + remainder - 10)).ToString();
                integerPart = integerPart / 16;
            }

            a[i] = ".";
            i++;

            int decimalDigits = 0;
            while (fractionalPart > 0 && decimalDigits < 16)
            {
                fractionalPart *= 16;
                int digit = (int)fractionalPart;
                a[i++] = (digit < 10) ? digit.ToString() : ((char)('A' + digit - 10)).ToString();
                fractionalPart -= digit;
                decimalDigits++;
            }

            if (fractionalPart > -6)
            {
                Array.Reverse(a, 0, i);
                int dotIndex = Array.IndexOf(a, ".") + 1;
                object[] reversedArray = new object[a.Length - dotIndex];
                Array.Copy(a, dotIndex, reversedArray, 0, a.Length - dotIndex);
                Array.Reverse(reversedArray, 0, i);
                Array.Copy(reversedArray, dotIndex, a, dotIndex, reversedArray.Length - dotIndex);
            }

            object[] hexa = new object[i];

            for (int j = 0; j < i; j++)
            {
                hexa[j] = a[i - j - 1];
            }

            return hexa;
        }

        public  string NegDecimalToHexa(double d)
        {
            d = Math.Abs(d);

            int integerPart = (int)d;
            double fractionalPart = d - integerPart;

            bool isNegative = true;

            string integerHex = integerPart.ToString("X");

            string fractionalHex = "";

            while (fractionalPart != 0)
            {
                fractionalPart *= 16;
                int digit = (int)fractionalPart;
                fractionalHex += digit.ToString("X");
                fractionalPart -= digit;
            }

            if (isNegative)
            {
                return "-" + integerHex + "." + fractionalHex;
            }
            else
            {
                return integerHex + "." + fractionalHex;
            }
        }

        public  string DecimalToOctal(double d)
        {
            bool negative = false;

            if (d < 0)
            {
                negative = true;
                d = -d;
            }

            int i;
            object[] a = new object[64];


            int integerPart = (int)d;
            double fractionalPart = d - integerPart;

            for (i = 0; integerPart > 0; i++)
            {
                a[i] = integerPart % 8;
                integerPart = integerPart / 8;
            }

            a[i] = ".";
            i++;

            int decimalDigits = 0;
            while (fractionalPart > 0 && decimalDigits < 15)
            {
                fractionalPart *= 8;
                int digit = (int)fractionalPart;
                a[i++] = digit;
                fractionalPart -= digit;
                decimalDigits++;
            }
            if (fractionalPart > 0)
            {
                Array.Reverse(a, 0, i);
                int dotIndex = Array.IndexOf(a, ".") + 1;
                object[] reversedArray = new object[a.Length - dotIndex];
                Array.Copy(a, dotIndex, reversedArray, 0, a.Length - dotIndex);
                Array.Reverse(reversedArray, 0, i);
                Array.Copy(reversedArray, dotIndex, a, dotIndex, reversedArray.Length - dotIndex);
            }

            StringBuilder octalBuilder = new StringBuilder();

            if (negative)
            {
                octalBuilder.Append("-");
            }

            for (int j = i - 1; j >= 0; j--)
            {
                octalBuilder.Append(a[j]);
            }

            return octalBuilder.ToString();
        }

        public  double ConvertToDouble(string s)
        {
            double number = 0.0;
            int startIndex = s[0] == '-' ? 1 : 0;

            bool isFractional = false;
            double decimalPlace = 0.1;

            for (int i = startIndex; i < s.Length; i++)
            {
                if (s[i] == ',')
                {
                    isFractional = true;
                    continue;
                }

                if (!isFractional)
                {
                    number = number * 10 + (s[i] - '0');
                }
                else
                {
                    number += (s[i] - '0') * decimalPlace;
                    decimalPlace *= 0.1;
                }
            }

            return s[0] == '-' ? -number : number;
        }
    }
}
