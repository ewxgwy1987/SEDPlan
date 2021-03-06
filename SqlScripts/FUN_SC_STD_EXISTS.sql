GO
USE [SEDPLAN];
GO

ALTER FUNCTION DBO.FUN_SC_STD_EXISTS(@Parts_Name NVARCHAR(100),@Spec NVARCHAR(100),@IsStandard BIT)
RETURNS BIT
AS
BEGIN

	DECLARE @ISEXISTS BIT;

	IF EXISTS
	(
		SELECT STD.Created_Time
		FROM STD_Parts STD
		WHERE STD.Parts_Name = @Parts_Name
		AND @IsStandard=1
		AND STD.Specification = @Spec
		AND STD.Revision=(SELECT MAX(Revision) FROM STD_Parts)
	) OR @IsStandard = 0
		SET @ISEXISTS=1;
	ELSE
		SET @ISEXISTS=0;

	RETURN @ISEXISTS;

END