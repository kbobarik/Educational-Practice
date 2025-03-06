using System;
using System.Collections.Generic;

namespace conference.Models;

public partial class ScheduleActivity
{
    public int IdSchedule { get; set; }

    public int IdActivity { get; set; }

    public int IdModerator { get; set; }

    public int IdSheduleActivity { get; set; }

    public int NumberDay { get; set; }

    public TimeOnly StartTime { get; set; }

    public virtual Activity IdActivityNavigation { get; set; } = null!;

    public virtual User IdModeratorNavigation { get; set; } = null!;

    public virtual Schedule IdScheduleNavigation { get; set; } = null!;

    public virtual ICollection<SheduleActivityJure> SheduleActivityJures { get; set; } = new List<SheduleActivityJure>();
}
