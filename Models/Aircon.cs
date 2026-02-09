using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace INV_MGMT_SYS
{
    [Table("Aircon")]
    public class Aircon : BaseModel
    {
        //Auto-generated id
        [PrimaryKey("id", false)]
        public string id { get; set; }

        [Column("Model")]
        public string model { get; set; }

        [Column("Brand")]
        public string brand { get; set; }

        [Column("hp")]
        public decimal hp { get; set; }

        [Column("Series")]
        public string series { get; set; }

        [Column("Price")]
        public decimal price { get; set; }

        [Column("Stock")]
        public int stock { get; set; }

        [Column("Catalogue_Link")]
        public string catalogueLink { get; set; }
    }
}