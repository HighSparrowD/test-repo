using UserManager.Data.Interfaces.Repositories.Users;
using UserManager.Data.Interfaces.Services;

namespace UserManager.Services.Background
{
    public class BackgroundWorker : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly ITimestampService timestamp;

        public BackgroundWorker(IServiceProvider serviceProvider)
        {
            _services = serviceProvider;
            timestamp = new TimestampService();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await CorrectTimerAsync();
            await Beat();
        }

        public async Task CorrectTimerAsync()
        {
            var now = timestamp.GetUtcNow();

            var differenceHours = 23 - now.Hour;
            var differenceMinutes = 59 - now.Minute;
            var differenceSeconds = 59 - now.Second;

            var timespan = new TimeSpan(differenceHours, differenceMinutes, differenceSeconds);

            Console.Out.WriteLine($"UTC Time now is {now}");
            await Console.Out.WriteLineAsync($"Correcting by timespan: {timespan}");
            await Task.Delay(timespan);
        }

        public async Task Beat()
        {
            await using var scope = _services.CreateAsyncScope();

            var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var now = timestamp.GetUtcNow();

            await userRepo.RemoveOldUsersFromDbAsync();
        }
    }
}
