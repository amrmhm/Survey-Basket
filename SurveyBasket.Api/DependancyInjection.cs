using Hangfire;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Persistence;
using SurveyBasket.Api.Setting;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace SurveyBasket.Api;

public static class  DependancyInjection
{

    public static IServiceCollection AddDependancy ( this IServiceCollection services , IConfiguration configuration)
    {

        services.AddControllers();
        // add Hybrid Cache

        services.AddHybridCache();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddSwaggerServices()
            .AddMapsterServices()
            .AddFluentValidationServices()
            .AddCorsPoliceServices(configuration)
            .AddConnectionString(configuration)
            .AddAuthenticationConfig(configuration)
            .AddBackgroundJob(configuration);

        services.AddScoped<IPollsServices, PollsServices>();
        services.AddScoped<IEmailSender, EmailServices>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IUserServices, UserServices>();
        services.AddScoped<IAuthServices, AuthServices>();
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<IQuestionServices, QuestionServices>();
        services.AddScoped<IVoteServices, VoteServices>();
        services.AddScoped<IResualtServices, ResualtServices>();
        services.AddScoped<IRoleServices, RoleServices>();


        // services.AddScoped<ICacheServices, CacheServices>();

        //Add AddHttpContextAccessor to Use In Auth Services to Get Origin
        services.AddHttpContextAccessor();

        // Add AddExHandler
        services.AddExceptionHandler<GLobleExceptionHandler>();
        services.AddProblemDetails();

       

        return services;
    }
    private static IServiceCollection AddConnectionString(this IServiceCollection services, IConfiguration configuration)
    {
        //Add ConnectionString

        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("ConnectionString Was Not Found");
        services.AddDbContext<ApplicationDbContext>(option =>
        option.UseSqlServer(connectionString));

        return services;
    }

    private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
    private static IServiceCollection AddMapsterServices(this IServiceCollection services)
    {

        //Add Mapster

        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));
        return services;
    }
    private static IServiceCollection AddFluentValidationServices(this IServiceCollection services)
    {
        //Add Fluent Validation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddFluentValidationAutoValidation();

        return services;
    }
    private static IServiceCollection AddCorsPoliceServices(this IServiceCollection services,IConfiguration configuration)
    {
        var corsSetting = configuration.GetSection("AllowedOrigin").Get<string[]>();
        //Add CORS Police
        services.AddCors(option =>
        
            option.AddDefaultPolicy(builder =>
            builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins(corsSetting!)));

        //================= AddPolice Or Multi Police  =====================//

        //var corsSetting =configuration.GetSection("AllowedOrigin").Get<string[]>();
        ////Add CORS Police
        //services.AddCors(option => {
        //    option.AddPolicy("MyPolice1", builder =>
        //    builder
        //    .AllowAnyHeader()
        //    .AllowAnyMethod()
        //    .WithOrigins(corsSetting!)
        //    //Or All Origins // .AllowAnyOrigins()
        //    //Or Spacific Methods //  .WithMethods("PUT,GET")
        //    //Or Spacific Headers // .WithHeaders(HeaderNames.Origin , HeaderNames.Accept, HeaderNames.ContentType) //Or .WithHeader ("accept", "content-type", "origin")
        //    );
        //    option.AddPolicy("MyPolice2", builder =>
        //builder
        //.AllowAnyHeader()
        //.AllowAnyMethod()
        //.WithOrigins(corsSetting!)
        ////Or All Origins // .AllowAnyOrigins()
        ////Or Spacific Methods //  .WithMethods("PUT,GET")
        ////Or Spacific Headers // .WithHeaders(HeaderNames.Origin , HeaderNames.Accept, HeaderNames.ContentType) //Or .WithHeader ("accept", "content-type", "origin")
        //);
        //});

        return services;
    }
    private static IServiceCollection AddAuthenticationConfig(this IServiceCollection services , IConfiguration configuration)
    {  //Add Identity Config
        services.AddIdentity<ApplicationUser, ApplicationRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

        //Add Permission Policy Services

        services.AddTransient<IAuthorizationHandler ,PermissionAuthorizationHandler >();
        services.AddTransient<IAuthorizationPolicyProvider , PermissionAuthorizationPolicyProvider>();

        // Add Mail Setting TO Use By IOption<MailSetting>

        services.Configure<MailSetting>(configuration.GetSection(nameof(MailSetting)));

        //Add Jwt Option To Use In Services Or Controller

        // services.Configure<JwtOption>(configuration.GetSection(JwtOption.SectionName)); // ==>>||

        //Or Use BindConfigration Methods And ValidateDataAnnotations And ValidateOnStart TO Validate Value 
        services.AddOptions<JwtOption>().BindConfiguration(JwtOption.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

           

        //Add Jwt Option To Use In Dependecy By Get
       var jwtSetting = configuration.GetSection(JwtOption.SectionName).Get<JwtOption>();

        //Add JwtBearer 

        services.AddAuthentication(option =>
        {
            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            opt.SaveToken = true;
            opt.RequireHttpsMetadata = true;
            opt.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting!.Key)),
                ValidIssuer = jwtSetting.Issuer,
                ValidAudience = jwtSetting.Audience



                //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                //ValidIssuer = configuration["Jwt:Issuer"],
                //ValidAudience = configuration["Jwt:Audience"]
            };
        });

        //Add Change Default Identity  
        services.Configure<IdentityOptions>(options =>
        {
            // Default Password settings.
            options.Password.RequiredLength = 8;
            // Default User settings.
            options.User.RequireUniqueEmail = true;
            // Default SignIn settings.
            options.SignIn.RequireConfirmedEmail = true;

        });

        return services;
    }

    private static IServiceCollection AddBackgroundJob(this IServiceCollection services, IConfiguration configuration)
    {

        // Add Hangfire services.
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        return services;
    }
}
