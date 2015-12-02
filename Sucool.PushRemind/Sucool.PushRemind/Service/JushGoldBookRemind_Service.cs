using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Jpush.api;
using Jpush.api.push.mode;
using Sucool.PushRemind.DAL.Service;
using Sucool.PushRemind.Model;
using Jpush.api.push.notification;

namespace Sucool.PushRemind.Service
{
    public class JushGoldBookRemind_Service
    {
        private JushGoldBookRemind_DAL _glb = null;
        private JPushClient client = null;
        private PushPayload payload = null;
        private List<Push_GoldBook_Model> list = null;
        public JushGoldBookRemind_Service()
        {
            client = new JPushClient(Config.PushApp_key, Config.PushMaster_secret);
        }

        private void OrderTimeoutdalRun(object o)
        {
            if (_glb == null)
            {
                _glb = new JushGoldBookRemind_DAL();
            }
            if (payload == null)
            {
                payload = new PushPayload();
            }
            list = _glb.GoldOutList();
            if (list.Count > 0)
            {
                List<string> list_users = new List<string>();
                list.ForEach(z => list_users.Add(z.UsedId.ToString().DESEncrypt().StringToMd5().Md532SubString()));
                //PushPayload payload = new PushPayload();
                payload.platform = Platform.all();
                payload.audience = Audience.s_alias(list_users.ToArray());
                payload.notification = new Notification().setAlert(Config.JushGoldBookRemindRemingMsg).setAndroid(new AndroidNotification().AddExtra("key", Config.JushGoPageGoldBook)).setIos(new IosNotification().AddExtra("key", Config.JushGoPageGoldBook));
                try
                {
                    client.SendPush(payload);
                    //记录日志
                    Config.PushLogAPP("APP优惠卷超时提醒", list_users.ListToStrings(), Config.JushGoldBookRemindRemingMsg);
                }
                catch (Jpush.api.common.APIRequestException ee)
                {
                    string.Format("\r\n>>>APP优惠卷超时提醒 推送异常：{0},time:{1}",
                    ee.Message,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    ).WriteLog("ERROR");
                }
            }
        }

        private void AddOrderTimeoutdalRun(object o)
        {
            if (_glb == null)
            {
                _glb = new JushGoldBookRemind_DAL();
            }
            if (payload == null)
            {
                payload = new PushPayload();
            }
            list = _glb.GoldAddList();
            if (list.Count > 0)
            {
                List<string> list_users = new List<string>();
                list.ForEach(z => list_users.Add(z.UsedId.ToString().DESEncrypt().StringToMd5().Md532SubString()));
                //PushPayload payload = new PushPayload();
                payload.platform = Platform.all();
                payload.audience = Audience.s_alias(list_users.ToArray());
                payload.notification = new Notification().setAlert(Config.JushAddGoldBookRemindRemingMsg).setAndroid(new AndroidNotification().AddExtra("key", Config.JushGoPageGoldBook)).setIos(new IosNotification().AddExtra("key", Config.JushGoPageGoldBook));
                try
                {
                    client.SendPush(payload);
                    //记录日志
                    Config.PushLogAPP("APP获取到优惠卷提醒", list_users.ListToStrings(), Config.JushAddGoldBookRemindRemingMsg);
                }
                catch (Jpush.api.common.APIRequestException ee)
                {
                    string.Format("\r\n>>>APP获取到优惠卷提醒 推送异常：{0},time:{1}",
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
                int time1 = Config.JushGoldBookRemindTimeHour1.ParseInt(9);
                int time2 = Config.JushGoldBookRemindTimeHour2.ParseInt(19);
                if (hour == time1 || hour == time2)
                {
                    //异步 把执行任务加入线程池
                    ThreadPool.QueueUserWorkItem(new WaitCallback(OrderTimeoutdalRun));
                }

            }
        }

        public void AddRun()
        {
            if (Config.IsALLWork())
            {
                int hour = DateTime.Now.ToString("HH").ParseInt();
                int time1 = Config.JushAddGoldBookRemindTimeHour1.ParseInt(8);
                int time2 = Config.JushAddGoldBookRemindTimeHour2.ParseInt(18);
                if (hour == time1 || hour == time2)
                {
                    //异步 把执行任务加入线程池
                    ThreadPool.QueueUserWorkItem(new WaitCallback(AddOrderTimeoutdalRun));
                }

            }
        }
    }
}
