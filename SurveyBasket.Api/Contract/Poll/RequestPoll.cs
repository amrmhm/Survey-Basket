using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.Api.Contract.Poll;

public record RequestPoll(
    
    string Title ,
      string Summary,
  bool IsPublished,
  DateOnly StartsAt,
  DateOnly EndsAt
    );


   
