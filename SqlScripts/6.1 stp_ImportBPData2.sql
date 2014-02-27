GO
USE [SEDPLAN];
GO

ALTER PROCEDURE dbo.stp_ImportBPData2
		  @PlanName NVARCHAR(100),
		  @SAID NVARCHAR(100),
		  @Quantity NVARCHAR(100),
		  @VarNames NVARCHAR(MAX),
		  @Vars NVARCHAR(MAX),
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

		DECLARE @PARA_NAME NVARCHAR(5);
		DECLARE @PARA NVARCHAR(50);
		DECLARE @VARLIST TABLE ( VARTYPE NVARCHAR(5), VARVALUE NVARCHAR(50));

		--SELECT * INTO #PARANAMES
		--FROM DBO.RPT_GETPARAMETERS('L,LS,A,TB,H');
		SELECT * INTO #PARANAMES
		FROM DBO.RPT_GETPARAMETERS(@VarNames)

		SELECT * INTO #PARAS
		FROM DBO.RPT_GETPARAMETERS(@Vars);

		DECLARE PARANAME_CURSOR CURSOR FOR SELECT PAR FROM #PARANAMES;
		OPEN PARANAME_CURSOR;

		DECLARE PARA_CURSOR CURSOR FOR SELECT PAR FROM #PARAS;
		OPEN PARA_CURSOR
		
		FETCH NEXT FROM PARANAME_CURSOR INTO @PARA_NAME;
		FETCH NEXT FROM PARA_CURSOR INTO @PARA;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @PARA != ''
			BEGIN
				SET @VAR_Type = @PARA_NAME;
				SET @VAR_Value = @PARA;
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

				INSERT INTO @VARLIST VALUES(@VAR_Type,@VAR_Value);
			END

			FETCH NEXT FROM PARANAME_CURSOR INTO @PARA_NAME;
			FETCH NEXT FROM PARA_CURSOR INTO @PARA;
			
		END

		-- If there is not variable for this SA, then set var type to be 'F'
		IF @MARK = 0
		BEGIN
			SET @IsMain = 1;
			SET @VAR_Type = 'F';
			SET @VAR_Value = '-';
			INSERT INTO BOM_Plan(Plan_Name,SA_ID,VAR_Type,VAR_Value,Quantity,IsMain,Revision,Created_Time)
			VALUES(@Plan_Name,@SA_ID,@VAR_Type,@VAR_Value,@Qty,@IsMain,@NewRevision,@Created_Time);
		END

		-- Check all variables and parameters for this SA ID are imported
		DECLARE @ERRMSG NVARCHAR(100);
		DECLARE @ISCOMP BIT;
		DECLARE @MAX_REV INT = (SELECT MAX(Revision) FROM SA_Component SC2 WHERE SC2.SA_ID=@SA_ID);

		SELECT DISTINCT SA_ID,VAR_Type
		INTO #TEMP_ALLVARTYPE
		FROM(
			SELECT SC.SA_ID,SC.VAR_Type
			FROM SA_Component SC
			WHERE SC.SA_ID = @SA_ID
				AND SC.IsVariable = 1
				AND SC.Revision=@MAX_REV
			UNION ALL
			SELECT SC.SA_ID,SC.Para_Type AS VAR_Type
			FROM SA_Component SC
			WHERE SC.SA_ID = @SA_ID
				AND SC.Para_Type != ''
				AND SC.Revision=@MAX_REV
		) AS ALLTYPES

		IF EXISTS
		(
			SELECT AVT.SA_ID,AVT.VAR_Type
			FROM #TEMP_ALLVARTYPE AVT
			WHERE AVT.VAR_Type NOT IN (SELECT VARTYPE FROM @VARLIST)
		)
		BEGIN
			SET @ERRMSG = 'The parameters are not complete. Please Check. Plan Name:'+@Plan_Name+', SA ID:'+@SA_ID+char(13)+char(10);
			THROW 60001,@ERRMSG,1
		END

		-- Check all var type and value are valid except 'TB,H'
		DECLARE @VARTYPE NVARCHAR(5);
		DECLARE @VARVALUE NVARCHAR(50);
		DECLARE VAR_CURSOR CURSOR FOR SELECT VARTYPE,VARVALUE FROM @VARLIST;
		OPEN VAR_CURSOR;

		FETCH NEXT FROM VAR_CURSOR INTO @VARTYPE,@VARVALUE;
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF @VARTYPE != 'TB' AND @VARTYPE != 'H'
			BEGIN
				IF
				NOT EXISTS(
							--If variable does not has para
							SELECT SC.SA_ID,DDV.DD_ID,DDV.VAR_Type,DDV.VAR_Value,DDV.VAR_Weight
							FROM SA_Component SC, DD_Variable_Map DDV
							WHERE SC.SA_ID=@SA_ID AND SC.VAR_Type=@VARTYPE AND SC.IsVariable=1 AND SC.Para_Type=''
							AND SC.Revision = (SELECT MAX(Revision) FROM SA_Component SC2 WHERE SC2.SA_ID=@SA_ID)
							AND SC.Specification=DDV.DD_ID AND DDV.VAR_Type=SC.VAR_Type AND DDV.VAR_Value=@VARVALUE
							AND DDV.Revision = (SELECT MAX(Revision) FROM DD_Variable_Map DDV2 WHERE DDV2.DD_ID=DDV.DD_ID)
						  ) AND 
				NOT EXISTS(
							-- If variable has para
							SELECT SC.SA_ID,DDV.DD_ID,DDV.VAR_Type,DDV.VAR_Value,DDV.VAR_Weight
							FROM SA_Component SC, DD_TYPES DDT, DD_Variable_Map DDV,@VARLIST VL
							WHERE SC.SA_ID=@SA_ID AND SC.VAR_Type=@VARTYPE AND SC.IsVariable=1 AND SC.Para_Type!=''
							AND SC.Revision = (SELECT MAX(Revision) FROM SA_Component SC2 WHERE SC2.SA_ID=@SA_ID)
							AND SC.Para_Type=VL.VARTYPE
							AND SC.SA_ID=DDT.SA_ID AND SC.Parts_Name=DDT.Parts_Name AND SC.Para_Type=DDT.Para_Type AND DDT.Para_Value=VL.VARVALUE
							AND DDT.Revision = (SELECT MAX(Revision) FROM DD_TYPES DDT2 WHERE DDT2.SA_ID=DDT.SA_ID)
							AND DDT.DD_ID=DDV.DD_ID AND DDV.VAR_Type=@VARTYPE AND DDV.VAR_Value=@VARVALUE
							AND DDV.Revision = (SELECT MAX(Revision) FROM DD_Variable_Map DDV2 WHERE DDV2.DD_ID=DDV.DD_ID)
						  )
				BEGIN
					SET @ERRMSG = 'The parameter value is not VALID. Please Check. SA ID:'+@SA_ID + ', VAR TYPE:' + @VARTYPE + ', VAR VALUE:' + @VARVALUE + char(13)+char(10);
					THROW 60001,@ERRMSG,1
				END
			END
			FETCH NEXT FROM VAR_CURSOR INTO @VARTYPE,@VARVALUE;
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