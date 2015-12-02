using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sucool.PushRemind.DAL.Service;
using System.Data;
using Sucool.PushRemind.Model;
using Jpush.api;
using Jpush.api.push.mode;
using Jpush.api.push.notification;

namespace Sucool.PushRemind.Service
{
    public class JushOrderTimeOutRemind_Service
    {
        private JushOrderTimeOutRemind_DAL _ordertimeoutdal = null;
        private List<Push_OrderOut_Model> list = null;
        private JPushClient client = null;

        public JushOrderTimeOutRemind_Service()
        {
            client = new JPushClient(Config.PushApp_key, Config.PushMaster_secret);
        }
        private void OrderTimeoutdalRun(object o)
        {
            if (_ordertimeoutdal == null)
            {
                _ordertimeoutdal = new JushOrderTimeOutRemind_DAL();
            }
            list = _ordertimeoutdal.OrderOutList();
            if (list.Count > 0)
            {
                List<string> list_users = new List<string>();
                list.ForEach(z => list_users.Add(z.UserId.ToString().DESEncrypt().StringToMd5().Md532SubString()));
                //JPushClient client = new JPushClient(Config.PushApp_key, Config.PushMaster_secret);
                PushPayload payload = new PushPayload();

                payload.platform = Platform.all();
                payload.audience = Audience.s_alias(list_users.ToArray());

                payload.notification = new Notification().setAlert(Config.JushOrderTimeOutRemindMsg).setAndroid(new AndroidNotification().AddExtra("key", Config.JushGoPageOrder)).setIos(new IosNotification().AddExtra("key", Config.JushGoPageOrder));
                try
                {
                    client.SendPush(payload);
                    //记录日志
                    Config.PushLogAPP("APP订单超时推送提醒", list_users.ListToStrings(), Config.JushOrderTimeOutRemindMsg);
                }
                catch (Jpush.api.common.APIRequestException ee)
                {
                    string.Format("\r\n>>>APP订单超时推送提醒 推送异常：{0},time:{1}",
                    ee.Message,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    ).WriteLog("ERROR");
                }
            }

        }


        public void Run()
        {
            if (Config.IsALLWork())
            {
                int hour = DateTime.Now.ToString("HH").ParseInt();
                int timeS = Config.JushOrderTimeOutRemindHourHour1.ParseInt(12);
                int timeY = Config.JushOrderTimeOutRemindHourHour2.ParseInt(21);
                int timeE = timeS + 1;

                if ((hour == timeS || hour == timeY) && Config.IsALLWork())
                {
                    //异步 把执行任务加入线程池
                    ThreadPool.QueueUserWorkItem(new WaitCallback(OrderTimeoutdalRun));
                }

            }
        }
    }

}
