using System.Threading;
using Cysharp.Threading.Tasks;

namespace App.Repository.ChainOfResponsibility
{
    public interface IHandler<TContext>
    {
        IHandler<TContext> SetNext(IHandler<TContext> next);
        UniTask Handle(TContext context, CancellationToken token = default);
    }
}