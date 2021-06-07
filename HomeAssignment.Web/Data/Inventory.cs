using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAssignment.Web.Data
{
    public class Inventory
    {
        [JsonProperty("art_id")]
        [Display(Name = "Article Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None), Key]
        public int Id { get; set; }
        [JsonProperty]
        public string Name { get; set; }
        [JsonProperty]
        public int Stock { get; set; }

        public ICollection<ProductInventory> ProductInventory { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
