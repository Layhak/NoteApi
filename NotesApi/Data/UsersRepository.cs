using Dapper;
using NotesApi.Models;

namespace NotesApi.Data;

public interface IUsersRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<User> CreateAsync(string username, string passwordHash);
    Task<User?> GetByIdAsync(int id);
}

public class UsersRepository : IUsersRepository
{
    private readonly DatabaseConnection _db;

    public UsersRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        using var connection = _db.CreateConnection();
        var sql = "SELECT * FROM Users WHERE Username = @Username";
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Username = username });
    }

    public async Task<User> CreateAsync(string username, string passwordHash)
    {
        using var connection = _db.CreateConnection();
        var sql = """
                  INSERT INTO Users (Username, PasswordHash, CreatedAt)
                  OUTPUT INSERTED.*
                  VALUES (@Username, @PasswordHash, GETDATE())
                  """;

        return await connection.QuerySingleAsync<User>(sql, new {
            Username = username,
            PasswordHash = passwordHash
        });
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        using var connection = _db.CreateConnection();
        var sql = "SELECT * FROM Users WHERE Id = @Id";
        return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }
}