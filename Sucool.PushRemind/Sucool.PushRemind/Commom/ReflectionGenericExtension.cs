using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.IO;

namespace Sucool.PushRemind
{
    //---------------------------------------------------------------
    //   反射泛型数据扩展方法类                                                   
    // —————————————————————————————————————————————————             
    // | varsion 1.0                                   |             
    // | creat by gd 2014.7.31                         |             
    // | 联系我:@大白2013 http://weibo.com/u/2239977692 |            
    // —————————————————————————————————————————————————             
    //                                                               
    // *使用说明：                                                    
    //    使用当前扩展类添加引用: using Extensions.ReflectionGenericExtension;                      
    //    使用所有扩展类添加引用: using Extensions;                         
    // -------------------------------------------------------------- 

    public static class ReflectionGenericExtension
    {
        #region 反射泛型数据

        /// <summary>  
        /// 利用反射和泛型  
        /// DataRow 数据集
        /// </summary>  
        /// <param name="dr">行元素</param>  
        /// <returns></returns>  
        public static T ConvertToModel<T>(this DataRow dr) where T : new()
        {

            // 获得此模型的类型  
            Type type = typeof(T);
            //定义一个临时变量  
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行  
            T t = new T();
            // 获得此模型的公共属性  
            PropertyInfo[] propertys = t.GetType().GetProperties();
            //遍历该对象的所有属性  
            foreach (PropertyInfo pi in propertys)
            {
                tempName = pi.Name;//将属性名称赋值给临时变量  
                //检查DataTable是否包含此列（列名==对象的属性名）    
                if (dr.Table.Columns.Contains(tempName))
                {
                    // 判断此属性是否有Setter  
                    if (!pi.CanWrite) continue;//该属性不可写，直接跳出  
                    //取值  
                    object value = dr[tempName];
                    //如果非空，则赋给对象的属性  
                    if (value != DBNull.Value)
                        pi.SetValue(t, value, null);
                }
            }
            //对象添加到泛型集合中
            return t;

        }
        /// <summary>
        /// 利用反射和泛型 
        /// 序列化SqlDataReader数据集
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T ConvertToModel<T>(this SqlDataReader reader) where T : new()
        {
            T t = new T();
            string tempName = string.Empty;
            using (reader)
            {
                if (reader.Read())
                {
                    PropertyInfo[] propertys = t.GetType().GetProperties();

                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;

                        if (ReaderExists(reader, tempName))
                        {
                            if (!pi.CanWrite)
                            {
                                continue;
                            }
                            var value = reader[tempName];

                            if (value != DBNull.Value)
                            {
                                pi.SetValue(t, value, null);
                            }
                        }
                    }
                }
                reader.Close();
            }
            return t;
        }

        /// <summary>  
        /// 利用反射和泛型  
        /// DataTable数据集
        /// </summary>  
        /// <param name="dt">表元素</param>  
        /// <returns></returns>  
        public static List<T> ConvertToList<T>(this DataTable dt) where T : new()
        {

            // 定义集合  
            List<T> ts = new List<T>();

            // 获得此模型的类型  
            Type type = typeof(T);
            //定义一个临时变量  
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性  
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性  
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量  
                    //检查DataTable是否包含此列（列名==对象的属性名）    
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter  
                        if (!pi.CanWrite) continue;//该属性不可写，直接跳出  
                        //取值  
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性  
                        if (value != DBNull.Value)
                            pi.SetValue(t, value, null);
                    }
                }
                //对象添加到泛型集合中  
                ts.Add(t);
            }

            return ts;

        }

        /// <summary>
        /// 利用反射和泛型 
        /// 序列化SqlDataReader数据集
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ConvertToList<T>(this SqlDataReader reader) where T : new()
        {
            List<T> datalist = new List<T>();
            string tempName = string.Empty;
            using (reader)
            {
                while (reader.Read())
                {
                    T t = new T();

                    PropertyInfo[] propertys = t.GetType().GetProperties();

                    foreach (PropertyInfo pi in propertys)
                    {
                        tempName = pi.Name;

                        if (ReaderExists(reader, tempName))
                        {
                            if (!pi.CanWrite)
                            {
                                continue;
                            }
                            var value = reader[tempName];

                            if (value != DBNull.Value)
                            {
                                pi.SetValue(t, value, null);
                            }
                        }
                    }

                    datalist.Add(t);
                }
                reader.Close();
            }

            return datalist;
        }


        /// <summary>
        /// 序列化xml内容
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="xmltext">xml内容</param>
        /// <returns></returns>
        public static T ConvertToModel<T>(this string xmltext) where T : new()
        {
            using (StringReader sr = new StringReader(xmltext))
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(T));
                return (T)xmlser.Deserialize(sr);
            }
        }

        /// <summary>
        /// 判断T类型列是否存在
        /// </summary>
        /// <param name="dr">数据流</param>
        /// <param name="columnName">列名称</param>
        /// <returns></returns>
        private static bool ReaderExists(SqlDataReader dr, string columnName)
        {
            dr.GetSchemaTable().DefaultView.RowFilter = "ColumnName= '" + columnName + "'";
            return (dr.GetSchemaTable().DefaultView.Count > 0);
        }



        #endregion
    }
}
