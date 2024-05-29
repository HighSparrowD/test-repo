using UserManager.Main.Contracts.Models;
using Models = UserManager.Main.Contracts.Models;

namespace UserManager.Data.Interfaces.Services.Users;

public interface IUserService
{
    Task<ICollection<Models.Users.User?>> GetUsers(CancellationToken cancellation = default);

    Task<Models.Users.User> GetUser(int id, CancellationToken cancellation = default);

    Task<Models.Users.User> CreateUser(UserNew model, CancellationToken cancellation = default);
    
    Task<Models.Users.User> SetUser(int id, UserUpdate model, CancellationToken cancellation = default);

    Task<Models.Users.User> SetPassword(int id, PasswordUpdate model, CancellationToken cancellation = default);

    Task<Models.Users.User> DeleteUser(int userId, CancellationToken cancellation = default);
}
