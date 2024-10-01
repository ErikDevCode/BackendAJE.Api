namespace BackEndAje.Api.Application.Actions.Commands.CreateAction
{
    using BackEndAje.Api.Application.Dtos.Actions;
    using MediatR;

    public class CreateActionCommand : IRequest<Unit>
    {
        public CreateActionDto Action { get; set; }
    }
}
