using Havit.Blazor.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.VisualBasic;
using UserManager.Admin.Components;
using UserManager.Admin.Endpoints;
using UserManager.Main.Contracts.Endpoints.Users;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<IUserEndpoint, UserEndpoint>();

builder.Services.AddRazorComponents();
builder.Services.AddRazorPages();
builder.Services.AddHxServices();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();