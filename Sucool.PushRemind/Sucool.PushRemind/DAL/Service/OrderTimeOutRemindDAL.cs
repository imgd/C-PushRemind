using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Sucool.PushRemind.DAL
{
    public class OrderTimeOutRemindDAL
    {
        /// <summary>
        /// 获取待推送的优惠卷用户信息
        /// </summary>
        /// <returns></returns>
        public DataSet GetRemindData()
        {
            #region sql
            string sqltext = @"DECLARE @tmible_df1 TABLE
                                        (
                                          id INT PRIMARY KEY
                                                 IDENTITY(1, 1) ,
                                          mobile CHAR(11)
                                        )                

                                    INSERT  INTO @tmible_df1
                                            ( mobile
                                            )
                                            SELECT  DISTINCT
                                                    dbo.UserBase.D_MobilePhone
                                            FROM    dbo.Orderinfo
                                                    INNER JOIN dbo.UserBase ON dbo.Orderinfo.UserId = dbo.UserBase.Id
                                            WHERE   Pay_status = 1
                                                    AND DATEDIFF(HOUR, Order_time, GETDATE()) >= @_timeouthnum
                                                    AND LastPushTime IS NULL
                                                    AND DATEADD(mi, dbo.GetTimeOut(2), Order_time) >= GETDATE()
                                                    AND dbo.UserBase.Status = 0
                                                    AND dbo.UserBase.D_MobilePhone IS NOT NULL                
                
                                    UPDATE  dbo.Orderinfo
                                    SET     LastPushTime = GETDATE()
                                    WHERE   Pay_status = 1
                                            AND DATEDIFF(HOUR, Order_time, GETDATE()) >= @_timeouthnum
                                            AND LastPushTime IS NULL
                                            AND DATEADD(mi, dbo.GetTimeOut(2), Order_time) >= GETDATE()        
        
                                     
                                    DECLARE @sendmobiles TABLE ( mobiles NVARCHAR(MAX) );                                    
                                    DECLARE @t_count FLOAT ,
                                        @t_pagesize FLOAT ,
                                        @t_pageindex INT ,
                                        @t_pagecount INT;      
    
                                    SET @t_pageindex = 1;
                                    SET @t_pagesize = @_cpageCount;
                                    SELECT  @t_count = COUNT(*)
                                    FROM    @tmible_df1
                                    SET @t_pagecount = CEILING(@t_count / @t_pagesize);
       
                                    WHILE @t_pageindex <= @t_pagecount
                                        BEGIN
                                            DECLARE @t_mobiles NVARCHAR(MAX)
                                            SET @t_mobiles = NULL;
        
                                            IF @t_pageindex = 1
                                                SELECT  @t_mobiles = ISNULL(( @t_mobiles + ',' ), '') + mobile
                                                FROM    ( SELECT TOP ( CAST(@t_pagesize AS INT) )
                                                                    mobile
                                                          FROM      @tmible_df1
                                                        ) AS tpagedata
                                            ELSE
                                                BEGIN
                                                    SELECT  @t_mobiles = ISNULL(( @t_mobiles + ',' ), '') + mobile
                                                    FROM    ( SELECT TOP ( CAST(@t_pagesize AS INT) )
                                                                        mobile
                                                              FROM      @tmible_df1
                                                              WHERE     id > ( SELECT   MAX(Id)
                                                                               FROM     ( SELECT TOP ( ( @t_pageindex
                                                                                                  - 1 )
                                                                                                  * CAST(@t_pagesize AS INT) )
                                                                                                  id
                                                                                          FROM    @tmible_df1
                                                                                          ORDER BY id ASC
                                                                                        ) AS tpagedatalins
                                                                             )
                                                            ) AS tpagedata
                                                END 
                    
                                            INSERT  INTO @sendmobiles
                                                    ( mobiles )
                                            VALUES  ( @t_mobiles )
        
                                            SET @t_pageindex = @t_pageindex + 1;               
        
                                        END
                                    SELECT  mobiles
                                    FROM    @sendmobiles";
            #endregion

            SqlParameter[] para = new SqlParameter[] { 
             new SqlParameter("@_cpageCount",SqlDbType.Int,4){Value=Config.MsgSendMaxCount},
             new SqlParameter("@_timeouthnum",SqlDbType.Int,4){Value=Config.OrderTimeOutRemindHourNum}
            };
            DataSet ds = SqlDBProvider.ExecuteSelect(sqltext, para);
            return ds;
        }
    }
}
