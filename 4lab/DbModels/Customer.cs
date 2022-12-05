using System;
using System.Collections.Generic;

namespace lab4.DbModels;

public partial class Customer
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
