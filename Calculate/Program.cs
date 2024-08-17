using System.Globalization;
using Calculate.Components;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
	var defaultCulture = new CultureInfo("en-US");
	options.DefaultRequestCulture = new RequestCulture(defaultCulture);
	options.SupportedCultures = new[] { defaultCulture };
	options.SupportedUICultures = new[] { defaultCulture };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Apply the localization settings
app.UseRequestLocalization();

app.MapRazorComponents<App>()
		.AddInteractiveServerRenderMode();

app.Run();
