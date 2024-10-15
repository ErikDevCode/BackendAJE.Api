namespace BackEndAje.Api.Application.OrderRequestDocument.Queries.GetOrderRequestDocumentById
{
    public class GetOrderRequestDocumentByIdResult
    {
        public int DocumentId { get; set; }

        public Byte[] DocumentContent { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }
    }
}