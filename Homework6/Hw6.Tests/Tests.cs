using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;
using Hw6;
using Xunit;
using Hw6Client ;
using Microsoft.FSharp.Control;


namespace Hw6Tests
{
    public class BasicTests : IClassFixture<CustomWebApplicationFactory<App.Startup>>
    {
        private readonly CustomWebApplicationFactory<App.Startup> _factory;
        private const decimal Epsilon = 0.001m;

        public BasicTests(CustomWebApplicationFactory<App.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData(15, 5, "Plus", 20, HttpStatusCode.OK)]
        [InlineData(15, 5, "Minus", 10, HttpStatusCode.OK)]
        [InlineData(15, 5, "Multiply", 75, HttpStatusCode.OK)]
        [InlineData(15, 5, "Divide", 3, HttpStatusCode.OK)]
        public async Task TestAllOperationsInt(int value1, int value2, string operation, int expectedValue,
            HttpStatusCode statusCode) =>
            await RunTest(value1.ToString(), value2.ToString(), operation, expectedValue.ToString(), statusCode);

        [Theory]
        [InlineData(15.6, 5.6, "Plus", 21.2, HttpStatusCode.OK)]
        [InlineData(15.6, 5.6, "Minus", 10, HttpStatusCode.OK)]
        [InlineData(15.6, 5.6, "Multiply", 87.36, HttpStatusCode.OK)]
        [InlineData(15.6, 5.6, "Divide", 2.7857, HttpStatusCode.OK)]
        public async Task TestAllOperationsDouble(double value1, double value2, string operation,
            double expectedValue, HttpStatusCode statusCode) =>
            await RunTest(value1.ToString(CultureInfo.InvariantCulture), value2.ToString(CultureInfo.InvariantCulture),
                operation, expectedValue.ToString(CultureInfo.InvariantCulture), statusCode);

        [Theory]
        [InlineData(15.6, 5.6, "Plus", 21.2, HttpStatusCode.OK)]
        [InlineData(15.6, 5.6, "Minus", 10, HttpStatusCode.OK)]
        [InlineData(15.6, 5.6, "Multiply", 87.36, HttpStatusCode.OK)]
        [InlineData(15.6, 5.6, "Divide", 2.7857, HttpStatusCode.OK)]
        public async Task TestAllOperationsDecimal(decimal value1, decimal value2, string operation,
            decimal expectedValue, HttpStatusCode statusCode) =>
            await RunTest(value1.ToString(CultureInfo.InvariantCulture), value2.ToString(CultureInfo.InvariantCulture),
                operation, expectedValue.ToString(CultureInfo.InvariantCulture), statusCode);

