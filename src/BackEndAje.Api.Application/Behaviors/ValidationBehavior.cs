namespace BackEndAje.Api.Application.Behaviors
{
    using BackEndAje.Api.Application.Dtos.Const;
    using FluentValidation;
    using MediatR;
    using Microsoft.AspNetCore.Http;

    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, IHttpContextAccessor httpContextAccessor)
        {
            this._validators = validators;
            this._httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (this._validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(this._validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(result => result.Errors).Where(f => f != null).ToList();

                if (failures.Count != 0)
                {
                    throw new ValidationException(failures);
                }
            }

            // 2. Validar y asignar el UserId solo si el request lo necesita
            if (request is IHasUserId || request is IHasAuditInfo || request is IHasAssignedBy || request is IHasCreatedByInfo || request is IHasUpdatedByInfo)
            {
                if (this._httpContextAccessor.HttpContext?.Items["UserId"] is not int userId || userId <= 0)
                {
                    throw new UnauthorizedAccessException(ConstName.MessageErrorUserId);
                }

                switch (request)
                {
                    case IHasAuditInfo auditCommand:
                        auditCommand.CreatedBy = userId;
                        auditCommand.UpdatedBy = userId;
                        break;
                    case IHasAssignedBy assignedCommand:
                        assignedCommand.AssignedBy = userId;
                        break;
                    case IHasCreatedByInfo createdCommand:
                        createdCommand.CreatedBy = userId;
                        break;
                    case IHasUpdatedByInfo updatedCommand:
                        updatedCommand.UpdatedBy = userId;
                        break;
                    case IHasUserId queryWithUserId:
                        queryWithUserId.UserId = userId;
                        break;
                }
            }

            return await next();
        }
    }

    public interface IHasAssignedBy
    {
        int AssignedBy { get; set; }
    }

    public interface IHasAuditInfo
    {
        int CreatedBy { get; set; }

        int UpdatedBy { get; set; }
    }

    public interface IHasCreatedByInfo
    {
        int CreatedBy { get; set; }
    }

    public interface IHasUpdatedByInfo
    {
        int UpdatedBy { get; set; }
    }

    public interface IHasUserId
    {
        int UserId { get; set; }
    }
}
