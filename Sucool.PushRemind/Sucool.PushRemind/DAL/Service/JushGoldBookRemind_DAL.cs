using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sucool.PushRemind.Model;
using System.Data.SqlClient;
using System.Data;

namespace Sucool.PushRemind.DAL.Service
{
    public class JushGoldBookRemind_DAL
    {
        public List<Push_GoldBook_Model> GoldOutList() 
        {
            List<Push_GoldBook_Model> list = new List<Push_GoldBook_Model>();
            string sql = @"SELECT DISTINCT UsedId FROM dbo.GoldBook WHERE Status = 1 AND UsedOrder IS NULL AND UsedId > 0 AND EndTime BETWEEN DATEADD(HOUR,1,GETDATE()) AND DATEADD(HOUR,@HOUR,GETDATE()) ";
            SqlParameter[] para=null;
            if (DateTime.Now.Hour == Config.JushGoldBookRemindTimeHour1 && Config.IsALLWork()) 
            {
                para = new SqlParameter[] 
                {
                    //第一个推送 9:00 推送 9:00--20:00的
                    new SqlParameter("@HOUR",SqlDbType.Int){Value=Config.JushGoldBookRemindTimeHour2-Config.JushGoldBookRemindTimeHour1+1}
                };
            }
            if (DateTime.Now.Hour == Config.JushGoldBookRemindTimeHour2 && Config.IsALLWork())
            {
                para = new SqlParameter[] 
                {
                    //第二个推送 19:00 推送 19:00--10:00的
                    new SqlParameter("@HOUR",SqlDbType.Int){Value=25-Config.JushGoldBookRemindTimeHour2+Config.JushGoldBookRemindTimeHour1}
                };
            }
            if (para != null)
            {
                using (SqlDataReader dr = SqlDBProvider.ExecuteReader(sql, para))
                {
                    list = dr.ConvertToList<Push_GoldBook_Model>();
                }
            }
            return list;
        }

        public List<Push_GoldBook_Model> GoldAddList() 
        {
            List<Push_GoldBook_Model> list = new List<Push_GoldBook_Model>();
            string sql = @"SELECT DISTINCT UsedId FROM dbo.GoldBook WHERE Status = 1 AND UsedOrder IS NULL AND UsedId > 0 AND ActivateTime BETWEEN DATEADD(HOUR,-@HOUR,GETDATE()) AND DATEADD(HOUR,-1,GETDATE())";
            SqlParameter[] para = null;
            if (DateTime.Now.Hour == Config.JushAddGoldBookRemindTimeHour1 && Config.IsALLWork()) 
            {
                para = new SqlParameter[] 
                {
                    new SqlParameter("@HOUR",SqlDbType.Int){Value=25-Config.JushAddGoldBookRemindTimeHour2+Config.JushAddGoldBookRemindTimeHour1}
                };
            }
            if (DateTime.Now.Hour == Config.JushAddGoldBookRemindTimeHour2 && Config.IsALLWork()) 
            {
                para = new SqlParameter[] 
                {
                    new SqlParameter("@HOUR",SqlDbType.Int){Value=Config.JushAddGoldBookRemindTimeHour2-Config.JushAddGoldBookRemindTimeHour1+1}
                };
            }
            if (para != null) 
            {
                using (SqlDataReader dr = SqlDBProvider.ExecuteReader(sql, para))
                {
                    list = dr.ConvertToList<Push_GoldBook_Model>();
                }
            }
            return list;
        }
    }
}
