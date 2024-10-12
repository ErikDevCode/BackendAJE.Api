using BackEndAje.Api.Application.Dtos.DocumentType;
using BackEndAje.Api.Application.Dtos.PaymentMethod;
using BackEndAje.Api.Application.Dtos.Users;

namespace BackEndAje.Api.Application.Clients.Queries.GetAllClients
{
    using BackEndAje.Api.Domain.Entities;

    public class GetAllClientsResult
    {
        public int ClientId { get; set; }

        public int ClientCode { get; set; }

        public string CompanyName { get; set; }

        public DocumentTypeDto DocumentType { get; set; }

        public string DocumentNumber { get; set; }

        public string Email { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public PaymentMethodDto PaymentMethod { get; set; }

        public int Route { get; set; }

        public UserDto Seller { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public District District { get; set; }

        public string? CoordX { get; set; }

        public string? CoordY { get; set; }

        public string? Segmentation { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAy { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}
