using System;
using System.Collections.Generic;

namespace final_task.Models;

public partial class TasksProject
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime Deadline { get; set; }

    public string Status { get; set; } = null!;

    public Guid? UserId { get; set; }

    public Guid ProjectId { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual User? User { get; set; }
}
