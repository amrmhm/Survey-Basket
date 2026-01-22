using System.Reflection;

namespace SurveyBasket.Api.Abstractions;

public static class ResualtExtentions
{
    public static ObjectResult ToProblem (this Resault resault )
    {
        if (resault.IsSuccess)
            throw new InvalidOperationException("Can Not Convert Success Resualt To Problem");
        var problem = Results.Problem(statusCode: resault.Error.statusCode);
        var problemDetails =problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;
        problemDetails!.Extensions = new Dictionary<string, object?>
                {
                    {

                        "errors", new[]{
                            resault.Error.Code ,
                            resault.Error.Description}
                    }
                };


        
        return new ObjectResult (problemDetails);
    }
}
