namespace BackEndAje.Api.Application.Dtos.Actions
{
    public class CreateActionDto
    {
        public string ActionName { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
