namespace OpenBox.Application.Common;

public interface ICommandHandler<in TCommand>
{
    Task Handle(TCommand command, CancellationToken ct);
}

public interface ICommandHandler<in TCommand, TResult>
{
    Task<TResult> Handle(TCommand command, CancellationToken ct);
}