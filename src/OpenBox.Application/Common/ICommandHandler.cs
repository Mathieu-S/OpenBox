namespace OpenBox.Application.Common;

/// <summary>
/// Represent an handler of a command without a return value.
/// </summary>
/// <typeparam name="TCommand">The type of command.</typeparam>
public interface ICommandHandler<in TCommand>
{
    Task Handle(TCommand command, CancellationToken ct);
}

/// <summary>
/// Represent an handler of a command with a return value.
/// </summary>
/// <typeparam name="TCommand">The type of command.</typeparam>
/// <typeparam name="TResult">The return type value.</typeparam>
public interface ICommandHandler<in TCommand, TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken ct);
}