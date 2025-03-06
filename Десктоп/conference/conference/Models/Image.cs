using System;
using System.Collections.Generic;

namespace conference.Models;

public partial class Image
{
    public int IdImage { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<City> IdCities { get; set; } = new List<City>();
}
