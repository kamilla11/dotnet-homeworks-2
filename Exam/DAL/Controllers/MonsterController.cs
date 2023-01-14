using System.Text.Json;
using GameLibrary;
using Microsoft.AspNetCore.Mvc;

namespace DAL.Controllers;

[ApiController]
public class MonsterController : ControllerBase
{
    private readonly IMonsterService _monsterService;

    private readonly ILogger<MonsterController> _logger;

    public MonsterController(ILogger<MonsterController> logger, IMonsterService monsterService)
    {
        _logger = logger;
        _monsterService = monsterService;
    }

    [HttpGet]
    [Route("GetRandomMonster")]
    public Monster GetRandomMonster()
    {
        var result =  _monsterService.GetRandomMonster();
        //  return JsonSerializer.Serialize(result);
        return result;
    }
}