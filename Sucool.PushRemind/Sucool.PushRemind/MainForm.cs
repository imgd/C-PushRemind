using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Sucool.PushRemind.Service;

namespace Sucool.PushRemind
{
    /// <summary>
    /// 推送系统主程序
    /// </summary>
    public partial class MainForm : Form
    {
        #region ServiceInstance
        //说明必须采用单利模式
        private GoldBookRemind_Service _s_gbkremind = null;
        private NotLoginUserRemind_Service _s_notuserloginremind = null;
        private OrderTimeOutRemind_Service _s_ordertimeoutremind = null;
        private JushOrderTimeOutRemind_Service _j_otoremind = null;
        private JushNotLoginUserRemind_Service _j_nologin = null;
        private JushGoldBookRemind_Service _j_gbkremind = null;
        private JushTiming_Service _j_timing = null;

        #endregion

        #region Constract
        public MainForm()
        {
            InitializeComponent();

            //检查全局配置且写入日志
            if (Config.PushLog())
            {
                WorkInit();
                //默认打开启动
                if (IsInitStart())
                {
                    GoldBookStart();
                    OrderTimeOutStart();
                    NotLoginUserStart();
                    JushOrderTimeOutStart();
                    JushNotLoginStart();
                    JushGoldBookStart();
                    JushAddGoldBookStart();
                    timingJushStart();
                }
            }
            else
            {
                this.Height = 0;
                this.Text = "**配置异常，请检查APP.config**";
                MessageBox.Show("全局参数配置异常，请检查", "系统警告");
            }            
        }

