using MediatR;

namespace Avian.Application.Interfaces;

public interface ICommand : IRequest<Unit>
{
}

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Unit>
    where TCommand : ICommand
{
}

public interface ICommand<out T> : IRequest<T>
{
}

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
}