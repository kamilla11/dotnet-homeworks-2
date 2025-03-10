using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using JetBrains.dotMemoryUnit;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace Hw13.Tests;

public class MemoryTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;

    public MemoryTest(WebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _output = output;
        DotMemoryUnitTestOutput.SetOutputMethod(_output.WriteLine);
        _client = factory.CreateClient();
    }

    [Fact]
    [DotMemoryUnit(FailIfRunWithoutSupport = false, CollectAllocations = true)]
    public void TestAsync()
    {
        var memoryBefore = dotMemory.Check();
        var testDataGenerator = new TestDataGenerator();
        var list = testDataGenerator.GetValue();
        long size = 0;
        for (var i = 0; i < 100; i++)
        {
            foreach (var element in list)
            {
                var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Calculator/CalculateMathExpression");
                var formModel = new Dictionary<string, string> { { "str", element } };
                postRequest.Content = new FormUrlEncodedContent(formModel);
                _client.SendAsync(postRequest).GetAwaiter().GetResult();

                size += Encoding.UTF8.GetBytes(element).Length;
            }
        }

        dotMemory.Check(memory =>
        {
            _output.WriteLine(memory.GetTrafficFrom(memoryBefore).CollectedMemory.SizeInBytes.ToString());
            _output.WriteLine(size.ToString());
            Assert.True(memory.GetTrafficFrom(memoryBefore).CollectedMemory.SizeInBytes >= size);
        });
    }
}

public class TestDataGenerator
{
    private readonly List<string> _list = new()
    {
        "@+@",
        "@+@+@",
        "@+@-@",
        "@+@*@",
        "@+@/@",

        "@-@",
        "@-@+@",
        "@-@-@",
        "@-@*@",
        "@-@/@",

        "@*@",
        "@*@+@",
        "@*@-@",
        "@*@*@",
        "@*@/@",

        "@/@",
        "@/@+@",
        "@/@-@",
        "@/@*@",
        "@/@/@",

        "@*(@+@)+@"
    };

    private int GenerateRandomValue(Random random) => random.Next(1, 10000);

    private string FillElement(Random random, string element)
    {
        var strBuilder = new StringBuilder();
        foreach (var ch in element)
        {
            if (ch.Equals('@'))
                strBuilder.Append(GenerateRandomValue(random));
            else
                strBuilder.Append(ch);
        }

        return strBuilder.ToString();
    }

    public IEnumerable<string> GetValue()
    {
        var random = new Random();
        foreach (var element in _list)
            yield return FillElement(random, element);
    }
}