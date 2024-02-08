using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Product: BaseModel 
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public string Status { get; set; }
        [ForeignKey("Category")]
        public Guid CategoryID { get; set; }
        [ForeignKey("User")]
        public Guid SupplierID { get; set; }
        public virtual Category Category { get; set; }  
        public virtual User User { get; set; }
    }
}
