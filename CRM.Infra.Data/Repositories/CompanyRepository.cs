using CRM.Core.Business.Repositories;
using CRM.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Infra.Data.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly IApplicationDbContext _dbContext;
    private readonly DbSet<Company> _companySet;
    private IQueryable<Company> _companies { get { return _companySet.Include(c => c.Creator).Where(c => c.DeletedAt == null); } }

    public CompanyRepository(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _companySet = dbContext.Companies;
    }

    public async Task<Company> AddOneAsync(Company company)
    {
        await _companySet.AddAsync(company);
        await _dbContext.SaveChangesAsync();
        _companySet.Attach(company);
        return company;
    }

    public async Task<Company?> GetOneAsync(Guid id)
    {
        return await _companies
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Company?> GetOneAsync(Guid id, string userName)
    {
        return await _companies
            .Where(c => c.Id == id && c.Creator.UserName == userName)
            .FirstOrDefaultAsync();
    }

    public async Task<ICollection<Company>> GetAllAsync()
    {
        return await _companies.ToListAsync();
    }

    public async Task<ICollection<Company>> GetAllAsync(string userName)
    {
        return await _companies.Where(c => c.Creator != null && c.Creator.UserName == userName).ToListAsync();
    }

    public async Task<Company> UpdateAsync(Company company)
    {
        company.UpdateAt = DateTime.UtcNow;
        _companySet.Update(company);
        await _dbContext.SaveChangesAsync();
        _companySet.Attach(company);
        return company;
    }

    public async Task<ICollection<Company>> GetManyAsync(ICollection<Guid> ids)
    {
        return await _companies.Where(c => ids.Contains(c.Id)).ToListAsync();
    }

    public async Task<ICollection<Company>> GetManyAsync(ICollection<Guid> ids, string userName)
    {
        return await _companies.Where(c => c.Creator != null && c.Creator.UserName == userName && ids.Contains(c.Id)).ToListAsync();
    }

    public async Task<ICollection<Company>> UpdateManyAsync(ICollection<Company> companies)
    {
        foreach(var company in companies)
        {
            company.UpdateAt = DateTime.UtcNow;
        }
        _companySet.UpdateRange(companies);
        await _dbContext.SaveChangesAsync();
        _companySet.AttachRange(companies);
        return companies;
    }

    public async Task DeleteManyAsync(ICollection<Company> companies)
    {
        _companySet.RemoveRange(companies);
        await _dbContext.SaveChangesAsync();
    }

    public async Task MarkAsDeletedAsync(ICollection<Company> companies)
    {
        foreach(var company in companies)
        {
            company.DeletedAt= DateTime.UtcNow;
        }
        _companySet.UpdateRange(companies);
        await _dbContext.SaveChangesAsync();
    }
}
