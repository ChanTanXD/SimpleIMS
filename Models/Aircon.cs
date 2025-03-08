using System;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace INV_MGMT_SYS
{
    [Table("AIRCON")]
    public class Aircon : BaseModel
    {
        //Auto-generated id
        [PrimaryKey("ID", false)]
        public string id { get; set; }

        [Column("MODEL")]
        public string model { get; set; }

        [Column("BRAND")]
        public string brand { get; set; }

        [Column("HP")]
        public float hp { get; set; }

        [Column("SERIES")]
        public string series { get; set; }

        [Column("PRICE")]
        public decimal price { get; set; }

        [Column("STOCK")]
        public int stock { get; set; }

        [Column("CATALOGUE_LINK")]
        public string catalogueLink { get; set; }
    }
}