namespace TestingKeysForRandomnessAccordingToTheFIPS140Standard;

public class RandomStringGenerator
{
    private readonly Random _random = new();
    private const string AvailableCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";

    public string GenerateRandomString(int length)
    {
        if (length <= 0)
        {
            throw new ArgumentException("Довжина має бути більше 0.");
        }

        var randomString = new char[length];

        for (var i = 0; i < length; i++)
        {
            // Генеруємо випадковий індекс для вибору символу з рядка availableCharacters
            var randomIndex = _random.Next(AvailableCharacters.Length);

            // Обираємо символ з рядка availableCharacters за випадковим індексом
            randomString[i] = AvailableCharacters[randomIndex];
        }

        return new string(randomString);
    }
}