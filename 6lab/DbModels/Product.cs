using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _6lab;

public partial class Product
{
    public int Id { get; set; }

    [Display(Name = "Name")]
    public string? Name { get; set; }

    [Display(Name = "Price")]
    public decimal? Price { get; set; }

    [JsonIgnore]
    public virtual ICollection<MaterialList> MaterialLists { get; } = new List<MaterialList>();

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
