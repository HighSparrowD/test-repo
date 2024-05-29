using UserManager.Data.Interfaces.Repositories.Users;
using UserManager.Data.Interfaces.Services.Users;
using UserManager.Main.Contracts.Models;
using UserManager.Main.Contracts.Models.Users;
using Models = UserManager.Main.Contracts.Models;
using Entities = UserManager.Main.Entities;

namespace UserManager.Services.Users
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<Models.Users.User> CreateUser(UserNew model, CancellationToken cancellation = default)
        {
            var user = await _userRepo.CreateUserAsync(model);
            var convertedUser = ConvertUser(user);

            return convertedUser;
        }

        public async Task<Models.Users.User> GetUser(int id, CancellationToken cancellation = default)
        {
            var user = await _userRepo.GetUserAsync(id);
            var convertedUser = ConvertUser(user);

            return convertedUser;
        }

        public async Task<ICollection<Models.Users.User?>> GetUsers(CancellationToken cancellation = default)
        {
            var users = await _userRepo.GetUsersAsync();

            return users.Select(u => (Models.Users.User?)u)
                .ToList();
        }

        public async Task<Models.Users.User> SetUser(int id, UserUpdate model, CancellationToken cancellation = default)
        {
            var user = await _userRepo.SetUserAsync(id, model);
            var convertedUser = ConvertUser(user);

            return convertedUser;
        }

        public async Task<Models.Users.User> SetPassword(int id, PasswordUpdate model, CancellationToken cancellation = default)
        {
            var user = await _userRepo.SetPasswordAsync(id, model, cancellation);
            var convertedUser = ConvertUser(user);

            return convertedUser;
        }

        public async Task<Models.Users.User> DeleteUser(int id, CancellationToken cancellation = default)
        {
            var user = await _userRepo.DeleteUserAsync(id);
            var convertedUser = ConvertUser(user);

            return convertedUser;
        }

        private Models.Users.User ConvertUser(Entities.Users.User user) => (Models.Users.User?)user??
            throw new ArgumentException($"User does not exist!");
    }
}
