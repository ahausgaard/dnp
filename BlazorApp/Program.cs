using BlazorApp.Auth;
using BlazorApp.Components;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddServiceDefaults();

builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// "webapi" is resolved by service discovery: Aspire supplies the address when run
// through the AppHost; otherwise it comes from "Services:webapi" in appsettings.json.
var apiAddress = new Uri("https+http://webapi");
builder.Services.AddHttpClient<IPostService, HttpPostService>(c => c.BaseAddress = apiAddress);
builder.Services.AddHttpClient<ICommentService, HttpCommentService>(c => c.BaseAddress = apiAddress);
builder.Services.AddHttpClient<IUserService, HttpUserService>(c => c.BaseAddress = apiAddress);

// Authentication: one SimpleAuthProvider per user circuit, shared by the login
// dialog (which calls Login) and Blazor (which asks it who is logged in).
builder.Services.AddHttpClient("webapi", c => c.BaseAddress = apiAddress);
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped(sp => new SimpleAuthProvider(
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("webapi")));
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<SimpleAuthProvider>());

builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found",
    createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapDefaultEndpoints();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();