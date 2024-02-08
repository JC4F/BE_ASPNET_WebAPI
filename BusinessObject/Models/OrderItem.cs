using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class OrderItem: BaseModel
    {
        public int Quantity { get; set; }
        [ForeignKey("Product")]
        public Guid ProductID { get; set; }
        [ForeignKey("User")]
        public Guid UserID { get; set; }
        [ForeignKey("Order")]
        public Guid OrderID { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
        public virtual Order Order { get; set; }
    }
}
