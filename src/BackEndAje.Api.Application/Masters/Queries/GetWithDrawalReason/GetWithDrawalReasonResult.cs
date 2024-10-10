namespace BackEndAje.Api.Application.Masters.Queries.GetWithDrawalReason
{
    public class GetWithDrawalReasonResult
    {
        public int WithDrawalReasonId { get; set; }

        public int ReasonRequestId { get; set; }

        public string WithDrawalReasonDescription { get; set; }

        public bool IsActive { get; set; }
    }
}
