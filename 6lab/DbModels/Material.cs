using System;
using System.Collections.Generic;

namespace _6lab;

public partial class Material
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public int? ProviderId { get; set; }

    public virtual ICollection<MaterialList> MaterialLists { get; } = new List<MaterialList>();

    public virtual Provider? Provider { get; set; }
}
