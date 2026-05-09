using System;
using System.Collections.Generic;

namespace final_task.Models;

public partial class ProjectMember
{
    public Guid ProjectId { get; set; }

    public Guid UserId { get; set; }

    public DateTime JoinedAt { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
