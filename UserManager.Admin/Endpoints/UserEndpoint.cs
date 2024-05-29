using System.Text;
using System.Text.Json;
using UserManager.Admin.Internals;
using UserManager.Main.Contracts.Endpoints.Users;
using UserManager.Main.Contracts.Models;
using UserManager.Main.Contracts.Models.Users;

namespace UserManager.Admin.Endpoints
{
    public class UserEndpoint : IUserEndpoint
    {
        private static HttpClient sharedClient = new()
        {
            BaseAddress = new Uri("https://localhost:44347/"),
        };

        public async Task<User?> CreateUser(UserNew model, CancellationToken cancellation = default)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await sharedClient.PostAsync(ApiUrls.UserAdd, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<User>(jsonResponse);

            return user;
        }

        public async Task<User?> DeleteUser(int userId, CancellationToken cancellation = default)
        {
            using HttpResponseMessage response = await sharedClient.DeleteAsync(ApiUrls.UserDelete, cancellation);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<User>(jsonResponse);

            return users;
        }

        public async Task<User> GetUser(int id, CancellationToken cancellation = default)
        {
            using HttpResponseMessage response = await sharedClient.GetAsync(ApiUrls.UserGet, cancellation);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<User>(jsonResponse);

            if (user == null)
                throw new ArgumentException("User does not exist!");

            return user;
        }

        public async Task<ICollection<User>?> GetUsers(CancellationToken cancellation = default)
        {
            using HttpResponseMessage response = await sharedClient.GetAsync(ApiUrls.UserList, cancellation);

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<ICollection<User>>(jsonResponse);

            return users;
        }

        public async Task<User?> SetPassword(int id, PasswordUpdate model, CancellationToken cancellation = default)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await sharedClient.PostAsync(string.Format(ApiUrls.UserPasswordSet, id), content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<User>(jsonResponse);

            return user;
        }

        public async Task<User?> SetUser(int id, UserUpdate model, CancellationToken cancellation = default)
        {
            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = await sharedClient.PutAsync(string.Format(ApiUrls.UserSet, id), content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<User>(jsonResponse);

            return user;
        }
    }
}
