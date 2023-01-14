using GameLibrary;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Monster>? Monsters { get; set; }

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var lizardfolk = new Monster()
        {
            Id = 1, 
            Name = "Lizardfolk",
            HitPoints = 22,
            AttackModifier = 2,
            AttackPerRound = 1,
            Damage = "1d6",
            DamageModifier = 2,
            Ac = 15
        };
        var goblin = new Monster()
        {
            Id = 2,
            Name = "Goblin",
            HitPoints = 7,
            AttackModifier = -1,
            AttackPerRound = 1,
            Damage = "1d6",
            DamageModifier = 2,
            Ac = 15 
        };
        var skeleton = new Monster()
        {
            Id = 3,
            Name = "Skeleton",
            HitPoints = 13, //хит поинты
            AttackModifier = 0, //модификатор атаки
            AttackPerRound = 1, //атак за раунд
            Damage = "1d6", //урон
            DamageModifier = 2, //модификатор урона
            Ac = 13 //класс брони
        };
        modelBuilder.Entity<Monster>().HasData(lizardfolk, goblin, skeleton);
        base.OnModelCreating(modelBuilder);
    }
}