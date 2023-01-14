using BAL.Models;
using GameLibrary;

namespace BAL.Services;

public interface IGameLogicService
{
    public IEnumerable<Round> ProcessGame(Fight fight);
}