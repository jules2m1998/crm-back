using CRM.Core.Business.Models.Company;
using CRM.Core.Business.Models.Product;
using CRM.Core.Business.UseCases.CommitUcs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.HeadProspectionModel;

public class HeadProspectionOuModel
{
    public ProductOutModel Product { get; set; } = null!;
    public CompanyOutModel Company { get; set; } = null!;
    public UserModel Agent { get; set; } = null!;
    public AddCommit.CommitOuModel Commit { get; set; } = null!;
}
