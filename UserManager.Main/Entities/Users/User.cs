using Models = UserManager.Main.Contracts.Models.Users;

namespace UserManager.Main.Entities.Users;

public class User
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required string Email { get; set; }

    public required string Password { get; set; }

    public DateTime Created { get; set; } 

    public DateTime? Updated { get; set; } 

    public DateTime? Deleted { get; set; }


    public static explicit operator Models.User?(User? user)
    {
        if (user == null)
            return null;

        return new Models.User
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Password = user.Password,
            Email = user.Email,
            Created = user.Created,
            Updated = user.Updated,
            Deleted = user.Deleted
        };
    }

    public static implicit operator User?(Models.User? user)
    {
        if (user == null)
            return null;

        return new Models.User
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Password = user.Password,
            Email = user.Email,
            Created = user.Created,
            Updated = user.Updated,
            Deleted = user.Deleted
        };
    }
}
