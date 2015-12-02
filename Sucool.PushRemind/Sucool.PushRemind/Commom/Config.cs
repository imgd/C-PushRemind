using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sucool.PushRemind
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public static class Config
    {
        /// <summary>
        /// 推送系统工作是时间范围
        /// </summary>
        /// <returns></returns>
        public static bool IsALLWork()
        {
            var timerange = ConfigHelper.GetAppSettingsString("SYSTEMWORKRANGETIME").Split(',');
            int hour = DateTime.Now.ToString("HH").ParseInt();
            DateTime dt = DateTime.Now;
            DateTime start = Convert.ToDateTime(dt.ToString("yyyy-MM-dd") +
                " " + timerange[0].ParseInt(8) + ":00:00");
            DateTime end = Convert.ToDateTime(dt.ToString("yyyy-MM-dd") +
                " " + timerange[1].ParseInt(22) + ":00:00");

            return (dt >= start && dt <= end);
        }

        /// <summary>
        /// 短信推送订单日志
        /// </summary>
        /// <param name="mobiles"></param>
        /// <param name="contents"></param>
        public static void PushLog(string type, string mobiles, string contents)
        {

            string.Format("\r\n任务类型：{3},推送手机：{0},时间：{1},短信内容：{2}",
                        mobiles, DateTime.Now.ToString("HH:mm:ss"),
                        contents, type).WriteLog();
        }

        /// <summary>
        /// APP推送订单日志
        /// </summary>
        /// <param name="mobiles"></param>
        /// <param name="contents"></param>
        public static void PushLogAPP(string type, string Users, string contents)
        {

            string.Format("\r\n任务类型：{3},推送用户：{0},时间：{1},推送内容：{2}",
                        Users, DateTime.Now.ToString("HH:mm:ss"),
                        contents, type).WriteLog(true);
        }

        public static bool PushLog()
        {

            var setting1 = ConfigHelper.GetAppSettingsString("SYSTEMWORKRANGETIME").ToString();
            var setting2 = ConfigHelper.GetAppSettingsString("MsgSendMaxCount").ToString();
            var setting3 = ConfigHelper.GetAppSettingsString("GoldBookRemindTimeHour").ToString();
            var setting4 = ConfigHelper.GetAppSettingsString("GoldBookRemindTimeInteval").ToString();
            var setting5 = ConfigHelper.GetAppSettingsString("GoldBookRemindDayNum").ToString();
            var setting6 = ConfigHelper.GetAppSettingsString("GoldBookRemindRemingMsg").ToString();
            var setting7 = ConfigHelper.GetAppSettingsString("NotLoginUserTimeHour").ToString();
            var setting8 = ConfigHelper.GetAppSettingsString("NotLoginUserTimeInteval").ToString();
            var setting9 = ConfigHelper.GetAppSettingsString("NotLoginUserRemingMsg").ToString();
            var setting10 = ConfigHelper.GetAppSettingsString("OrderTimeOutRemindHourNum").ToString();
            var setting11 = ConfigHelper.GetAppSettingsString("OrderTimeOutRemindInteval").ToString();
            var setting12 = ConfigHelper.GetAppSettingsString("OrderTimeOutRemindMsg").ToString();
            var setting13 = ConfigHelper.GetConfigConnnString("DBCONNECTION").ToString();
            var setting14 = ConfigHelper.GetAppSettingsString("ISINITSTARTWORK").ToString();
            var setting15 = ConfigHelper.GetAppSettingsString("LOGBASEPATH").ToString();
            //APP推送
            var jushsetting1 = ConfigHelper.GetAppSettingsString("PushApp_key").ToString();
            var jushsetting2 = ConfigHelper.GetAppSettingsString("PushMaster_secret").ToString();

            var jushsetting3 = ConfigHelper.GetAppSettingsString("JushGoldBookRemindTimeHour1").ToString();
            var jushsetting4 = ConfigHelper.GetAppSettingsString("JushGoldBookRemindTimeHour2").ToString();
            var jushsetting5 = ConfigHelper.GetAppSettingsString("JushGoldBookRemindTimeInteval").ToString();
            var jushsetting6 = ConfigHelper.GetAppSettingsString("JushGoldBookRemindRemingMsg").ToString();
            var jushsetting7 = ConfigHelper.GetAppSettingsString("JushAddGoldBookRemindTimeHour1").ToString();
            var jushsetting8 = ConfigHelper.GetAppSettingsString("JushAddGoldBookRemindTimeHour2").ToString();
            var jushsetting9 = ConfigHelper.GetAppSettingsString("JushAddGoldBookRemindTimeInteval").ToString();
            var jushsetting10 = ConfigHelper.GetAppSettingsString("JushAddGoldBookRemindRemingMsg").ToString();
            var jushsetting11 = ConfigHelper.GetAppSettingsString("JushOrderTimeRemindHourHour1").ToString();
            var jushsetting12 = ConfigHelper.GetAppSettingsString("JushOrderTimeRemindHourHour2").ToString();
            var jushsetting13 = ConfigHelper.GetAppSettingsString("JushOrderTimeOutRemindInteval").ToString();
            var jushsetting14 = ConfigHelper.GetAppSettingsString("JushOrderTimeOutRemindMsg").ToString();
            var jushsetting15 = ConfigHelper.GetAppSettingsString("JushNotLoginUsersHour").ToString();
            var jushsetting16 = ConfigHelper.GetAppSettingsString("JushNotLoginUsersDay").ToString();
            var jushsetting17 = ConfigHelper.GetAppSettingsString("JushNotLoginUsersInteval").ToString();
            var jushsetting18 = ConfigHelper.GetAppSettingsString("JushNotLoginUsersRemingMsg").ToString();
            var jushsetting19 = ConfigHelper.GetAppSettingsString("JushTimingInteval").ToString();

            if (string.IsNullOrWhiteSpace(setting1) ||
                string.IsNullOrWhiteSpace(setting2) ||
                string.IsNullOrWhiteSpace(setting3) ||
                string.IsNullOrWhiteSpace(setting4) ||
                string.IsNullOrWhiteSpace(setting5) ||
                string.IsNullOrWhiteSpace(setting6) ||
                string.IsNullOrWhiteSpace(setting7) ||
                string.IsNullOrWhiteSpace(setting8) ||
                string.IsNullOrWhiteSpace(setting9) ||
                string.IsNullOrWhiteSpace(setting10) ||
                string.IsNullOrWhiteSpace(setting11) ||
                string.IsNullOrWhiteSpace(setting12) ||
                string.IsNullOrWhiteSpace(setting13) ||
                string.IsNullOrWhiteSpace(setting14) ||
                string.IsNullOrWhiteSpace(setting15)||
                string.IsNullOrWhiteSpace(jushsetting1)||
                string.IsNullOrWhiteSpace(jushsetting2)||
                string.IsNullOrWhiteSpace(jushsetting3)||
                string.IsNullOrWhiteSpace(jushsetting4)||
                string.IsNullOrWhiteSpace(jushsetting5)||
                string.IsNullOrWhiteSpace(jushsetting6)||
                string.IsNullOrWhiteSpace(jushsetting7)||
                string.IsNullOrWhiteSpace(jushsetting8)||
                string.IsNullOrWhiteSpace(jushsetting9)||
                string.IsNullOrWhiteSpace(jushsetting10)||
                string.IsNullOrWhiteSpace(jushsetting11)||
                string.IsNullOrWhiteSpace(jushsetting12)||
                string.IsNullOrWhiteSpace(jushsetting13)||
                string.IsNullOrWhiteSpace(jushsetting14)||
                string.IsNullOrWhiteSpace(jushsetting15)||
                string.IsNullOrWhiteSpace(jushsetting16)||
                string.IsNullOrWhiteSpace(jushsetting17)||
                string.IsNullOrWhiteSpace(jushsetting18)||
                string.IsNullOrWhiteSpace(jushsetting19)
                )
            {
                string.Format(">>警告:系统配置异常(详细见日志文件),time:{0}",
                DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).WriteLog("ERROR");
                return false;
            }
            else
            {
                //记录SMS配置系统日志
                string.Format("==============================================================SMS系统配置================================================================\r\n" +
                    "系统启动成功，启动时间{0},当前系统配置：系统工作时间 单位/h：{1},群发手机最大数量:{2}" +
                 "\r\n优惠卷超时：工作时间:{3},工作频率：{4} ms/次,提前提醒时间{5}天,提醒内容：{6}" +
                 "\r\n未登陆用户：工作时间:{7},工作频率：{8} ms/次,提醒内容：{9}" +
                 "\r\n订单超时：工作时间:{10},工作频率：{11} ms/次,提醒内容：{12}" +
                "\r\n==============================================================SMS系统配置================================================================",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    setting1,
                    setting2,
                    setting3,
                    setting4,
                    setting5,
                    setting6,
                    setting7,
                    setting8,
                    setting9,
                    setting10,
                    setting11,
                    setting12
                    ).WriteLog();

                //记录APP配置系统日志
                string.Format("==============================================================APP推送系统配置================================================================\r\n"+
                    "系统启动成功，启动时间{0}"+
                    "\r\nAPP推送优惠券超时：工作时间1:{1},工作时间2:{2},工作频率:{3},提醒内容:{4}"+
                    "\r\nAPP推送优惠券新增：工作时间1:{5},工作时间2:{6},工作频率:{7},提醒内容:{8}"+
                    "\r\nAPP推送订单超时：工作时间1:{9},工作时间2:{10},工作频率:{11},提醒内容:{12}"+
                    "\r\nAPP推送登录提醒：工作时间1:{13},工作时间2:{14},工作频率:{15},提醒内容:{16}"+
                    "\r\nAPP定时推送：工作频率:{17}"+
                    "\r\n==============================================================APP推送系统配置================================================================",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    jushsetting3,
                    jushsetting4,
                    jushsetting5,
                    jushsetting6,
                    jushsetting7,
                    jushsetting8,
                    jushsetting9,
                    jushsetting10,
                    jushsetting11,
                    jushsetting12,
                    jushsetting13,
                    jushsetting14,
                    jushsetting15,
                    jushsetting16,
                    jushsetting17,
                    jushsetting18,
                    jushsetting19
                    ).WriteLog(true);
                return true;
            }
        }

        /*
         *短信内容需要配置到config里面待加
         */

        #region global 全局
        /// <summary>
        /// 单条短信群发 最多支持的手机号 数量
        /// 多个手机号 ','  号隔开
        /// 136XXX,136XXX,136XXX
        /// </summary>
        public static int MsgSendMaxCount = ConfigHelper.GetAppSettingsString("MsgSendMaxCount").ParseInt(20);


        #endregion

        #region 优惠卷提醒
        /// <summary>
        /// 工作时间 执行时间
        /// </summary>
        public static string GoldBookRemindTimeHour = ConfigHelper.GetAppSettingsString("GoldBookRemindTimeHour");
        /// <summary>
        /// 任务执行频率 一个小时
        /// </summary>
        public static int GoldBookRemindTimeInteval = ConfigHelper.GetAppSettingsString("GoldBookRemindTimeInteval").ParseInt(3600000);
        /// <summary>
        /// 优惠卷过期提醒 提前推送天数 单位day
        /// </summary>
        public static int GoldBookRemindDayNum = ConfigHelper.GetAppSettingsString("GoldBookRemindDayNum").ParseInt(2);
        /// <summary>
        /// 优惠卷过期提醒 提醒消息内容
        /// </summary>
        public static string GoldBookRemindRemingMsg = ConfigHelper.GetAppSettingsString("GoldBookRemindRemingMsg");

        #endregion

        #region 长期未登陆用户
        /// <summary>
        /// 工作时间 执行时间
        /// </summary>
        public static string NotLoginUserTimeHour = ConfigHelper.GetAppSettingsString("NotLoginUserTimeHour");
        /// <summary>
        /// 任务执行频率 一个小时
        /// </summary>
        public static int NotLoginUserTimeInteval = ConfigHelper.GetAppSettingsString("NotLoginUserTimeInteval").ParseInt(3600000);
        /// <summary>
        /// 优惠卷过期提醒 提醒消息内容
        /// </summary>
        public static string NotLoginUserRemingMsg = ConfigHelper.GetAppSettingsString("NotLoginUserRemingMsg");

        #endregion

        #region 订单支付超时提醒

        /// <summary>
        /// 订单超时提前发送 小时
        /// </summary>
        public static int OrderTimeOutRemindHourNum = ConfigHelper.GetAppSettingsString("OrderTimeOutRemindHourNum").ParseInt(2);

        /// <summary>
        /// 任务执行频率 一个小时
        /// </summary>
        public static int OrderTimeOutRemindInteval = ConfigHelper.GetAppSettingsString("OrderTimeOutRemindInteval").ParseInt(3600000);
        /// <summary>
        /// 优惠卷过期提醒 提醒消息内容
        /// </summary>
        public static string OrderTimeOutRemindMsg = ConfigHelper.GetAppSettingsString("OrderTimeOutRemindMsg");
        #endregion

        #region 极光账号

        public static string PushApp_key = ConfigHelper.GetAppSettingsString("PushApp_key");

        public static string PushMaster_secret = ConfigHelper.GetAppSettingsString("PushMaster_secret"); 

        #endregion

        #region APP订单超时推送
        /// <summary>
        /// 订单超时推送时间1
        /// </summary>
        public static int JushOrderTimeOutRemindHourHour1 = ConfigHelper.GetAppSettingsString("JushOrderTimeRemindHourHour1").ParseInt(12);

        /// <summary>
        /// 订单超时推送时间1
        /// </summary>
        public static int JushOrderTimeOutRemindHourHour2 = ConfigHelper.GetAppSettingsString("JushOrderTimeRemindHourHour2").ParseInt(21);

        /// <summary>
        /// 执行频率
        /// </summary>
        public static int JushOrderTimeOutRemindInteval = ConfigHelper.GetAppSettingsString("JushOrderTimeOutRemindInteval").ParseInt(3600000);

        /// <summary>
        /// 推送信息
        /// </summary>
        public static string JushOrderTimeOutRemindMsg = ConfigHelper.GetAppSettingsString("JushOrderTimeOutRemindMsg"); 
        #endregion

        #region APP长时间未登录推送
        /// <summary>
        /// 长时间未登录APP推送时间
        /// </summary>
        public static int JushNotLoginUsersHour = ConfigHelper.GetAppSettingsString("JushNotLoginUsersHour").ParseInt(20);

        /// <summary>
        /// 长时间未登录APP推送间隔
        /// </summary>
        public static int JushNotLoginUsersDay = ConfigHelper.GetAppSettingsString("JushNotLoginUsersDay").ParseInt(7);

        /// <summary>
        /// 长时间未登录APP推送频率
        /// </summary>
        public static int JushNotLoginUsersInteval = ConfigHelper.GetAppSettingsString("JushNotLoginUsersInteval").ParseInt(3600000);

        /// <summary>
        /// 长时间未登录APP推送信息
        /// </summary>
        public static string JushNotLoginUsersRemingMsg = ConfigHelper.GetAppSettingsString("JushNotLoginUsersRemingMsg");
	#endregion

        #region APP推送优惠券到期提醒
        /// <summary>
        /// APP推送优惠券到期时间1
        /// </summary>
        public static int JushGoldBookRemindTimeHour1 = ConfigHelper.GetAppSettingsString("JushGoldBookRemindTimeHour1").ParseInt(9);

        /// <summary>
        /// APP推送优惠券到期时间2
        /// </summary>
        public static int JushGoldBookRemindTimeHour2 = ConfigHelper.GetAppSettingsString("JushGoldBookRemindTimeHour2").ParseInt(19);

        /// <summary>
        /// APP推送优惠券到期频率
        /// </summary>
        public static int JushGoldBookRemindTimeInteval = ConfigHelper.GetAppSettingsString("JushGoldBookRemindTimeInteval").ParseInt(3600000);

        /// <summary>
        /// APP推送优惠券到期信息
        /// </summary>
        public static string JushGoldBookRemindRemingMsg = ConfigHelper.GetAppSettingsString("JushGoldBookRemindRemingMsg"); 
        #endregion

        #region APP新增优惠券提醒
        /// <summary>
        /// APP推送优惠券新增时间1
        /// </summary>
        public static int JushAddGoldBookRemindTimeHour1 = ConfigHelper.GetAppSettingsString("JushAddGoldBookRemindTimeHour1").ParseInt(8);

        /// <summary>
        /// APP推送优惠券新增时间2
        /// </summary>
        public static int JushAddGoldBookRemindTimeHour2 = ConfigHelper.GetAppSettingsString("JushAddGoldBookRemindTimeHour2").ParseInt(18);

        /// <summary>
        /// APP推送优惠券新增频率
        /// </summary>
        public static int JushAddGoldBookRemindTimeInteval = ConfigHelper.GetAppSettingsString("JushAddGoldBookRemindTimeInteval").ParseInt(3600000);

        /// <summary>
        /// APP推送优惠券新增信息
        /// </summary>
        public static string JushAddGoldBookRemindRemingMsg = ConfigHelper.GetAppSettingsString("JushAddGoldBookRemindRemingMsg");  
        #endregion

        #region APP定时推送
        /// <summary>
        /// APP定时推送频率
        /// </summary>
        public static int JushTimingInteval = ConfigHelper.GetAppSettingsString("JushTimingInteval").ParseInt(1800000); 
        #endregion

        #region APP推送跳转
        /// <summary>
        /// 跳转优惠券页面
        /// </summary>
        public static int JushGoPageGoldBook = ConfigHelper.GetAppSettingsString("JushGoPageGoldBook").ParseInt(1);

        /// <summary>
        /// 跳转订单页面
        /// </summary>
        public static int JushGoPageOrder = ConfigHelper.GetAppSettingsString("JushGoPageOrder").ParseInt(2);

        /// <summary>
        /// 跳转主页
        /// </summary>
        public static int JushGoPageIndex = ConfigHelper.GetAppSettingsString("JushGoPageIndex").ParseInt(4);

        /// <summary>
        /// 跳转转盘
        /// </summary>
        public static int JushGoPageLogin = ConfigHelper.GetAppSettingsString("JushGoPageLogin").ParseInt(3);  
        #endregion

    }
}
