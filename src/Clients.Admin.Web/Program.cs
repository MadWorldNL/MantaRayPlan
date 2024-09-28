using MadWorldNL.MantaRayPlan.Web;
using MadWorldNL.MantaRayPlan.Web.Configurations;
using MadWorldNL.MantaRayPlan.Web.Services.MessageBuses;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddOptions<ApiSettings>()
    .Configure(builder.Configuration.GetSection(ApiSettings.Key).Bind);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient(ApiTypes.AdminBff, (serviceProvider, client) =>
    {
        var apiUrlsOption = serviceProvider.GetService<IOptions<ApiSettings>>()!;
        client.BaseAddress = new Uri(apiUrlsOption.Value.Address);
    });

builder.Services.AddScoped<IMessageBusService, MessageBusService>();

await builder.Build().RunAsync();