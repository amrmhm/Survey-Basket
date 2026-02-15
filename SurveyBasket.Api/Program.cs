

using Hangfire;
using HangfireBasicAuthenticationFilter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;
using Serilog;
using SurveyBasket.Api;

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
    // Add Scalar Ui 
    app.MapScalarApiReference();

    //to Add Swagger After Removed Used 
    // app.UseSwaggerUI(option => option.SwaggerEndpoint("/openapi/v1.json", "v1"));
    //Add Open Api 

    app.MapOpenApi();
    //Add Authorization OpenApi
    // .RequireAuthorization("AdminOnly");

    //app.UseSwagger();
    //app.UseSwaggerUI(options => {
    //    var descriptions = app.DescribeApiVersions();
    //    foreach (var description in descriptions)
    //        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    //    });
}
//app.MapScalarApiReference();
//app.MapOpenApi();


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
        ],
    DashboardTitle = "Survey Basket Dashboard",
    //IsReadOnlyFunc = (DashboardContext context) => true // To Disable Delete In Dashboard 


}
);
//Accessing services found in the DI container (ICollection service)
//Note : Donont forget to add scoped service (services.AddScoped<INotificationService, NotificationService>());

var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
var scope = scopeFactory.CreateScope();
var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
RecurringJob.AddOrUpdate("notificationService", () => notificationService.SendNewPollNotification(null), Cron.Daily);
// Or Use Cron Expression to Determine Specific time
//RecurringJob.AddOrUpdate("notificationService", () => notificationService.SendNewPollNotification(null) , "9 0 * * *");
app.MapControllers();

//Add Health Check Root Pattern 

app.MapHealthChecks("health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapHealthChecks("health-api", new HealthCheckOptions
{
    Predicate = c => c.Tags.Contains("Api"), // Add Spesific Api
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
//Add Rate Limitter
app.UseRateLimiter();

//Use Static Files TO Open SSl Certificate
app.UseStaticFiles();

app.Run();
