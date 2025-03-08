using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase;
using Supabase.Gotrue;
using Supabase.Gotrue.Interfaces;
using Client = Supabase.Client;

namespace INV_MGMT_SYS
{
    class DatabaseClient
    {
        public Client client;
        public DatabaseClient(Client client)
        {
            this.client = client;
        }
    }
}
