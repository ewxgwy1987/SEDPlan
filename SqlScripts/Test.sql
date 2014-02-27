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

EXEC stp_ImportBPData2 @PlanName,@SAID,@Quantity,@PARAS,@Revision,@CRTIME;
--EXEC stp_ImportBPData @PlanName,@SAID,@Quantity,@L_para,@LS_para,@A_para,@Revision,@CRTIME;

SELECT * FROM BOM_Plan;
DELETE FROM BOM_PLAN;

SELECT dbo.FUN_VAR_EXISTS('IS1-60-001','L','10');


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
------------------------------------Check if BP var type is valid-------------------------------------

SELECT

-----------------------------------------------------------------------------------------------------
SELECT * FROM BOM_PLAN;
select * from SA_Process;

select max(Revision) as MAXREV from SA_Component where SA_ID='IS1-30-001';

TRUNCATE TABLE SA_Component;
TRUNCATE TABLE SA_Variable_Map;
TRUNCATE TABLE STD_Parts;
TRUNCATE TABLE BOM_Plan;
drop database SEDPLAN;


SELECT * INTO #TEST
FROM DBO.RPT_GETPARAMETERS('10,20,,TS,,') 

SELECT * FROM #TEST;
DROP TABLE #TEST;

