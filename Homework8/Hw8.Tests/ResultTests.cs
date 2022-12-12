using System;
using Hw8.Calculator;
using Xunit;

namespace Hw8.Tests;

public class ResultTests
{
    [Theory]
    [InlineData(true, "Error")]
    [InlineData(false, "")]
    public void Result_IncorrectInput_InvalidOperationMessageReturned(bool success, string error)
    {
        //act + assert
        Assert.Throws<InvalidOperationException>(() => { new Result(success, error); });
    }
}