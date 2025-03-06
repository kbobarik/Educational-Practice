using System;
using System.Collections.Generic;

namespace conference.Models;

public partial class City
{
    public int IdCity { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Image> IdImages { get; set; } = new List<Image>();
}
