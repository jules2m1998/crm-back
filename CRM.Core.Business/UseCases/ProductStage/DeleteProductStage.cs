using CRM.Core.Business.Repositories;
using MediatR;
using ProductStage = CRM.Core.Domain.Entities.ProductStage;
namespace CRM.Core.Business.UseCases.ProductStage;

public static class DeleteProductStage
{
    public record Command(Guid Id): IRequest;
    public class Handler : IRequestHandler<Command>
    {
        private readonly IStageRepository _repo;

        public Handler(IStageRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            await _repo.DeleteStageAsync(request.Id);
        }
    }
}
