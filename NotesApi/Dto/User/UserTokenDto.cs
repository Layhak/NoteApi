namespace NotesApi.Dto.User;

public class UserTokenDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}