        private bool IsInitStart()
        {
            return ConfigHelper.GetAppSettingsString("ISINITSTARTWORK").ToString().Trim() == "ON";
        }
        /// <summary>
        /// 初始化timer
        /// </summary>
        public void WorkInit()
        {
            //优惠卷自动提醒
            this.timer_goldbookRemid.Stop();
            this.timer_goldbookRemid.Interval = Config.GoldBookRemindTimeInteval;
            this.timer_goldbookRemid.Tick += new EventHandler(timer_goldbookRemid_Tick);
            //未登陆用户提醒
            this.timer_notuserloginRemind.Stop();
            this.timer_notuserloginRemind.Interval = Config.NotLoginUserTimeInteval;
            this.timer_notuserloginRemind.Tick += new EventHandler(timer_notuserloginRemind_Tick);

            //订单超时提醒
            this.timer_ordertimeout.Stop();
            this.timer_ordertimeout.Interval = Config.OrderTimeOutRemindInteval;
            this.timer_ordertimeout.Tick += new EventHandler(timer_ordertimeout_Tick);

            //APP订单超时推送提醒
            this.timer_ordertimeoutJush.Stop();
            this.timer_ordertimeoutJush.Interval = Config.JushOrderTimeOutRemindInteval;
            this.timer_ordertimeoutJush.Tick += new EventHandler(timer_ordertimeoutJush_Tick);

            //APP长时间未登录推送提醒
            this.timer_notloginuserJush.Stop();
            this.timer_notloginuserJush.Interval = Config.JushNotLoginUsersInteval;
            this.timer_notloginuserJush.Tick += new EventHandler(timer_notloginuserJush_Tick);

            //APP优惠券到期推送
            this.timer_goldbookRemindJush.Stop();
            this.timer_goldbookRemindJush.Interval = Config.JushGoldBookRemindTimeInteval;
            this.timer_goldbookRemindJush.Tick += new EventHandler(timer_goldbookRemindJush_Tick);

            //APP优惠券新增推送
            this.timer_addgoldbookJush.Stop();
            this.timer_addgoldbookJush.Interval = Config.JushAddGoldBookRemindTimeInteval;
            this.timer_addgoldbookJush.Tick += new EventHandler(timer_addgoldbookJush_Tick);

            //APP定时推送
            this.timer_timingJush.Stop();
            this.timer_timingJush.Interval = Config.JushTimingInteval;
            this.timer_timingJush.Tick += new EventHandler(timer_timingJush_Tick);

            ConfigHelper.SetAppSettingVal("LASTSTARTTIME", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }


        #endregion

        #region Methods

        #region 优惠卷过期提醒

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_gbkremind_stop_Click(object sender, EventArgs e)
        {
            GoldBookEnd();
        }
        /// <summary>
        /// 启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_gbkremind_start_Click(object sender, EventArgs e)
        {
            GoldBookStart();
        }
        private void GoldBookStart()
        {
            this.timer_goldbookRemid.Tag = 1;
            this.btn_gbkremind_start.Visible = false;
            this.btn_gbkremind_stop.Visible = true;
            //开启定时器
            this.timer_goldbookRemid.Start();
        }
        private void GoldBookEnd()
        {
            this.timer_goldbookRemid.Tag = 0;
            this.btn_gbkremind_start.Visible = true;
            this.btn_gbkremind_stop.Visible = false;
            //关闭定时器
            this.timer_goldbookRemid.Stop();
            //回收垃圾
            GC.Collect();
        }

        /// <summary>
        /// timer 执行任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_goldbookRemid_Tick(object sender, EventArgs e)
        {
            if (_s_gbkremind == null)
            {
                _s_gbkremind = new GoldBookRemind_Service();
            }
            _s_gbkremind.Run();
        }

        #endregion

        #region 未登录用户提醒
        private void btn_notloginremind_start_Click(object sender, EventArgs e)
        {
            NotLoginUserStart();
        }

        private void btn_notloginremind_stop_Click(object sender, EventArgs e)
        {
            NotLoginUserEnd();
        }

        void timer_notuserloginRemind_Tick(object sender, EventArgs e)
        {
            if (_s_notuserloginremind == null)
            {
                _s_notuserloginremind = new NotLoginUserRemind_Service();
            }

            _s_notuserloginremind.Run();
        }

        private void NotLoginUserStart()
        {
            this.timer_notuserloginRemind.Tag = 1;
            this.btn_notloginremind_start.Visible = false;
            this.btn_notloginremind_stop.Visible = true;
            //开启定时器
            this.timer_notuserloginRemind.Start();
        }
        private void NotLoginUserEnd()
        {
            this.timer_notuserloginRemind.Tag = 0;
            this.btn_notloginremind_start.Visible = true;
            this.btn_notloginremind_stop.Visible = false;
            //关闭定时器
            this.timer_notuserloginRemind.Stop();
            //回收垃圾
            GC.Collect();
        }
        #endregion

        #region 订单超时提醒
        private void btn_ordertimeremind_start_Click(object sender, EventArgs e)
        {
            OrderTimeOutStart();
        }

        private void btn_ordertimeoutremind_stop_Click(object sender, EventArgs e)
        {
            OrderTimeOutEnd();
        }

        void timer_ordertimeout_Tick(object sender, EventArgs e)
        {
            if (_s_ordertimeoutremind == null)
            {
                _s_ordertimeoutremind = new OrderTimeOutRemind_Service();
            }
            _s_ordertimeoutremind.Run();
        }

        private void OrderTimeOutStart()
        {
            this.timer_ordertimeout.Tag = 1;
            this.btn_ordertimeremind_start.Visible = false;
            this.btn_ordertimeoutremind_stop.Visible = true;
            //开启定时器
            this.timer_ordertimeout.Start();
        }
        private void OrderTimeOutEnd()
        {
            this.timer_ordertimeout.Tag = 0;
            this.btn_ordertimeremind_start.Visible = true;
            this.btn_ordertimeoutremind_stop.Visible = false;
            //关闭定时器
            this.timer_ordertimeout.Stop();
            //回收垃圾
            GC.Collect();
        }
        #endregion

        #endregion

        #region HotKey
        /// <summary>
        /// 窗体激活
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Activated(object sender, EventArgs e)
        {
            //注册热键Shift+S，Id号为100。HotKey.KeyModifiers.Shift也可以直接使用数字4来表示。
            HotKey.RegisterHotKey(Handle, 100, HotKey.KeyModifiers.WindowsKey, Keys.Z);
            //注册热键Ctrl+B，Id号为101。HotKey.KeyModifiers.Ctrl也可以直接使用数字2来表示。
            HotKey.RegisterHotKey(Handle, 101, HotKey.KeyModifiers.WindowsKey, Keys.X);

            HotKey.RegisterHotKey(Handle, 102, HotKey.KeyModifiers.WindowsKey, Keys.A);

            HotKey.RegisterHotKey(Handle, 103, HotKey.KeyModifiers.Ctrl, Keys.F1);

        }

        private void MainForm_Leave(object sender, EventArgs e)
        {
            //注销Id号为100的热键设定
            HotKey.UnregisterHotKey(Handle, 100);
            //注销Id号为101的热键设定
            HotKey.UnregisterHotKey(Handle, 101);

            HotKey.UnregisterHotKey(Handle, 102);

            HotKey.UnregisterHotKey(Handle, 103);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:
                            this.Hide();
                            break;
                        case 101:
                            this.Show();
                            break;
                        case 102:
                            var timestart = ConfigHelper.GetAppSettingsString("LASTSTARTTIME");
                            var timerange = ConfigHelper.GetAppSettingsString("SYSTEMWORKRANGETIME").Split(',');
                            MessageBox.Show("速库消息推送后台\r\n当前版本：V2.0.0.0\r\nV1.0创建时间：2015.8.20 by gd \r\nV2.0更新时间：2015.9.15 by zl 协助修改 by gd\r\n\r\n系统最近一次启动时间：" + timestart + " \r\n系统工作时间范围：每日" + timerange[0] + "点 - " + timerange[1] + "点\r\n快捷键：win+A , win+Z , win+X 启动关闭所有任务：ctrl+F1\r\n\r\n短信发送条数总计：" + ConfigHelper.GetAppSettingsString("SMSSendCount") + " 条------------------------------------------------------------------\r\n如有疑问↓↓\r\nweibo：@大白2013\r\nmail：yanggongde@sucool.com", "提示");
                            break;
                        case 103:
                            if (timer_goldbookRemid.Tag.ToString() == "0")
                            {
                                GoldBookStart();
                                OrderTimeOutStart();
                                NotLoginUserStart();
                                JushOrderTimeOutStart();
                                JushNotLoginStart();
                                JushGoldBookStart();
                                JushAddGoldBookStart();
                                timingJushStart();
                            }
                            else
                            {
                                GoldBookEnd();
                                OrderTimeOutEnd();
                                NotLoginUserEnd();
                                JushOrderTimeOutEnd();
                                JushNotLoginEnd();
                                JushGoldBookEnd();
                                JushAddGoldBookEnd();
                                timingJushEnd();
                            }
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        #endregion



        #region 订单超时APP推送
        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ordertimeremindJush_start_Click(object sender, EventArgs e)
        {
            JushOrderTimeOutStart();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ordertimeoutremindJush_stop_Click(object sender, EventArgs e)
        {
            JushOrderTimeOutEnd();
        }

        private void JushOrderTimeOutStart()
        {
            this.timer_ordertimeoutJush.Tag = 1;
            this.btn_ordertimeremindJush_start.Visible = false;
            this.btn_ordertimeoutremindJush_stop.Visible = true;
            //开启定时器
            this.timer_ordertimeoutJush.Start();
        }
        private void JushOrderTimeOutEnd()
        {
            this.timer_ordertimeoutJush.Tag = 0;
            this.btn_ordertimeremindJush_start.Visible = true;
            this.btn_ordertimeoutremindJush_stop.Visible = false;
            //关闭定时器
            this.timer_ordertimeoutJush.Stop();
            //回收垃圾
            GC.Collect();
        }

        void timer_ordertimeoutJush_Tick(object sender, EventArgs e)
        {
            if (_j_otoremind == null)
            {
                _j_otoremind = new JushOrderTimeOutRemind_Service();
            }
            _j_otoremind.Run();
        }
        #endregion

        #region 长时间未登录APP推送
        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_NotLoginUserRemindJush_Start_Click(object sender, EventArgs e)
        {
            JushNotLoginStart();
        }
        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_notloginuserremind_End_Click(object sender, EventArgs e)
        {
            JushNotLoginEnd();
        }

        private void JushNotLoginStart()
        {
            this.timer_notloginuserJush.Tag = 1;
            this.btn_NotLoginUserRemindJush_Start.Visible = false;
            this.btn_notloginuserremind_End.Visible = true;
            //开启定时器
            this.timer_notloginuserJush.Start();
        }
        private void JushNotLoginEnd()
        {
            this.timer_ordertimeoutJush.Tag = 0;
            this.btn_NotLoginUserRemindJush_Start.Visible = true;
            this.btn_notloginuserremind_End.Visible = false;
            //关闭定时器
            this.timer_notloginuserJush.Stop();
            //回收垃圾
            GC.Collect();
        }

        void timer_notloginuserJush_Tick(object sender, EventArgs e)
        {
            if (_j_nologin == null)
            {
                _j_nologin = new JushNotLoginUserRemind_Service();
            }
            _j_nologin.Run();
        }
        #endregion

        #region 优惠券过期APP推送
        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_gbkremindJush_start_Click(object sender, EventArgs e)
        {
            JushGoldBookStart();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_gbkremindJush_stop_Click(object sender, EventArgs e)
        {
            JushGoldBookEnd();
        }

        private void JushGoldBookStart()
        {
            this.timer_goldbookRemindJush.Tag = 1;
            this.btn_gbkremindJush_start.Visible = false;
            this.btn_gbkremindJush_stop.Visible = true;
            //开启定时器
            this.timer_goldbookRemindJush.Start();
        }
        private void JushGoldBookEnd()
        {
            this.timer_goldbookRemindJush.Tag = 0;
            this.btn_gbkremindJush_start.Visible = true;
            this.btn_gbkremindJush_stop.Visible = false;
            //关闭定时器
            this.timer_goldbookRemindJush.Stop();
            //回收垃圾
            GC.Collect();
        }

        void timer_goldbookRemindJush_Tick(object sender, EventArgs e)
        {
            if (_j_gbkremind == null)
            {
                _j_gbkremind = new JushGoldBookRemind_Service();
            }
            _j_gbkremind.Run();
        }
        #endregion

        #region 优惠券新增APP推送
        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_addgbkremindJush_start_Click(object sender, EventArgs e)
        {
            JushAddGoldBookStart();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_addgbkremindJush_stop_Click(object sender, EventArgs e)
        {
            JushAddGoldBookEnd();
        }

        private void JushAddGoldBookStart()
        {
            this.timer_addgoldbookJush.Tag = 1;
            this.btn_addgbkremindJush_start.Visible = false;
            this.btn_addgbkremindJush_stop.Visible = true;
            //开启定时器
            this.timer_addgoldbookJush.Start();
        }
        private void JushAddGoldBookEnd()
        {
            this.timer_addgoldbookJush.Tag = 0;
            this.btn_addgbkremindJush_start.Visible = true;
            this.btn_addgbkremindJush_stop.Visible = false;
            //关闭定时器
            this.timer_addgoldbookJush.Stop();
            //回收垃圾
            GC.Collect();
        }

        void timer_addgoldbookJush_Tick(object sender, EventArgs e)
        {
            if (_j_gbkremind == null)
            {
                _j_gbkremind = new JushGoldBookRemind_Service();
            }
            _j_gbkremind.AddRun();
        }
        #endregion

        #region 定时推送
        /// <summary>
        /// 开启
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_timingJush_start_Click(object sender, EventArgs e)
        {
            timingJushStart();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_timingJush_stop_Click(object sender, EventArgs e)
        {
            timingJushEnd();
        }

        private void timingJushStart()
        {
            this.timer_timingJush.Tag = 1;
            this.btn_timingJush_start.Visible = false;
            this.btn_timingJush_stop.Visible = true;
            //开启定时器
            this.timer_timingJush.Start();
        }
        private void timingJushEnd()
        {
            this.timer_timingJush.Tag = 0;
            this.btn_timingJush_start.Visible = true;
            this.btn_timingJush_stop.Visible = false;
            //关闭定时器
            this.timer_timingJush.Stop();
            //回收垃圾
            GC.Collect();
        }

        void timer_timingJush_Tick(object sender, EventArgs e)
        {
            if (_j_timing == null)
            {
                _j_timing = new JushTiming_Service();
            }
            _j_timing.Run();
        }
        #endregion

       
    }
}
