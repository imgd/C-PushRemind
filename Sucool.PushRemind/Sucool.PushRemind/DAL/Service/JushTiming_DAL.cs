using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sucool.PushRemind.Model;
using System.Data.SqlClient;
using System.Data;

namespace Sucool.PushRemind.DAL.Service
{
    public class JushTiming_DAL
    {
        public List<Push_Model> PushTiming()
        {
            string sql = @"SELECT * FROM dbo.SC_APPPush WHERE SendTime BETWEEN GETDATE() AND DATEADD(SECOND,@SECOND,GETDATE()) AND SendType=1 AND Status=0";
            List<Push_Model> list = new List<Push_Model>();
            SqlParameter[] para = new SqlParameter[] 
            {
                new SqlParameter("@SECOND",SqlDbType.Int){Value=Config.JushTimingInteval/1000}
            };
            using (SqlDataReader dr = SqlDBProvider.ExecuteReader(sql, para))
            {
                list = dr.ConvertToList<Push_Model>();
            }
            return list;
        }


        /// <summary>
        /// 定时发送更改数据库
        /// </summary>
        /// <param name="MessageID"></param>
        /// <param name="ErrorCode"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int PushTimingUpdateStatu(long MessageID, int ErrorCode, int Id)
        {
            string sql = @"UPDATE dbo.SC_APPPush SET MessageID=@MessageID,ErrorCode=@ErrorCode WHERE Id=@Id";
            SqlParameter[] para = new SqlParameter[] 
            {
                new SqlParameter("@MessageID",SqlDbType.BigInt){Value=MessageID},
                new SqlParameter("@ErrorCode",SqlDbType.Int){Value=ErrorCode},
                new SqlParameter("@Id",SqlDbType.Int){Value=Id}
            };
            if (SqlDBProvider.ExecuteNonQuery(sql, para) > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

    }
}
