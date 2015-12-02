using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;

namespace Sucool.PushRemind
{
    public class GoldBookRemind_Service
    {
        private GoldBookRemind_DAL _gbkdal = null;
        //private object lockthis = new object();
        /// <summary>
        /// 优惠卷过期提醒 任务
        /// </summary>
        private void GoldBookRemindRun(object o)
        {
            if (_gbkdal == null)
            {
                _gbkdal = new GoldBookRemind_DAL();
            }

            //加锁防止其他线程同时调用 线程过少不用加锁                
            DataSet ds = _gbkdal.GetRemindData();


            DataRowCollection rows = ds.Tables[0].Rows;
            int count = 0;
            foreach (DataRow item in rows)
            {
                string mobiles = item["mobiles"].ToString().Trim();
                count += mobiles.Split(',').Count();

                SMSHelper.SendMsg(mobiles, Config.GoldBookRemindRemingMsg);

                //记录日志
                Config.PushLog("优惠卷超时提醒",mobiles, Config.GoldBookRemindRemingMsg);                
            }
            ConfigHelper.SetAppSettingVal("SMSSendCount", 
                (ConfigHelper.GetAppSettingsString("SMSSendCount").ParseInt(0) + count).ToString());
        }
        public void Run()
        {
            int HH = Config.GoldBookRemindTimeHour.ParseInt(16);
            DateTime dt = DateTime.Now;
            DateTime start = Convert.ToDateTime(dt.ToString("yyyy-MM-dd") +
                " " + HH + ":00:00");
            DateTime end = Convert.ToDateTime(dt.ToString("yyyy-MM-dd") +
                " " + HH + ":59:59");

            if ((dt >= start && dt <= end) && Config.IsALLWork())
            {
                //异步 把执行任务加入线程池
                ThreadPool.QueueUserWorkItem(new WaitCallback(GoldBookRemindRun));
            }
        }

    }
}
