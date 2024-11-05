using FOURINROW.Services;
using FOURINROW.Domain;
using DotNetEnv;
namespace FOURINROW.Cmd
{
    class Program
    {
        private static UserService _userService;
        private static GameService _gameService;

        static void Main(string[] args)
        {
            Env.Load();
            var userNameDB = Environment.GetEnvironmentVariable("USERNAME_DB");
            var passwordDB = Environment.GetEnvironmentVariable("PASSWORD_DB");
            var hostDB = Environment.GetEnvironmentVariable("HOST_DB");
            var port = Environment.GetEnvironmentVariable("PORT_DB");
            var databaseDB = Environment.GetEnvironmentVariable("NAME_DB");
            string connectionString = $"Server={hostDB};Port={port};Database={databaseDB};User ID={userNameDB};Password={passwordDB};";
            _userService = new UserService(connectionString);
            _gameService = new GameService("ranking.txt");
            Console.WriteLine("Добро пожаловать в игру!");
            Console.WriteLine("Если хотите зарегистрироваться, нажмите 1. Если хотите авторизоваться нажмите 2.");

            switch (Console.ReadLine())
            {
                case "1":
                    HandleRegistration();
                    break;
                case "2":
                    if (!HandleAuthentication()) return;
                    break;
                default:
                    Console.WriteLine("Некорректный ввод.");
                    return;
            }

            PlayGame();
        }

        static void HandleRegistration()
        {
            Console.Write("Введите логин: ");
            string username = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string password = Console.ReadLine();

            _userService.RegisterUser(username, password);
            Console.WriteLine("Регистрация успешна!");
        }

        static bool HandleAuthentication()
        {
            while (true)
            {
                Console.Write("Введите логин: ");
                string username = Console.ReadLine();
                Console.Write("Введите пароль: ");
                string password = Console.ReadLine();

                if (_userService.AuthenticateUser(username, password))
                    return true;
                Console.WriteLine("Неверный логин или пароль. Попробуйте снова.");
            }
        }

        static void PlayGame()
        {
            var game = new Game();
            Console.WriteLine("Игра началась! Угадай 4-значное число без повторяющихся цифр.");

            while (true)
            {
                Console.Write("Введи свою догадку: ");
                string guess = Console.ReadLine();

                if (guess.Length != 4)
                {
                    Console.WriteLine("Число должно состоять из 4 цифр. Попробуй снова.");
                    continue;
                }

                var (correctNumbers, correctPositions) = game.CheckGuess(guess);
                Console.WriteLine($"Угадано цифр: {correctNumbers}, на своих местах: {correctPositions}");

                if (correctPositions == 4)
                {
                    Console.WriteLine($"Поздравляю! Ты угадал число {game.SecretNumber} за {game.Attempts} попыток.");
                    _gameService.UpdateRanking(game.Attempts);
                    break;
                }
            }
        }
    }
}