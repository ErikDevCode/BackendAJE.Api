namespace BackEndAje.Api.Application.Dtos.Actions
{
    public class UpdateActionDto
    {
        public int ActionId { get; set; }

        public string ActionName { get; set; }

        public int UpdatedBy { get; set; }
    }
}
