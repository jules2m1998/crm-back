using CRM.Core.Business.Models.Prospect;
using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.Design;
using System.Linq.Expressions;

namespace CRM.Infra.Data.Repositories;

public class ProspectionRepository : IProspectionRepository
{
    private readonly IApplicationDbContext _dbContext;
    private readonly DbSet<Prospect> _dbSet;

    private IQueryable<Prospect> _included { get
        {
            return _dbSet
            .Include(pr => pr.Creator)
            .Include(pr => pr.Company)
            .Include(pr => pr.Product)
            .Include(pr => pr.Agent);
        } }

    private IQueryable<Prospect?> _eachProspect { get
        {
            return _included
            .GroupBy(pr => new { pr.ProductId, pr.CompanyId })
            .Select(gr => gr.OrderByDescending(g => g.CreatedAt).FirstOrDefault());
        } }

    public ProspectionRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Prospects;
    }

    public async Task<ICollection<Prospect>> GetProspectionByAgent(Guid agentId)
    {
        var r = await _eachProspect
            .ToListAsync();

        return r.Where(pr => pr != null && pr.Agent.Id == agentId).ToList()!;
    }

    public async Task<ICollection<Prospect>> GetProspectionByAgent(Guid agentId, string userName)
    {
        var r = await _eachProspect
            .ToListAsync();

        return r.Where(pr => pr != null && pr.Agent.Id == agentId && pr.Creator != null && pr.Creator.UserName == userName).ToList()!;
    }

    public async Task<Prospect?> GetTheCurrentAsync(Guid productId, Guid companyId)
    {
        var r = await _eachProspect
            .ToListAsync();
        return r.FirstOrDefault(pr => pr != null && pr.ProductId == productId && pr.CompanyId == companyId);
    }

    public async Task<Prospect> SaveAsync(Prospect product)
    {
        await _dbSet.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        _dbSet.Attach(product);
        return product;
    }

    public async Task<ICollection<Prospect>> GetProspectionByCompany(Guid companyId)
    {
        var r = await _eachProspect
        .ToListAsync();
        return r.Where(pr => pr != null && pr.Company.Id == companyId).ToList()!;
    }

    public async Task<ICollection<Prospect>> GetProspectionByCompany(Guid companyId, string userName)
    {
        var r = await _eachProspect
            .ToListAsync();
        return r.Where(pr => pr != null && pr.Company.Id == companyId && pr.Creator != null && pr.Creator.UserName == userName).ToList()!;
    }

    public async Task<ICollection<Prospect>> GetProspectionByProduct(Guid productId)
    {
        var r = await _eachProspect
        .ToListAsync();
        return r.Where(pr => pr != null && pr.Product.Id == productId).ToList()!;
    }

    public async Task<ICollection<Prospect>> GetProspectionByProduct(Guid productId, string userName)
    {
        var r = await _eachProspect
            .ToListAsync();
        return r.Where(pr => pr != null && pr.Product.Id == productId && pr.Creator != null && pr.Creator.UserName == userName).ToList()!;
    }

    public async Task<ICollection<Prospect>> GetManyAsync(ICollection<ProspectionInModel> models)
    {
        var prospects = await GetAllAsync();
        return prospects
            .Where(
                p => 
                    models
                        .Contains(new ProspectionInModel(p.ProductId, p.CompanyId, p.Agent.Id))
                        )
            .ToList();
    }

    public async Task<ICollection<Prospect>> GetManyAsync(ICollection<ProspectionInModel> models, string userName)
    {
        return await _included
            .Where(
                pr => 
                    models.FirstOrDefault(
                        m => pr.CompanyId == m.CompanyId
                        && pr.Agent.Id == m.AgentId 
                        && pr.ProductId == m.ProductId
                        && pr.Creator != null && pr.Creator.UserName == userName) != null)
            .ToListAsync();
    }

    public async Task<ICollection<Prospect>> UpdateAsync(ICollection<Prospect> prospections)
    {
        foreach(var prospect in prospections)
        {
            prospect.UpdatedAt = DateTime.UtcNow;
        }
        _dbSet.UpdateRange(prospections);
        await _dbContext.SaveChangesAsync();
        _dbSet.AttachRange(prospections);

        return prospections;
    }

    public async Task<ICollection<Prospect>> GetAllAsync()
    {
        return await _included.ToListAsync();
    }

    public async Task<Prospect?> GetOneAsync(Guid agentId, Guid productId, Guid companyId)
    {
        return await _included.FirstOrDefaultAsync(ps => ps.Agent.Id == agentId && ps.ProductId == productId && ps.CompanyId == companyId);
    }

    public async Task<Prospect?> GetOneAsync(Guid agentId, Guid productId, Guid companyId, string creatorUserName)
    {
        return await _included.FirstOrDefaultAsync(ps => ps.Agent.Id == agentId && ps.ProductId == productId && ps.CompanyId == companyId && ps.Creator != null && ps.Creator.UserName == creatorUserName);
    }
}
