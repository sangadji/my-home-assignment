using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAssignment.Web.Data
{
    public class ProductInventory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None), Key]
        [JsonProperty]
        public int Id { get; set; }
       
        [ForeignKey("Product")]
        [JsonProperty]
        public int ProductId { get; set; }
        [ForeignKey("Inventory")]
        [JsonProperty("art_id")]
        public int InventoryId { get; set; }
        [JsonProperty("amount_of")]
        public int Amount { get; set; }

        public virtual Product Product { get; set; }
        public virtual Inventory Inventory { get; set; }

    }
}
