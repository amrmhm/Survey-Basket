namespace SurveyBasket.Api.Contract.Authentication;

public record RequestRegister(
     string Email,
     string Password,
     string FirstName,
     string LastName

    );
