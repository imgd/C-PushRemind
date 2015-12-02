using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sucool.PushRemind.Model;
using System.Data.SqlClient;
using System.Data;

namespace Sucool.PushRemind.DAL.Service
{
    public class JushNotLoginUserRemind_DAL
    {
        public List<Push_NotLoginUser_Model> NotLoginUserList() 
        {
            string sql = @"SELECT Id FROM dbo.UserBase WHERE DATEDIFF(DAY,LastLoginTime,GETDATE())%@DAY=0 AND Status=0";
            SqlParameter[] para = new SqlParameter[] 
            {
                new SqlParameter("@DAY",SqlDbType.Int){Value=Config.JushNotLoginUsersDay}
            };
            List<Push_NotLoginUser_Model> list = new List<Push_NotLoginUser_Model>();
            using (SqlDataReader dr = SqlDBProvider.ExecuteReader(sql, para)) 
            {
                list = dr.ConvertToList<Push_NotLoginUser_Model>();
            }
            return list;
        }
    }
}
