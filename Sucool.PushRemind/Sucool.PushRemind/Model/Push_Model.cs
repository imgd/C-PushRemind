using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sucool.PushRemind.Model
{
    public class Push_Model
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 发送类型 0 立即发送  1定时发送
        /// </summary>
        public int SendType { get; set; }
        /// <summary>
        /// 发送内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 推送平台
        /// </summary>
        public int SendPlatform { get; set; }
        /// <summary>
        /// 推送目标
        /// </summary>
        public int SendAudience { get; set; }
        /// <summary>
        /// 目标关键字
        /// </summary>
        public string AudienceName { get; set; }
        /// <summary>
        /// 消息Id
        /// </summary>
        public long MessageID { get; set; }
        /// <summary>
        /// 成功码 0 成功
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// 可选参数
        /// </summary>
        public string Extras { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }

    public class Push_GoldBook_Model
    {
        public int UsedId { get; set; }
    }

    public class Push_OrderOut_Model 
    {
        public int UserId { get; set; }
    }

    public class Push_NotLoginUser_Model
    {
        public int Id { get; set; }
    }
}
