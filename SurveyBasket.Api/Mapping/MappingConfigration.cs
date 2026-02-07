using SurveyBasket.Api.Contract.Question;
using SurveyBasket.Api.Contract.Users;

namespace SurveyBasket.Api.Mapping;

public class MappingConfigration : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<RequestQuestion, Question>()
            .Map(dest =>dest.Answer , src => src.Answer.Select(answer => new Answer { Content = answer}));

        config.NewConfig<RequestRegister, ApplicationUser>()
            .Map(dest => dest.UserName , src => src.Email);

        config.NewConfig<RequestCreateUser, ApplicationUser>()
            .Map(dest => dest.UserName , src => src.Email);

        config.NewConfig<RequestRegister, ApplicationUser>()
            .Map(dest => dest.EmailConfirmed , src => true);

        config.NewConfig<(ApplicationUser user, IList<string> role), ResponseUser>()
            .Map(dest => dest , src => src.user)
            .Map(dest => dest.Roles , src => src.role);

        config.NewConfig<RequestUpdateUser, ApplicationUser>()
         .Map(dest => dest.UserName, src => src.Email);
        config.NewConfig<RequestUpdateUser, ApplicationUser>()
         .Map(dest => dest.NormalizedEmail, src => src.Email.ToUpper());



    }
}
