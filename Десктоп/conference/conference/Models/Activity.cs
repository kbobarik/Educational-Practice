using System;
using System.Collections.Generic;

namespace conference.Models;

public partial class Activity
{
    public int IdActivity { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<ScheduleActivity> ScheduleActivities { get; set; } = new List<ScheduleActivity>();
}
