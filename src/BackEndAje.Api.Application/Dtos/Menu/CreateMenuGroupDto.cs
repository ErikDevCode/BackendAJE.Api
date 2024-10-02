namespace BackEndAje.Api.Application.Dtos.Menu
{
    public class CreateMenuGroupDto
    {
        public string GroupName { get; set; }

        public bool IsSeparator { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
