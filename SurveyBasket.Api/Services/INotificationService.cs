namespace SurveyBasket.Api.Services;

public interface INotificationService
{
    public Task SendNewPollNotification(int? pollId = null);
}
