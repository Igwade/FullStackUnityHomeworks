using EitherMonad;

namespace App.Repository.ChainOfResponsibility
{
    public interface IContext
    {
        public IResult Result { get; }
        public void SetError(string error);
    }
}