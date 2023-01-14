using BAL.Models;
using GameLibrary;

namespace BAL.Services;

public class GameLogicService : IGameLogicService
{
    private FightResult _fightResult;
    private List<Round> fightLog;
    private int roundCounts = 0;

    public IEnumerable<Round> ProcessGame(Fight fight)
    {
        _fightResult = new FightResult(fight);
        fightLog = new List<Round>();
        while (!_fightResult.Win)
        {
            var round = Fight();
            if (round is not null) fightLog.Add(round);
        }
        
        return fightLog;
    }

    private Round? Fight()
    {
        var currentRound = new Round() { Id = ++roundCounts, Rounds = new List<FightResult>()};
        var playerAttackCount = _fightResult.Player!.AttackPerRound;
        var monsterAttackCount = _fightResult.Monster!.AttackPerRound;
        while (playerAttackCount != 0 || monsterAttackCount != 0)
        {
            if (_fightResult.IsPlayerTurn)
            {
                if (playerAttackCount == 0)
                {
                    _fightResult.IsPlayerTurn = false;
                    continue;
                }

                currentRound.Rounds.Add(Attack(_fightResult.Player!, _fightResult.Monster!));
                _fightResult.IsPlayerTurn = false;
                playerAttackCount--;
            }
            else
            {
                if (monsterAttackCount == 0)
                {
                    _fightResult.IsPlayerTurn = true;
                    continue;
                }

                currentRound.Rounds.Add(Attack(_fightResult.Monster!, _fightResult.Player!));
                _fightResult.IsPlayerTurn = true;
                monsterAttackCount--;
            }
        }

        return currentRound;
    }

    private FightResult Attack(Creature attackCreature, Creature aimCreature)
    {
        var attackDice = Dice.DiceTwenty.Roll();
        _fightResult.AttackDice = attackDice;
        if (attackDice == 20)
        {
            aimCreature.HitPoints -= CalculateDamage(true, attackCreature);
            _fightResult.Status = RoundStatus.CriticalHit;
        }
        else if (attackDice == 1)
        {
            _fightResult.Status = RoundStatus.CriticalMiss;
        }
        else if (attackDice + attackCreature.AttackModifier > aimCreature.Ac)
        {
            aimCreature.HitPoints -= CalculateDamage(false, attackCreature);
            _fightResult.Status = RoundStatus.Hit;
        }
        else
        {
            _fightResult.Status = RoundStatus.Miss;
        }

        if (aimCreature.HitPoints <= 0)
        {
            _fightResult.Win = true;
        }

        return _fightResult.DeepClone();
    }

    //проверка на корректность данных и на damage уже якобы произведена
    private int CalculateDamage(bool isCriticalHits, Creature creature)
    {
        var totalDamage = 0;
        var numbOfThrows = int.Parse(creature.Damage![0].ToString());
        var damageDice = new Dice(int.Parse(creature.Damage![2].ToString()));

        for (var j = 0; j < numbOfThrows; j++)
        {
            totalDamage += damageDice.Roll();
        }

        _fightResult.DamageDice = totalDamage;
        totalDamage += creature.DamageModifier + 1;
        if (isCriticalHits) totalDamage *= 2;
        _fightResult.Damage = totalDamage;
        return totalDamage;
    }
}