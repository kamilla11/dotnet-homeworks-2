namespace Hw9.ErrorMessages;

public static class MathErrorMessager
{
    public const string DivisionByZero = "Division by zero"; //done
    public const string EmptyString =  "Empty string"; //done
    public const string IncorrectBracketsNumber = "The number of closing and opening brackets does not match"; //done
    public const string StartingWithOperation =  "An expression cannot start with an operation sign";  //done
    public const string EndingWithOperation =  "An expression cannot end with an operation sign"; //done
    public const string NotNumber =  "There is no such number";
    public const string UnknownCharacter =  "Unknown character"; //done
    public const string TwoOperationInRow = "There are two operations in a row"; //done
    public const string InvalidOperatorAfterParenthesis = "After the opening brackets, only negation can go"; //done
    public const string OperationBeforeParenthesis = "There is only a number before the closing parenthesis"; //done

    public static string NotNumberMessage(string num) =>
        $"{NotNumber} {num}";
    
    public static string UnknownCharacterMessage(char symbol) =>
        $"{UnknownCharacter} {symbol}";

    public static string TwoOperationInRowMessage(string firstOperation, string secondOperation) =>
        $"{TwoOperationInRow} {firstOperation} and {secondOperation}";

    public static string InvalidOperatorAfterParenthesisMessage(string operation) =>
        $"{InvalidOperatorAfterParenthesis} ({operation}";

    public static string OperationBeforeParenthesisMessage(string operation) =>
        $"{OperationBeforeParenthesis} {operation})";
}