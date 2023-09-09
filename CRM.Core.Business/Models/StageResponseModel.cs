using CRM.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models;

public class StageResponseModel
{
    public class In
    {
        public string Response { get; set; } = string.Empty;
        public Guid? NextStageId { get; set; }
        public Guid StageId { get; set; }
    }
    public class Out
    {
        public string Response { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdateAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsActivated { get; set; } = true;
        public ProductStageModel.Out? NextStage { get; set; }
        public ProductStageModel.Out Stage { get; set; } = null!;
    }
}
