using Chijimu.API.Services;
using Chijimu.API.Services.Interfaces;
using Chijimu.Data.Contexts;
using Chijimu.Data.Repositories;
using Chijimu.Data.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<UrlContext>(options =>
    UrlContext.SetConnectionString(options, builder.Configuration));

builder.Services.AddTransient<IUrlService, UrlService>();
builder.Services.AddTransient<IUrlRepository, UrlRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();