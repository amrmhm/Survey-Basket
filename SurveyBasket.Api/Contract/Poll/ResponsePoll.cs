namespace SurveyBasket.Api.Contract.Poll;

public record ResponsePoll(
    int Id,
     string Title,
      string Summary,
      bool IsPublished,
      DateOnly StartsAt,
    DateOnly EndsAt


    );

public record ResponsePollV2(
    int Id,
     string Title,
      string Summary,
      DateOnly StartsAt,
    DateOnly EndsAt


    );

