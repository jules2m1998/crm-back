using MediatR;

namespace CRM.Core.Business.UseCases.Test
{
    public class GetTestLisHandler : IRequestHandler<GetTestLisQuery, string[]>
    {
        public Task<string[]> Handle(GetTestLisQuery request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new string[] {"Test"});
        }
    }
}
