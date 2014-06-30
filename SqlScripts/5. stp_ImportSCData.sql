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
		DECLARE @IsSubSA BIT;
		DECLARE @Para_Type NVARCHAR(5) = @PARATYPE;
		DECLARE @Process_ID BIGINT;
		DECLARE @CREATED_TIME DATETIME = CAST(@CRTIME AS datetime);
		DECLARE @NEW_REVISION INT = CAST(@REVISION AS INT);

		IF RTRIM(LTRIM(@SPEC)) != '' OR RTRIM(LTRIM(@PARATYPE)) != ''
		BEGIN
			-- If the inserted SA part has variable, then set @IsVariable to be 1 and @IsStandard to be 0
			IF @VAR != ''
			BEGIN
				SET @IsVariable = 1;
				SET @VAR_Type = @VAR;
				SET @IsStandard = 0;
				SET @IsSubSA = 0;
				--SET @Parts_Weight= 0;
			END
			-- If the inserted SA part has no variable, then it may be a fixed components or a sub SA
			ELSE
			BEGIN
				SET @IsVariable = 0;
				--If the @Specification is an existed SA in SA_Component, then set @IsSubSA to be 1
				IF EXISTS(SELECT SA_ID FROM SA_Component WHERE SA_ID=@Specification)
				BEGIN
					SET @VAR_Type = '-';
					SET @IsStandard = 0;
					SET @IsSubSA = 1;
				END
				--If this SA parts is not sub SA
				ELSE
				BEGIN
					SET @IsSubSA = 0;

					IF @Para_Type != ''
					BEGIN
						SET @IsStandard = 0;
						SET @VAR_Type = 'F';
					END
					ELSE
					BEGIN
						-- if it is not fixed weight, then set isStandard to be 0
						IF EXISTS(SELECT * FROM DD_Variable_Map DDV WHERE DDV.DD_ID = @Specification AND DDV.VAR_Type = 'F')
						BEGIN
							SET @IsStandard = 0;
							SET @VAR_Type = 'F';
						END
						ELSE
						BEGIN
							SET @IsStandard = 1;
							SET @VAR_Type = '-';
						END
						--SET @Parts_Weight = @WEIGHT;
					END
				END
			END

			SELECT @Process_ID = Process_ID
			FROM SA_Process SAP
			WHERE SAP.Process_Name = @PROCESS;

			--print @SA_ID + ' ' + @Parts_Name + ' ' + @Specification + ' ' + @LHS_INT + ' ' + @RHS_INT + ' ' + @PCE_INT + ' ' + @IsVariable
			-- + ' ' + @VAR_Type + ' ' + @IsStandard + ' ' + @Para_Type + ' ' + @Process_ID + ' ' + @NEW_REVISION + ' ' + @CREATED_TIME;

			DECLARE @ERRMSG VARCHAR(max);
			--1. If it is standard part, then check this part exists in STD_Parts or not
			IF DBO.FUN_SC_STD_EXISTS(@Parts_Name,@Specification,@IsStandard)=0
			BEGIN
				SET @ERRMSG = 'Cannot find STD parts. Parts Name:' + @Parts_Name + ', Specification:' + @Specification + '.'  + CHAR(13) + CHAR(10);;
				SET @ERRMSG = @ERRMSG + '@/' + @SAID + '/' + @PARTSNAME + '/' + @SPEC + CHAR(13) + CHAR(10);
				THROW 60001,@ERRMSG,1
			END

			--2. If it has parameter and it is not sub SA, then check this part exists in DD_TYPES or not
			IF DBO.FUN_SC_PARA_EXISTS(@SA_ID,@Parts_Name,@Para_Type,@IsSubSA)=0
			BEGIN
				SET @ERRMSG = 'Cannot find this part in SA Detail Types. SA ID:' 
					+ @SA_ID + ', Parts Name:' + @Parts_Name + ', Detail Type:' + @Para_Type + '.'  + CHAR(13) + CHAR(10);;
				SET @ERRMSG = @ERRMSG + '@/' + @SAID + '/' + @PARTSNAME + '/' + @SPEC + CHAR(13) + CHAR(10);
				THROW 60001,@ERRMSG,1
			END

			--3. Check the process of this part exists or not
			IF DBO.FUN_SC_PROCESS_EXISTS(@Process_ID)=0
			BEGIN
				SET @ERRMSG = 'Cannot find process. Process:' + @PROCESS + '. '  + CHAR(13) + CHAR(10);
				SET @ERRMSG = @ERRMSG + '@/' + @SAID + '/' + @PARTSNAME + '/' + @SPEC + CHAR(13) + CHAR(10);
				THROW 60001,@ERRMSG,1
			END

			--4.
			IF DBO.FUN_SC_VAR_EXISTS(@Specification,@VAR_Type,@IsVariable,@Para_Type)=0
			BEGIN
				SET @ERRMSG = 'Cannot find variable. Specification:' + @Specification + ', Variable Type:' + @VAR_Type + '.'  + CHAR(13) + CHAR(10);;
				SET @ERRMSG = @ERRMSG + '@/' + @SAID + '/' + @PARTSNAME + '/' + @SPEC + CHAR(13) + CHAR(10);
				THROW 60001,@ERRMSG,1
			END


			INSERT INTO SA_Component
			(SA_ID,Parts_Name,Specification,LHS,RHS,PCE,IsVariable,
			VAR_Type,IsStandard,IsSubSA,Para_Type,Process_ID,Revision,Created_Time)
			VALUES 
			(@SA_ID,@Parts_Name,@Specification,@LHS_INT,@RHS_INT,@PCE_INT,@IsVariable,
			@VAR_Type,@IsStandard,@IsSubSA,@Para_Type,@Process_ID,@NEW_REVISION,@CREATED_TIME);

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
