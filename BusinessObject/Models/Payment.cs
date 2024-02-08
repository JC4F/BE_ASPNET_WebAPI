using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Payment: BaseModel
    {
        public string PaymentCode { get; set; }
        public string SecurityHash { get; set; }
        public float Price { get; set; }
        [ForeignKey("Order")]
        public Guid OrderID { get; set; }

        [ForeignKey("User")]
        public Guid UserID { get; set; }

        public virtual Order Order { get; set; }

        public virtual User User { get; set; }
    }
}
