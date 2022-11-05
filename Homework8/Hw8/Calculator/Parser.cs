using System.Globalization;

namespace Hw8.Calculator;

public class Parser : IParser
{
    public Result<Value> ParseCalcArguments(string[] args)
    {
        if (!IsArgLengthSupported(args)) return Result.Fail<Value>(Messages.InvalidLength);
        if (!double.TryParse(args[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var val1) ||
            !double.TryParse(args[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var val2))
            return Result.Fail<Value>(Messages.InvalidNumberMessage);
        var operation = ParseOperation(args[1]);
        if (operation is Operation.Invalid) return Result.Fail<Value>(Messages.InvalidOperationMessage);
        if (IsDividingByZero(operation, val2)) return Result.Fail<Value>(Messages.DivisionByZeroMessage);
        return Result.Ok(new Value() { Val1 = val1, Operation = operation, Val2 = val2 });
    }

    private static bool IsArgLengthSupported(string[] args) => args.Length == 3;

    private static Operation ParseOperation(string arg)
    {
        return arg switch
        {
            "plus" => Operation.Plus,
            "minus" => Operation.Minus,
            "multiply" => Operation.Multiply,
            "divide" => Operation.Divide,

            "Plus" => Operation.Plus,
            "Minus" => Operation.Minus,
            "Multiply" => Operation.Multiply,
            "Divide" => Operation.Divide,

            _ => Operation.Invalid
        };
    }

    private static bool IsDividingByZero(Operation operation, double arg2)
    {
        return operation is Operation.Divide && arg2 == 0;
    }
}