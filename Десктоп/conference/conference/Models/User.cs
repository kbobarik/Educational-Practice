using System;
using System.Collections.Generic;

namespace conference.Models;

public partial class User
{
    public int IdUser { get; set; }

    public DateOnly DateOfBitrh { get; set; }

    public int? IdCounrty { get; set; }

    public int? IdDirection { get; set; }

    public string Password { get; set; } = null!;

    public string Image { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int IdGender { get; set; }

    public int IdRole { get; set; }

    public int? IdTypeOfEvents { get; set; }

    public virtual Country? IdCounrtyNavigation { get; set; }

    public virtual Direction? IdDirectionNavigation { get; set; }

    public virtual Gender IdGenderNavigation { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual TypeOfEvent? IdTypeOfEventsNavigation { get; set; }

    public virtual ICollection<ScheduleActivity> ScheduleActivities { get; set; } = new List<ScheduleActivity>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    public virtual ICollection<SheduleActivityJure> SheduleActivityJures { get; set; } = new List<SheduleActivityJure>();
}
