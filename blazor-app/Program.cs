using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using SchengenCalculator;
using SchengenCalculator.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// API base address — change to your deployed API URL for production
var apiBase = builder.Configuration["ApiBaseUrl"]
    ?? "https://localhost:7001/";

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBase) });
builder.Services.AddMudServices();
builder.Services.AddScoped<AirportService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TripApiService>();

await builder.Build().RunAsync();
