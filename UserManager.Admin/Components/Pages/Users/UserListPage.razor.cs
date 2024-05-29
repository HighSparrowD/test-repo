using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;
using UserManager.Admin.Internals;
using UserManager.Main.Contracts.Endpoints.Users;
using UserManager.Main.Contracts.Models.Users;

namespace UserManager.Admin.Components.Pages.Users
{
    public partial class UserListPage
    {
        [Inject]
        IUserEndpoint UserEndpoint { get; set; } = default!;

        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        public List<User>? Users { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            var result = await UserEndpoint.GetUsers();
            Users = result?.ToList();
        }

        private void OnAddClick()
        {
            NavigationManager.NavigateTo(PageUrls.UserAddUrl);
        }

        private void OnItemSelected(User user)
        {
            NavigationManager.NavigateTo(string.Format(PageUrls.UserEditUrl, user.Id));
        }
    }
}
