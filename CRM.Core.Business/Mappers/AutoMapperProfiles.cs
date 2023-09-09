using AutoMapper;
using CRM.Core.Business.Models;
using CRM.Core.Business.Models.Company;
using CRM.Core.Business.Models.HeadProspectionModel;
using CRM.Core.Business.Models.Product;
using CRM.Core.Business.UseCases.CommitUcs;
using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Mappers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<StageResponse, StageResponseModel.In>().ReverseMap();
            CreateMap<StageResponse, StageResponseModel.Out>().ReverseMap();

            CreateMap<ProductStage, ProductStageModel.In>().ReverseMap();
            CreateMap<ProductStage, ProductStageModel.Out>().ReverseMap();

            CreateMap<Commit, AddCommit.AddCommitModel>().ReverseMap();
            CreateMap<Commit, AddCommit.CommitOuModel>().ReverseMap();

            CreateMap<HeadProspection, HeadProspectionInModel>().ReverseMap();
            CreateMap<HeadProspection, HeadProspectionOuModel>().ReverseMap();

            CreateMap<User, UserModel>().ReverseMap();

            CreateMap<Product, ProductOutModel>().ReverseMap();
            CreateMap<Company, CompanyOutModel>().ReverseMap();
        }
    }
}
