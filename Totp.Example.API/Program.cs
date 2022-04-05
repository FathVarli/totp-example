using HashidsNet;
using Totp.Example.API.ServiceLayer;

var builder = WebApplication.CreateBuilder(args);


var services = builder.Services;
services.AddControllers();

services.AddSingleton<IHashids>(_ => new Hashids("Fatih",11));
services.AddSingleton<IOtpService, OtpService>();

var app = builder.Build();

app.MapControllers();

app.Run();