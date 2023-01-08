using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace _6lab;

public partial class Worker
{
    public int Id { get; set; }

    [Display(Name = "Name")]
    public string? Name { get; set; }

    [Display(Name = "Section")]
    public string? Section { get; set; }

    [Display(Name = "Position")]
    public string? Position { get; set; }

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
