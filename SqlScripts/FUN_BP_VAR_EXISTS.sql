GO
USE [SEDPLAN];
GO

ALTER FUNCTION DBO.FUN_BP_VAR_EXISTS(
	@SA_ID NVARCHAR(50),
	@VAR_TYPE NVARCHAR(5),
	@VAR_Value NVARCHAR(50)
)
RETURNS BIT
AS
BEGIN

	DECLARE @ISEXISTS BIT;
	DECLARE @ERRMSG NVARCHAR(200);

	--DECLARE @SA_ID NVARCHAR(50)='MS01-10-001'
	--DECLARE @VAR_TYPE NVARCHAR(5)='BR'
	--DECLARE @VAR_Value NVARCHAR(50)='EC'

	IF EXISTS (SELECT Para_Name FROM SA_Paras WHERE Para_Name=@VAR_TYPE) OR @VAR_TYPE='LR' OR @VAR_TYPE='EW'
	BEGIN
		--If it is normal parameter, then check whether the parts in the @SA_ID with this parameter and its value exist in DD_TYPES or not
		IF @VAR_TYPE!='EW'
		BEGIN
			IF EXISTS
			(
				SELECT DD_ID
				FROM vw_DD_TYPES DDT,vw_SA_Component SC
				WHERE SC.SA_ID=@SA_ID AND SC.Para_Type=@VAR_TYPE
				--AND SC.Revision=(SELECT MAX(Revision) FROM SA_Component SC2 WHERE SC2.SA_ID=@SA_ID)
				AND SC.Parts_Name=DDT.Parts_Name AND DDT.SA_ID = SC.SA_ID
				AND DDT.Para_Type=@VAR_TYPE AND DDT.Para_Value=@VAR_Value
				--AND DDT.Revision=(SELECT MAX(Revision) FROM DD_TYPES DDT2 WHERE DDT2.SA_ID=@SA_ID)
			) OR @VAR_TYPE='LR' 
				SET @ISEXISTS=1;
			ELSE
			BEGIN
				SET @ISEXISTS=0;
			END
		END
		--If it is 'EW' parameter, then check if the parts in the subSA of @SA_ID with this parameter and its value exist in DD_TYPES or not
		ELSE
		BEGIN
			IF EXISTS
			(
				SELECT DD_ID
				FROM vw_DD_TYPES DDT,vw_SA_Component SC
				WHERE SC.SA_ID IN (SELECT SC2.Specification FROM vw_SA_Component SC2 WHERE SC2.Para_Type='EW' AND SC2.SA_ID=@SA_ID)
				AND SC.Para_Type=@VAR_TYPE
				--AND SC.Revision=(SELECT MAX(Revision) FROM SA_Component SC2 WHERE SC2.SA_ID=@SA_ID)
				AND SC.Parts_Name=DDT.Parts_Name AND DDT.SA_ID = SC.SA_ID
				AND DDT.Para_Type=@VAR_TYPE AND DDT.Para_Value=@VAR_Value
				--AND DDT.Revision=(SELECT MAX(Revision) FROM DD_TYPES DDT2 WHERE DDT2.SA_ID=@SA_ID)
			) 
				SET @ISEXISTS=1;
			ELSE
			BEGIN
				SET @ISEXISTS=0;
			END

		END
	END
	ELSE
	BEGIN
		IF EXISTS
		(
			SELECT SA_ID
			FROM vw_SA_Component SC
			WHERE SC.SA_ID=@SA_ID AND SC.VAR_Type=@VAR_TYPE AND SC.IsVariable=1
		) 
			SET @ISEXISTS=1;
		ELSE
			SET @ISEXISTS=0;
	END

	--PRINT @ISEXISTS;

	RETURN @ISEXISTS;

END

