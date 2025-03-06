using System;
using System.Collections.Generic;

namespace conference.Models;

public partial class SheduleActivityJure
{
    public int IdSheduleActivityJure { get; set; }

    public int IdScheduleActivity { get; set; }

    public int IdJure { get; set; }

    public virtual User IdJureNavigation { get; set; } = null!;

    public virtual ScheduleActivity IdScheduleActivityNavigation { get; set; } = null!;
}
