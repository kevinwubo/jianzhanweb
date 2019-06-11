using DataRepository.DataAccess.BaseData;
using DataRepository.DataModel;
using Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Helper;

namespace Service.BaseBiz
{
    public class ManagerService : BaseService
    {
        public static List<ManagerEntity> GetManagerAll()
        {
            List<ManagerEntity> all = new List<ManagerEntity>();
            ManagerRepository mr = new ManagerRepository();
            List<ManagerInfo> miList = Cache.Get<List<ManagerInfo>>("ManagerALL");
            if (miList.IsEmpty())
            {
                miList = mr.GetAllManager();
                Cache.Add("ManagerALL", miList);
            }
            if (!miList.IsEmpty())
            {
                foreach (ManagerInfo info in miList)
                {
                    ManagerEntity entity = TranslateManagerEntity(info);
                    all.Add(entity);
                }
            }

            return all;

        }


        private static ManagerEntity TranslateManagerEntity(ManagerInfo info)
        {
            ManagerEntity entity = new ManagerEntity();
            if (info != null)
            {
 
            }
            return entity;
        }
    }
}
