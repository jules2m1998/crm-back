using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities
{
    public class Skill: BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Place { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set;}
        public bool IsCurrent { get; set; } = false;

        public virtual User? Student { get; set; } = null!;
        public virtual User? Expert { get; set; } = null!;

        public Skill()
        {
        }

        public Skill(string name, string place, DateTime startDate, DateTime? endDate, bool isCurrent, User? student, User? expert)
        {
            Name = name;
            Place = place;
            StartDate = startDate;
            EndDate = endDate;
            IsCurrent = isCurrent;
            Student = student;
            Expert = expert;
        }
    }
}
