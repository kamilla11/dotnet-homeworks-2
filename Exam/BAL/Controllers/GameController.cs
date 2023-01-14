using BAL.Models;
using BAL.Services;
using Microsoft.AspNetCore.Mvc;
using GameLibrary;

namespace BAL.Controllers;

[ApiController]
public class GameController : ControllerBase
{
    private readonly ILogger<GameController> _logger;
    private readonly IGameLogicService _gameLogic;

    public GameController(ILogger<GameController> logger, IGameLogicService gameLogic)
    {
        _logger = logger;
        _gameLogic = gameLogic;
    }

    [HttpPost]
    [Route("GetFightResult")]
    public IEnumerable<Round> Post(Fight fight)
    {
        return _gameLogic.ProcessGame(fight);
    }
}