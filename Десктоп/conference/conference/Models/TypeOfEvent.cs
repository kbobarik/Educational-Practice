using System;
using System.Collections.Generic;

namespace conference.Models;

public partial class TypeOfEvent
{
    public int IdTypeOfEvent { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
