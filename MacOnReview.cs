Sql = $@"DECLARE @vacation_days FLOAT
	DECLARE @vacation_days_minu FLOAT
	DECLARE @vacation_type_no INT
	DECLARE @user_no INT
	SELECT @user_no = U.user_no 
	FROM [dbo].[tb_user] U WITH(NOLOCK)
	WHERE U.user_id = '{user_id}'
	
	SELECT @vacation_days = V.vacation_days, @vacation_days_minu = -V.vacation_days
	FROM [dbo].[tb_vacation_type]V 
	WHERE V.type = '{vacation_type}' AND V.vaction_name = '{vacation_name}'
	AND V.daily_work_hour = '{work_hour}'
	-- 지금 여기까지는 아무것도 없는 상황 
	--> 현재 이상황에서 viewTable 에는 vacation_type_no 에대한 값이 없는상황
	-- 이부분은 아무런 데이터 값이 없는 경우 (초기부분일때)  
	IF(@vacation_type_no IS NULL)
	BEGIN
  	     INSERT INTO [dbo].[tb_vacation_type] WITH(ROWLOCK)
	     (company_no , vacation_name , type , daily_work_hour , vacation_days , apply_YN , use_YN , allday_YN, start_time , end_time)
VALUES
(1,'{vacation_name}','{vacation_type}','{work_hour}','{vacation_days},'{reward_YN}','Y','{allday_YN}','{start_time}','{end_time}')
-- 여기에서 SCOPE_IDENTITY() 를 이용하여 마지막 막 열에 넣어진 값을 반환 한후
-- sequence 처리 되어서 +1 증가 되어 넘버를 넣게됨 
SET @vacation_type_no = SCOPE_IDENTITY()
SET @vacation_days_minu = -{vacation_days}
@SET @vacation_days = {vacation_days}
END

-- user_no => user에 대한 Data 있고 , vacation_type => vacation 이 있는경우 (NOT NULL)
IF(@user_no IS NOT NULL AND @vacation_type_no IS NOT NULL)
BEGIN
    IF NOT EXISTS(
	SELECT *
	FROM [dbo].[tb_vacation] V WITH(NOLOCK)
	INNER JOIN [dbo].[tb_approval] A ON A.approval_no = V.approval_no AND A.status = 'accept'
	INNER JOIN [dbo].[tb_approval] WITH(ROWLOCK)
	WHERE CONVERT(DATE,V.start_date) = '{start_date}'
     )
     BEGIN
	INSERT INTO [dbo].[tb_approval] WITH(ROWLOCK)
	(user_no , approval_type, start_time ,end_time , accept_time,status, memo , accept_user_no , vacation_type_no , vacation_name , vacation_days, vacatiom_work_hour)

SET @approval_no = SCOPE_IDENTITY()

 VALUES
                                         (@user_no, @vacation_type_no, '{vacation_name}', @vacation_days_minu, '{start_date + " 00:00:00.000"}', '{end_date + " 23:59:59.000"}', '{memo}', @approval_no, '{work_hour}', '{reward_YN}', '{allday_YN}', '{start_time}', '{end_time}')

                                         INSERT INTO [dbo].[tb_approval_line] WITH(ROWLOCK)
                                         (approval_no, approval_order, user_sent_to, approvalYN, approval_time)
                                         VALUES
                                         (@approval_no, 1, 1, 'Y', SYSDATETIME())


