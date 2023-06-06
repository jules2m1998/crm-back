using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Domain.Entities;

public enum EmailType
{
    FIRST,
    SECOND,
    LAST
}

public class Email: BaseEntity
{
    [EnumDataType(typeof(EmailType))]
    public EmailType EmailType { get; set; } = EmailType.FIRST;
    public bool IsSend { get; set; } = false;

    public virtual Event Event { get; set; } = null!;
}
