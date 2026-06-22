namespace CQRS;

public interface ICommandDispatcher
{
    Task Dispatch<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand;

    Task<TResponse> Dispatch<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>;
}

public class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task Dispatch<TCommand>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand
    {
        ArgumentNullException.ThrowIfNull(command);
        var handler = (ICommandHandler<TCommand>?)serviceProvider.GetService(typeof(ICommandHandler<TCommand>))
            ?? throw new InvalidOperationException($"No handler registered for command type '{typeof(TCommand).FullName}'.");

        return handler.Handle(command, cancellationToken);
    }

    public Task<TResponse> Dispatch<TCommand, TResponse>(TCommand command, CancellationToken cancellationToken = default)
        where TCommand : ICommand<TResponse>
    {
        ArgumentNullException.ThrowIfNull(command);
        var handler = (ICommandHandler<TCommand, TResponse>?)serviceProvider.GetService(typeof(ICommandHandler<TCommand, TResponse>))
            ?? throw new InvalidOperationException($"No handler registered for command type '{typeof(TCommand).FullName}'.");

        return handler.Handle(command, cancellationToken);
    }
}