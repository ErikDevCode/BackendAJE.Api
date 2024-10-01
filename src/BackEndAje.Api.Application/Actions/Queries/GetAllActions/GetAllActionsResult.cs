namespace BackEndAje.Api.Application.Actions.Queries.GetAllActions
{
    using BackEndAje.Api.Domain.Entities;

    public class GetAllActionsResult : AuditableEntity
    {
        public int ActionId { get; set; }

        public string ActionName { get; set; }
    }
}
