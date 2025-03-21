namespace BackEndAje.Api.Application.Clients.Queries.GetExportClient
{
    using BackEndAje.Api.Application.Behaviors;
    using MediatR;

    public class ExportClientsQuery : IRequest<byte[]>
    {
    }
}