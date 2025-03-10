using System.Globalization;
using System.Net.Http.Json;
using Hw11;
using Hw11.Dto;
using Hw11.ErrorMessages;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Hw11Tests;

public class IntegrationCalculatorControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public IntegrationCalculatorControllerTests(WebApplicationFactory<Program> fixture)
    {
        _client = fixture.CreateClient();
    }

    [Theory]
    [InlineData("10", "10")]
    [InlineData("2 + 3", "5")]
    [InlineData("(10 - 3) * 2", "14")]
    [InlineData("3 - 4 / 2", "1")]
    [InlineData("8 * (2 + 2) - 3 * 4", "20")]
    [InlineData("10 - 3 * (-4)", "22")]
    public async Task Calculate_CalculateExpression_Success(string expression, string result)
    {
        var response = await CalculateAsync(expression);
        Assert.True(response!.IsSuccess);
        Assert.Equal(result, response.Result.ToString(CultureInfo.InvariantCulture));
    }

    [Theory]
    [InlineData(null, MathErrorMessager.EmptyString)]
    [InlineData("", MathErrorMessager.EmptyString)]
    [InlineData("10 + i", $"{MathErrorMessager.UnknownCharacter} i")]
    [InlineData("10 : 2", $"{MathErrorMessager.UnknownCharacter} :")]
    [InlineData("3 - 4 / 2.2.3", $"{MathErrorMessager.NotNumber} 2.2.3")]
    [InlineData("2 - 2.23.1 - 23", $"{MathErrorMessager.NotNumber} 2.23.1")]
    [InlineData("8 - / 2", $"{MathErrorMessager.TwoOperationInRow} - and /")]
    [InlineData("8 + (34 - + 2)", $"{MathErrorMessager.TwoOperationInRow} - and +")]
    [InlineData("4 - 10 * (/10 + 2)", $"{MathErrorMessager.InvalidOperatorAfterParenthesis} (/")]
    [InlineData("10 - 2 * (10 - 1 /)", $"{MathErrorMessager.OperationBeforeParenthesis} /)")]
    [InlineData("* 10 + 2", MathErrorMessager.StartingWithOperation)]
    [InlineData("10 + 2 -", MathErrorMessager.EndingWithOperation)]
    [InlineData("((10 + 2)", MathErrorMessager.IncorrectBracketsNumber)]
    [InlineData("(10 - 2))", MathErrorMessager.IncorrectBracketsNumber)]
    [InlineData("10 / 0", MathErrorMessager.DivisionByZero)]
    [InlineData("10 / (1 - 1)", MathErrorMessager.DivisionByZero)]
    public async Task Calculate_CalculateExpression_Error(string expression, string result)
    {
        var response = await CalculateAsync(expression);
        Assert.False(response!.IsSuccess);
        Assert.Equal(result, response.ErrorMessage);
    }

    private async Task<CalculationMathExpressionResultDto?> CalculateAsync(string expression)
    {
        var response = await _client.PostCalculateExpressionAsync(expression);
        return await response.Content.ReadFromJsonAsync<CalculationMathExpressionResultDto>();
    }
}