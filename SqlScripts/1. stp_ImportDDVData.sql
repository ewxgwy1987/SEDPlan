GO
USE [SEDPLAN];
GO

ALTER PROCEDURE dbo.stp_ImportDDVData
		  @DDID NVARCHAR(100),
		  @L_para NVARCHAR(100),
		  @LS_para NVARCHAR(100),
		  @A_para NVARCHAR(100),
		  @Weight NVARCHAR(100),
		  @Revision NVARCHAR(100),
		  @CRTIME NVARCHAR(100)
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		DECLARE @DD_ID NVARCHAR(50) = @DDID;
		DECLARE @VAR_Type NVARCHAR(5);
		DECLARE @VAR_Value NVARCHAR(50);
		DECLARE @VAR_Weight FLOAT = CAST(@Weight AS FLOAT);
		DECLARE @NewRevision int = CAST(@Revision AS INT);
		DECLARE @Created_Time Datetime = CAST(@CRTIME AS datetime);

		IF @L_para != ''
		BEGIN
			SET @VAR_Type = 'L';
			SET @VAR_Value = @L_para;
			INSERT INTO DD_Variable_Map(DD_ID,VAR_Type,VAR_Value,VAR_Weight,Revision,Created_Time)
			VALUES(@DD_ID,@VAR_Type,@VAR_Value,@VAR_Weight,@NewRevision,@Created_Time);
		END

		IF @LS_para != ''
		BEGIN
			SET @VAR_Type = 'LS';
			SET @VAR_Value = @LS_para;
			INSERT INTO DD_Variable_Map(DD_ID,VAR_Type,VAR_Value,VAR_Weight,Revision,Created_Time)
			VALUES(@DD_ID,@VAR_Type,@VAR_Value,@VAR_Weight,@NewRevision,@Created_Time);
		END

		IF @A_para != ''
		BEGIN
			SET @VAR_Type = 'A¡ã';
			SET @VAR_Value = @A_para;
			INSERT INTO DD_Variable_Map(DD_ID,VAR_Type,VAR_Value,VAR_Weight,Revision,Created_Time)
			VALUES(@DD_ID,@VAR_Type,@VAR_Value,@VAR_Weight,@NewRevision,@Created_Time);
		END

		DECLARE @ERRMSG VARCHAR(max);
		IF @L_para='' AND @LS_para='' AND @A_para=''
		BEGIN
			SET @ERRMSG = 'All variable values are empty. No Data can be inserted.'+ CHAR(13) + CHAR(10);
			THROW 60001,@ERRMSG,1
		END
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT>0
			ROLLBACK TRANSACTION;
		THROW
	END CATCH;

	IF @@TRANCOUNT>0
		COMMIT TRANSACTION;
END