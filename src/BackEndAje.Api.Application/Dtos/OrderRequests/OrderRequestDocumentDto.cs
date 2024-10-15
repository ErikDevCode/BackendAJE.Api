namespace BackEndAje.Api.Application.Dtos.OrderRequests
{
    public class OrderRequestDocumentDto
    {
        public string DocumentName { get; set; }

        public decimal DocumentWeight { get; set; }

        public byte[] DocumentContent { get; set; }

        public bool IsActive { get; set; }
    }
}
