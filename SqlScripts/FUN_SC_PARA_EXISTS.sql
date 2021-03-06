GO
USE [SEDPLAN];
GO

ALTER FUNCTION DBO.FUN_SC_PARA_EXISTS(@SA_ID NVARCHAR(50),@Parts_Name NVARCHAR(100),@Para_Type NVARCHAR(5),@IsSubSA BIT)
RETURNS BIT
AS
BEGIN

	DECLARE @ISEXISTS BIT;

	IF EXISTS
	(
		SELECT DD_ID
		FROM DD_TYPES DDT
		WHERE DDT.SA_ID = @SA_ID
			AND DDT.Parts_Name = @Parts_Name
			AND DDT.Para_Type = @Para_Type
			AND @Para_Type != ''
			AND @IsSubSA=0 -- NOT SUB SA
			AND DDT.Revision=(SELECT MAX(Revision) FROM DD_TYPES DDT2 WHERE DDT2.SA_ID=@SA_ID)
	) OR @Para_Type = '' OR @IsSubSA=1
		SET @ISEXISTS = 1;
	ELSE
		SET @ISEXISTS=0;

	RETURN @ISEXISTS;

END