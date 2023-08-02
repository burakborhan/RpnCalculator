using CalculatorWeb.Data;
using CalculatorWeb.Data.Interfaces;

namespace CalculatorWeb.Data
{
    public class RpnCalculate : IRpnCalculate
    {
        private readonly IConvertions _convertions;

        public RpnCalculate(IConvertions convertions)
        {
            _convertions = convertions;
        }

        public double CalculateExpression(string expression)
        {
            if (string.IsNullOrEmpty(expression) || !expression.Any(char.IsDigit))
                return 0.0;

            int index = 0;
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();

            expression = expression.Trim().Replace(" ", string.Empty);

            for (int i = 0; i < expression.Length; i++)
            {
                if (expression[i] >= '0' && expression[i] <= '9' || expression[i] == ',')
                    continue;

                if (expression[i] == '(')
                {
                    operatorStack.Push(expression[i].ToString());
                }
                else if (expression[i] == ')')
                {
                    if (i != index)
                    {
                        string operand = expression.Substring(index, i - index);
                        double number = _convertions.ConvertToDouble(operand);
                        operandStack.Push(number);
                    }

                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                    {
                        double operand2 = operandStack.Pop();
                        double operand1 = operandStack.Pop();
                        string operatorSymbol = operatorStack.Pop();
                        double result = PerformOperation(operand1, operand2, operatorSymbol);
                        operandStack.Push(result);
                    }
                    operatorStack.Pop();
                }
                else if (expression[i] == '^' || expression[i] == '*' || expression[i] == '/' || expression[i] == '+' || expression[i] == '-')
                {
                    if (i != index)
                    {
                        string operand = expression.Substring(index, i - index);
                        double number = _convertions.ConvertToDouble(operand);
                        operandStack.Push(number);
                    }

                    while (operatorStack.Count > 0 && operatorStack.Peek() != "(" && operatorPrecedence[expression[i]] <= operatorPrecedence[operatorStack.Peek()[0]])
                    {
                        double operand2 = operandStack.Pop();
                        double operand1 = operandStack.Pop();
                        string operatorSymbol = operatorStack.Pop();
                        double result = PerformOperation(operand1, operand2, operatorSymbol);
                        operandStack.Push(result);
                    }

                    operatorStack.Push(expression[i].ToString());
                }

                index = i + 1;
            }

            if (expression.Length != index)
            {
                string operand = expression.Substring(index, expression.Length - index);
                double number = _convertions.ConvertToDouble(operand);
                operandStack.Push(number);
            }

            while (operatorStack.Count > 0)
            {
                double operand2 = operandStack.Pop();
                double operand1 = operandStack.Pop();
                string operatorSymbol = operatorStack.Pop();
                double result = PerformOperation(operand1, operand2, operatorSymbol);
                operandStack.Push(result);
            }

            return operandStack.Peek();
        }
        private static Dictionary<char, int> operatorPrecedence = new Dictionary<char, int>()
        {
            { '^', 3 },
            { '*', 2 },
            { '/', 2 },
            { '+', 1 },
            { '-', 1 }
        };
        private static double PerformOperation(double operand1, double operand2, string operatorSymbol)
        {
            switch (operatorSymbol)
            {
                case "^":
                    return Math.Pow(operand1, operand2);
                case "*":
                    return operand1 * operand2;
                case "/":
                    return operand1 / operand2;
                case "+":
                    return operand1 + operand2;
                case "-":
                    return operand1 - operand2;
                default:
                    throw new ArgumentException("Invalid operator symbol: " + operatorSymbol);
            }
        }
    }
}
