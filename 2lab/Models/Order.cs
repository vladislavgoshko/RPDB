using System;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;
using System.Xml.Linq;

namespace _2lab.Models
{
    public partial class Order
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public int? ProductId { get; set; }
        public int? Amount { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ExecutionStartDate { get; set; }
        public DateTime? ImplementationDate { get; set; }
        public DateTime? DeliveryOrderDate { get; set; }
        public int? WorkerId { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual Product? Product { get; set; }
        public virtual Worker? Worker { get; set; }
        public override string ToString()
        {
            return $"Id: {Id}, CustomerId: {CustomerId}, ProductId: {ProductId}, " +
                $"Amount: {Amount}, OrderDate: {OrderDate.ToString().Split()[0]}, " +
                $"ExecutionStartDate: {ExecutionStartDate.ToString().Split()[0]}, " +
                $"ImplementationDate: {ImplementationDate.ToString().Split()[0]}, " +
                $"DeliveryOrderDate: {DeliveryOrderDate.ToString().Split()[0]}, " +
                $"WorkerId: {WorkerId}";
        }
    }
}
