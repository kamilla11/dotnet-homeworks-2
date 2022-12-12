using Hw11.ErrorMessages;
using System.Linq.Expressions;

namespace Hw11.Services.Expressions;

public class ExpressionTree
{
    public static Expression GenerateExpressionTree(string postfix)
    {
        var tokens = postfix.Split(' ');
        var nodes = new Stack<Expression>();
        foreach (var token in tokens)
        {
            if (double.TryParse(token, out var value))
            {
                nodes.Push(Expression.Constant(value, typeof(double)));
            }
            else
            {
                var right = nodes.Pop();
                var left = nodes.Pop();

                var operation = token switch
                {
                    "+" => Expression.Add(left, right),
                    "-" => Expression.Subtract(left, right),
                    "/" => Expression.Divide(left, right),
                    "*" => Expression.Multiply(left, right)
                };
                nodes.Push(operation);
            }
        }

        return nodes.Pop();
    }
}