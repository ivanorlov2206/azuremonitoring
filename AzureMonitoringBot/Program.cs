using AzureMonitoringBot.Models;
using AzureMonitoringBot.Services;
using AzureMonitoringBot.Services.ChatServices;
using AzureMonitoringBot.Services.ChatServices.Telegram;
using AzureMonitoringBot.Services.Notification;
using AzureMonitoringBot.Services.Queues;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddHostedService<WebhookService>();
builder.Services.AddHostedService<NotificationService>();

builder.Services.Configure<AzureMonitoringDatabaseSettings>(
    builder.Configuration.GetSection("Database"));
builder.Services.Configure<TelegramApiSettings>(
    builder.Configuration.GetSection("Telegram"));

builder.Services.AddSingleton<IMessengerApiClient, TelegramApiClient>();
builder.Services.AddSingleton<IUsersService, UsersMongoService>();
builder.Services.AddSingleton<IUsersConnectionDataService, UsersConnectionDataMongoService>();
builder.Services.AddSingleton<IQueuesService, QueuesService>();
builder.Services.AddSingleton<IUserCommunicationService, UserCommunicationService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var app = builder.Build();

//app.UseHttpsRedirection();

app.MapControllers();

app.Run();
