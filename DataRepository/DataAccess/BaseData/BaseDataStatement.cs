using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.BaseData
{
    public class BaseDataStatement
    {
        public static string InsertTelephone = @"INSERT INTO dt_TempInfo(Telephone) VALUES(@Telephone)";

        public static string GetTelephone = @"SELECT ID,Telephone FROM dt_TempInfo(NOLOCK)";

        public static string GetQRCode = @"SELECT * FROM dt_QRCode(NOLOCK) WHERE 1=1 ";

        public static string GetCodeValuesByRule = @"SELECT * FROM dt_Codes(NOLOCK) WHERE 1=1";

        public static string GetAllCity = @"SELECT * FROM City(NOLOCK)";

        public static string GetAllHasCity = @"SELECT * FROM City(NOLOCK) WHERE HasCar=1 and HasBattery=1";

        public static string GetAllProvince = @"SELECT ProvinceID,ProvinceName FROM Province(NOLOCK)";

        public static string GetAllBaseData = @"SELECT * FROM BaseData(NOLOCK) ORDER BY TypeCode";

        public static string GetAllBaseDataByRule = @"SELECT * FROM BaseData(NOLOCK) WHERE 1=1 ";

        public static string GetBaseDataByKey = @"SELECT * FROM BaseData(NOLOCK) WHERE ID=@ID";

        public static string Remove = @"UPDATE BaseData SET Status=0 WHERE ID=@ID";

        public static string GetBaseDataByType = @"SELECT * FROM BaseData(NOLOCK) WHERE TypeCode=@TypeCode AND Status=1";

        public static string GetBaseDataByPCode = @"SELECT * FROM BaseData(NOLOCK) WHERE PCode=@PCode AND Status=1";

        public static string CreateNewData = @"INSERT INTO [BaseData]([TypeCode],[PCode],[ValueInfo],[Description],[Status],[CreateDate]) VALUES (@TypeCode,@PCode,@ValueInfo,@Description,@Status,@CreateDate)";

        public static string ModifyBaseData = @"UPDATE [BaseData]
                                               SET [TypeCode] =@TypeCode,[PCode]=@PCode,[ValueInfo]=@ValueInfo
                                                  ,[Description]=@Description
                                                  ,[Status] = @Status
                                             WHERE ID=@ID";
        public static string CreateAttachment = @"INSERT INTO [Attachment]([FileName],[FileExtendName],[FilePath],[UploadDate],[FileType],[BusinessType],[Channel],[FileSize],[Remark],[Operator],[CreateDate]) 
                                                     VALUES(@FileName,@FileExtendName,@FilePath,@UploadDate,@FileType,@BusinessType,@Channel,@FileSize,@Remark,@Operator,@CreateDate);SELECT IDENT_CURRENT('Attachment')";

        public static string GetAttachmentByKey = @"SELECT * FROM [Attachment](NOLOCK) WHERE AttachmentID IN (#ids#)";

        public static string ModifyCodes = @"UPDATE dt_Codes SET CodeValues=@CodeValues,change_date=GETDATE() WHERE ID=@ID";
    }
}
