class Program
{
    static string GenerateNumber()
    {
        Random rand = new Random();
        string number = "";

        while (number.Length < 4)
        {
            int digit = rand.Next(0, 10);
            if (!number.Contains(digit.ToString()))
            {
                number += digit.ToString();
            }
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

    static bool AuthenticateUser()
    {
        string filePath = "user.txt";
        Console.Write("Введите логин: ");
        string inputLogin = Console.ReadLine();
        Console.Write("Введите пароль: ");
        string inputPassword = Console.ReadLine();

        // Чтение файла с пользователями
        if (System.IO.File.Exists(filePath))
        {
            string[] users = System.IO.File.ReadAllLines(filePath);
            foreach (string user in users)
            {
                var credentials = user.Split(' ');
                if (credentials[0] == inputLogin && credentials[1] == inputPassword)
                {
                    LogAction($"Пользователь {inputLogin} успешно авторизовался.");
                    return true;
                }
            }
        }

        Console.WriteLine("Неверный логин или пароль. Попробуйте снова.");
        LogAction($"Неудачная попытка входа для логина {inputLogin}.");
        return false;
    }

    static void UpdateRanking(int attempts)
    {
        string filePath = "ranking.txt";

        // Чтение существующего рейтинга
        List<int> rankings = new List<int>();
        if (System.IO.File.Exists(filePath))
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);
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
        System.IO.File.WriteAllLines(filePath, rankings.ConvertAll(x => x.ToString()));

        int position = rankings.IndexOf(attempts) + 1;
        Console.WriteLine($"Ты занимаешь {position}-е место в рейтинге с {attempts} попытками.");
        LogAction($"Игрок завершил игру за {attempts} попыток и занял {position}-е место в рейтинге.");
    }

    static void LogAction(string message)
    {
        Console.WriteLine($"{DateTime.Now}: {message}");
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Добро пожаловать в игру!");
        
        while (!AuthenticateUser()) {}

        // Начало игры после успешной авторизации
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
