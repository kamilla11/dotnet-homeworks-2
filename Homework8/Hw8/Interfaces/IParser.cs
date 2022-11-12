namespace Hw8.Calculator;

public interface IParser
{
    Result<Value> ParseCalcArguments(string[] args);
}