GO
USE [SEDPLAN];
GO
ALTER FUNCTION [dbo].[RPT_GETPARAMETERS]
(
 @parameter varchar(max)
)
RETURNS 
@temp  TABLE 
(
	PAR NVARCHAR(50)
)
AS
BEGIN

DECLARE @d char(1)=','
set @Parameter= @Parameter +@d 
DECLARE @PLen int= len(@Parameter)
DECLARE @SIndex int=1
DECLARE @EIndex int= 0


	WHILE (@PLen > @EIndex)
	Begin
	SET @EIndex = CHARINDEX(@d , @Parameter)-1
	INSERT INTO @temp (PAR ) VALUES (LTRIM(RTRIM(SUBSTRING(@Parameter,@SIndex ,@EIndex))))
	SET @SIndex = CHARINDEX(@d , @Parameter)+1
	SET @Parameter =  SUBSTRING(@Parameter, @SIndex, @PLen )
	SET @SIndex = 1
	SET @EIndex = 0
	SET @PLen = LEN(@Parameter)
	End
	
	RETURN
END