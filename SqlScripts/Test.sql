-------------------------------Test for stp_ImportDDVData
DECLARE @SAID NVARCHAR(100)='SA-Test1';
DECLARE @L_para NVARCHAR(100)='';
DECLARE @LS_para NVARCHAR(100)='';
DECLARE @A_para NVARCHAR(100)='24';
DECLARE @Weight NVARCHAR(100)='23';
DECLARE @Revision NVARCHAR(100)='0';
DECLARE @CRTIME NVARCHAR(100)=GETDATE();

EXEC stp_ImportDDVData @SAID,@L_para,@LS_para,@A_para,@Weight,@Revision,@CRTIME;

SELECT * FROM DD_Variable_Map;
SELECT * FROM SA_VAR_CURRENT;
select max(Revision) as MAXREV from SA_Variable_Map where SA_ID='SA-Test1';

-------------------------------Test for stp_ImportSTDData

DECLARE @STDImportName NVARCHAR(100)='STD-TEST';
DECLARE @PartsName NVARCHAR(100)='TEST1'
DECLARE @Spec NVARCHAR(100)='SFTMH-23T Ø1-7/16"';
DECLARE @STDWeight NVARCHAR(100)='22';
DECLARE @Revision NVARCHAR(100)='0';
DECLARE @CRTIME NVARCHAR(100)=GETDATE();

EXEC stp_ImportSTDData @PartsName,@Spec,@STDWeight,@Revision,@CRTIME;

SELECT * FROM STD_Parts;

-------------------------------Test for 

select * from DD_TYPES;

-------------------------------Test for stp_ImportSCData
DECLARE @SAID NVARCHAR(100)='SA-Test1';
DECLARE @PARTSNAME NVARCHAR(100)='SIDE PANEL';
DECLARE @SPEC NVARCHAR(100)='';
DECLARE @LHS NVARCHAR(100)='1';
DECLARE @RHS NVARCHAR(100)='1';
DECLARE @PCE NVARCHAR(100)='';
DECLARE @VAR NVARCHAR(100)='L';
DECLARE @PARATYPE NVARCHAR(5)='H';
DECLARE @PROCESS NVARCHAR(100)='FOR FABRICATION';
DECLARE @REVISION NVARCHAR(100)='0';
DECLARE @CRTIME NVARCHAR(100)=GETDATE();
EXEC stp_ImportSCData @SAID,@PARTSNAME,@SPEC,@LHS,@RHS,@PCE,@VAR,@PARATYPE,@PROCESS,@REVISION,@CRTIME;

DELETE FROM SA_Component
SELECT * FROM SA_Component;

INSERT INTO SA_Component(SA_ID,Parts_Name,Specification,LHS,RHS,PCE,IsVariable,
		VAR_Type,IsStandard,Para_Type,Process_ID,Revision,Created_Time)
values('SA-Test1','SIDE PANEL','SS',1,1,0,1,'L',0,'H',1003,0,GETDATE());

select sa_id from SA_Component where SA_ID like'b%';
select SA_ID from SA_Component where SA_ID like's%'

-------------------------------Test for stp_ImportBPData
DECLARE @PlanName NVARCHAR(100)='TEST-BP1';
DECLARE @SAID NVARCHAR(100)='SA-Test1';
DECLARE @Quantity NVARCHAR(100)='50'
DECLARE @PARANAMES NVARCHAR(100)='L,LS,A°,TB,H'
DECLARE @PARAS NVARCHAR(100) = '88.25,46.875,,,300';--'L,LS,A,TB,H'
DECLARE @Revision NVARCHAR(100)='1';
DECLARE @CRTIME NVARCHAR(100)=GETDATE();

EXEC stp_ImportBPData @PlanName,@SAID,@Quantity,@PARAS,@Revision,@CRTIME;
--EXEC stp_ImportBPData @PlanName,@SAID,@Quantity,@L_para,@LS_para,@A_para,@Revision,@CRTIME;

