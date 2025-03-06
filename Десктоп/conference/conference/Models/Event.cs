using System;
using System.Collections.Generic;

namespace conference.Models;

public partial class Event
{
    public int IdEvent { get; set; }

    public int IdCity { get; set; }

    public string? Title { get; set; }

    public DateOnly Date { get; set; }

    public int IdTypeOfEvent { get; set; }

    public string? Image { get; set; }

    public virtual City IdCityNavigation { get; set; } = null!;

    public virtual TypeOfEvent IdTypeOfEventNavigation { get; set; } = null!;

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
