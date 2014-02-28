GO
USE [SEDPLAN];
GO
ALTER PROCEDURE dbo.stp_CalBOMPlan
		  @PlanName NVARCHAR(100)
AS
BEGIN
	--DECLARE @PlanName NVARCHAR(100)='BOMPlanTest';
	--Prepare temp data
	SELECT * INTO #TMP_DDV
	FROM DD_Variable_Map DDV
	WHERE DDV.Revision=(SELECT MAX(Revision) FROM DD_Variable_Map DDV2 WHERE DDV2.DD_ID=DDV.DD_ID);

	SELECT * INTO #TMP_STD
	FROM STD_Parts STD
	WHERE STD.Revision=(SELECT MAX(Revision) FROM STD_Parts STD2);

	SELECT * INTO #TMP_DDT
	FROM DD_TYPES DDT
	WHERE DDT.Revision=(SELECT MAX(Revision) FROM DD_TYPES DDT2 WHERE DDT2.SA_ID=DDT.SA_ID);

	SELECT * INTO #TMP_SC
	FROM SA_Component SC
	WHERE SC.Revision=(SELECT MAX(Revision) FROM SA_Component SC2 WHERE SC2.SA_ID=SC.SA_ID);

	SELECT * INTO #TMP_BP
	FROM BOM_Plan BP
	WHERE BP.Plan_Name=@PlanName
	AND BP.Revision=(SELECT MAX(Revision) FROM BOM_Plan BP2 WHERE BP2.Plan_Name=BP.Plan_Name);

	CREATE TABLE #TMP_CATALOGUESUM
	(
		Parts_Name NVARCHAR(100),
		Spec NVARCHAR(100),
		VAR_TYPE NVARCHAR(5),
		VAR_VALUE NVARCHAR(50),
		LHS INT,
		RHS INT,
		PCE INT,
		UNIT_WEIGHT FLOAT,
		Process_ID bigint,
	)

	-- 1. Calculate for STD parts.	(SC.IsStandard=1)
	INSERT INTO #TMP_CATALOGUESUM
	SELECT SC.Parts_Name,
	SC.Specification AS Spec,
	'' AS VAR_TYPE,
	'' AS VAR_VALUE,
	BP.Quantity*SC.LHS AS LHS,
	BP.Quantity*SC.RHS AS RHS,
	BP.Quantity*SC.PCE AS PCE,
	STD.STD_Weight AS UNIT_WEIGHT,
	SC.Process_ID
	FROM #TMP_SC SC,#TMP_BP BP, #TMP_STD STD
	WHERE BP.IsMain=1 AND SC.SA_ID=BP.SA_ID
	AND SC.IsStandard=1
	AND SC.Parts_Name=STD.Parts_Name AND SC.Specification=STD.Specification;

	-- 2. Calculate for Fixed Weight without Para types. (sc.IsVariable=0 and sc.Para_Type='' and sc.IsStandard=0 AND sc.Var_Type='F')
	INSERT INTO #TMP_CATALOGUESUM
	SELECT SC.Parts_Name,
	SC.Specification AS Spec,
	'' AS VAR_TYPE,
	'' AS VAR_VALUE,
	BP.Quantity*SC.LHS AS LHS,
	BP.Quantity*SC.RHS AS RHS,
	BP.Quantity*SC.PCE AS PCE,
	DDV.VAR_Weight AS UNIT_WEIGHT,
	SC.Process_ID
	FROM #TMP_SC SC,#TMP_BP BP, #TMP_DDV DDV
	WHERE BP.IsMain=1 AND SC.SA_ID=BP.SA_ID
	AND SC.IsVariable=0 AND SC.Para_Type='' AND SC.IsStandard=0 AND SC.VAR_Type='F'
	AND SC.VAR_Type=DDV.VAR_Type AND SC.Specification=DDV.DD_ID;
	
	-- 3. Calculate for Fixed Weight with Para type. (sc.IsVariable=0 and sc.Para_Type!='' and sc.IsStandard=0 sc.Var_Type='F')
	INSERT INTO #TMP_CATALOGUESUM
	SELECT SC.Parts_Name,
	DDV.DD_ID AS Spec,
	'' AS VAR_TYPE,
	'' AS VAR_VALUE,
	BP.Quantity*SC.LHS AS LHS,
	BP.Quantity*SC.RHS AS RHS,
	BP.Quantity*SC.PCE AS PCE,
	DDV.VAR_Weight AS UNIT_WEIGHT,
	SC.Process_ID
	FROM #TMP_SC SC,#TMP_BP BP, #TMP_DDV DDV, #TMP_DDT DDT
	WHERE SC.SA_ID=BP.SA_ID
	AND SC.IsVariable=0 AND SC.Para_Type!='' AND SC.IsStandard=0 AND SC.VAR_Type='F'
	AND BP.VAR_Type=SC.Para_Type AND BP.VAR_Value=DDT.Para_Value
	AND SC.SA_ID=DDT.SA_ID AND SC.Para_Type=DDT.Para_Type AND SC.Parts_Name=DDT.Parts_Name
	AND DDV.DD_ID=DDT.DD_ID AND DDV.VAR_Type=SC.VAR_Type;

	-- 4. Calculate for variable weight without Para types. (sc.IsVariable=1 and sc.Para_Type='')
	INSERT INTO #TMP_CATALOGUESUM
	SELECT SC.Parts_Name,
	SC.Specification AS Spec,
	DDV.VAR_Type AS VAR_TYPE,
	DDV.VAR_Value AS VAR_VALUE,
	BP.Quantity*SC.LHS AS LHS,
	BP.Quantity*SC.RHS AS RHS,
	BP.Quantity*SC.PCE AS PCE,
	DDV.VAR_Weight AS UNIT_WEIGHT,
	SC.Process_ID
	FROM #TMP_SC SC,#TMP_BP BP, #TMP_DDV DDV
	WHERE SC.SA_ID=BP.SA_ID AND SC.VAR_Type=BP.VAR_Type
	AND SC.IsVariable=1 AND SC.Para_Type=''
	AND DDV.DD_ID=SC.Specification AND DDV.VAR_Type=SC.VAR_Type AND DDV.VAR_Value=BP.VAR_Value;

	-- 5. Calculate for variable weight with Para types. (sc.IsVariable=1 and sc.Para_Type!='')
	INSERT INTO #TMP_CATALOGUESUM
	SELECT SC.Parts_Name,
	DDV.DD_ID AS Spec,
	DDV.VAR_Type AS VAR_TYPE,
	DDV.VAR_Value AS VAR_VALUE,
	BP1_VAR.Quantity*SC.LHS AS LHS,
	BP1_VAR.Quantity*SC.RHS AS RHS,
	BP1_VAR.Quantity*SC.PCE AS PCE,
	DDV.VAR_Weight AS UNIT_WEIGHT,
	SC.Process_ID
	FROM #TMP_SC SC,#TMP_BP BP1_VAR, #TMP_DDV DDV, #TMP_DDT DDT, #TMP_BP BP2_PARA
	WHERE SC.SA_ID=BP1_VAR.SA_ID AND SC.VAR_Type=BP1_VAR.VAR_Type 
	AND SC.IsVariable=1 AND SC.Para_Type!=''
	AND SC.SA_ID=BP2_PARA.SA_ID AND SC.Para_Type=BP2_PARA.VAR_Type
	AND SC.SA_ID=DDT.SA_ID AND SC.Parts_Name=DDT.Parts_Name AND SC.Para_Type=DDT.Para_Type AND DDT.Para_Value=BP2_PARA.VAR_Value
	AND DDT.DD_ID=DDV.DD_ID AND DDV.VAR_Type=SC.VAR_Type AND DDV.VAR_Value=BP1_VAR.VAR_Value;

	--DECLARE @PlanName NVARCHAR(100)='BOMPlanTest';
	DECLARE @REVERSION INT = (SELECT MAX(Revision) FROM BOM_Detail BD WHERE BD.Parts_Name=@PlanName);
	IF @REVERSION IS NULL
		SET @REVERSION=0;
	ELSE 
		SET @REVERSION = @REVERSION + 1;
	DECLARE @CRTIME DATETIME = GETDATE();

	--SELECT * FROM #TMP_CATALOGUESUM;
	INSERT INTO BOM_Detail(Plan_Name,Parts_Name,Specification,VAR_TYPE,VAR_VALUE,LHS,RHS,PCE,Total_Weight,Process_ID,Revision,Created_Time)
	SELECT @PlanName,CS.Parts_Name,CS.Spec,VAR_TYPE,VAR_VALUE,SUM(CS.LHS),SUM(CS.RHS),SUM(CS.PCE),SUM(CS.LHS+CS.RHS+CS.PCE)*SUM(CS.UNIT_WEIGHT),CS.Process_ID,@REVERSION,@CRTIME
	FROM #TMP_CATALOGUESUM CS
	GROUP BY CS.Parts_Name,CS.Spec,VAR_TYPE,VAR_VALUE,CS.Process_ID
	ORDER BY CS.Parts_Name,CS.Spec ASC;

	DROP TABLE #TMP_DDV;
	DROP TABLE #TMP_STD;
	DROP TABLE #TMP_DDT;
	DROP TABLE #TMP_SC;
	DROP TABLE #TMP_BP;
	DROP TABLE #TMP_CATALOGUESUM;
END