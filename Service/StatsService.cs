using DataRepository.DataAccess.Data;
using Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    /// <summary>
    /// 数据统计
    /// </summary>
    public class StatsService
    {
        private StatsRepository repository = new StatsRepository();
        public List<InquiryAdver> GetInquiryAdver(string datetime)
        {
            List<InquiryAdver> listInquiry = new List<InquiryAdver>();
            DataTable dt = repository.GetProInquiryAdver(datetime).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {

                string mess = "";
                foreach (DataRow dr in dt.Rows)
                {
                    InquiryAdver info = new InquiryAdver();
                    info.datetime = dr["date"].ToString();
                    info.info = "<b>PC新：</b><b  style='font-size:larger;color:Red'>（" + dr["PCCount"].ToString() + "）</b><b>手机新(" + dr["MobileCount"].ToString() + ")</b><b>广告新(" + dr["ADCount"].ToString() + ")</b>";
                    listInquiry.Add(info);
                }
            }

            DataTable dtTotal = repository.GetProInquiryAdver_Total(datetime).Tables[0];
            if (dtTotal != null && dtTotal.Rows.Count > 0)
            {

                string mess = "";
                foreach (DataRow dr in dtTotal.Rows)
                {
                    InquiryAdver info = new InquiryAdver();
                    info.datetime = dr["date"].ToString() + "汇总";
                    info.info = "<b>PC新：</b><b  style='font-size:larger;color:Red'>（" + dr["PCCount"].ToString() + "）</b><b>手机新(" + dr["MobileCount"].ToString() + ")</b><b>广告新(" + dr["ADCount"].ToString() + ")</b>";
                    listInquiry.Add(info);
                }
            }
            return listInquiry;

        }



        /// <summary>
        /// 统计数据
        /// </summary>
        /// <returns></returns>
        public List<InquiryStatsInfo> GetProInquiry(string datetime)
        {
            List<InquiryStatsInfo> listInfo = new List<InquiryStatsInfo>();
            DataTable dt = repository.GetDateTime(datetime).Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                InquiryStatsInfo info = new InquiryStatsInfo();
                DataTable dtInquiry = repository.GetProInquiry(dr["date"].ToString(), "厦门").Tables[0];
                string message = "";
                if (dtInquiry != null && dtInquiry.Rows.Count > 0)
                {
                    foreach (DataRow drIn in dtInquiry.Rows)
                    {
                        message += "<b>" + drIn["real_name"].ToString() + "：</b><b style='font-size:larger;color:Red'>" + drIn["SystemCount"].ToString() + "</b><b>（" + drIn["HandCount"].ToString() + "）" + "</b>，";
                    }
                }
                info.datetime = dr["date"].ToString();
                info.cityname = "<b>厦门</b>";
                info.info = message.TrimEnd('，');
                listInfo.Add(info);

                InquiryStatsInfo infoWys = new InquiryStatsInfo();
                dtInquiry = repository.GetProInquiry(dr["date"].ToString(), "武夷山").Tables[0];
                message = "";
                if (dtInquiry != null && dtInquiry.Rows.Count > 0)
                {
                    foreach (DataRow drIn in dtInquiry.Rows)
                    {
                        message += "<b>" + drIn["real_name"].ToString() + "：</b><b style='font-size:larger;color:Red'>" + drIn["SystemCount"].ToString() + "</b><b>（" + drIn["HandCount"].ToString() + "）" + "</b>，";
                    }
                }
                infoWys.datetime = "";//dr["date"].ToString();
                infoWys.cityname = "<b>武夷山</b>";
                infoWys.info = message.TrimEnd('，');
                listInfo.Add(infoWys);


                InquiryStatsInfo infoBj = new InquiryStatsInfo();
                dtInquiry = repository.GetProInquiry(dr["date"].ToString(), "北京").Tables[0];
                message = "";
                if (dtInquiry != null && dtInquiry.Rows.Count > 0)
                {
                    foreach (DataRow drIn in dtInquiry.Rows)
                    {
                        message += "<b>" + drIn["real_name"].ToString() + "：</b><b style='font-size:larger;color:Red'>" + drIn["SystemCount"].ToString() + "</b><b>（" + drIn["HandCount"].ToString() + "）" + "</b>，";
                    }
                }
                infoBj.datetime = "";//dr["date"].ToString();
                infoBj.cityname = "<b>北京</b>";
                infoBj.info = message.TrimEnd('，');
                listInfo.Add(infoBj);
            }

            string mess = "";

            string monthstr = dt.Rows[0]["date"].ToString().Substring(0, 7);
            DataTable dtTotal = repository.GetStatisticsOfMonth(monthstr, "厦门").Tables[0];
            InquiryStatsInfo infoTotal = new InquiryStatsInfo();
            if (dtTotal != null && dtTotal.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTotal.Rows)
                {
                    mess += "<b>" + dr["real_name"].ToString() + "：</b><b style='font-size:larger;color:Red'>" + dr["SystemCount"].ToString() + "</b><b>（" + dr["HandCount"].ToString() + "）</b>" + "，";
                }
            }
            infoTotal.datetime = monthstr + "汇总";
            infoTotal.cityname = "<b>厦门</b>";
            infoTotal.info = mess.TrimEnd('，');
            listInfo.Add(infoTotal);


            DataTable dtTotalWYS = repository.GetStatisticsOfMonth(monthstr, "武夷山").Tables[0];
            InquiryStatsInfo infoTotalWYS = new InquiryStatsInfo();
            mess = "";
            if (dtTotalWYS != null && dtTotalWYS.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTotalWYS.Rows)
                {
                    mess += "<b>" + dr["real_name"].ToString() + "：</b><b style='font-size:larger;color:Red'>" + dr["SystemCount"].ToString() + "</b><b>（" + dr["HandCount"].ToString() + "）</b>" + "，";
                }
            }
            infoTotalWYS.datetime = "";//monthstr + "汇总";
            infoTotalWYS.cityname = "<b>武夷山</b>";
            infoTotalWYS.info = mess.TrimEnd('，');
            listInfo.Add(infoTotalWYS);


            DataTable dtTotalBJ = repository.GetStatisticsOfMonth(monthstr, "北京").Tables[0];
            InquiryStatsInfo infoTotalBJ = new InquiryStatsInfo();
            mess = "";
            if (dtTotalBJ != null && dtTotalBJ.Rows.Count > 0)
            {
                foreach (DataRow dr in dtTotalBJ.Rows)
                {
                    mess += "<b>" + dr["real_name"].ToString() + "：</b><b style='font-size:larger;color:Red'>" + dr["SystemCount"].ToString() + "</b><b>（" + dr["HandCount"].ToString() + "）</b>" + "，";
                }
            }
            infoTotalBJ.datetime = "";//monthstr + "汇总";
            infoTotalBJ.cityname = "<b>北京</b>";
            infoTotalBJ.info = mess.TrimEnd('，');
            listInfo.Add(infoTotalBJ);

            return listInfo;
        }
    }
}
