using SurveyBasket.Api.Abstractions.Const;

namespace SurveyBasket.Api.Contract.Users;

public class RequestUpdateProfileValidator : AbstractValidator<RequestUpdateProfile>
{
    public RequestUpdateProfileValidator()
    {

        RuleFor(c => c.FirstName)
            .NotEmpty()
            .Length(3, 100);


        RuleFor(c => c.LastName)
            .NotEmpty()
            .Length(3, 100);

    }
}

