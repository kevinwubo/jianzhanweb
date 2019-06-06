using Infrastructure.EntityFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataModel
{
    public class ManagerInfo
    {
        [DataMapping("id", DbType.Int32)]
        public int id { get; set; }

        [DataMapping("role_id", DbType.Int32)]
        public int role_id { get; set; }

        [DataMapping("role_type", DbType.Int32)]
        public int role_type { get; set; }

        [DataMapping("user_name", DbType.String)]
        public string user_name { get; set; }

        [DataMapping("password", DbType.String)]
        public string password { get; set; }

        [DataMapping("salt", DbType.String)]
        public string salt { get; set; }

        [DataMapping("real_name", DbType.String)]
        public string real_name { get; set; }

        [DataMapping("telephone", DbType.String)]
        public string telephone { get; set; }

        [DataMapping("email", DbType.String)]
        public string email { get; set; }

        [DataMapping("salesCount", DbType.Int32)]
        public int salesCount { get; set; }

        [DataMapping("CityName", DbType.String)]
        public string CityName { get; set; }

        [DataMapping("artisanID", DbType.Int32)]
        public int is_lock { get; set; }

        [DataMapping("add_time", DbType.DateTime)]
        public DateTime add_time { get; set; }
    }
}
