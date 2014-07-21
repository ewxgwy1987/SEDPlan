GO
USE [SEDPLAN];
GO

ALTER FUNCTION DBO.FUN_DDT_DDID_EXISTS(@DD_ID NVARCHAR(50))
RETURNS BIT
AS
BEGIN

	DECLARE @ISEXISTS BIT;

	IF EXISTS
	(
		SELECT	DD_ID 
		FROM	DD_Variable_Map DDV 
		WHERE	DDV.DD_ID=@DD_ID
			AND DDV.VAR_Type='F'
			AND DDV.Revision=(SELECT MAX(Revision) FROM DD_Variable_Map WHERE VAR_Type='F')
	) OR
	EXISTS
	(
		SELECT	DD_ID 
		FROM	DD_Variable_Map DDV 
		WHERE	DDV.DD_ID=@DD_ID
			AND DDV.VAR_Type!='F'
	)
		SET @ISEXISTS=1;
	ELSE
		SET @ISEXISTS=0;

	RETURN @ISEXISTS;

END