using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace SurveyBasket.Api.Abstractions;

public class Resault
{
    public Resault(bool isSuccess , Error error)
    {
        if ((isSuccess && error != Error.None ) || (!isSuccess && error == Error.None))
            throw new InvalidOperationException();
          IsSuccess = isSuccess ;
          Error = error ;
    }
    public bool IsSuccess { get; }
    public bool IsFaliure => !IsSuccess;
    public Error Error { get; } = default!;

    public static Resault Success() => new(true, Error.None);
    public static Resault Faliure(Error error) => new(false, error);
    public static Resault<TValue> Success<TValue>(TValue value ) => new (value , true , Error.None);
    public static Resault<TValue> Faliure<TValue>(Error error) => new (default! , false , error);

}

public class Resault<TValue> : Resault
{
    private readonly TValue _value;
    public Resault(TValue value , bool isSuccess ,Error error) : base (isSuccess , error)
    {
        _value = value ;
    }

    public TValue Value => IsSuccess ? _value : throw new InvalidOperationException("Faliure Resault Can Not Have Value");
}