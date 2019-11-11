using Common;
using Entity.ViewModel;
using Infrastructure.Cache;
using Service;
using Service.BaseBiz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class UploadController:BaseController
    {
        public JsonResult UploadFile()
        {
            // 文件数为0证明上传不成功
            if (Request.Files.Count == 0)
            {
                throw new Exception("请选择上传文件！");
            }
            string dataid = Request.Form["dataId"];
            string type = Request.Form["type"];
            string urlSufix = DateTime.Now.ToString("yyyyMMdd");
            string reuploadPath = "/upload/UploadFiles/" + urlSufix + "/";
            string uploadPath = Server.MapPath("../UploadFiles/" + urlSufix+"/");
            string fileName = "";
            // 如果UploadFiles文件夹不存在则先创建
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // 保存文件到UploadFiles文件夹
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];

                string filePath = uploadPath + Path.GetFileName(file.FileName);
                fileName = file.FileName;

                // 获取文件扩展名
                string fileExtension = Path.GetExtension(filePath).ToLower();

                // 如果服务器上已经存在该文件则要修改文件名与其储存路径
                while (System.IO.File.Exists(filePath))
                {
                    Random rand = new Random();
                    fileName = rand.Next().ToString() + "-" + file.FileName;
                    filePath = uploadPath + Path.GetFileName(fileName);
                }
                // 把文件的存储路径保存起来
                // 保存文件到服务器
                file.SaveAs(filePath);

                string Url =  reuploadPath + fileName;
                if (type.Equals("News"))//新闻图片
                { 
                    ArticleEntity entity=new ArticleEntity();
                    entity.id=dataid.ToInt(0);
                    entity.img_url = Url; 
                    ArticleService.ModifyImageUrlByID(entity);
                }
                if (type.Equals("Product"))//产品图片
                {
                    ProductService.ModifyImagesByID(dataid.ToInt(0), Url);
                }
                if (type.Equals("Artisan"))//艺人图片
                {
                    ArtisanService.ModifyIDHead(dataid.ToInt(0), Url);
                }

            }

            return Json(reuploadPath + fileName);
        }
        
    }
}
