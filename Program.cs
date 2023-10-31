namespace TestingKeysForRandomnessAccordingToTheFIPS140Standard;

public static class Program
{
    public static void Main()
    {
        var inputString = new RandomStringGenerator().GenerateRandomString(2500);
        
        Console.WriteLine("Монобiтний тест: " + FIPS140.RunMonobitTest(inputString));
        Console.WriteLine("Тест максимальної довжини серiї: " + FIPS140.RunMaxLengthSeriesTest(inputString));
        Console.WriteLine("Тест Поккера: " + FIPS140.RunPokerTest(inputString));
        Console.WriteLine("Тест довжин серiй: " + FIPS140.RunSeriesTest(inputString));
    }
}