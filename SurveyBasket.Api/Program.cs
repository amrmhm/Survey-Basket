

using Hangfire;
using Hangfire.Dashboard;
using HangfireBasicAuthenticationFilter;
using MapsterMapper;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Api;
using SurveyBasket.Api.Contract;
using SurveyBasket.Api.Persistence;
using SurveyBasket.Api.Services;
using System.Net;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDependancy(builder.Configuration);

//Add Response Caching 

//builder.Services.AddResponseCaching();
//Add Output Cache

//builder.Services.AddOutputCache(option =>
//{
//    option.AddPolicy("Polls",
//    c => c.Expire(TimeSpan.FromSeconds(60))
//    .Tag("avalibaleQuestion"));
//});

// Add Memory Caching 
//builder.Services.AddMemoryCache();

// Add DistributedMemory Caching 
//builder.Services.AddDistributedMemoryCache();

//Add SeriLog to Read From AppSetting
builder.Host.UseSerilog((context, configration) =>
configration.ReadFrom.Configuration(context.Configuration)


);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Add Middleware To add http request To Log File
app.UseSerilogRequestLogging();

// Add Middleware ExHandler
app.UseExceptionHandler();


app.UseHttpsRedirection();
//UseAuthorization ???  police ???? ???? 
//default Police

app.UseCors();

//Add Police Or Multi Police
//app.UseCors("MyPolice1");
//app.UseCors("MyPolice2");

app.UseAuthorization();

//Add Response Caching after UseCors and UseAuthorization
//app.UseResponseCaching();

// Add Output Cache 

//app.UseOutputCache();

//Add Hangfire Dashboard And Add Authuentcation to Hangfire Dashboard
app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization =
    [
        new HangfireCustomBasicAuthenticationFilter
        {
            User = app.Configuration.GetValue<string>("HangfireSettings:UserName"),
            Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
        }
        ] ,
    DashboardTitle = "Survey Basket Dashboard",
    //IsReadOnlyFunc = (DashboardContext context) => true // To Disable Delete In Dashboard 


}
);
//Accessing services found in the DI container (ICollection service)
//Note : Donont forget to add scoped service (services.AddScoped<INotificationService, NotificationService>());

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var scope = scopeFactory.CreateScope();
var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
RecurringJob.AddOrUpdate("notificationService", () => notificationService.SendNewPollNotification(null),Cron.Daily);
// Or Use Cron Expression to Determine Specific time
//RecurringJob.AddOrUpdate("notificationService", () => notificationService.SendNewPollNotification(null) , "9 0 * * *");
app.MapControllers();

app.Run();
