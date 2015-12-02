using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Sucool.PushRemind.Model;
using Jpush.api;
using Jpush.api.push.mode;
using Sucool.PushRemind.DAL.Service;
using Jpush.api.push.notification;

namespace Sucool.PushRemind.Service
{
    public class JushTiming_Service
    {
        private JushTiming_DAL _j_time = null;
        private List<Push_Model> list = null;
        private JPushClient client = null;
        private PushPayload payload = null;
        private AndroidNotification android = null;
        private IosNotification ios = null;
        private Dictionary<string, object> data = null;

        public JushTiming_Service()
        {
            client = new JPushClient(Config.PushApp_key, Config.PushMaster_secret);
        }

        private void TimingJush(object o) 
       {
            if (_j_time == null) 
            {
                _j_time = new JushTiming_DAL();
            }
            if (payload == null) 
            {
                payload = new PushPayload();
            }
            
            list = _j_time.PushTiming();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {

                    long MessageID = 0;
                    int ErrorCode = 0;
                    switch (item.SendPlatform)
                    {
                        case 1: payload.platform = Platform.android();
                            break;
                        case 2: payload.platform = Platform.ios();
                            break;
                        case 3: payload.platform = Platform.android_ios();
                            break;
                        default:
                            break;
                    }
                    switch (item.SendAudience)
                    {
                        case 0: payload.audience = Audience.all();
                            break;
                        case 1:
                            string[] tagArray = item.AudienceName.Split(',');
                            payload.audience = Audience.s_tag(tagArray);
                            break;
                        case 2: payload.audience = Audience.s_alias(item.AudienceName);
                            break;
                        case 3: payload.audience = Audience.s_registrationId(item.AudienceName);
                            break;
                        default:
                            break;
                    }
                    string strJosn = item.Extras == null ? "" : item.Extras;
                    if (strJosn.Length > 0)
                    {
                        data = strJosn.JsonToDocuments();
                        if (data.Count > 0)
                        {
                            android = new AndroidNotification();
                            ios = new IosNotification();
                            foreach (var tt in data)
                            {
                                android.AddExtra(tt.Key, tt.Value.ToString());
                                ios.AddExtra(tt.Key, tt.Value.ToString());
                            }
                        }
                        payload.notification = new Notification().setAlert(item.Content).setAndroid(android).setIos(ios);
                    }
                    else 
                    {
                        payload.notification = new Notification().setAlert(item.Content);
                    }
                    
                    try
                    {
                        var result = client.SendPush(payload);
                        //由于统计数据并非非是即时的,所以等待一小段时间再执行下面的获取结果方法
                        System.Threading.Thread.Sleep(5000);
                        MessageID = Convert.ToInt64(result.msg_id);
                        //记录日志
                        Config.PushLogAPP("APP定时发送", "所有", item.Content);
                    }
                    catch (Jpush.api.common.APIRequestException ee)
                    {
                        MessageID = ee.MsgId;
                        ErrorCode = ee.ErrorCode;
                        

                        string.Format("\r\n>>>APP定时发送 推送异常：{0},time:{1}",
                        ee.Message,
                        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        ).WriteLog("ERROR");
                    }

                    _j_time.PushTimingUpdateStatu(MessageID, ErrorCode, item.Id);
                    android = null;
                    ios = null;
                }
            }

        }

        public void Run()
        {
            if (Config.IsALLWork())
            {
                //异步 把执行任务加入线程池
                ThreadPool.QueueUserWorkItem(new WaitCallback(TimingJush));
            }
        }
    }
}
