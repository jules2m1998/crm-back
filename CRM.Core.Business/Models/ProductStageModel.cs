using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models;

public static class ProductStageModel
{
    public class In
    {
        public Guid ProductId { get; set; }
        public bool IsFirst { get; set; } = false;
        public string Name { get; set; } = string.Empty;
        public bool? IsDone { get; set; } = false;
        public bool? IsActivated { get; set; } = true;
    }

    public class Out: BaseModelOut
    {
        public string Name { get; set; } = string.Empty;
        public bool IsDone { get; set; } = false;
        public bool IsFirst { get; set; } = false;
    }
}
