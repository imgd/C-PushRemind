using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sucool.PushRemind.DAL.Service;
using Sucool.PushRemind.Model;
using Jpush.api;
using Jpush.api.push.mode;
using Jpush.api.push.notification;

namespace Sucool.PushRemind.Service
{
    public class JushNotLoginUserRemind_Service
    {
        private JushNotLoginUserRemind_DAL _notlogin = null;
        private List<Push_NotLoginUser_Model> list = null;
        private JPushClient client = null;
        private PushPayload payload = null;
        public JushNotLoginUserRemind_Service() 
        {
            client = new JPushClient(Config.PushApp_key, Config.PushMaster_secret);
        }

        private void OrderTimeoutdalRun(object o)
        {
            if (_notlogin == null)
            {
                _notlogin = new JushNotLoginUserRemind_DAL();
            }
            if (payload == null) 
            {
                payload = new PushPayload();
            }
            list = _notlogin.NotLoginUserList();
            if (list.Count > 0)
            {
                List<string> list_users = new List<string>();
                list.ForEach(z => list_users.Add(z.Id.ToString().DESEncrypt().StringToMd5().Md532SubString()));
                //PushPayload payload = new PushPayload();
                payload.platform = Platform.all();
                payload.audience = Audience.s_alias(list_users.ToArray());
                payload.notification = new Notification().setAlert(Config.JushNotLoginUsersRemingMsg).setAndroid(new AndroidNotification().AddExtra("key", Config.JushGoPageLogin)).setIos(new IosNotification().AddExtra("key", Config.JushGoPageLogin));
                try
                {
                    client.SendPush(payload);
                    //记录日志
                    Config.PushLogAPP("APP长时间未登录推送提醒", list_users.ListToStrings(), Config.JushNotLoginUsersRemingMsg);
                }
                catch (Jpush.api.common.APIRequestException ee)
                {
                    string.Format("\r\n>>>APP长时间未登录推送 推送异常：{0},time:{1}",
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
                if (hour==time1||hour==time2)
                {
                    //异步 把执行任务加入线程池
                    ThreadPool.QueueUserWorkItem(new WaitCallback(OrderTimeoutdalRun));
                }

            }
        }
    }
}
