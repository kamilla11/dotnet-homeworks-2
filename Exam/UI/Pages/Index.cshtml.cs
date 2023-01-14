using GameLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using BAL.Models;

namespace UI.Pages;

public class IndexModel : PageModel
{
    private readonly HttpClient _client;
    private static readonly string DalPath = "https://localhost:7777";
    private static readonly string BalPath = "https://localhost:8888";
    private readonly IHttpClientFactory _clientFactory;

    public List<string> FightLog { get; set; }
    public Monster Monster { get; set; }

    public bool IsPlayerLose { get; set; }

    [BindProperty] public int Id { get; set; }
    [BindProperty] public string? Name { get; set; }
    [BindProperty] public int HitPoints { get; set; }
    [BindProperty] public int AttackModifier { get; set; }
    [BindProperty] public int AttackPerRound { get; set; }
    [BindProperty] public string? Damage { get; set; }
    [BindProperty] public int DamageModifier { get; set; }
    [BindProperty] public int Ac { get; set; }


    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _client = clientFactory.CreateClient("DndUI");
    }

    public void OnGet()
    {
    }

    public async Task OnPost()
    {
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        var monsterResponse = await _client.GetAsync($"{DalPath}/GetRandomMonster");
        var monster = await monsterResponse.Content.ReadAsAsync<Monster>();
        Monster = monster;

        var player = new Player()
        {
            Id = Id, Name = Name, HitPoints = HitPoints, AttackModifier = AttackModifier,
            AttackPerRound = AttackPerRound, Damage = Damage, DamageModifier = DamageModifier, Ac = Ac
        };

        var fight = new Fight() { Player = player, Monster = monster };

        HttpResponseMessage fightResponse = await _client.PostAsJsonAsync(
            $"{BalPath}/GetFightResult", fight);

        var fightLog = await fightResponse.Content.ReadAsAsync<List<Round>>();

        FightLog = ConvertResponse(fightLog);
    }

    private List<string> ConvertResponse(List<Round> fightLog)
    {
        var builder = new List<string>();
        foreach (var round in fightLog)
        {
            builder.Add($"Раунд {round.Id}");
            foreach (var fight in round.Rounds)
            {
                Creature attackCreature, aimCreature;
                if (fight.IsPlayerTurn)
                {
                    attackCreature = fight.Player!;
                    aimCreature = fight.Monster!;
                }
                else
                {
                    attackCreature = fight.Monster!;
                    aimCreature = fight.Player!;
                }

                builder.Add(attackCreature.Name);
                switch (fight.Status)
                {
                    case RoundStatus.CriticalHit:
                        builder.Add(
                            $"{fight.AttackDice} (+{attackCreature.AttackModifier}) критическое попадание. " +
                            $"{fight.DamageDice} (+{attackCreature.DamageModifier + 1}) * 2 наносит {fight.Damage} " +
                            $"урона игроку {aimCreature.Name}({aimCreature.HitPoints}). ");
                        break;
                    case RoundStatus.CriticalMiss:
                        builder.Add($"{fight.AttackDice}(+{attackCreature.AttackModifier}) критический промах. ");
                        break;
                    case RoundStatus.Hit:
                        builder.Add(
                            $"{fight.AttackDice} (+{attackCreature.AttackModifier}) больше {aimCreature.Ac}, попадание. " +
                            $"{fight.DamageDice} (+{attackCreature.DamageModifier + 1}) наносит {fight.Damage} " +
                            $"урона игроку {aimCreature.Name}({aimCreature.HitPoints}). ");
                        break;
                    case RoundStatus.Miss:
                        builder.Add(
                            $"{fight.AttackDice}(+{attackCreature.AttackModifier}) меньше {aimCreature.Ac}, промах. ");
                        break;
                }

                if (fight.Win)
                {
                    if (attackCreature == fight.Monster!) IsPlayerLose = true;
                    builder.Add($"{aimCreature.Name} мертв(а). {attackCreature.Name} победил(а).");
                    break;
                }
            }
        }

        return builder;
    }
}