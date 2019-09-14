using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common
{ 
    public class HtmlHelper
    {
        #region 获取模板页的Html代码
        /// <summary>
        /// 获取页面的Html代码
        /// </summary>
        /// <param name="url">模板页面路径</param>
        /// <param name="encoding">页面编码</param>
        /// <returns></returns>
        public static string GetHtml(string url, System.Text.Encoding encoding)
        {
            byte[] buf = new WebClient().DownloadData(url);
            if (encoding != null)
            {
                return encoding.GetString(buf);
            }
            string html = System.Text.Encoding.UTF8.GetString(buf);
            encoding = GetEncoding(html);
            if (encoding == null || encoding == System.Text.Encoding.UTF8)
            {
                return html;
            }
            return encoding.GetString(buf);
        }

        /// <summary>
        /// 获取页面的编码
        /// </summary>
        /// <param name="html">Html源码</param>
        /// <returns></returns>
        public static System.Text.Encoding GetEncoding(string html)
        {
            string pattern = @"(?i)\bcharset=(?<charset>[-a-zA-Z_0-9]+)";
            string charset = Regex.Match(html, pattern).Groups["charset"].Value;
            try
            {
                return System.Text.Encoding.GetEncoding(charset);
            }
            catch (ArgumentException)
            {
                return null;
            }
        }
        #endregion

        #region 用于生成Html静态页
        /// <summary>
        /// 创建静态文件
        /// </summary>
        /// <param name="result">Html代码</param>
        /// <param name="createpath">生成路径</param>
        /// <returns></returns>
        public static bool CreateFileHtmlByTemp(string result, string createpath)
        {
            if (!string.IsNullOrEmpty(result))
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;//+ @"Html\";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileFullPath = path + "/" + createpath;

                try
                {                    
                    if (File.Exists(fileFullPath))
                    {
                        File.Delete(fileFullPath);
                    }

                    FileStream fs2 = new FileStream(fileFullPath, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs2, new System.Text.UTF8Encoding(false));//去除UTF-8 BOM
                    sw.Write(result);
                    sw.Close();
                    fs2.Close();
                    fs2.Dispose();
                    return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return false;
        }
        #endregion

        #region 调用静态模板，并且传递数据模型实体类 创建Html静态页
        /// <summary>
        /// 解析模板生成静态页
        /// </summary>
        /// <param name="temppath">模板地址</param>
        /// <param name="path">静态页地址</param>
        /// <param name="t">数据模型</param>
        /// <returns></returns>
        public static bool CreateStaticPage(string temppath, string path)
        {
            try
            {
                //获取模板Html
                string TemplateContent = GetHtml(temppath, System.Text.Encoding.UTF8);
                if (string.IsNullOrEmpty(TemplateContent))
                {
                    return false;
                }
                //创建静态文件
                return CreateFileHtmlByTemp(TemplateContent, path);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
