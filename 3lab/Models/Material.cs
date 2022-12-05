using System;
using System.Collections.Generic;

namespace WebApplicationSewingCompany.Models
{
    public partial class Material
    {
        public Material()
        {
            MaterialLists = new HashSet<MaterialList>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public double? QuantityInStock { get; set; }
        public int? ProviderId { get; set; }

        public virtual Provider? Provider { get; set; }
        public virtual ICollection<MaterialList> MaterialLists { get; set; }
    }
}
