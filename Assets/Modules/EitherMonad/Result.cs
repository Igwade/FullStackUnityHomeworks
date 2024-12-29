using System;

namespace EitherMonad
{
    public class Result<TSuccess, TError>
    {
        private readonly TSuccess _success;
        private readonly TError _error;
        private readonly bool _isSuccess;

        private Result(TSuccess success, TError error, bool isSuccess)
        {
            _success = success;
            _error = error;
            _isSuccess = isSuccess;
        }

        public bool IsSuccess => _isSuccess;
        public bool IsError => !_isSuccess;

        public TSuccess Success => IsSuccess ? _success : throw new InvalidOperationException("No value in Success.");
        public TError Error => IsError ? _error : throw new InvalidOperationException("No value in Error.");

        public static Result<TSuccess, TError> FromSuccess(TSuccess success) =>
            new Result<TSuccess, TError>(success, default, true);

        public static Result<TSuccess, TError> FromError(TError error) =>
            new Result<TSuccess, TError>(default, error, false);

        public TResult Match<TResult>(Func<TSuccess, TResult> onSuccess, Func<TError, TResult> onError) =>
            IsSuccess ? onSuccess(_success) : onError(_error);
        
        public void Match(Action<TSuccess> onSuccess, Action<TError> onError)
        {
            if (IsSuccess)
                onSuccess(_success);
            else
                onError(_error);
        }

        public override string ToString() =>
            IsSuccess ? $"Success({_success})" : $"Error({_error})";
        
        public static implicit operator Result<TSuccess, TError>(TSuccess success) =>
            FromSuccess(success);
        
        public static implicit operator Result<TSuccess, TError>(TError error) =>
            FromError(error);
    }
}