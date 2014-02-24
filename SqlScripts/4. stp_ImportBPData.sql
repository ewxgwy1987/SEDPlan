GO
USE [SEDPLAN];
GO

CREATE PROCEDURE dbo.stp_ImportBPData
		  @PlanName NVARCHAR(100),
		  @SAID NVARCHAR(100),
		  @Quantity NVARCHAR(100),
		  @L_para NVARCHAR(100),
		  @LS_para NVARCHAR(100),
		  @A_para NVARCHAR(100),
		  @Revision NVARCHAR(100),
		  @CRTIME NVARCHAR(100)
AS
BEGIN
	DECLARE @Plan_Name NVARCHAR(100) = @PlanName;
	DECLARE @SA_ID NVARCHAR(50) = @SAID;
	DECLARE @VAR_Type NVARCHAR(5);
	DECLARE @VAR_Value NVARCHAR(50);
	DECLARE @Qty INT = CAST(@Quantity AS INT);
	DECLARE @IsMain BIT = 0;
	DECLARE @NewRevision INT = CAST(@Revision AS INT);
	DECLARE @Created_Time Datetime = CAST(@CRTIME AS datetime);

	IF @L_para != ''
	BEGIN
		SET @VAR_Type = 'L';
		SET @VAR_Value = @L_para;
		IF @IsMain = 0
			SET @IsMain = 1;
		INSERT INTO SA_Variable_Map VALUES(@Plan_Name,@SA_ID,@VAR_Type,@VAR_Value,@Qty,@IsMain,@NewRevision,@Created_Time);
	END

	IF @LS_para != ''
	BEGIN
		SET @VAR_Type = 'LS';
		SET @VAR_Value = @LS_para;
		IF @IsMain = 0
			SET @IsMain = 1;
		INSERT INTO SA_Variable_Map VALUES(@Plan_Name,@SA_ID,@VAR_Type,@VAR_Value,@Qty,@IsMain,@NewRevision,@Created_Time);
	END

	IF @A_para != ''
	BEGIN
		SET @VAR_Type = 'A¡ã';
		SET @VAR_Value = @A_para;
		IF @IsMain = 0
			SET @IsMain = 1;
		INSERT INTO SA_Variable_Map VALUES(@Plan_Name,@SA_ID,@VAR_Type,@VAR_Value,@Qty,@IsMain,@NewRevision,@Created_Time);
	END
END