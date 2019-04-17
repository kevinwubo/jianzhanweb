using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class EnumHelper
    {

    }
    //承运商：Carrier 仓库：Storage  客户：Customer  门店：Store Receiver:收货人
    public enum UnionType
    {
        Carrier,
        Storage,
        Customer,
        Store,
        Receiver
    }
}
