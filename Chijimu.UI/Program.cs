using Chijimu.UI.Configuration;
using Chijimu.UI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<ChijimuApiService>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection(ApiSettings.Name));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();