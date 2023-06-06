﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Core.Business.Models.Event;

public class EventInModel: EventInWithoutUserName
{
    public string UserName { get; set; } = string.Empty;
}

public class EventInWithoutUserName
{
    public Guid? ProductId { get; set; }
    public Guid? CompanyId { get; set; }
    public Guid? AgentId { get; set; }
    public Guid? OwnerId { get; set; } = null;

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;

    public ICollection<Guid>? ContactIds { get; set; }
}
