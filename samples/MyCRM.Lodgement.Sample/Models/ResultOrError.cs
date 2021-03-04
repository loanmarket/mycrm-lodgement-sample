using System;

namespace MyCRM.Lodgement.Sample.Models
{
    public sealed class ResultOrError<TResult, TError>
    {
        public bool IsError { get; }
        public TResult Result { get; }
        public TError Error { get; }

        public ResultOrError(TResult result) => Result = result;

        public ResultOrError(TError error)
        {
            Error = error;
            IsError = true;
        }

        public static implicit operator ResultOrError<TResult, TError>(TResult result) => new ResultOrError<TResult, TError>(result);
        public static implicit operator ResultOrError<TResult, TError>(TError error) => new ResultOrError<TResult, TError>(error);

        public TOutcome Match<TOutcome>(Func<TResult, TOutcome> success, Func<TError, TOutcome> failure) => IsError ? failure(Error) : success(Result);
    }
}
