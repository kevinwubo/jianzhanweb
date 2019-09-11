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
    public class ManagerService:BaseService
    {
        public static List<ManagerEntity> GetManagerAll()
        {
            List<ManagerEntity> all = new List<ManagerEntity>();
            ManagerRepository mr = new ManagerRepository();
            List<ManagerInfo> miList = Cache.Get<List<ManagerInfo>>("ManagerALLNEW1");
            if (miList.IsEmpty())
            {
                miList = mr.GetAllManager();
                Cache.Add("ManagerALLNEW1", miList);
            }
            if (!miList.IsEmpty())
            {
                foreach (ManagerInfo mInfo in miList)
                {
                    ManagerEntity ManagerEntity = TranslateManagerEntity(mInfo);
                    all.Add(ManagerEntity);
                }
            }

            return all;
        }



        private static ManagerEntity TranslateManagerEntity(ManagerInfo info)
        {
            ManagerEntity entity = new ManagerEntity();
            if (info != null)
            {
                entity.id = info.id;
                entity.role_id = info.role_id;
                entity.role_type = info.role_type;
                entity.user_name = info.user_name;
                entity.password = info.password;
                entity.salt = info.salt;
                entity.real_name = info.real_name;
                entity.telephone = info.telephone;
                entity.email = info.email;
                entity.salesCount = info.salesCount;
                entity.CityName = info.CityName;
                entity.is_lock = info.is_lock;
                entity.add_time = info.add_time;
                //当天咨询量
                List<InquiryEntity> list = InquiryService.GetInquiryByRule("", "", "", " AND AddDate Between '" + DateTime.Now.ToShortDateString() + " 00:00:01' and '" + DateTime.Now.ToShortDateString() + " 23:59:59'", "新", entity.id.ToString());
                entity.currentSalesCount = list != null && list.Count > 0 ? list.Count : 0;
            }
            return entity;
        }
    }
}
