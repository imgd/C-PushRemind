using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace Sucool.PushRemind.DAL.Service
{
    public class NotLoginUserRemindDAL
    {
        /// <summary>
        /// 获取待推送的未登陆用户
        /// 未登录day 7天发一次 以后每隔一个月发一次
        /// =7
        /// days % 30=7
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
                                                SELECT  DISTINCT  D_MobilePhone
                                                FROM    dbo.UserBase
                                                WHERE   Status = 0
                                                        AND D_MobilePhone IS NOT NULL
                                                        AND ( DATEDIFF(DAY, LastLoginTime, GETDATE()) = 7                                                              
                                                              OR DATEDIFF(DAY, LastLoginTime, GETDATE()) % 30 = 7
                                                            )
                                     
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
             new SqlParameter("@_cpageCount",SqlDbType.Int,4){Value=Config.MsgSendMaxCount}
            };
            DataSet ds = SqlDBProvider.ExecuteSelect(sqltext, para);
            return ds;
        }
    }
}
