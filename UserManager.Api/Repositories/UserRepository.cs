using Microsoft.EntityFrameworkCore;
using UserManager.Data.Data;
using UserManager.Data.Interfaces.Repositories.Users;
using UserManager.Data.Interfaces.Services;
using UserManager.Main.Contracts.Models;
using UserManager.Main.Utilities.Security;
using Entities = UserManager.Main.Entities.Users;

namespace UserManager.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly int oldSpanInDays = 60;

        private UserDbContext _context;
        private ITimestampService _timeStamp;
        
        public UserRepository(UserDbContext context, ITimestampService timestamp)
        {
            _context = context;
            _timeStamp = timestamp;
        }

        public async Task<Entities.User> CreateUserAsync(UserNew model, CancellationToken cancellation = default)
        {
            var user = new Entities.User
            {
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Password = PasswordManager.HashPassword(model.Password),
                Created = _timeStamp.GetUtcNow()
            };

            await _context.Users.AddAsync(user, cancellation);
            await _context.SaveChangesAsync(cancellation);

            return user;
        }

        public async Task<Entities.User> GetUserAsync(int userId, CancellationToken cancellation = default)
        {
            var user = await _context.Users.Where(u => u.Id == userId)
                .FirstOrDefaultAsync(cancellation);

            if (user == null)
                throw new ArgumentException($"User with Id {userId} does not exist!");

            return user;
        }

        public async Task<ICollection<Entities.User>> GetUsersAsync(CancellationToken cancellation = default)
        {
            var users = await _context.Users
                .ToListAsync(cancellation);

            return users;
        }

        public async Task<Entities.User> SetUserAsync(int userId, UserUpdate model, CancellationToken cancellation = default)
        {
            // I am using standard EF, because .ExecuteUpdate() does not return entity I require.
            var user = await GetUserAsync(userId, cancellation);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Updated = _timeStamp.GetUtcNow();

            await _context.SaveChangesAsync(cancellation);
            return user;
        }

        public async Task<Entities.User> DeleteUserAsync(int userId, CancellationToken cancellation = default)
        {
            // I am using standard EF, because .ExecuteUpdate() does not return entity I require.
            var user = await GetUserAsync(userId, cancellation);

            user.Deleted = _timeStamp.GetUtcNow();

            await _context.SaveChangesAsync(cancellation);
            return user;
        }

        public async Task<Entities.User> SetPasswordAsync(int userId, PasswordUpdate model, CancellationToken cancellation = default)
        {
            return await SetPasswordAsync(userId, model.Password, cancellation);
        }

        public async Task<Entities.User> SetPasswordAsync(int userId, string password, CancellationToken cancellation = default)
        {
            var user = await GetUserAsync(userId, cancellation);

            var hashedPassword = PasswordManager.HashPassword(password);

            user.Password = hashedPassword;

            await _context.SaveChangesAsync(cancellation);
            return user;
        }

        public Entities.User SetPassword(Entities.User user, string password, CancellationToken cancellation = default)
        {
            var hashedPassword = PasswordManager.HashPassword(password);

            user.Password = hashedPassword;

            _context.SaveChanges();
            return user;
        }

        public async Task<bool> LoginUserAsync(int userId, string password, CancellationToken cancellation = default)
        {
            var storedHash = await _context.Users.Where(u => u.Id == userId)
                .Select(u => u.Password)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(storedHash))
                throw new ArgumentException($"User with Id {userId} does not exist!");

            var logedIn = PasswordManager.VerifiPassword(storedHash, password);

            return logedIn;
        }

        public async Task RemoveOldUsersFromDbAsync(CancellationToken cancellation = default)
        {
            var deletedUsersCount = await _context.Users.Where(u => u.Deleted != null && 
            (u.Deleted.Value - _timeStamp.GetUtcNow()).TotalDays >= oldSpanInDays)
                .ExecuteDeleteAsync();

            await Console.Out.WriteLineAsync($"{deletedUsersCount} users deleted from DB");

            await _context.SaveChangesAsync(cancellation);
        }
    }
}
