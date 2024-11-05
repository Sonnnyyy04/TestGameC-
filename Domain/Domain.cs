namespace FOURINROW.Domain
{
    // public class User
    // {
    //     public string Username { get; set; }
    //     public string PasswordHash { get; set; }
    // }

    public class Game
    {
        public string SecretNumber { get; private set; }
        public int Attempts { get; private set; }

        public Game()
        {
            SecretNumber = GenerateNumber();
            Attempts = 0;
        }

        public string GenerateNumber()
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

        public (int correctNumbers, int correctPositions) CheckGuess(string guess)
        {
            Attempts++;
            
            int correctNumbers = 0, correctPositions = 0;

            for (int i = 0; i < 4; i++)
            {
                if (guess[i] == SecretNumber[i])
                {
                    correctPositions++;
                }
            }

            foreach (char c in guess)
            {
                if (SecretNumber.Contains(c))
                {
                    correctNumbers++;
                }
            }

            return (correctNumbers, correctPositions);
        }
    }
}