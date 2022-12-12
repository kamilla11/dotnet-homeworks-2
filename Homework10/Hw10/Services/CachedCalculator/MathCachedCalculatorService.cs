using Hw10.DbModels;
using Hw10.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
    private readonly IMemoryCache _cache;
    private readonly IMathCalculatorService _simpleCalculator;

    public MathCachedCalculatorService(IMemoryCache cache, IMathCalculatorService simpleCalculator)
    {
        _cache = cache;
        _simpleCalculator = simpleCalculator;
    }

    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        if (expression is not null && _cache.TryGetValue(expression, out double result))
        {
            await Task.Delay(1000);
            return new CalculationMathExpressionResultDto(result);
        }
            
        
        var calculation = await _simpleCalculator.CalculateMathExpressionAsync(expression);
        if (calculation.IsSuccess)
            _cache.Set(expression!,  calculation.Result, new MemoryCacheEntryOptions());

        return calculation;
    }
}