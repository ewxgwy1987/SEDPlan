GO
USE [SEDPLAN];
GO

ALTER PROCEDURE dbo.stp_ImportSTDData
		  --@STDImportName NVARCHAR(100),
		  @PartsName NVARCHAR(100),
		  @Spec NVARCHAR(100),
		  @STDWeight NVARCHAR(100),
		  @Revision NVARCHAR(100),
		  @CRTIME NVARCHAR(100)
AS
BEGIN

	BEGIN TRANSACTION;
	BEGIN TRY
		--DECLARE @STD_ImportName NVARCHAR(100) = @STDImportName;
		--DECLARE @Parts_Name NVARCHAR(100) = @PartsName;
		--DECLARE @Specification NVARCHAR(100) = @Spec;
		DECLARE @STD_Weight FLOAT = CAST(@STDWeight AS FLOAT);
		DECLARE @NewRevision INT = CAST(@Revision AS INT);
		DECLARE @Created_Time Datetime = CAST(@CRTIME AS datetime);

		INSERT INTO STD_Parts(Parts_Name,Specification,STD_Weight,Revision,Created_Time)
		VALUES(@PartsName,@Spec,@STD_Weight,@NewRevision,@Created_Time);
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT>0
			ROLLBACK TRANSACTION;
		THROW
	END CATCH;

	IF @@TRANCOUNT>0
		COMMIT TRANSACTION;
END