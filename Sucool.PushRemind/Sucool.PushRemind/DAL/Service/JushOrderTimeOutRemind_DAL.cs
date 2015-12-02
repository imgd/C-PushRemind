using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Sucool.PushRemind.Model;

namespace Sucool.PushRemind.DAL.Service
{
    public class JushOrderTimeOutRemind_DAL
    {
        public List<Push_OrderOut_Model> OrderOutList()
        {
            List<Push_OrderOut_Model> list = new List<Push_OrderOut_Model>();
            string sql = @"SELECT DISTINCT UserId  FROM dbo.Orderinfo WHERE dbo.GetOrderStatus(Id)=1 AND Status=0 AND Pay_time IS NULL AND  DATEADD(HOUR,24,Order_time) BETWEEN DATEADD(HOUR,1,GETDATE()) AND DATEADD(HOUR,@HOUR,GETDATE())";
            SqlParameter[] para=null;
            if (DateTime.Now.Hour == Config.JushOrderTimeOutRemindHourHour1 && Config.IsALLWork()) 
            {
                para = new SqlParameter[] 
                {
                    //第一个推送 12:00 推送 13:00--22:00的订单
                    new SqlParameter("@HOUR",SqlDbType.Int){Value=Config.JushOrderTimeOutRemindHourHour2-Config.JushOrderTimeOutRemindHourHour1+1}
                };
            }
            if (DateTime.Now.Hour == Config.JushOrderTimeOutRemindHourHour2 && Config.IsALLWork()) 
            {
                //第第二个推送 21:00 推送 21:00--12:00的订单
                para = new SqlParameter[] 
                {
                    new SqlParameter("@HOUR",SqlDbType.Int){Value=25-Config.JushOrderTimeOutRemindHourHour2+Config.JushOrderTimeOutRemindHourHour1}
                };
            }
            if (para != null) 
            {
                using (SqlDataReader dr = SqlDBProvider.ExecuteReader(sql, para))
                {
                    list = dr.ConvertToList<Push_OrderOut_Model>();
                }
            }
            return list;
        }
    }
}
