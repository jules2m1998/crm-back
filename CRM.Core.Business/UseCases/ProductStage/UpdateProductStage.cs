using AutoMapper;
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
        private readonly IMapper mapper;

        public Handler(IProductStageRepository repo, IMapper mapper)
        {
            _repo = repo;
            this.mapper = mapper;
        }

        public async Task<ProductStageModel.Out> Handle(Command request, CancellationToken cancellationToken)
        {
            Domain.Entities.ProductStage productStage = 
                await _repo.GetOneAsync(request.Id) 
                ?? throw new NotFoundEntityException("This stage don't exist !");
            var model = request.Model;
            productStage.StageLevel = model.StageLevel;
            productStage.Name = model.Name;
            productStage.Question = model.Question;

            if (model.IsActivated != null) 
                productStage.IsActivated = model.IsActivated ?? false;

            await _repo.UpdateAsync(productStage);
            return mapper.Map<ProductStageModel.Out>(productStage);
        }
    }
}
