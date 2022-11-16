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
        Assert.Equal("Unknown character", exception.Message);
    }

    [Theory]
    [InlineData("(1 + 3) - (--)", MathErrorMessager.WrongNotationForANegativeNumber)]
    [InlineData("(1 + 3) - (-4 / 5)", MathErrorMessager.WrongNotationForANegativeNumber)]
    public async Task ConvertToPostfix_ExpressionWithNegativeNumber_Error(string expression, string error)
    {
        //act
        var response = () =>(object)PostfixParser.ConvertToPostfix(expression);
        //assert
        var exception = Assert.Throws<Exception>(response);
        Assert.Equal(error, exception.Message);
    }
    
}
