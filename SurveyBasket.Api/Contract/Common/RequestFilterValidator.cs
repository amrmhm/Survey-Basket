

namespace SurveyBasket.Api.Contract.Common;

public class RequestFilterValidator : AbstractValidator<RequestFilter>
{
    public RequestFilterValidator()
    {
        RuleFor(c => c.PageSize)
            .LessThanOrEqualTo(20);

    }
}
