using GameLibrary;

namespace DAL;

public class MonsterService: IMonsterService
{
    private readonly AppDbContext _context;

    public MonsterService(AppDbContext context)
    {
        _context = context;
    }
   
    public Monster GetRandomMonster()
    {
        IEnumerable<Monster> monsters = _context.Monsters!;
        var rnd = new Random();
        var id = rnd.Next(1,monsters.Count());
        return monsters.First(m => m.Id == id);
    }
}