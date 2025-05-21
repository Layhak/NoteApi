using Dapper;
using NotesApi.Dto.Note;
using NotesApi.Models;

namespace NotesApi.Data;

public interface INotesRepository
{
    Task<IEnumerable<Note>> GetAllByUserIdAsync(int userId);
    Task<Note?> GetByIdAsync(int id, int userId);
    Task<Note> CreateAsync(NoteCreateDto noteDto, int userId);
    Task<Note?> UpdateAsync(int id, NoteUpdateDto noteDto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
}

public class NotesRepository : INotesRepository
{
    private readonly DatabaseConnection _db;

    public NotesRepository(DatabaseConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Note>> GetAllByUserIdAsync(int userId)
    {
        using var connection = _db.CreateConnection();
        var sql = "SELECT * FROM Notes WHERE UserId = @UserId ORDER BY UpdatedAt DESC";
        return await connection.QueryAsync<Note>(sql, new { UserId = userId });
    }

    public async Task<Note?> GetByIdAsync(int id, int userId)
    {
        using var connection = _db.CreateConnection();
        var sql = "SELECT * FROM Notes WHERE Id = @Id AND UserId = @UserId";
        return await connection.QuerySingleOrDefaultAsync<Note>(sql, new { Id = id, UserId = userId });
    }

    public async Task<Note> CreateAsync(NoteCreateDto noteDto, int userId)
    {
        using var connection = _db.CreateConnection();
        var sql = """
                  INSERT INTO Notes (Title, Content, UserId, CreatedAt, UpdatedAt)
                  OUTPUT INSERTED.*
                  VALUES (@Title, @Content, @UserId, GETDATE(), GETDATE())
                  """;

        return await connection.QuerySingleAsync<Note>(sql, new
        {
            noteDto.Title,
            noteDto.Content,
            UserId = userId
        });
    }

    public async Task<Note?> UpdateAsync(int id, NoteUpdateDto noteDto, int userId)
    {
        using var connection = _db.CreateConnection();
        var sql = """
                  UPDATE Notes 
                  SET Title = @Title, Content = @Content, UpdatedAt = GETDATE()
                  OUTPUT INSERTED.*
                  WHERE Id = @Id AND UserId = @UserId
                  """;

        return await connection.QuerySingleOrDefaultAsync<Note>(sql, new
        {
            Id = id,
            noteDto.Title,
            noteDto.Content,
            UserId = userId
        });
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        using var connection = _db.CreateConnection();
        var sql = "DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId";
        var affectedRows = await connection.ExecuteAsync(sql, new { Id = id, UserId = userId });
        return affectedRows > 0;
    }
}