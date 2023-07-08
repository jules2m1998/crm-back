using CRM.Core.Business.Extensions;
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

public static class UpdateProductStage
{
    public record Command(Guid Id, ProductStageModel.In Model): IRequest<ProductStageModel.Out>;

    public class Handler : IRequestHandler<Command, ProductStageModel.Out>
    {
        private readonly IProductStageRepository _repo;

        public Handler(IProductStageRepository repo)
        {
            _repo = repo;
        }

        public async Task<ProductStageModel.Out> Handle(Command request, CancellationToken cancellationToken)
        {
            Domain.Entities.ProductStage productStage = 
                await _repo.GetOneAsync(request.Id) 
                ?? throw new NotFoundEntityException("This stage don't exist !");
            var model = request.Model;
            productStage.IsFirst = model.IsFirst;
            productStage.Name = model.Name;
            if (model.IsDone != null) 
                productStage.IsDone = model.IsDone ?? false;
            if (model.IsActivated != null) 
                productStage.IsActivated = model.IsActivated ?? false;

            await _repo.UpdateAsync(productStage);
            return productStage.ToModel();
        }
    }
}
