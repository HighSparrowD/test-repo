using UserManager.Main.Contracts.Models;
using UserManager.Main.Contracts.Models.Users;

namespace UserManager.Main.Contracts.Endpoints.Users
{
    public interface IUserEndpoint
    {
        Task<ICollection<User>?> GetUsers(CancellationToken cancellation = default);

        Task<User> GetUser(int id, CancellationToken cancellation = default);

        Task<User?> CreateUser(UserNew model, CancellationToken cancellation = default);

        Task<User?> SetUser(int id, UserUpdate model, CancellationToken cancellation = default);

        Task<User?> SetPassword(int id, PasswordUpdate model, CancellationToken cancellation = default);

        Task<User?> DeleteUser(int userId, CancellationToken cancellation = default);
    }
}
