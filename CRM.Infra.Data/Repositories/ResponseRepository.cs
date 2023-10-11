using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Repositories;

public class ResponseRepository : BaseRepository<StageResponse>, IResponseRepository
{
    public ResponseRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task DeleteReponseWithCommitAsync(Guid reponseId)
    {
        using var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            var commits = await _dbContext.Commits.Where(x => x.ResponseId.Equals(reponseId)).ToListAsync();
            _dbContext.Commits.RemoveRange(commits);
            var response = await GetByIdAsync(reponseId);
            if (response is null) return;
            Set.Remove(response);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch { 
            transaction.Rollback();
            throw;
        }
    }
}
