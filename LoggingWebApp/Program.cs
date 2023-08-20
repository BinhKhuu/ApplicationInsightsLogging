using BlazorApplicationInsights;
using LoggingWebApp;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:7189") });

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
});

builder.Services.AddBlazorApplicationInsights(async applicationInsights =>
{
	var telemetryItem = new TelemetryItem()
	{
		Tags = new Dictionary<string, object>()
		{
			{ "ai.cloud.role", "SPA" },
			{ "ai.cloud.roleInstance", "Blazor Wasm" },
		}
	};

	await applicationInsights.AddTelemetryInitializer(telemetryItem);
});

await builder.Build().RunAsync();
