

using MapsterMapper;
using Serilog;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Api;
using SurveyBasket.Api.Contract;
using SurveyBasket.Api.Persistence;
using SurveyBasket.Api.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDependancy(builder.Configuration);

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
app.MapControllers();

app.Run();
