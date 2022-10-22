namespace OpenBox.Application.Common;

/// <summary>
/// Represent an handler of a query.
/// </summary>
/// <typeparam name="T">The type of query.</typeparam>
/// <typeparam name="TResult">The return type value.</typeparam>
public interface IQueryHandler<in T, TResult>
{
    Task<TResult> Handle(T query, CancellationToken ct);
}