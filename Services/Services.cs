using MySql.Data.MySqlClient;
namespace FOURINROW.Services

{
  public class UserService
  {
    private readonly string _connectionString;

    public UserService(string connectionString)
    {
      _connectionString = connectionString;
    }

    public bool AuthenticateUser(string username, string password)
    {
      using (var connection = new MySqlConnection(_connectionString))
      {
        connection.Open();
        var command = new MySqlCommand(
            "SELECT password_hash FROM users WHERE username = @username",
            connection);
        command.Parameters.AddWithValue("@username", username);

        var result = command.ExecuteScalar();
        if (result == null) return false;

        string storedHash = result.ToString();
        return BCrypt.Net.BCrypt.Verify(password, storedHash);
      }
    }

    public void RegisterUser(string username, string password)
    {
      string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

      using (var connection = new MySqlConnection(_connectionString))
      {
        connection.Open();
        var command = new MySqlCommand(
            "INSERT INTO users (username, password_hash) VALUES (@username, @passwordHash)",
            connection);

        command.Parameters.AddWithValue("@username", username);
        command.Parameters.AddWithValue("@passwordHash", hashedPassword);
        command.ExecuteNonQuery();
      }
    }
  }
  public class GameService
  {
    private readonly string _rankingFilePath;

    public GameService(string rankingFilePath)
    {
      _rankingFilePath = rankingFilePath;
    }

    public void UpdateRanking(int attempts)
    {
      var rankings = new List<int>();
      if (File.Exists(_rankingFilePath))
      {
        rankings = File.ReadAllLines(_rankingFilePath)
                       .Select(int.Parse)
                       .ToList();
      }

      rankings.Add(attempts);
      rankings.Sort();

      File.WriteAllLines(_rankingFilePath, rankings.Select(x => x.ToString()));

      int position = rankings.IndexOf(attempts) + 1;
      Console.WriteLine($"Ты занимаешь {position}-е место в рейтинге с {attempts} попытками.");
    }
  }
}