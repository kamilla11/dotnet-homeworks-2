using System.Globalization;
using System.Linq.Expressions;
using Hw9.ErrorMessages;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Hw9.Services;

namespace Hw9.Tests;

public class ServicesTests : IClassFixture<WebApplicationFactory<Program>>
{

    [Theory]
    [InlineData("2 4 @ 7 * 5 -", MathErrorMessager.UnknownCharacter)]
    [InlineData("1 3 5 $ +", MathErrorMessager.UnknownCharacter)]
    public async Task GenerateExpressionTree_InvalidData(string expression, string result)
    {
        //act
        var response = () =>ExpressionTree.GenerateExpressionTree(expression);
        //assert
        var exception = Assert.Throws<Exception>(response);
        //The thrown exception can be used for even more detailed assertions.
        Assert.Equal(result, exception.Message);
    }

    [Fact]
    public async Task GetResult_InvalidData()
    {
        //arrange
        var const1 = Expression.Constant(1.0, typeof(double));
        var const2 = Expression.Constant(2.0, typeof(double));
        var expression = Expression.Equal(const1, const2);
        //act
        var response = () =>(object)ExpressionTreeVisitor.GetResult(expression, 1, 2);
        //assert
        var exception = Assert.Throws<Exception>(response);
        //The thrown exception can be used for even more detailed assertions.
        Assert.Equal("Unknown character", exception.Message);
    }
}

