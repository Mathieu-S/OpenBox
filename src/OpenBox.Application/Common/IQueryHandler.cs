namespace OpenBox.Application.Common;

public interface IQueryHandler<in T, TResult>
{
    Task<TResult> Handle(T query, CancellationToken ct);
}