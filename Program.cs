using System;
using System.Numerics;

class Program
{
    static string GenerateNumber()
    {
        Random rand = new Random();
        string number = "";

        while (number.Length < 4)
        {
            //char digit = (char)('0' + rand.Next(0, 10)); 
            int digit = rand.Next(0, 10);
            if (!number.Contains(digit.ToString())){
                number += digit.ToString();
            }
            // 0 - 48 ascii
            //if (!number.Contains(digit))
            //{
            //    number += digit;
            //}
        }
        return number;
    }

    static (int correctNumbers, int correctPositions) CalculateMatches(string secret, string guess)
    {
        int correctNumbers = 0, correctPositions = 0;

        // Подсчет количества совпадений по позиции
        for (int i = 0; i < 4; i++)
        {
            if (guess[i] == secret[i])
            {
                correctPositions++;
            }
        }

        // Подсчет количества совпадений по значению (без учета позиции)
        foreach (char c in guess)
        {
            if (secret.Contains(c))
            {
                correctNumbers++;
            }
        }

        return (correctNumbers, correctPositions);
    }
    static void UpdateRanking(int attempts)
    {
        string filePath = "ranking.txt";

        // Чтение существующего рейтинга
        List<int> rankings = new List<int>();
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            int prevAttempts;
            foreach (var line in lines)
            {
                if (int.TryParse(line, out prevAttempts))
                {
                    rankings.Add(prevAttempts);
                }
            }
        }

        // Добавление нового результата
        rankings.Add(attempts);

        rankings.Sort();

        // Запись обновлённого рейтинга в файл
        File.WriteAllLines(filePath, rankings.ConvertAll(x => x.ToString()));

        int position = rankings.IndexOf(attempts) + 1;
        Console.WriteLine($"Ты занимаешь {position}-е место в рейтинге с {attempts} попытками.");
    }


    static void Main(string[] args)
    {
        string secretNumber = GenerateNumber();
        Console.WriteLine("Игра началась! Угадай 4-значное число без повторяющихся цифр.");

        string guess;
        int attempts = 0;

        while (true)
        {
            Console.Write("Введи свою догадку: ");
            guess = Console.ReadLine();

            if (guess.Length != 4)
            {
                Console.WriteLine("Число должно состоять из 4 цифр. Попробуй снова.");
                continue;
            }

            attempts++;
            var (correctNumbers, correctPositions) = CalculateMatches(secretNumber, guess);
            Console.WriteLine($"Угадано цифр: {correctNumbers}, на своих местах: {correctPositions}");

            if (correctPositions == 4)
            {
                Console.WriteLine($"Поздравляю! Ты угадал число {secretNumber} за {attempts} попыток.");
                UpdateRanking(attempts);
                break;
            }
        }
    }
}
