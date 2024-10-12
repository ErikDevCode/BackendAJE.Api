namespace BackEndAje.Api.Domain.Entities
{
    public class DocumentType : AuditableEntity
    {
        public int DocumentTypeId { get; set; }
        
        public string DocumentTypeName { get; set; }
    }
}
