using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace HomeAssignment.Web.Data
{
    public class Product
    {
        [Display(Name = "Product Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None), Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Articles")]
        public ICollection<ProductInventory> ProductInventory { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
