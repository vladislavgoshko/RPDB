using System;
using System.Collections.Generic;

namespace _6lab;

public partial class Provider
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? MaterialAmount { get; set; }

    public decimal? Price { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public virtual ICollection<Material> Materials { get; } = new List<Material>();
}
