using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace SurveyBasket.Api.OpenApi;

public class InformationSchemeTransformer(ApiVersionDescription descriptions) : IOpenApiDocumentTransformer
{
    public ApiVersionDescription Description { get; } = descriptions;


    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {

        document.Info = new()
        {
            Title = "Survey Basket",
            Version = Description.ApiVersion.ToString(),
            Description = $"API Descriptions {(Description.IsDeprecated ? "This Api Has Been Deprecated" : string.Empty)}"
        };

        return Task.CompletedTask;
    }
}
