using CRM.Core.Business.Models.Company;
using CRM.Core.Business.Models.Contact;
using CRM.Core.Business.Models.Event;
using CRM.Core.Business.Models.PhoneNumber;
using CRM.Core.Business.Models.Product;
using CRM.Core.Business.Models.Prospect;
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

    public static ProspectionOutModel ToModel(this Prospect p)
    {
        return new ProspectionOutModel(
            p.CreatedAt,
            p.Product.ToProductOutModel(),
            p.Company.ToCompanyOutModel(),
            p.Agent.ToUserModel(), 
            p.Creator?.ToUserModel(),
            p.UpdatedAt,
            p.IsActivated);
    }

    public static ContactOutModel ToModel(this Contact contact)
    {
        return new ContactOutModel
        {
            Id = contact.Id,
            Name = contact.Name,
            Email = contact.Email,
            Job = contact.Job,
            Phones = contact.Phones.Select(p => p.ToModelWithoutContact()).ToList(),
            Visibility = contact.Visibility,
            SharedTo = contact.SharedTo.Select(u => u.ToUserModel()).ToList(),
            Company = contact.Company.ToCompanyOutModel()
        };
    }

    public static PhoneOutWithoutContact ToModelWithoutContact(this PhoneNumber phone)
    {
        return new PhoneOutWithoutContact
        {
            Value = phone.Value,
            CreatedAt = phone.CreatedAt
        };
    }

    public static EventOutModel ToModel(this Event evnt) => new()
    {
        Id = evnt.Id,
        StartDate = evnt.StartDate,
        EndDate = evnt.EndDate,
        Description = evnt.Description,
        Name = evnt.Name,
        Topic = evnt.Topic,

        Prospection = evnt.Prospect?.ToModel(),
        Creator = evnt.Creator?.ToUserModel(),
        Owner = evnt.Owner.ToUserModel(),
        Contacts = evnt.Contact?.Select(c => c.ToModel()).ToList()
    };
    public static EventOutModel ToSimpleModel(this Event evnt)
    {
        ProspectionOutModel? prospectionModel = null;

        if(evnt.Prospect != null)
        {
            var p = evnt.Prospect;
            prospectionModel = new(p.CreatedAt, null, null, p.Agent.ToUserModel(), p.Creator?.ToUserModel(), p.UpdatedAt, p.IsActivated);
        }
        return new()
        {
            Id = evnt.Id,
            StartDate = evnt.StartDate,
            EndDate = evnt.EndDate,
            Description = evnt.Description,
            Name = evnt.Name,

            Prospection = prospectionModel,
            Creator = evnt.Creator?.ToUserModel(),
            Owner = evnt.Owner.ToUserModel(),
            Contacts = evnt.Contact?.Select(c => new ContactOutModel
            {
                Id= c.Id,
                Email = c.Email,
                Name = c.Name,
                Phones = c.Phones.Select(p => new PhoneOutWithoutContact
                {
                    CreatedAt = p.CreatedAt,
                    Value = p.Value
                }).ToList(),
                Job = c.Job,
                Visibility = c.Visibility
            }).ToList()
        };
    }
}
