namespace Hw1;

public static class Parser
{
    public static void ParseCalcArguments(string[] args,
        out double val1,
        out CalculatorOperation operation,
        out double val2)
    {
        if (!IsArgLengthSupported(args)) throw new ArgumentException("Incorrect data. Length more than 3.");
        if (!double.TryParse(args[0], out val1)) throw new ArgumentException("Incorrect first argument!");
        if (!double.TryParse(args[2], out val2)) throw new ArgumentException("Incorrect second argument!");
        operation = ParseOperation(args[1]);
    }

    private static bool IsArgLengthSupported(string[] args) => args.Length == 3;

    private static CalculatorOperation ParseOperation(string arg)
    {
        return arg switch
        {
            "+" => CalculatorOperation.Plus,
            "-" => CalculatorOperation.Minus,
            "*" => CalculatorOperation.Multiply,
            "/" => CalculatorOperation.Divide,
            _ => throw new InvalidOperationException("Incorrect operation!")
        };
    }
}