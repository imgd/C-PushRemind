using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualBasic.ApplicationServices;

namespace Sucool.PushRemind
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
            SingleInstanceManager manager = new SingleInstanceManager();//单实例管理器  
            manager.Run(new string[] { });
        }
        
        public class SingleInstanceManager : WindowsFormsApplicationBase
        {
            MainForm app;

            public SingleInstanceManager()
            {
                //单实例设置,触发NEXTINSTENCE事件  
                this.IsSingleInstance = true;

                //由于没有用主窗体，设置多窗口SDI应用程序,只有当所有窗体关闭后才关闭程序  
                this.ShutdownStyle = ShutdownMode.AfterAllFormsClose;
            }

            protected override bool OnStartup(Microsoft.VisualBasic.ApplicationServices.StartupEventArgs eventArgs)
            {
                //在程序第一次运行时调用  
                app = new MainForm();
                Application.Run(app);
                //运行后返回FLASE  
                return false;
            }

            protected override void OnStartupNextInstance(StartupNextInstanceEventArgs eventArgs)
            {
                base.OnStartupNextInstance(eventArgs);
                app.Activate();
                //提示已经运行  
                MessageBox.Show("推送系统已经在运行！", "确定", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        } 
    }
}
