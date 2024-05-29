using System.Text.Json.Serialization;

namespace UserManager.Main.Contracts.Models;

public class UserUpdate
{
    [JsonPropertyName("firstName")]
    public required string FirstName { get; set; }

    [JsonPropertyName("lastName")]
    public required string LastName { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }
}