SELECT * FROM BOM_Plan;
DELETE FROM BOM_PLAN;

DECLARE @SA_ID NVARCHAR(100)='SA-Test1';
DECLARE @VARTYPE NVARCHAR(5) = 'LS';
DECLARE @VARVALUE NVARCHAR(50) = '46.875';
SELECT SC.SA_ID,DDV.DD_ID,DDV.VAR_Type,DDV.VAR_Value,DDV.VAR_Weight
FROM SA_Component SC, DD_Variable_Map DDV
WHERE SC.SA_ID=@SA_ID AND SC.VAR_Type=@VARTYPE AND SC.IsVariable=1 AND SC.Para_Type=''
AND SC.Revision = (SELECT MAX(Revision) FROM SA_Component SC2 WHERE SC2.SA_ID=@SA_ID)
AND SC.Specification=DDV.DD_ID AND DDV.VAR_Type=SC.VAR_Type AND DDV.VAR_Value=@VARVALUE
AND DDV.Revision = (SELECT MAX(Revision) FROM DD_Variable_Map DDV2 WHERE DDV2.DD_ID=DDV.DD_ID)

SELECT * FROM  DD_Variable_Map DDV WHERE DDV.DD_ID='IS1-58-002'
-----------------------------------------------------------------------------------------------------
------------------------------------Check if BP data is complete-------------------------------------
DECLARE @PLANNAME NVARCHAR(100) = '';

SELECT DISTINCT SA_ID,VAR_Type
INTO #TEMP_ALLVARTYPE
FROM(
	SELECT SC.SA_ID,SC.VAR_Type
	FROM SA_Component SC, BOM_Plan BP
	WHERE BP.Plan_Name = @PLANNAME
		AND BP.SA_ID = SC.SA_ID
		AND SC.IsVariable = 1
	UNION ALL
	SELECT SC.SA_ID,SC.Para_Type AS VAR_Type
	FROM SA_Component SC, BOM_Plan BP
	WHERE BP.Plan_Name = @PLANNAME
		AND BP.SA_ID = SC.SA_ID
		AND SC.Para_Type != ''
) AS ALLTYPES

SELECT AVT.SA_ID,AVT.VAR_Type
FROM #TEMP_ALLVARTYPE AVT
WHERE NOT EXISTS
	(	SELECT DISTINCT VAR_Type 
		FROM BOM_PLAN BP
		WHERE BP.Plan_Name = @PLANNAME
			AND BP.SA_ID = AVT.SA_ID
			AND BP.VAR_Type = AVT.VAR_Type
	)
-----------------------------------------------------------------------------------------------------
------------------------------------Statistics-------------------------------------

DECLARE @PlanName NVARCHAR(100)='BOMPlanTest'
EXEC stp_CalBOMPlan @PlanName;
--SELECT * FROM BOM_Detail;

DECLARE @REVERSION INT = (SELECT MAX(Revision) FROM BOM_Detail BD WHERE BD.Plan_Name=@PlanName);
	IF @REVERSION IS NULL
		SET @REVERSION=0;
	ELSE 
		SET @REVERSION = @REVERSION + 1;

select @REVERSION

-----------------------------------------------------------------------------------------------------
SELECT * FROM BOM_PLAN;
select * from SA_Process;

select max(Revision) as MAXREV from SA_Component where SA_ID='IS1-30-001';

TRUNCATE TABLE DD_Variable_Map;
TRUNCATE TABLE STD_Parts;
TRUNCATE TABLE SA_Component;
TRUNCATE TABLE DD_TYPES;
TRUNCATE TABLE BOM_Plan;
TRUNCATE TABLE BOM_DETAIL;


drop database SEDPLAN;


SELECT * INTO #TEST
FROM DBO.RPT_GETPARAMETERS('10,20,,TS,,') 

