using Microsoft.AspNetCore.Components;
using UserManager.Admin.ViewModel;
using UserManager.Main.Contracts.Endpoints.Users;
using UserManager.Main.Contracts.Models;

namespace UserManager.Admin.Components.Pages.Users
{
    public partial class UserEditPage
    {
        [Parameter]
        public int? Id { get; set; }

        [Inject]
        IUserEndpoint UserEndpoint { get; set; } = default!;

        public bool ShowPasswordInput { get; set; }

        public UserViewModel Model { get; set; } = default!;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            Model = new();

            if (Id != null)
            {
                var user = await UserEndpoint.GetUser((int)Id);

                Model.SetFromDto(user);
            }
        }

        protected async Task Submit()
        {
            var source = CancellationTokenSource.CreateLinkedTokenSource();
            if (Id is null)
            {
                await AddUser(source.Token);
            }
            else
            {
                await UpdateUser(source.Token); ;
            }
        }

        private async Task AddUser(CancellationToken cancellation)
        {
            var newModel = new UserNew
            {
                LastName = Model.LastName,
                FirstName = Model.FirstName,
                Email = Model.Email,
                Password = Model.Password
            };

            var user = await UserEndpoint.CreateUser(newModel, cancellation);

            if(user != null)
                Model.SetFromDto(user);
        }

        private async Task UpdateUser(CancellationToken cancellation)
        {
            var updateModel = new UserUpdate
            {
                LastName = Model.LastName,
                FirstName = Model.FirstName,
                Email = Model.Email
            };

            var user = await UserEndpoint.SetUser((int)Id!, updateModel, cancellation);

            if (user != null)
                Model.SetFromDto(user);
        }

        private async Task ResetPassword()
        {
            var source = CancellationTokenSource.CreateLinkedTokenSource();
            var updateModel = new PasswordUpdate
            {
                Password = Model.Password
            };

            var user = await UserEndpoint.SetPassword((int)Id!, updateModel, source.Token);

            if (user != null)
                Model.SetFromDto(user);
        }

        private void TogglePasswordShow() => ShowPasswordInput = !ShowPasswordInput;
    }
}
