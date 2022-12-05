using System;
using System.Collections.Generic;

namespace _2lab
{
    public partial class MaterialsForProduct
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public double? MaterialAmount { get; set; }
        public int MtId { get; set; }
        public string? MatName { get; set; }
    }
}
