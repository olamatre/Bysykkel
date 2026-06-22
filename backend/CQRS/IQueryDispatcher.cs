namespace CQRS;

public interface IQueryDispatcher
{
    Task<TResponse> Dispatch<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>;
}

public class QueryDispatcher(IServiceProvider serviceProvider) : IQueryDispatcher
{
    public Task<TResponse> Dispatch<TQuery, TResponse>(TQuery query, CancellationToken cancellationToken = default)
        where TQuery : IQuery<TResponse>
    {
        ArgumentNullException.ThrowIfNull(query);
        var handler = (IQueryHandler<TQuery, TResponse>?)serviceProvider.GetService(typeof(IQueryHandler<TQuery, TResponse>))
            ?? throw new InvalidOperationException($"No handler registered for query type '{typeof(TQuery).FullName}'.");

        return handler.HandleAsync(query, cancellationToken);
    }
}