SELECT * FROM #TEST;
DROP TABLE #TEST;

select ROW_NUMBER() OVER(ORDER BY BD.Plan_Name,BD.Parts_Name,BD.Specification ASC) AS ITEM_NO,
BD.Parts_Name,BD.Specification,BD.VAR_TYPE,BD.VAR_VALUE,BD.LHS,BD.RHS,BD.PCE,BD.Total_Weight,SP.Process_Name
from BOM_Detail BD, SA_Process SP
WHERE BD.Process_ID=SP.Process_ID AND BD.Plan_Name=@;

select * from SA_Component sc where sc.IsVariable=1 and sc.Para_Type='' and sc.Specification='IS1-58-002'
SELECT * FROM DD_Variable_Map DDV WHERE DDV.VAR_Type='LS' AND DDV.DD_ID='IS1-58-002' AND DDV.VAR_Value='45.625';
SELECT * FROM BOM_Plan;

EXEC stp_RPT_GETDATETIMEFORMAT;

DECLARE @BOMPlan NVARCHAR(100)='BOMPlanTest';
SELECT ROW_NUMBER() OVER(ORDER BY BP.SA_ID ASC) AS ITEM_NO,
BP.SA_ID,
BP_L.VAR_VALUE AS L_VAL,
BP_LS.VAR_VALUE AS LS_VAL,
BP_A.VAR_VALUE AS A_VAL,
BP_TB.VAR_VALUE AS TB_VAL, 
BP_H.VAR_VALUE AS H_VAL,
BP.QUANTITY
FROM BOM_Plan BP
LEFT JOIN BOM_PLAN BP_L ON BP.SAUID=BP_L.SAUID AND BP.Revision=BP_L.Revision AND BP_L.VAR_TYPE='L'
LEFT JOIN BOM_PLAN BP_LS ON BP.SAUID=BP_LS.SAUID AND BP.Revision=BP_LS.Revision AND BP_LS.VAR_TYPE='LS'
LEFT JOIN BOM_PLAN BP_A ON BP.SAUID=BP_A.SAUID AND BP.Revision=BP_A.Revision AND BP_A.VAR_TYPE='A°'
LEFT JOIN BOM_PLAN BP_H ON BP.SAUID=BP_H.SAUID AND BP.Revision=BP_H.Revision AND BP_H.VAR_TYPE='H'
LEFT JOIN BOM_PLAN BP_TB ON BP.SAUID=BP_TB.SAUID AND BP.Revision=BP_TB.Revision AND BP_TB.VAR_TYPE='TB'
WHERE BP.Plan_Name=@BOMPlan AND BP.ISMAIN=1
AND BP.Revision=(SELECT MAX(Revision) FROM BOM_Plan BP2 WHERE BP2.Plan_Name=@BOMPlan);


SELECT * FROM ProjectInfo;
SELECT * FROM SA_Process;
SELECT * FROM DD_Variable_Map;
SELECT * FROM DD_Variable_Map DDV WHERE DDV.VAR_Type='F'; 
SELECT * FROM STD_Parts;
SELECT * FROM SA_Component;
SELECT * FROM DD_TYPES;
SELECT * FROM BOM_Plan;
SELECT * FROM BOM_DETAIL;
SELECT * FROM [PICTURES];
SELECT * FROM SA_INFO;
SELECT * FROM DD_COLOR;
  

SELECT * FROM DD_Variable_Map where VAR_Weight=0;


select * from PICTURES;
select distinct Revision,DD_ID FROM DD_Variable_Map where VAR_Weight=0;

select max(Revision) from BOM_Detail
SELECT * FROM BOM_Detail where Revision=4;
SELECT * FROM BOM_Plan where Revision=4;

SELECT * FROM SA_Component where Para_Type='EW'

SELECT * 
FROM BOM_Detail 
where Revision=6 and COLOR='RBL5014' 
and Parts_Name like 'CHAIN%';