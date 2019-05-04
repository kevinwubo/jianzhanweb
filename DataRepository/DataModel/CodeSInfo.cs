using Infrastructure.EntityFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataModel
{
    [Serializable]
    public class CodeSInfo
    {
        [DataMapping("ID", DbType.Int32)]
        public int ID { get; set; }
                
        [DataMapping("Code", DbType.String)]
        public string Code { get; set; }

        [DataMapping("CodeName", DbType.String)]
        public string CodeName { get; set; }

        [DataMapping("CodeValues", DbType.String)]
        public string CodeValues { get; set; }

        [DataMapping("Sort", DbType.Int32)]
        public string Sort { get; set; }

        [DataMapping("OperatorID", DbType.String)]
        public string OperatorID { get; set; }

        [DataMapping("IsShow", DbType.Byte)]
        public string IsShow { get; set; }

        [DataMapping("Adddate", DbType.DateTime)]
        public DateTime Adddate { get; set; }

        [DataMapping("change_date", DbType.DateTime)]
        public DateTime change_date { get; set; }
    }
}
