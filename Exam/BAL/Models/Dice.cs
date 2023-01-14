namespace BAL.Models;

public class Dice
{
    public static Dice DiceTwenty => new Dice(20);

    public int NumberOfSides { get; set; }

    public Dice(int count)
    {
        NumberOfSides = count;
    }
    
    public int Roll()
    {
        var rnd = new Random();
        return rnd.Next(1, NumberOfSides);
    }
}