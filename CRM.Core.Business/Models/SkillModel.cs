using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models
{
    public class SkillModel
    {
        public string Name { get; set; } = null!;
        public string Place { get; set; } = null!;
        public bool IsCurrent { get; set; } = false;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public SkillModel()
        {
        }

        public SkillModel(string name, string place, bool isCurrent, DateTime startDate, DateTime? endDate)
        {
            Name = name;
            Place = place;
            IsCurrent = isCurrent;
            StartDate = startDate;
            EndDate = endDate;
        }

        public override bool Equals(object? obj)
        {
            return obj is SkillModel model &&
                   Name == model.Name &&
                   Place == model.Place &&
                   IsCurrent == model.IsCurrent &&
                   StartDate == model.StartDate &&
                   EndDate == model.EndDate;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Place, IsCurrent, StartDate, EndDate);
        }
    }
}
