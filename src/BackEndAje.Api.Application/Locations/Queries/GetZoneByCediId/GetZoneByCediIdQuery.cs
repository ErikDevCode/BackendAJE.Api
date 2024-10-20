namespace BackEndAje.Api.Application.Locations.Queries.GetZoneByCediId
{
    using MediatR;

    public class GetZoneByCediIdQuery : IRequest<List<GetZoneByCediIdResult>>
    {
        public int CediId { get; set; }
    }
}