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
		  @PARATYPE NVARCHAR(100),
		  @PROCESS NVARCHAR(100),
		  @REVISION NVARCHAR(100),
		  @CRTIME NVARCHAR(100)
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		DECLARE @SA_ID NVARCHAR(50) = @SAID;
		DECLARE @Parts_Name NVARCHAR(100) = @PARTSNAME;
		DECLARE @Specification NVARCHAR(100) = @SPEC;

		DECLARE @LHS_INT INT = CAST(@LHS AS INT);
		DECLARE @RHS_INT INT = CAST(@RHS AS INT);
		DECLARE @PCE_INT INT = CAST(@PCE AS INT);

		DECLARE @IsVariable BIT;
		DECLARE @VAR_Type NVARCHAR(5);
		DECLARE @IsStandard BIT;
		DECLARE @Para_Type NVARCHAR(5) = @PARATYPE;
		DECLARE @Process_ID BIGINT;
		DECLARE @CREATED_TIME DATETIME = CAST(@CRTIME AS datetime);
		DECLARE @NEW_REVISION INT = CAST(@REVISION AS INT);

		IF @VAR != ''
		BEGIN
			SET @IsVariable = 1;
			SET @VAR_Type = @VAR;
			SET @IsStandard = 0;
			--SET @Parts_Weight= 0;
		END
		ELSE
		BEGIN
			SET @IsVariable = 0;
			SET @VAR_Type = 'F';

			IF @Para_Type != ''
			BEGIN
				SET @IsStandard = 0;
			END
			ELSE
			BEGIN
				-- if it is not fixed weight, then set isStandard to be 0
				IF EXISTS(SELECT * FROM DD_Variable_Map DDV WHERE DDV.DD_ID = @Specification AND DDV.VAR_Type = 'F')
					SET @IsStandard = 0;
				ELSE
					SET @IsStandard = 1;
				--SET @Parts_Weight = @WEIGHT;
			END

		END

		SELECT @Process_ID = Process_ID
		FROM SA_Process SAP
		WHERE SAP.Process_Name = @PROCESS;

		--print @SA_ID + ' ' + @Parts_Name + ' ' + @Specification + ' ' + @LHS_INT + ' ' + @RHS_INT + ' ' + @PCE_INT + ' ' + @IsVariable
		-- + ' ' + @VAR_Type + ' ' + @IsStandard + ' ' + @Para_Type + ' ' + @Process_ID + ' ' + @NEW_REVISION + ' ' + @CREATED_TIME;

		INSERT INTO SA_Component
		(SA_ID,Parts_Name,Specification,LHS,RHS,PCE,IsVariable,
		VAR_Type,IsStandard,Para_Type,Process_ID,Revision,Created_Time)
		VALUES 
		(@SA_ID,@Parts_Name,@Specification,@LHS_INT,@RHS_INT,@PCE_INT,@IsVariable,
		@VAR_Type,@IsStandard,@Para_Type,@Process_ID,@NEW_REVISION,@CREATED_TIME);

	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT>0
			ROLLBACK TRANSACTION;
		THROW
	END CATCH;

	IF @@TRANCOUNT>0
		COMMIT TRANSACTION;
END
