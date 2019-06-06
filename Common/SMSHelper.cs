using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SMSHelper
    {
        public static void SeedSMS(string phone, string msg)
        {
            try
            {
                String account = "N1524341";//API账号
                String password = "k4nrbLgyK";//API密码
                String url = "http://smssh1.253.com/msg/send/json";
                //String phone = "15802148204";
                //String msg = "【盏天下】您的验证码是123456";//253短信测试内容
                string postJsonTpl = "\"account\":\"{0}\",\"password\":\"{1}\",\"phone\":\"{2}\",\"report\":\"true\",\"msg\":\"{3}\"";
                string jsonBody = string.Format(postJsonTpl, account, password, phone, msg);
                String result = doPostMethodToObj(url, "{" + jsonBody + "}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static string doPostMethodToObj(string url, string jsonBody)
        {
            string result = String.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";

            //BLL.Log.WriteTextLog("短信发送请求：" + jsonBody, DateTime.Now);
            httpWebRequest.Method = "POST";
            // Create NetworkCredential Object
            NetworkCredential admin_auth = new NetworkCredential("N1524341", "k4nrbLgyK");
            // Set your HTTP credentials in your request header
            httpWebRequest.Credentials = admin_auth;
            // callback for handling server certificates
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(jsonBody);
                streamWriter.Flush();
                streamWriter.Close();
                HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                    // BLL.Log.WriteTextLog("短信发送结果：" + result, DateTime.Now);
                }
            }
            return result;
        }
    }
}
