using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Reflection;
using System.Xml;

namespace Sucool.PushRemind
{
    public static class ConfigHelper
    {
        #region Base

        /*ConnectionStrings节点*/
        public static string GetConfigConnnString(string configName)
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[configName].ToString();
            }
            catch (Exception e)
            {
                string.Format(">>ConnectionStrings节点Name:[{0}]读取异常：{1},time:{2}",
                    configName, e.Message,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).WriteLog("ERROR");

                return string.Empty;
            }

        }

        /*AppSettings节点*/
        public static string GetAppSettingsString(string configName)
        {
            try
            {
                return ConfigurationManager.AppSettings[configName].ToString();
            }
            catch (Exception e)
            {
                string.Format(">>AppSettings节点Name:[{0}]读取异常：{1},time:{2}",
                    configName, e.Message,
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")).WriteLog("ERROR");

                return string.Empty; ;
            }

        }

        public static void SetAppSettingVal(string AppKey, string AppValue)
        {
            XmlDocument xDoc = new XmlDocument();

            xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");

            XmlNode xNode;

            XmlElement xElem1;//XmlElement xElem2;

            xNode = xDoc.SelectSingleNode("//appSettings");

            xElem1 = (XmlElement)xNode.SelectSingleNode("//add[@key='" + AppKey + "']");

            if (xElem1 != null)
            {
                xElem1.SetAttribute("value", AppValue);
            }
            else
            {
                var xElem2 = xDoc.CreateElement("add");
                xElem2.SetAttribute("name", AppKey);
                xElem2.SetAttribute("connectionString", AppValue);
                xNode.AppendChild(xElem2);
            }
            xDoc.Save(System.Windows.Forms.Application.ExecutablePath + ".config");
        }



        /*自定义节点*/
        public static object GetOtherNodeString(string nodeName)
        {
            return ConfigurationManager.GetSection(nodeName);
        }
        #endregion
    }
}
