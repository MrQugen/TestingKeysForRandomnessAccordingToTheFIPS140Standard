using System.Text;

namespace TestingKeysForRandomnessAccordingToTheFIPS140Standard;

public static class FIPS140
{
    private static string ConvertStringToBinary(string inputString)
    {
        byte[] byteArray = Encoding.UTF8.GetBytes(inputString);
        StringBuilder binaryStringBuilder = new StringBuilder();

        foreach (byte b in byteArray)
        {
            binaryStringBuilder.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
        }

        return binaryStringBuilder.ToString();
    }

    public static bool RunMonobitTest(string inputString)
    {
        string binaryInput = ConvertStringToBinary(inputString);

        // Реалізація монобітного тесту
        int onesCount = binaryInput.Count(c => c == '1');
        int n = binaryInput.Length;

        double s = Math.Abs(onesCount - n / 2.0) / Math.Sqrt(n / 4.0);

        // Поріг для тесту
        double alpha = 0.01;

        return s < alpha;
    }

    public static bool RunMaxLengthSeriesTest(string inputString)
    {
        string binaryInput = ConvertStringToBinary(inputString);

        // Реалізація тесту максимальної довжини серії
        int maxSeriesLength = 36;
        int currentSeriesLength = 1;
        char previousBit = binaryInput[0];

        for (int i = 1; i < binaryInput.Length; i++)
        {
            char currentBit = binaryInput[i];

            if (currentBit == previousBit)
            {
                currentSeriesLength++;

                if (currentSeriesLength > maxSeriesLength)
                {
                    return false; // Серія більше максимальної довжини, тест не пройдений
                }
            }
            else
            {
                currentSeriesLength = 1;
            }

            previousBit = currentBit;
        }

        return true; // Всі серії задовольняють вимогам, тест пройдений
    }

    public static bool RunSeriesTest(string inputString)
    {
        string binaryInput = ConvertStringToBinary(inputString);
        int seriesLength = 0;
        char currentBit = binaryInput[0];
        int zeroSeriesLength = 0;
        int oneSeriesLength = 0;
        bool isZeroSeries = currentBit == '0';

        for (int i = 0; i < binaryInput.Length; i++)
        {
            if (binaryInput[i] == currentBit)
            {
                seriesLength++;
            }
            else
            {
                if (currentBit == '0')
                {
                    zeroSeriesLength = Math.Max(zeroSeriesLength, seriesLength);
                }
                else
                {
                    oneSeriesLength = Math.Max(oneSeriesLength, seriesLength);
                }

                seriesLength = 1;
                currentBit = binaryInput[i];
            }
        }

        // Пороги для довжин серій (ваш вибір)
        int maxSeriesLengthForZeros = 6;
        int maxSeriesLengthForOnes = 6;

        return zeroSeriesLength <= maxSeriesLengthForZeros &&
               oneSeriesLength <= maxSeriesLengthForOnes;
    }

    public static bool RunPokerTest(string inputString)
    {
        string binaryInput = ConvertStringToBinary(inputString);

        int m = 4; // Довжина блоку для тесту Поккера
        if (binaryInput.Length % m != 0)
        {
            throw new ArgumentException("Довжина вхідної послідовності не кратна довжині блоку (m).");
        }

        int numBlocks = binaryInput.Length / m;
        Dictionary<string, int> blockCounts = new Dictionary<string, int>();

        for (int i = 0; i < numBlocks; i++)
        {
            string block = binaryInput.Substring(i * m, m);
            if (blockCounts.ContainsKey(block))
            {
                blockCounts[block]++;
            }
            else
            {
                blockCounts[block] = 1;
            }
        }

        int uniqueBlockCount = blockCounts.Count;
        int totalBlockCount = numBlocks;
        double chiSquareValue = CalculateChiSquareValue(blockCounts, uniqueBlockCount, totalBlockCount);

        // Поріг для тесту (ваш вибір)
        double alpha = 0.01;

        return chiSquareValue < alpha;
    }

    private static double CalculateChiSquareValue(Dictionary<string, int> blockCounts, int uniqueBlockCount,
        int totalBlockCount)
    {
        double expectedFrequency = (double) totalBlockCount / uniqueBlockCount;
        double chiSquareValue = 0;

        foreach (var blockCount in blockCounts.Values)
        {
            chiSquareValue += Math.Pow(blockCount - expectedFrequency, 2) / expectedFrequency;
        }

        return chiSquareValue;
    }
}