        [Theory]
        [InlineData("biba", "5.6", "Plus", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("15.6", "boba", "Plus", "Could not parse value 'boba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "boba", "Plus", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "5.6", "Minus", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("15.6", "boba", "Minus", "Could not parse value 'boba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "boba", "Minus", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "5.6", "Multiply", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("15.6", "boba", "Multiply", "Could not parse value 'boba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "boba", "Multiply", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "5.6", "Divide", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("15.6", "boba", "Divide", "Could not parse value 'boba'", HttpStatusCode.BadRequest)]
        [InlineData("biba", "boba", "Divide", "Could not parse value 'biba'", HttpStatusCode.BadRequest)]
        [InlineData("15.6.6", "5.6", "Divide", "Could not parse value '15.6.6'", HttpStatusCode.BadRequest)]
        public async Task TestAllOperationsIncorrectValues(string value1, string value2, string operation,
            string expectedValue, HttpStatusCode statusCode) =>
            await RunTest(value1, value2, operation, expectedValue, statusCode);
        
        [Theory]
        [InlineData("15.6", "5.6", "@", "Could not parse value '@'", HttpStatusCode.BadRequest)]
        [InlineData("15.6", "5.6", "$", "Could not parse value '$'", HttpStatusCode.BadRequest)]
        [InlineData("15.6", "5.6", "^", "Could not parse value '^'", HttpStatusCode.BadRequest)]
        public async Task TestIncorrectOperation(string value1, string value2, string operation,
            string expectedValue, HttpStatusCode statusCode) =>
            await RunTest(value1, value2, operation, expectedValue, statusCode);

        [Fact]
        public async Task TestParserDividingByZero() =>
            await RunTest("15.6", "0", "Divide", "DivideByZero", HttpStatusCode.OK, true);

        
        private async Task RunTest(string value1, string value2, string operation, string expectedValueOrError,
            HttpStatusCode statusCode, bool isDividingByZero = false)
        {
            // arrange
            var url = $"/calculate?value1={value1}&operation={operation}&value2={value2}";
            
            // act
            var client = _factory.CreateClient();
            var response = await client.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();

            // assert
            Assert.True(response.StatusCode == statusCode);
            if (statusCode == HttpStatusCode.OK && !isDividingByZero)
                Assert.True(Math.Abs(decimal.Parse(expectedValueOrError, CultureInfo.InvariantCulture) -
                                     decimal.Parse(result, CultureInfo.CurrentCulture)) < Epsilon);
            else
                Assert.Contains(expectedValueOrError, result);
        }
        
        [Theory]
        [InlineData("15", "5", "Plus", "20", HttpStatusCode.OK)]
        [InlineData("15", "5", "Minus", "10", HttpStatusCode.OK)]
        [InlineData("15", "5", "Multiply", "75", HttpStatusCode.OK)]
        [InlineData("15", "5", "Divide", "3", HttpStatusCode.OK)]
        
        public void TestCorrectResultOnClient(string value1, string value2, string operation,
            string expectedValue, HttpStatusCode statusCode)
        {
            // arrange
            var url = $"http://localhost:5000/calculate?value1={value1}&operation={operation}&value2={value2}";
            
            // act
            var client = _factory.CreateClient();
            var result = FSharpAsync.RunSynchronously(Program.getAsync(client, url), null, null);
            var convertedResult = (result is null) ? "" : result;
            
            // assert
            Assert.Equal(expectedValue, convertedResult);
        }

        [Fact]
        public void TestIncorrectArgumentCount()
        {
            //arrange
            var args =  new [] {"3","+","4","5"};

            //act
            var result = Parser.parseCalcArguments(args);
            
            //assert
            if (result.ErrorValue is not null) Assert.Equal(result.ErrorValue, "WrongArgLength");
            else throw new InvalidOperationException("This test must always return Error Result Type");
        }

        [Theory]
        [InlineData("15", "Plus", "5", 20)]
        [InlineData("15", "Minus", "5", 10)]
        [InlineData("15", "Multiply", "5", 75)]
        [InlineData("15", "Divide", "5", 3)]
        [InlineData("15.6", "Plus", "5.6", 21.2)]
        [InlineData("15.6", "Minus", "5.6", 10)]
        [InlineData("15.6", "Multiply", "5.6", 87.36)]
        [InlineData("15.6", "Divide", "5.6", 2.7857)]
        public void ValuesParsedCorrectly(string value1, string operation, string value2,
            double expectedValue)
        {
            //arrange
            var args =  new [] {value1,operation,value2};
    
            //act
            var result = Parser.parseCalcArguments(args);

            if (result.ResultValue is not null) Assert.True(Math.Abs(expectedValue - double.Parse(result.ResultValue)) < 0.001);
            else throw new InvalidOperationException();
        }


        [Theory]
        [InlineData(15, 5, Calculator.CalculatorOperation.Plus, 20)]
        [InlineData(15, 5, Calculator.CalculatorOperation.Minus, 10)]
        [InlineData(15, 5, Calculator.CalculatorOperation.Multiply, 75)]
        [InlineData(15, 5, Calculator.CalculatorOperation.Divide, 3)]
        public void CorrectCalculationResults(int value1, int value2, Calculator.CalculatorOperation operation,
            int expectedValue)
        {
            //var actual = Calculator.calculate<int>();
            var actual = Calculator.calculate<int, int, int>(value1, operation, value2);
    
            //assert
            Assert.Equal(expectedValue, actual);
        }

        [Theory]
        [InlineData("+", "Plus")]
        [InlineData("-", "Minus")]
        [InlineData("/", "Divide")]
        [InlineData("*", "Multiply")]
        [InlineData("@", "Default")]
        public void TestConvertOperation(string operation, string expectedValue)
        {
            //act
            var actual = Program.convertOperation(operation);

            Assert.Equal(expectedValue, actual);
        }
        
    }
}