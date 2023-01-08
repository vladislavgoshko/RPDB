using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace _6lab;

public partial class Customer
{
    public int Id { get; set; }

    [Display(Name = "Name")]
    public string? Name { get; set; }

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}