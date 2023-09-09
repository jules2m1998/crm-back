using CRM.Core.Business.Models.HeadProspectionModel;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Repositories;

public class HeadProspectionRepository : IHeadProspectionRepository
{
    private readonly ApplicationDbContext dbContext;
    private DbSet<HeadProspection> HeadProspections => dbContext.HeadProspections;
    private IQueryable<HeadProspection> Included =>
        HeadProspections
            .Include(x => x.Product)
                .ThenInclude(x => x.Stages)
            .Include(x => x.Company)
            .Include(x => x.Agent)
            .Include(x => x.Commit)
                .ThenInclude(x => x.Response);

    public HeadProspectionRepository(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<int> AddAdync(HeadProspection head, CancellationToken cancellationToken)
    {
        await HeadProspections.AddAsync(head, cancellationToken);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> DeleteAsync(HeadProspection data, CancellationToken cancellationToken)
    {
        HeadProspections.Remove(data);
        return await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ICollection<HeadProspection>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await Included.ToListAsync(cancellationToken);
    }

    public async Task<HeadProspection?> GetByIndexAsync(Guid productId, Guid companyId, Guid agentId, CancellationToken cancellationToken)
    {
        return await Included.FirstOrDefaultAsync(x => x.ProductId == productId && x.CompanyId == companyId && x.AgentId == agentId, cancellationToken);
    }

    public async Task UpdateAsync(HeadProspection data, CancellationToken cancellationToken)
    {
        HeadProspections.Update(data);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
