using System.Diagnostics.CodeAnalysis;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    private readonly ICalculator _calculator;
    private readonly IParser _parser;

    public CalculatorController(ICalculator calculator, IParser parser)
    {
        _calculator = calculator;
        _parser = parser;
    }

    public ActionResult<double> Calculate(
        string val1,
        string operation,
        string val2)
    {
        var args = new [] { val1, operation, val2 };
        var parsing = _parser.ParseCalcArguments(args);
        if (parsing.Success)
            return _calculator.Calculate(parsing.Value.Val1, parsing.Value.Operation, parsing.Value.Val2);
        else return BadRequest(parsing.Error);
    }

    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}