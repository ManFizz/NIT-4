namespace ToDo_RestAPI.Auth;


public class TokenResponse
{
    public required string access_token { get; set; }
    public required int expires_in { get; set; }
    public required int refresh_expires_in { get; set; }
    public required string refresh_token { get; set; }
    public required string token_type { get; set; }
}