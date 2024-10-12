namespace BackEndAje.Api.Application.Clients.Commands.CreateClient
{
    using MediatR;

    public class CreateClientCommand : IRequest<Unit>
    {
        public int ClientCode { get; set; }

        public string CompanyName { get; set; }

        public int DocumentTypeId { get; set; }

        public string DocumentNumber { get; set; }

        public string Email { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public int PaymentMethodId { get; set; }

        public int Route { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string DistrictId { get; set; }

        public string? CoordX { get; set; }

        public string? CoordY { get; set; }

        public string? Segmentation { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public int UpdatedBy { get; set; }
    }
}