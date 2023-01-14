using Newtonsoft.Json;

namespace GameLibrary;

public class FightResult:Fight
{
    public RoundStatus Status { get; set; }
    public bool IsPlayerTurn { get; set; }
    public bool IsMonsterTurn => !IsPlayerTurn;
    public int AttackDice { get; set; }
    public int DamageDice { get; set; }
    public int Damage { get; set; }
    public bool Win { get; set; }

    public FightResult()
    {
    }
    
    public FightResult(Fight fight)
    {
        Player = fight.Player;
        Monster = fight.Monster;
        IsPlayerTurn = true;
        Win = false;
    }
    
    public FightResult DeepClone()
    {
        var serialized = JsonConvert.SerializeObject(this);
        return JsonConvert.DeserializeObject<FightResult>(serialized)!;
    }
}