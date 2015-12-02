using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sucool.PushRemind.DAL.Service;
using System.Data;
using System.Threading;

namespace Sucool.PushRemind
{
    public class NotLoginUserRemind_Service
    {
        public NotLoginUserRemindDAL _notloginuserremiddal = null;

        private void NotLoginUserRun(object o)
        {
            if (_notloginuserremiddal == null)
            {
                _notloginuserremiddal = new NotLoginUserRemindDAL();
            }

            DataSet ds = _notloginuserremiddal.GetRemindData();


            DataRowCollection rows = ds.Tables[0].Rows;
            int count = 0;
            foreach (DataRow item in rows)
            {
                string mobiles = item["mobiles"].ToString().Trim();
                count += mobiles.Split(',').Count();
                SMSHelper.SendMsg(mobiles, Config.NotLoginUserRemingMsg);                
                //记录日志
                Config.PushLog("未登陆用户提醒", mobiles, Config.NotLoginUserRemingMsg);
            }

            ConfigHelper.SetAppSettingVal("SMSSendCount",
                (ConfigHelper.GetAppSettingsString("SMSSendCount").ParseInt(0) + count).ToString());
        }
        public void Run()
        {
            int HH = Config.NotLoginUserTimeHour.ParseInt(17);
            DateTime dt = DateTime.Now;
            DateTime start = Convert.ToDateTime(dt.ToString("yyyy-MM-dd") +
                " " + HH + ":00:00");
            DateTime end = Convert.ToDateTime(dt.ToString("yyyy-MM-dd") +
                " " + HH + ":59:59");

            if ((dt >= start && dt <= end) && Config.IsALLWork())
            {
                //异步 把执行任务加入线程池
                ThreadPool.QueueUserWorkItem(new WaitCallback(NotLoginUserRun));
            }
        }
    }

}
