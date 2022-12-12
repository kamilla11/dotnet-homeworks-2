using Hw11.Exceptions;
using Hw11.Services.Expressions;
using Hw11.Services.Parser;

namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        var postfix = PostfixParser.ConvertToPostfix(expression);
            var expressionTree = ExpressionTree.GenerateExpressionTree(postfix);
            var treeVisited = await new ExpressionTreeVisitor().VisitAsync((dynamic)expressionTree);
            return treeVisited;
    }
}