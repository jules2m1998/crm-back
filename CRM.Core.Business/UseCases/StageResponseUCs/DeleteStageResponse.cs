using AutoMapper;
using CRM.Core.Business.Models;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.UseCases.StageResponseUCs;

public static class DeleteStageResponse
{
    public record Command(Guid Id): IRequest<StageResponseModel.Out?>;
    public class Handler : IRequestHandler<Command, StageResponseModel.Out?>
    {
        private readonly IResponseRepository _repo;
        private readonly IMapper _mapper;

        public Handler(IResponseRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        public async Task<StageResponseModel.Out?> Handle(Command request, CancellationToken cancellationToken)
        {
            await _repo.DeleteReponseWithCommitAsync(request.Id);
            return null;
        }
    }
}
