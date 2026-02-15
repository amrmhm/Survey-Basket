//using Asp.Versioning.ApiExplorer;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.Extensions.Options;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;

//namespace SurveyBasket.Api.Swagger;

//public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider descriptionProvider) : IConfigureOptions<SwaggerGenOptions>
//{
//    private readonly IApiVersionDescriptionProvider _descriptionProvider = descriptionProvider;

//    public void Configure(SwaggerGenOptions options)
//    {
//        foreach (var description in _descriptionProvider.ApiVersionDescriptions)
//        {
//            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
//        }

//            //Add Support Authenticate TO Swagger
//            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
//            {
//                Name = "Authentication",
//                Description = "Please Add Jwt Token",
//                In = ParameterLocation.Header,
//                BearerFormat = "JWT",
//                Type = SecuritySchemeType.Http, // بناء علي ارساله في header - نوعه Jwt
//                Scheme = JwtBearerDefaults.AuthenticationScheme

//            });
//            //Add Support Authenticate TO Swagger
//            options.AddSecurityRequirement(new OpenApiSecurityRequirement
//            {
//                {
//                    new OpenApiSecurityScheme
//                    {
//                        Reference = new OpenApiReference
//                        {
//                            Id = JwtBearerDefaults.AuthenticationScheme ,
//                            Type = ReferenceType.SecurityScheme
//                        }
//                    },
//                    Array.Empty<string>()
//                }

//            });
//        }


//    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description) =>
//        new()
//        {
//            Title = "Survey Basket",
//            Version = description.ApiVersion.ToString(),
//            Description = $"API Descriptions {(description.IsDeprecated ? "This Api Has Been Deprecated" : string.Empty)}"
//        };
//}
