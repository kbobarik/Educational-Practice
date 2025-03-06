using System;
using System.Collections.Generic;

namespace conference.Models;

public partial class Direction
{
    public int IdDirection { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
