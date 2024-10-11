namespace BackEndAje.Api.Application.Masters.Queries.GetAllProductTypes
{
    public class GetAllProductTypesResult
    {
        public int ProductTypeId { get; set; }

        public string ProductTypeDescription { get; set; }

        public bool IsActive { get; set; }
    }
}