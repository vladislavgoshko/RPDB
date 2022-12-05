using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab4.DbModels;

public partial class Provider
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public double? MaterialAmount { get; set; }

    public decimal? Price { get; set; }
    [DisplayFormat(DataFormatString = "{0:d}")]
    public DateTime? DeliveryDate { get; set; }

    public virtual ICollection<Material> Materials { get; } = new List<Material>();
}
