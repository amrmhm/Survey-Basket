namespace SurveyBasket.Api.Authentication.Filters;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {

        //var user = context.User.Identity;
        //if (user is null || !user.IsAuthenticated)
        //{
        //    return Task.CompletedTask;
        //}

        //var hasPermission = context.User.Claims.Any(c => c.Value == requirement.Permission && c.Type == Permission.Type);

        //if (!hasPermission)
        //{
        //    return Task.CompletedTask;
        //}

        //context.Succeed(requirement);
        //return Task.CompletedTask;


        //OR
        if (context.User.Identity is not { IsAuthenticated: true }
        || !context.User.Claims.Any(c => c.Value == requirement.Permission && c.Type == Permission.Type))
            return Task.CompletedTask;
        context.Succeed(requirement);
        return Task.CompletedTask;

    }
}
