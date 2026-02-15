
using SurveyBasket.Api.Helpers;
using SurveyBasket.Api.Persistence;

namespace SurveyBasket.Api.Services;

public class NotificationService(ApplicationDbContext context,
    IHttpContextAccessor httpContextAccessor,
    UserManager<ApplicationUser> userManager,
    IEmailSender emailSender
    ) : INotificationService
{
    private readonly ApplicationDbContext _context = context;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task SendNewPollNotification(int? pollId = null)
    {
        IEnumerable<Poll> polls = [];
        if (pollId.HasValue)
        {
            var poll = await _context.Polls.SingleOrDefaultAsync(c => c.Id == pollId && c.IsPublished);
            polls = [poll!];
        }
        else
        {
            polls = await _context.Polls.Where(c => c.IsPublished && c.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
                .AsNoTracking()
                .ToListAsync();


        }


        //  Send Notification to Member users
        //Select Member
        var users = await _userManager.GetUsersInRoleAsync(DefaultRole.Member);

        //Add Original URL
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        foreach (var poll in polls)
        {
            foreach (var user in users)
            {
                var placeholder = new Dictionary<string, string>
                {
                    {"{{name}}",user.FirstName},
                    {"{{pollTill}}", poll.Title },
                    {"{{endDate}}",poll.EndsAt.ToString() },
                    {"{{url}}",$"{origin}/polls/start/{pollId}"  }

                };

                var bodyBuilder = EmailBodyBuilder.GenerateEmailBody("PollNotification", placeholder);

                await _emailSender.SendEmailAsync(user.Email!, $"🛎️ Survey Baskeet - NewPoll:{poll.Title}", bodyBuilder);


            }

        }


    }
}
