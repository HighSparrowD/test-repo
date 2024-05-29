using System.Text.Json.Serialization;

namespace UserManager.Main.Contracts.Models;

public class PasswordUpdate
{
    [JsonPropertyName("password")]
    public required string Password { get; set; }
}
