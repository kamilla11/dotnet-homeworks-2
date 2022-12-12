using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Hw11.ErrorMessages;

namespace Hw11.Services.Expressions;

public class ExpressionTreeVisitor
{
    [ExcludeFromCodeCoverage]
    public async Task<double> VisitExpressionAsync(Expression expression)
    {
        return await VisitAsync((dynamic)expression);
    }

    public async Task<double> VisitAsync(BinaryExpression expression)
    {
        await Task.Delay(1000);
        var task1 = Task.Run(() => VisitExpressionAsync(expression.Left));
        var task2 = Task.Run(() => VisitExpressionAsync(expression.Right));
        var res = await Task.WhenAll(task1, task2);
        await Task.Yield();
        return GetResult(expression, res[0], res[1]);
    }

    public async Task<double> VisitAsync(ConstantExpression expression)
    {
        return (double)expression.Value;
    }

    public static double GetResult(Expression binaryExpr, double value1, double value2)
    {
        return binaryExpr.NodeType switch
        {
            ExpressionType.Add => value1 + value2,
            ExpressionType.Subtract => value1 - value2,
            ExpressionType.Multiply => value1 * value2,
            ExpressionType.Divide => (value2 < Double.Epsilon)
                ? throw new DivideByZeroException(MathErrorMessager.DivisionByZero)
                : value1 / value2,
            _ => throw new Exception(MathErrorMessager.UnknownCharacter)
        };
    }
}