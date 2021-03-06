GO
USE [SEDPLAN];
GO

ALTER PROCEDURE dbo.stp_ImportFWData
		  @DDID NVARCHAR(100),
		  @Weight NVARCHAR(100),
		  @COLOR NVARCHAR(100),
		  @Revision NVARCHAR(100),
		  @CRTIME NVARCHAR(100)
AS
BEGIN

	BEGIN TRANSACTION;
	BEGIN TRY
		DECLARE @DD_ID NVARCHAR(50) = @DDID;
		DECLARE @VAR_Type NVARCHAR(5) = 'F';
		DECLARE @VAR_Value NVARCHAR(50) = '-';
		DECLARE @VAR_Weight FLOAT = CAST(@Weight AS FLOAT);
		DECLARE @NewRevision int = CAST(@Revision AS INT);
		DECLARE @Created_Time Datetime = CAST(@CRTIME AS datetime);

		INSERT INTO DD_Variable_Map(DD_ID,VAR_Type,VAR_Value,VAR_Weight,Revision,Created_Time)
		VALUES(@DD_ID,@VAR_Type,@VAR_Value,@VAR_Weight,@NewRevision,@Created_Time);

		IF LEN(@COLOR)<>0
		BEGIN
			IF EXISTS(SELECT DD_ID FROM DD_COLOR WHERE DD_ID=@DD_ID)
				UPDATE DD_COLOR SET COLOR=@COLOR WHERE DD_ID=@DD_ID;
			ELSE
				INSERT INTO DD_COLOR VALUES(@DD_ID,@COLOR);
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