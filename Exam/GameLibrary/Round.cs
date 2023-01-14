using GameLibrary;

namespace BAL.Models;

public class Round
{
    public int Id { get; set; }
    public List<FightResult> Rounds { get; set; }
}