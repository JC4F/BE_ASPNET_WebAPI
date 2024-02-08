using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Order: BaseModel
    {
        public bool isCheckout { get; set; }
        [ForeignKey("User")]
        public Guid UserID { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<OrderItem> Orders { get; set; }
    }
}
