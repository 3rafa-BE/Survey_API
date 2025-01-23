using Mapster;
using Survey.Contracts.Poll;
using Survey.Contracts.Question;
using Survey.Contracts.Register;
using Survey.Models;

namespace Survey.Mapping
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Poll, PollResponse>().Map(dest => dest.Summary, src => src.Summary);
            config.NewConfig<QuestionRequest, Question>()
                .Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer { Content = answer }));
            config.NewConfig<Poll, PollRequest>();
            config.NewConfig<registerRequest, ApplicationUser>()
                .Map(dest => dest.UserName, src => src.Email);
        }
    }
}
