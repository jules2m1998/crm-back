using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.ProductStage;

public static class DeleteProductStage
{
    public record Command(Guid Id): IRequest;
    public class Handler : IRequestHandler<Command>
    {
        private readonly IProductStageRepository _repo;

        public Handler(IProductStageRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var item =
                await _repo.GetOneAsync(request.Id)
                ?? throw new NotFoundEntityException("This stage don't exist !");
            await _repo.DeleteAsync(item);
            return new Unit();
        }
    }
}
