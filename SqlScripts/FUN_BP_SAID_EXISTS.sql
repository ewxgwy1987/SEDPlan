GO
USE [SEDPLAN];
GO

ALTER FUNCTION DBO.FUN_BP_SAID_EXISTS(@SA_ID NVARCHAR(50))
RETURNS BIT
AS
BEGIN

	DECLARE @ISEXISTS BIT;

	IF EXISTS
	(
		SELECT SC.SA_ID
		FROM SA_Component SC
		WHERE SC.SA_ID = @SA_ID
	)
		SET @ISEXISTS=1;
	ELSE
		SET @ISEXISTS=0;

	RETURN @ISEXISTS;

END
