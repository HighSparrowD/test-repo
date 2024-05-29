using UserManager.Main.Contracts.Models;
using UserManager.Main.Entities.Users;

namespace UserManager.Data.Interfaces.Repositories.Users;

public interface IUserRepository
{
    Task<ICollection<User>> GetUsersAsync(CancellationToken cancellation = default);

    Task<User> GetUserAsync(int id, CancellationToken cancellation = default);

    Task<User> CreateUserAsync(UserNew model, CancellationToken cancellation = default);

    Task<User> SetUserAsync(int userId, UserUpdate model, CancellationToken cancellation = default);

    Task<User> SetPasswordAsync(int userId, PasswordUpdate model, CancellationToken cancellation = default);

    Task<User> SetPasswordAsync(int userId, string password, CancellationToken cancellation = default);

    User SetPassword(User user, string password, CancellationToken cancellation = default);

    Task<bool> LoginUserAsync(int userId, string password, CancellationToken cancellation = default);

    Task<User> DeleteUserAsync(int userId, CancellationToken cancellation = default);

    Task RemoveOldUsersFromDbAsync(CancellationToken cancellation = default);
}
