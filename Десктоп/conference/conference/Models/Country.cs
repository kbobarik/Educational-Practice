using System;
using System.Collections.Generic;

namespace conference.Models;

public partial class Country
{
    public int IdCountry { get; set; }

    public string RussianTitle { get; set; } = null!;

    public string EnglishTitle { get; set; } = null!;

    public string? Code1 { get; set; }

    public int? Code2 { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
