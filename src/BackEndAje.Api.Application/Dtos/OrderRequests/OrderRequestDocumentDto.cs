namespace BackEndAje.Api.Application.Dtos.OrderRequests
{
    public class OrderRequestDocumentDto
    {
        public int DocumentId { get; set; }

        public string DocumentName { get; set; }

        public decimal DocumentWeight { get; set; }

        public string Url { get; set; }

        public bool IsActive { get; set; }
    }
}
