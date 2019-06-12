using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.BaseData
{
    public class ManagerStatement
    {

        public static string GetManagerByRule = @"SELECT * FROM dt_manager(NOLOCK) WHERE 1=1";

        public static string GetAllManager = @"SELECT * FROM dt_manager(NOLOCK) WHERE is_lock=0 ";

        public static string GetBaseDataBySalesName = @"SELECT * FROM dt_manager(NOLOCK) WHERE real_name=@real_name";

        public static string GetManagerByID = @"SELECT * FROM dt_manager(NOLOCK) WHERE id=@id";
    }
}
