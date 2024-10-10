namespace BackEndAje.Api.Application.Masters.Queries.GetAllReasonRequest
{
    public class GetAllReasonRequestResult
    {
        public int ReasonRequestId { get; set; }

        public string ReasonDescription { get; set; }

        public bool IsActive { get; set; }
    }
}
