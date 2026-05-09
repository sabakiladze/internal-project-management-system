using System;
using System.Collections.Generic;

namespace final_task.Models;

public partial class UsersRole
{
    public Guid UsersId { get; set; }

    public Guid RoleId { get; set; }

    public DateTime? AssignedAr { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual User Users { get; set; } = null!;
}
