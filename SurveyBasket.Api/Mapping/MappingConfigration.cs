using SurveyBasket.Api.Contract.Question;

namespace SurveyBasket.Api.Mapping;

public class MappingConfigration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<RequestQuestion, Question>()
            .Map(dest =>dest.Answer , src => src.Answer.Select(answer => new Answer { Content = answer}));

        config.NewConfig<RequestRegister, ApplicationUser>()
            .Map(dest => dest.UserName , src => src.Email);


    }
}
