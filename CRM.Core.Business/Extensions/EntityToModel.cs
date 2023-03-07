using CRM.Core.Business.Models.Company;
using CRM.Core.Business.Models.Product;
using CRM.Core.Business.Models.Supervision;
using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Extensions;

public static class EntityToModel
{
    public static ProductOutModel ToProductOutModel(this Product product)
    {
        var creator = product.Creator;
        return new ProductOutModel(product.Id, product.Name, product.Logo, product.Description, product.CreatedAt, product.UpdateAt, creator?.ToUserModel());
    }

    public static CompanyOutModel ToCompanyOutModel(this Company company)
    {
        var creator = company.Creator;
        return new CompanyOutModel(company.Name, company.Description, company.Logo, company.CEOPicture, company.CEOName, company.Values, company.Mission, company.Concurrent, company.Location, company.ActivityArea, company.Size, company.Id, company.CreatedAt, company.UpdateAt, company.IsActivated, creator?.ToUserModel());
    }

    public static SupervisionOutModel SupervisionHistoryToModel(this SupervisionHistory history)
    {
        return new SupervisionOutModel(history.Supervised.ToUserModel(), history.Supervisor.ToUserModel(), history.IsActive, history.CreatedAt);
    }
}
