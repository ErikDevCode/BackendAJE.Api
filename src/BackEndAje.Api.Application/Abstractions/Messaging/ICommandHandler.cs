namespace BackEndAje.Api.Application.Abstractions.Messaging
{
    using MediatR;

    public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
    }
}
