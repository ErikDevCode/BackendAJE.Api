namespace BackEndAje.Api.Application.Masters.Queries.GetAllTimeWindows
{
    public class GetAllTimeWindowsResult
    {
        public int TimeWindowId { get; set; }

        public string TimeRange { get; set; }

        public bool IsActive { get; set; }
    }
}
