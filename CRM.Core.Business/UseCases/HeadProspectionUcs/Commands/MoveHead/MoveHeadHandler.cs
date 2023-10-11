using AutoMapper;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using CRM.Core.Domain.Exceptions;
using MediatR;

namespace CRM.Core.Business.UseCases.HeadProspectionUcs.Commands.MoveHead;

public class MoveHeadHandler : IRequestHandler<MoveHeadCommand, MoveHeadResponse>
{
    private readonly IAsyncRepository<HeadProspection> _repo;
    private readonly IAsyncRepository<StageResponse> _responseRepo;
    private readonly IAsyncRepository<Commit> _commitRepo;
    private readonly IMapper _mapper;

    public MoveHeadHandler(
        IAsyncRepository<HeadProspection> repo, 
        IAsyncRepository<StageResponse> responseRepo, 
        IAsyncRepository<Commit> commitRepo, 
        IMapper mapper)
    {
        _repo = repo;
        _responseRepo = responseRepo;
        _commitRepo = commitRepo;
        _mapper = mapper;
    }

    public async Task<MoveHeadResponse> Handle(MoveHeadCommand request, CancellationToken cancellationToken)
    {
        var validator = new MoveHeadValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if(!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Aggregate(new Dictionary<string, List<string>>(), (acc, x) =>
            {
                if (acc.TryGetValue(x.PropertyName, out var list))
                    list.Add(x.ErrorMessage);
                else
                    acc.Add(x.PropertyName, new List<string>() { x.ErrorMessage });

                return acc;
            });

            throw new BaseException(errors);
        }

        var head = await _repo
            .FindOneWhere(h => 
                h.ProductId == request.ProductId &&
                h.CompanyId == request.CompanyId && 
                h.AgentId == request.AgentId
            ) ?? throw new BaseException(new Dictionary<string, List<string>> { { nameof(MoveHeadCommand.ProductId), new List<string>() { "This prospection doesn't exist" } } });

        var commit = new Commit
        {
            Message = request.Message ?? string.Empty,
            ParentId = head.CommitId,
            ResponseId = request.ResponseId
        };

        await _commitRepo.AddAsync(commit);

        head.CommitId = commit.Id;
        await _repo.UpdateAsync(head);

        var dto = _mapper.Map<MoveHeadDto>(head);

        return new MoveHeadResponse() { Data = dto, Message = "Head successfully moved !", Success = true };
    }
}
