GO
USE [SEDPLAN];
GO

ALTER FUNCTION DBO.FUN_SC_VAR_EXISTS(@Specification NVARCHAR(100),@VAR_TYPE NVARCHAR(5),@IsStandard BIT,@Para_Type NVARCHAR(5))
RETURNS BIT
AS
BEGIN

	DECLARE @ISEXISTS BIT;

	IF EXISTS
	(
		SELECT DD_ID
		FROM DD_Variable_Map DDV
		WHERE DDV.DD_ID = @Specification
			AND @IsStandard=0
			AND @Para_Type =''
			AND DDV.VAR_Type = @VAR_TYPE
			AND DDV.Revision=(SELECT MAX(Revision) FROM DD_Variable_Map DDV2 WHERE DDV2.DD_ID=@Specification)
			
	) OR @IsStandard = 1 OR @Para_Type != ''
		SET @ISEXISTS = 1;
	ELSE
		SET @ISEXISTS=0;

	RETURN @ISEXISTS;

END