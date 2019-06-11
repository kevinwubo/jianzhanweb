using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModel
{
    public class ManagerEntity
    {
        public int id { get; set; }
        public int role_id { get; set; }
        public int role_type { get; set; }

        public string user_name { get; set; }
        public string password { get; set; }
        public string salt { get; set; }
        public string real_name { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }
        public int salesCount { get; set; }
        public string CityName { get; set; }
        public int is_lock { get; set; }
        public DateTime add_time { get; set; }

        public int currentSalesCount { get;set;}
    }
}
