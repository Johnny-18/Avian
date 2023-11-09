using MediatR;

namespace Avian.Application.Interfaces;

public interface IQuery<out T> : IRequest<T>
{
}

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
}
