using System;
using System.Collections.Generic;

namespace lab4.DbModels;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<MaterialList> MaterialLists { get; } = new List<MaterialList>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
