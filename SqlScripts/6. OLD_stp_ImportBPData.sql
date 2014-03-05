GO
USE [SEDPLAN];
GO

ALTER PROCEDURE dbo.stp_ImportBPData
		  @PlanName NVARCHAR(100),
		  @SAID NVARCHAR(100),
		  @Quantity NVARCHAR(100),
		  @PARAS NVARCHAR(MAX),
		  --@L_para NVARCHAR(100),
		  --@LS_para NVARCHAR(100),
		  --@A_para NVARCHAR(100),
		  --@TB_para NVARCHAR(100),
		  --@H_para NVARCHAR(100),
		  @Revision NVARCHAR(100),
		  @CRTIME NVARCHAR(100)
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		DECLARE @Plan_Name NVARCHAR(100) = @PlanName;
		DECLARE @SA_ID NVARCHAR(50) = @SAID;
		DECLARE @VAR_Type NVARCHAR(5);
		DECLARE @VAR_Value NVARCHAR(50);
		DECLARE @Qty INT = CAST(@Quantity AS INT);
		DECLARE @IsMain BIT = 0;
		DECLARE @NewRevision INT = CAST(@Revision AS INT);
		DECLARE @Created_Time Datetime = CAST(@CRTIME AS datetime);

		DECLARE @MARK BIT = 0;

		DECLARE @PARANAMES NVARCHAR(50) = 'L,LS,A,TB,H';
		SELECT INTO 
		

		IF (@L_para IS NULL OR @L_para = '')
		AND (@LS_para IS NULL OR @LS_para = '')
		AND (@A_para IS NULL OR @A_para = '')
		BEGIN
			SET @VAR_Type = 'F';
			SET @VAR_Value = '-';
			IF @MARK = 0
			BEGIN
				SET @MARK = 1;
				SET @IsMain = 1;
			END
			ELSE 
			BEGIN
				SET @IsMain = 0;
			END
			INSERT INTO BOM_Plan(Plan_Name,SA_ID,VAR_Type,VAR_Value,Quantity,PARA_TB,PARA_H,IsMain,Revision,Created_Time)
			VALUES(@Plan_Name,@SA_ID,@VAR_Type,@VAR_Value,@Qty,@IsMain,@NewRevision,@Created_Time);
		END
		ELSE
		BEGIN
			IF @L_para != ''
			BEGIN
				SET @VAR_Type = 'L';
				SET @VAR_Value = @L_para;

				IF @MARK = 0
				BEGIN
					SET @MARK = 1;
					SET @IsMain = 1;
				END
				ELSE 
				BEGIN
					SET @IsMain = 0;
				END

				INSERT INTO BOM_Plan(Plan_Name,SA_ID,VAR_Type,VAR_Value,Quantity,PARA_TB,PARA_H,IsMain,Revision,Created_Time)
				VALUES(@Plan_Name,@SA_ID,@VAR_Type,@VAR_Value,@Qty,@IsMain,@NewRevision,@Created_Time);
			END

			IF @LS_para != ''
			BEGIN
				SET @VAR_Type = 'LS';
				SET @VAR_Value = @LS_para;
		
				IF @MARK = 0
				BEGIN
					SET @MARK = 1;
					SET @IsMain = 1;
				END
				ELSE 
				BEGIN
					SET @IsMain = 0;
				END

				INSERT INTO BOM_Plan(Plan_Name,SA_ID,VAR_Type,VAR_Value,Quantity,PARA_TB,PARA_H,IsMain,Revision,Created_Time)
				VALUES(@Plan_Name,@SA_ID,@VAR_Type,@VAR_Value,@Qty,@IsMain,@NewRevision,@Created_Time);
			END

			IF @A_para != ''
			BEGIN
				SET @VAR_Type = 'A¡ã';
				SET @VAR_Value = @A_para;
		
				IF @MARK = 0
				BEGIN
					SET @MARK = 1;
					SET @IsMain = 1;
				END
				ELSE 
				BEGIN
					SET @IsMain = 0;
				END

				INSERT INTO BOM_Plan(Plan_Name,SA_ID,VAR_Type,VAR_Value,Quantity,IsMain,Revision,Created_Time)
				VALUES(@Plan_Name,@SA_ID,@VAR_Type,@VAR_Value,@Qty,@IsMain,@NewRevision,@Created_Time);
			END
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