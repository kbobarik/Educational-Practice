using System;
using System.Collections.Generic;

namespace conference.Models;

public partial class Schedule
{
    public int IdSchedule { get; set; }

    public int IdWinner { get; set; }

    public int IdEvent { get; set; }

    public virtual Event IdEventNavigation { get; set; } = null!;

    public virtual User IdWinnerNavigation { get; set; } = null!;

    public virtual ICollection<ScheduleActivity> ScheduleActivities { get; set; } = new List<ScheduleActivity>();
}
