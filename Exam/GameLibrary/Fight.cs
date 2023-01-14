namespace GameLibrary;

public enum RoundStatus
{
    Hit,
    Miss,
    CriticalHit,
    CriticalMiss,
}

public class Fight
{
    public Player? Player { get; set; }
    public Monster? Monster { get; set; }
}