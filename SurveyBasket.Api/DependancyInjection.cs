using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Hangfire;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Health;
using SurveyBasket.Api.OpenApi;
using SurveyBasket.Api.Persistence;
using SurveyBasket.Api.Setting;
//using SurveyBasket.Api.Swagger;
//using SurveyBasket.Swagger;
//using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;

namespace SurveyBasket.Api;

public static class DependancyInjection
{

    public static IServiceCollection AddDependancy(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddControllers();
        // add Hybrid Cache

        services.AddHybridCache();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services
            //.AddSwaggerServices()
            .AddMapsterServices()
            .AddFluentValidationServices()
            .AddCorsPoliceServices(configuration)
            .AddConnectionString(configuration)
            .AddAuthenticationConfig(configuration)
            .AddBackgroundJob(configuration)
            .AddRateLimitter();

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

        // Add Heathy Checks Using Ui Client Package
        //services.AddHealthChecks()
        //    .AddDbContextCheck<ApplicationDbContext>("DataBase");

        //Or
        // Add Heathy Checks Using Sql Server Package And Hangfire Package And Uri Package And Add Custom HealthChecks

        var connectionString = configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("ConnectionString Was Not Found");
        services.AddHealthChecks()
            .AddSqlServer(name: "DataBase", connectionString: connectionString)
            .AddHangfire(options =>
            {
                options.MinimumAvailableServers = 1;
            })
            .AddUrlGroup(name: "Google Api", uri: new Uri("https://google.com"), tags: ["Api"], httpMethod: HttpMethod.Get) // Add Tages Add Verbs
            .AddUrlGroup(name: "Meta Api", uri: new Uri("https://facebook.com"), tags: ["Api"], httpMethod: HttpMethod.Get)
            .AddCheck<MailProviderHealthChecks>(name: "MailProvider");

        //Add Api Versioning In Url
        services.AddApiVersioning(options =>

        {

            //يمكنك استخدام اكتر من version 
            // options.ApiVersionReader = ApiVersionReader.Combine(new UrlSegmentApiVersionReader(),new HeaderApiVersionReader());
            //Add Api Versioning In Url
            // options.ApiVersionReader = new UrlSegmentApiVersionReader();

            //Add Api Versioning In Query String
            // options.ApiVersionReader = new QueryStringApiVersionReader("x-apiversion");

            //Add Api Versioning In Media Type
            // options.ApiVersionReader = new MediaTypeApiVersionReader("x-apiversion");

            //Add Api Versioning In Header
            options.ApiVersionReader = new HeaderApiVersionReader("x-apiversion");
            options.DefaultApiVersion = new ApiVersion(1);  //default api
            options.AssumeDefaultVersionWhenUnspecified = true; // لو لم يتم تحديد فيرجن اعمل ال dufualt
            options.ReportApiVersions = true;
        })
            .AddApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'V"; // format يبدا بحرف v ثم رقم الاصدار
                option.SubstituteApiVersionInUrl = true;

            });
        // Add Open Api Versiong To Reqister IApiVersionDescriptionProviderIApiVersionDescriptionProvider
        services.AddEndpointsApiExplorer()
            .AddOpenApiServices();


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

    //private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    //{

    //    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    //    services.AddEndpointsApiExplorer();
    //    services.AddSwaggerGen(
    //        option => option.OperationFilter<SwaggerDefaultValues>());// Add default Value In Header In Swagger In Version 2


    //    // Add Services Implimentation 
    //    services.AddTransient<IConfigureOptions<SwaggerGenOptions>,ConfigureSwaggerOptions>();
    //    return services;
    //}
    private static IServiceCollection AddOpenApiServices(this IServiceCollection services)
    {


        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        //  services.AddEndpointsApiExplorer()
        //    .AddOpenApi(
        //Suport Authentication
        //options =>
        //{
        //    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
        //}
        // Add Change Title-Version-Description
        //options =>
        //{
        //    options.AddDocumentTransformer((document, context, cancellationToken) =>
        //    {
        //        document.Info = new()
        //        {
        //            Title = "Checkout API",
        //            Version = "v1",
        //            Description = "API For Survey Basket."
        //        };
        //        return Task.CompletedTask;
        //    });
        //}
        //  );

        //Add Suport Version 2
        var servicesProvider = services.BuildServiceProvider();
        var apiVersionDescriptionProvider = servicesProvider.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            services.AddOpenApi(description.GroupName,
                options =>
                {
                    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                    options.AddDocumentTransformer(new InformationSchemeTransformer(description));
                    //Or Use 
                    //    options.AddDocumentTransformer((document, context, cancellationToken) =>
                    //{
                    //    document.Info = new()
                    //    {
                    //        Title = "Survey Basket",
                    //        Version = description.ApiVersion.ToString(),
                    //        Description = $"API Descriptions {(description.IsDeprecated ? "This Api Has Been Deprecated" : string.Empty)}"
                    //    };
                    //    return Task.CompletedTask;
                    //});
                });
        }

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
    private static IServiceCollection AddCorsPoliceServices(this IServiceCollection services, IConfiguration configuration)
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
    private static IServiceCollection AddAuthenticationConfig(this IServiceCollection services, IConfiguration configuration)
    {  //Add Identity Config
        services.AddIdentity<ApplicationUser, ApplicationRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();

        //Add Permission Policy Services

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        // Add Mail Setting TO Use By IOption<MailSetting>

        // services.Configure<MailSetting>(configuration.GetSection(nameof(MailSetting)));
        //Or Use 

        services.AddOptions<MailSetting>()
         .BindConfiguration(nameof(MailSetting))
         .ValidateDataAnnotations()
         .ValidateOnStart();

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
        // Add Authorization To Open Api Doc
        //services.AddAuthorization(options =>
        //options.AddPolicy("AdminOnly", option => option.RequireRole(DefaultRole.Admin))) ;

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
    private static IServiceCollection AddRateLimitter(this IServiceCollection services)
    {
        //Add Customize Rate Limit for ipAddress 
        services.AddRateLimiter(rateLimiterOption =>
        {
            rateLimiterOption.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            rateLimiterOption.AddPolicy(RateLimit.IpLimit, option =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: option.Connection.RemoteIpAddress?.ToString(),
                factory: options => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 2, //كم ريكوست ح اتعامل معه 
                    Window = TimeSpan.FromSeconds(30) // بعد كم اتعامل مع ريكوست جديد
                }));
        });
        //Add Customize Rate Limit for users 
        services.AddRateLimiter(rateLimiterOption =>
        {
            rateLimiterOption.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            rateLimiterOption.AddPolicy(RateLimit.UserLimit, option =>
            RateLimitPartition.GetFixedWindowLimiter(
                partitionKey: option.User.GetUserId(),
                factory: options => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 2, //كم ريكوست ح اتعامل معه 
                    Window = TimeSpan.FromSeconds(30) // بعد كم اتعامل مع ريكوست جديد
                }));
        });

        //Add Concurrency Limiter 

        services.AddRateLimiter(rateLimiterOption =>
        {
            rateLimiterOption.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            rateLimiterOption.AddConcurrencyLimiter(RateLimit.ConcurrencyLimit, option =>
            {
                option.PermitLimit = 2; // كم ريكوست يتنفذ في نفس الوقت
                option.QueueLimit = 1; // كم ريكوست يكون enquery
                option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; //fifo first in first out

            });
        });

        //Add Token Bucket Limiter 
        //services.AddRateLimiter(rateLimiterOption =>
        //{
        //    rateLimiterOption.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        //    rateLimiterOption.AddTokenBucketLimiter("token", option =>
        //    {
        //        option.TokenLimit = 2; //كم توكن لدي في الجردل
        //        option.QueueLimit = 1;  // كم ريكوست يكون enquery
        //        option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        //        option.ReplenishmentPeriod = TimeSpan.FromSeconds(30); // يضيف توكن للجردل كل كم
        //        option.TokensPerPeriod = 2; // يضيف عدد كم للجردل بعد يفضي
        //        option.AutoReplenishment = true; // هل هنالك مكان فاضي في الجردل عشان يضيف 
        //    });
        //});

        //Add FixedWindow 
        //services.AddRateLimiter(rateLimiterOption =>
        //{
        //    rateLimiterOption.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        //    rateLimiterOption.AddFixedWindowLimiter("fixed", option =>
        //    {
        //        option.PermitLimit = 2; //كم ريكوست ح اتعامل معه 
        //        option.Window = TimeSpan.FromSeconds(30); // بعد كم اتعامل مع ريكوست جديد
        //        option.QueueLimit = 1;  // كم ريكوست يكون enquery
        //        option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;

        //    });
        //});

        //Add AddSlidingWindowLimiter
        //services.AddRateLimiter(rateLimiterOption =>
        //{
        //    rateLimiterOption.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        //    rateLimiterOption.AddSlidingWindowLimiter("sliding", option =>
        //    {
        //        option.PermitLimit = 2; //كم ريكوست ح اتعامل معه 
        //        option.Window = TimeSpan.FromSeconds(30); // بعد كم اتعامل مع ريكوست جديد
        //        option.SegmentsPerWindow = 2; //ح اقسم الويندو لي كم سيقمينت
        //        option.QueueLimit = 1;  // كم ريكوست يكون enquery
        //        option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;

        //    });
        //});

        return services;
    }
}
