using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sync
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.GetCurrentClassLogger().Info($"부서 동기화 시작");
            SyncDept();
            LogManager.GetCurrentClassLogger().Info($"부서 동기화 종료{Environment.NewLine}");

            LogManager.GetCurrentClassLogger().Info($"사용자 동기화 시작");
            SyncUser();
            LogManager.GetCurrentClassLogger().Info($"사용자 동기화 종료{Environment.NewLine}");

            LogManager.GetCurrentClassLogger().Info($"부서 사용자 동기화 시작");
            SyncUser_Dept();
            LogManager.GetCurrentClassLogger().Info($"부서 사용자 동기화 종료");


        }

        static void SyncDept()
        {
            List<string> create_list = new List<string>();
            List<string> update_list = new List<string>();

            string sql = string.Empty;

            try
            {
                #region Step 1 : 부서 정보 조회

                List<object> items = new List<object>();
                DBHelper dbHelper = new DBHelper();
                DBHelper_Oracle dbHelper_Oracle = new DBHelper_Oracle();
                //sql => VW_PCOFF_DEPT : 구축사에서 합의해서 나한테 전달해준 View (Table) => Oracle DB
                // 우리쪽 DB ( ms sql ) --> 해당하는 테이블 (tb_dept) --> 컬럼이 저거 보다 많아요
                // --> 그 많은 컬럼중에서 고객사가 원하는 연동 프로그램 요구사항에 따라서 컬럼을 더 추가해서 select 를 해서 program 을 만들어주던지
                // 아니면 필요없는 기능은 빼야할지 --> 관건 
                sql = @"SELECT DEPT_ID, DEPT_NAME, PARENT_DEPT_ID
                        FROM VW_PCOFF_DEPT";
                //connectionString 
                string connectionString_HR = ConfigurationManager.ConnectionStrings["ConnDB_HR"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

                #endregion
                //Oracle SQL --> Execute 
                using (DataTable dt = dbHelper_Oracle.ExecuteSQL(sql, connectionString_HR))
                {
                    //dt --> 연동테이블 
                    if (dt == null || dt.Rows.Count <= 0)
                    {
                        LogManager.GetCurrentClassLogger().Info($"HR DB 데이터 없음");
                        return;
                    }

                    #region Step 2 : 부서추가

                    int sync_count = 0;
                    //정렬 api ==> 처음상태에서 VW_PCOFF_DEPT View 를 영원아웃도어 쪽에서 dbvisualizer 로 select 해보면
                    //상위부서 --> 0 으로표기
                    // 중요한건 뒤죽박죽 임 (순서대로 나열되어있지않음)
                    // 때문에 상위부서를 0으로 끌어올려서 순차적으로 정렬하기 위함 
                    DataTable dt_sort = dt.Clone();//이부분은 datatable 을 복제를 하는겁니다. -> 우선적으로 dt 는 현재 정렬이 되어있지않은 상태에요
                    //테이블 하나를 복제해서 정렬을 해주기 위함
                    // 테이블을 복제한다음 Clear() api 를 사용해서 안에있는 정렬되지않은 데이터를 지운다음 정렬된 데이터로 넣기위한 api 를 사용한것이 Clear()
                    dt_sort.Clear();
                    Dictionary<string, string> dic = new Dictionary<string, string>();

                    for (int rowIndex = 0; rowIndex < dt.Rows.Count; rowIndex++)
                    {
                        string dept_id = dt.Rows[rowIndex]["DEPT_ID"].ToString();
                        string parent_dept_id = dt.Rows[rowIndex]["PARENT_DEPT_ID"].ToString();

                        Parent_DeptSync(dt_sort, dt, dt.Rows[rowIndex], dept_id, parent_dept_id, dic);//--> 정렬하는 재귀함수 
                    }

                    List<string> erp_dept_id_list = new List<string>();
                    //정렬한 데이터를 string 에 담아서 sql (procedure) 에 사용이됨 
                    foreach (DataRow dr in dt_sort.Rows)
                    {
                        //1) Oracle data 정렬  -> 추출 -> 우리쪽 DB(ms sql) 으로 전달을 해주고 있는 상황 
                        string dept_id = Convert.ToString(dr["DEPT_ID"]);
                        string parent_dept_id = Convert.ToString(dr["PARENT_DEPT_ID"]);
                        string dept_name = Convert.ToString(dr["DEPT_NAME"]);

                        sql = $@"
                                 -- Step 1 : dept_no 조회(dept_id)를 기준으로 조회 
                                 DECLARE @dept_no INT
                                 SELECT @dept_no = dept_no
                                 FROM [dbo].[tb_dept] WITH(NOLOCK)
                                 WHERE dept_id = '{dept_id}'

                                 -- Step 2 : parent_dept_no 조회(parent_dept_id)를 기준으로 조회 
                                 DECLARE @parent_dept_no INT
                                 SELECT @parent_dept_no = dept_no
                                 FROM [dbo].[tb_dept] WITH(NOLOCK)
                                 WHERE dept_id = '{parent_dept_id}'

                                 -- Step 3 : 부서 추가 / 수정
                                 IF(@dept_no IS NULL)
                                 BEGIN
                                    IF('{parent_dept_id}' = '0')
                                     BEGIN
                                        INSERT INTO [dbo].[tb_dept] WITH(ROWLOCK)
                                        (company_no, parent_dept_no, dept_id, name)
                                        VALUES
                                        (1, '0', '{dept_id}', '{dept_name}')
                    
                                        SELECT '{dept_id}' AS dept_id
                                        SELECT SCOPE_IDENTITY() AS dept_no
                                    END
                                    ELSE
                                    BEGIN
                                        IF(@parent_dept_no IS NOT NULL)
							            BEGIN
                                            INSERT INTO [dbo].[tb_dept] WITH(ROWLOCK)
                                            (company_no, parent_dept_no, dept_id, name)
                                            VALUES
                                            (1, @parent_dept_no, '{dept_id}', '{dept_name}')

                                            SELECT '{dept_id}' AS dept_id
                                            SELECT SCOPE_IDENTITY() AS dept_no
                                        END
                                    END
                                END
                                ELSE
                                BEGIN
                                    IF('{parent_dept_id}' = '0')
                                    BEGIN
                                        UPDATE [dbo].[tb_dept] WITH(ROWLOCK)
                                        SET name = CASE WHEN '{dept_name}' IS NULL THEN name ELSE '{dept_name}' END
                                            ,parent_dept_no = 0
                                        WHERE dept_no = @dept_no

                                        SELECT '{dept_id}' AS dept_id
                                        SELECT @dept_no AS dept_no
                                    END
                                    ELSE
                                    BEGIN
                                        IF(@parent_dept_no IS NOT NULL)
							            BEGIN
                                            UPDATE [dbo].[tb_dept] WITH(ROWLOCK)
                                            SET name = CASE WHEN '{dept_name}' IS NULL THEN name ELSE '{dept_name}' END
                                                ,parent_dept_no = @parent_dept_no
                                            WHERE dept_no = @dept_no

                                            SELECT '{dept_id}' AS dept_id
                                            SELECT @dept_no AS dept_no
                                        END
                                    END
                                END";
                        //연동 진행 

                        using (DataTable dt_dept = dbHelper.ExecuteSQL(sql, connectionString))
                        {
                            if (dt_dept.Rows.Count > 0)
                            {
                                ++sync_count;
                                LogManager.GetCurrentClassLogger().Info($"부서 추가 : {dept_id}");
                            }
                        }

                        erp_dept_id_list.Add(dept_id);
                    }

                    LogManager.GetCurrentClassLogger().Info($"총 부서 추가 : {sync_count}");

                    #endregion

                    #region Step 3 : 타임키퍼 부서 정보 조회

                    sql = @"SELECT dept_id
                            FROM [dbo].[tb_dept] WITH(NOLOCK)";

                    //부서정보 조회 list 
                    List<string> dept_id_list = new List<string>();
                    using (DataTable dt_dept = dbHelper.ExecuteSQL(sql, connectionString))
                    {
                        if (dt_dept == null || dt_dept.Rows.Count <= 0)
                        {
                            //데이터가 없을때 
                            LogManager.GetCurrentClassLogger().Info($"부서 데이터 없음");
                            return;
                        }

                        //데이터가 있을때 
                        foreach (DataRow dr in dt_dept.Rows)
                        {
                            //list 에 담아서 조회 or 또다른 작업할때 사용이 되겠죠
                            string dept_id = Convert.ToString(dr["dept_id"]);
                            dept_id_list.Add(dept_id);
                        }
                    }

                    #endregion

                    #region Step 4 : 타임키퍼 부서 삭제 --> 조회했던 데이터들을 list 에서 뽑아내서 삭제하는 방식 

                    sync_count = 0;
                    foreach (string dept_id in dept_id_list.Except(erp_dept_id_list))
                    {
                        // 삭제 --> 기준 --> dept_id
                        // list 에 저장되었던 dept_id 를 뽑아내서 삭제하는 방식 
                        sql = $@"DECLARE @dept_no INT
                                 SELECT TOP 1 @dept_no = dept_no
                                 FROM [dbo].[tb_dept] WITH(NOLOCK)
                                 WHERE dept_id = '{dept_id}'

                                 DELETE
                                 FROM [dbo].[tb_dept] WITH(ROWLOCK)
                                 WHERE dept_no = @dept_no";

                        dbHelper.ExecuteSQL(sql, connectionString);
                        ++sync_count;
                        LogManager.GetCurrentClassLogger().Info($"부서 삭제 : {dept_id}");
                    }

                    LogManager.GetCurrentClassLogger().Info($"총 부서 삭제 : {sync_count}");

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Info(sql);
                LogManager.GetCurrentClassLogger().Info($"StackTrace : {ex.StackTrace}");
                LogManager.GetCurrentClassLogger().Info($"Message : {ex.Message}");
            }
        }



        //재귀함수를 이용한 정렬 
        static void Parent_DeptSync(DataTable pdtClone, DataTable pdt, DataRow dr, string deptId, string parent_dept_Id, Dictionary<string, string> dic)
        {
            if (!dic.ContainsKey(deptId))
            {
                if (!dic.ContainsKey(parent_dept_Id))
                {
                    DataRow[] drSelect = pdt.Select($"DEPT_ID = '{parent_dept_Id}'");

                    if (drSelect.Length > 0)
                    {
                        Parent_DeptSync(pdtClone, pdt, drSelect[0], drSelect[0]["DEPT_ID"].ToString(), drSelect[0]["PARENT_DEPT_ID"].ToString(), dic);
                        dic.Add(deptId, parent_dept_Id);
                    }
                    else
                    {
                        dic.Add(deptId, parent_dept_Id);
                    }
                }
                else
                {
                    dic.Add(deptId, parent_dept_Id);
                }

                DataRow drNew = pdtClone.NewRow();
                drNew["DEPT_ID"] = dr["DEPT_ID"];
                drNew["PARENT_DEPT_ID"] = dr["PARENT_DEPT_ID"];
                drNew["DEPT_NAME"] = dr["DEPT_NAME"];
                pdtClone.Rows.Add(drNew);
            }
        }




        static void SyncUser()
        {
            List<string> create_list = new List<string>();
            List<string> update_list = new List<string>();

            string sql = string.Empty;

            try
            {
                #region Step 1 : 사용자 정보 조회

                DBHelper dbHelper = new DBHelper();
                DBHelper_Oracle dbHelper_Oracle = new DBHelper_Oracle();
                //VW_PCOFF_USER --> 사용자 정보 view table 
                sql = @"SELECT USER_ID, DEPT_ID, USER_NAME, POSITION
                        FROM VW_PCOFF_USER";

                string connectionString_HR = ConfigurationManager.ConnectionStrings["ConnDB_HR"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

                #endregion

                using (DataTable dt = dbHelper_Oracle.ExecuteSQL(sql, connectionString_HR))
                {
                    if (dt == null || dt.Rows.Count <= 0)
                    {
                        // 데이터가 없을때 
                        LogManager.GetCurrentClassLogger().Info($"HR DB 데이터 없음");
                        return;
                    }

                    #region Step 2 : 사용자 추가 --> 사용자 정보를 조회 , 조회한거를 담아주는거에요
                    //oracle (우리쪽 data 가 아님 : 고객사)
                    //고객사 data --> 우리쪽에서 가져와서 --> Ms sql에 넣는 작업 : 연동


                    int sync_count = 0;

                    List<string> erp_user_id_list = new List<string>();
                    //oracle 에 서 뽑은 data 
                    foreach (DataRow dr in dt.Rows)
                    {
                        string user_id = Convert.ToString(dr["USER_ID"]);
                        string user_name = Convert.ToString(dr["USER_NAME"]);
                        string position = Convert.ToString(dr["POSITION"]);
                        //oracle data --> ms sql 에 전달해주는 작업 : sql
                        // retireYN = 'N' == 퇴사 안한사람
                        // 퇴사한사람을 왜 사용자 추가를 하겠나요.. 
                        sql = $@"
                                 -- Step 1 : user_no 조회
                                 DECLARE @user_no INT
                                 SELECT @user_no = user_no
                                 FROM [dbo].[tb_user] WITH(NOLOCK)
                                 WHERE user_id = '{user_id}' and retireYN = 'N'

                                 -- Step 2 : 디폴트 비밀번호 조회
                                 DECLARE @default_password VARCHAR(1024)
                                 SELECT @default_password = default_password
                                 FROM [dbo].[tb_company] WITH(NOLOCK)
                                 WHERE company_no = 1

                                 -- Step 3 : 표준 근로 시간 조회
                                 DECLARE @work_start_time TIME(7),
                                         @work_end_time TIME(7)

                                 SELECT @work_start_time = work_start_time,
                                        @work_end_time = work_end_time
                                 FROM [dbo].[tb_company] WITH(NOLOCK)
                                 WHERE company_no = 1

                                 -- Step 4 : 사용자 추가
                                IF(@user_no IS NULL)
                                BEGIN
                                    INSERT INTO [dbo].[tb_user] WITH(ROWLOCK)
                                    (company_no, user_id, name, password, user_work_type, position)
                                    VALUES
                                    (1, '{user_id}', '{user_name}', @default_password, '사무직', '{position}')

                                    SELECT '{user_id}' AS user_id
                                END
                                ELSE
                                BEGIN
                                    UPDATE [dbo].[tb_user] WITH(ROWLOCK)
                                    SET name = '{user_name}'
                                        ,position = '{position}'
                                    WHERE user_no = @user_no AND user_id <> 'admin'
                    
                                    SELECT '{user_id}' AS user_id
                                END";

                        using (DataTable dt_user = dbHelper.ExecuteSQL(sql, connectionString))
                        {
                            if (dt_user.Rows.Count > 0)
                            {
                                ++sync_count;
                                LogManager.GetCurrentClassLogger().Info($"사용자 추가 : {user_id}");
                            }
                        }

                        erp_user_id_list.Add(user_id);
                    }

                    #endregion

                    #region Step 3 : 타임키퍼 사용자 정보 조회
                    //select
                    sql = @"SELECT user_id
                            FROM [dbo].[tb_user] U WITH(NOLOCK)
                            WHERE user_id <> 'admin'";

                    List<string> user_id_list = new List<string>();

                    using (DataTable dt_user = dbHelper.ExecuteSQL(sql, connectionString))
                    {
                        if (dt_user == null || dt_user.Rows.Count <= 0)
                        {
                            LogManager.GetCurrentClassLogger().Info($"DB 데이터 없음");
                            return;
                        }

                        foreach (DataRow dr in dt_user.Rows)
                        {
                            string user_id = Convert.ToString(dr["user_id"]);
                            user_id_list.Add(user_id);
                        }
                    }

                    #endregion

                    #region Step 4 : 타임키퍼 사용자 삭제

                    sync_count = 0;
                    foreach (string user_id in user_id_list.Except(erp_user_id_list))
                    {
                        sql = $@"
                                 DECLARE @user_no INT
                                 SELECT TOP 1 @user_no = user_no
                                 FROM [dbo].[tb_user] WITH(NOLOCK)
                                 WHERE user_id = '{user_id}'

                                 -- 부속 소속정보 제거
                                 IF EXISTS(SELECT *
                                           FROM [dbo].[tb_dept_user] WITH(NOLOCK)
                                           WHERE user_no = @user_no)
                                 BEGIN
                                    DELETE [dbo].[tb_dept_user] WITH(ROWLOCK)
                                    WHERE user_no = @user_no
                                 END

                                 --퇴사로 전환
                                 UPDATE [dbo].[tb_user] WITH(ROWLOCK)
                                 SET retireYN = 'Y'
                                     ,retire_time = SYSDATETIME()
                                 WHERE user_no = @user_no AND retireYN = 'N'";

                        dbHelper.ExecuteSQL(sql, connectionString);
                        ++sync_count;
                        LogManager.GetCurrentClassLogger().Info($"사용자 삭제 : {user_id}");
                    }

                    LogManager.GetCurrentClassLogger().Info($"총 사용자 삭제 : {sync_count}");

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Info(sql);
                LogManager.GetCurrentClassLogger().Info($"StackTrace : {ex.StackTrace}");
                LogManager.GetCurrentClassLogger().Info($"Message : {ex.Message}");
            }
        }

        static void SyncUser_Dept()
        {
            string sql = string.Empty;

            try
            {
                #region Step 1 : ERP 부서 정보 조회

                DBHelper dbHelper = new DBHelper();
                DBHelper_Oracle dbHelper_Oracle = new DBHelper_Oracle();

                sql = @"SELECT USER_ID, DEPT_ID, USER_NAME, POSITION
                        FROM VW_PCOFF_USER";

                string connectionString_HR = ConfigurationManager.ConnectionStrings["ConnDB_HR"].ConnectionString;
                string connectionString = ConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

                #endregion

                using (DataTable dt = dbHelper_Oracle.ExecuteSQL(sql, connectionString_HR))
                {
                    if (dt == null || dt.Rows.Count <= 0)
                    {
                        LogManager.GetCurrentClassLogger().Info($"사용자 부서 DB 데이터 없음");
                        return;
                    }

                    #region Step 2 : 사용자 부서 추가

                    int sync_count = 0;
                    List<string> erp_user_id_list = new List<string>();
                    foreach (DataRow dr in dt.Rows)
                    {
                        string user_id = Convert.ToString(dr["USER_ID"]);
                        string dept_id = Convert.ToString(dr["DEPT_ID"]);

                        sql = $@"
                                 -- Step 1 :user_no 조회
                                 DECLARE @user_no INT
                                 SELECT @user_no = user_no
                                 FROM [dbo].[tb_user] WITH(NOLOCK)
                                 WHERE user_id = '{user_id}' and retireYN = 'N'

                                -- Step 2 : dept_no 조회
                                DECLARE @dept_no INT
                                SELECT @dept_no = dept_no
                                FROM [dbo].[tb_dept] WITH(NOLOCK)
                                WHERE dept_id = '{dept_id}'

                                -- Step 3 : 사용자 부서 추가
                                IF(@user_no IS NOT NULL AND @dept_no IS NOT NULL)
                                BEGIN
                                    IF NOT EXISTS(SELECT user_no
                                                  FROM [dbo].[tb_dept_user] WITH(NOLOCK)
                                                  WHERE user_no = @user_no)
                                    BEGIN
                                        INSERT INTO [dbo].[tb_dept_user]
                                        (user_no, dept_no, main_dept_YN)
                                        VALUES
                                        (@user_no, @dept_no, 'Y')

                                        SELECT '{user_id}' AS user_id
                                    END
                                    ELSE
                                    BEGIN
                                        UPDATE [dbo].[tb_dept_user] WITH(ROWLOCK)
                                        SET dept_no = @dept_no
                                        WHERE user_no = @user_no

                                        SELECT '{user_id}' AS user_id
                                    END
                               END";

                        using (DataTable dt_user = dbHelper.ExecuteSQL(sql, connectionString))
                        {
                            if (dt_user.Rows.Count > 0)
                            {
                                ++sync_count;
                                LogManager.GetCurrentClassLogger().Info($"사용자 부서 추가 및 수정 : {user_id}");
                            }
                        }

                        erp_user_id_list.Add(user_id);
                    }

                    LogManager.GetCurrentClassLogger().Info($"총 사용자 부서 추가 및 수정 : {sync_count}");

                    #endregion
                }
            }
            catch (Exception ex)
            {
                LogManager.GetCurrentClassLogger().Info(sql);
                LogManager.GetCurrentClassLogger().Info($"StackTrace : {ex.StackTrace}");
                LogManager.GetCurrentClassLogger().Info($"Message : {ex.Message}");
            }
        }
    }
}
