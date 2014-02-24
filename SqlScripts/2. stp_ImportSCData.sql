GO
USE [SEDPLAN];
GO

ALTER PROCEDURE dbo.stp_ImportSCData
		  @SAID NVARCHAR(100),
		  @PARTSNAME NVARCHAR(100),
		  @SPEC NVARCHAR(100),
		  @LHS NVARCHAR(100),
		  @RHS NVARCHAR(100),
		  @PCE NVARCHAR(100),
		  @VAR NVARCHAR(100),
		  @WEIGHT NVARCHAR(100),
		  @PROCESS NVARCHAR(100),
		  @REVISION NVARCHAR(100),
		  @CRTIME NVARCHAR(100)
AS
BEGIN
	DECLARE @SA_ID NVARCHAR(50) = @SAID;
	DECLARE @Parts_Name NVARCHAR(100) = @PARTSNAME;
	DECLARE @Specification NVARCHAR(100) = @SPEC;

	DECLARE @LHS_INT INT = CAST(@LHS AS INT);
	DECLARE @RHS_INT INT = CAST(@RHS AS INT);
	DECLARE @PCE_INT INT = CAST(@PCE AS INT);

	DECLARE @IsVariable BIT;
	DECLARE @VAR_Type NVARCHAR(5);
	DECLARE @IsStandard BIT;
	DECLARE @Parts_Weight FLOAT;
	DECLARE @Process_ID BIGINT;
	DECLARE @CREATED_TIME DATETIME;
	DECLARE @NEW_REVISION INT;

	IF @VAR != ''
	BEGIN
		SET @IsVariable = 1;
		SET @VAR_Type = @VAR;
		SET @IsStandard = 0;
		SET @Parts_Weight= 0;
	END
	ELSE
	BEGIN
		SET @IsVariable= 0;
		SET @VAR_Type = '';

		IF @WEIGHT=''
		BEGIN
			SET @IsStandard = 1;
			SET @Parts_Weight = 0;
		END
		ELSE
		BEGIN
			SET @IsStandard = 0;
			SET @Parts_Weight = @WEIGHT;
		END
	END

	SELECT @Process_ID = Process_ID
	FROM SA_Process SAP
	WHERE SAP.Process_Name = @PROCESS;

	--SET @CREATED_TIME = GETDATE();

	--SELECT @NEW_REVISION = MAX(Revision)
	--FROM SA_Component SC
	--WHERE SC.SA_ID=@SAID AND SC.Parts_Name=@PARTSNAME AND SC.Specification=@SPEC;

	--IF @NEW_REVISION IS NULL
	--	SET @NEW_REVISION = 0;
	--ELSE
	--	SET @NEW_REVISION = @NEW_REVISION + 1;
	SET @NEW_REVISION = CAST(@REVISION AS INT);
	SET @CREATED_TIME = CAST(@CRTIME AS datetime);

	INSERT INTO SA_Component
	(SA_ID,Parts_Name,Specification,LHS,RHS,PCE,IsVariable,
	VAR_Type,IsStandard,Parts_Weight,Process_ID,Revision,Created_Time)
	VALUES 
	(@SA_ID,@Parts_Name,@Specification,@LHS_INT,@RHS_INT,@PCE_INT,@IsVariable,
	@VAR_Type,@IsStandard,@Parts_Weight,@Process_ID,@NEW_REVISION,@CREATED_TIME);

END