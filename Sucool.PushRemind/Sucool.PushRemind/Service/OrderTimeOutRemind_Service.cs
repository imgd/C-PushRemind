using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Sucool.PushRemind.DAL;
using System.Threading;

namespace Sucool.PushRemind
{
    public class OrderTimeOutRemind_Service
    {
        public OrderTimeOutRemindDAL _ordertimeoutdal = null;

        private void OrderTimeoutdalRun(object o)
        {
            if (_ordertimeoutdal == null)
            {
                _ordertimeoutdal = new OrderTimeOutRemindDAL();
            }

            DataSet ds = _ordertimeoutdal.GetRemindData();


            DataRowCollection rows = ds.Tables[0].Rows;
            int count = 0;
            foreach (DataRow item in rows)
            {
                string mobiles = item["mobiles"].ToString().Trim();
                count += mobiles.Split(',').Count();
                SMSHelper.SendMsg(mobiles, Config.OrderTimeOutRemindMsg);

                //记录日志
                Config.PushLog("订单超时提醒", mobiles, Config.OrderTimeOutRemindMsg); 
            }
            ConfigHelper.SetAppSettingVal("SMSSendCount",
                (ConfigHelper.GetAppSettingsString("SMSSendCount").ParseInt(0) + count).ToString());
        }
        public void Run()
        {          

            if (Config.IsALLWork())
            {
                //异步 把执行任务加入线程池
                ThreadPool.QueueUserWorkItem(new WaitCallback(OrderTimeoutdalRun));
            }
        }
    }
}
