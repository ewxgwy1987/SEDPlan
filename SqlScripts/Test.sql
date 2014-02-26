
-------------------------------Test for stp_ImportSCData
DECLARE @SAID NVARCHAR(100)='SA-Test1';
DECLARE @PARTSNAME NVARCHAR(100)='SIDE PANEL';
DECLARE @SPEC NVARCHAR(100)='IS1-57-004';
DECLARE @LHS NVARCHAR(100)='1';
DECLARE @RHS NVARCHAR(100)='1';
DECLARE @PCE NVARCHAR(100)='';
DECLARE @VAR NVARCHAR(100)='L';
DECLARE @WEIGHT NVARCHAR(100)='-56461';
DECLARE @PROCESS NVARCHAR(100)='FOR FABRICATION';
DECLARE @REVISION NVARCHAR(100)='3';
DECLARE @CRTIME NVARCHAR(100)=GETDATE();
EXEC stp_ImportSCData @SAID,@PARTSNAME,@SPEC,@LHS,@RHS,@PCE,@VAR,@WEIGHT,@PROCESS,@REVISION,@CRTIME;

SELECT * FROM SA_Component;

select sa_id from SA_Component where SA_ID like'b%';
select SA_ID from SA_Component where SA_ID like's%'

-------------------------------Test for stp_ImportSVData
DECLARE @SAID NVARCHAR(100)='SA-Test1';
DECLARE @L_para NVARCHAR(100)='';
DECLARE @LS_para NVARCHAR(100)='';
DECLARE @A_para NVARCHAR(100)='24';
DECLARE @Weight NVARCHAR(100)='23';
DECLARE @Revision NVARCHAR(100)='0';
DECLARE @CRTIME NVARCHAR(100)=GETDATE();

EXEC stp_ImportSVData @SAID,@L_para,@LS_para,@A_para,@Weight,@Revision,@CRTIME;
EXEC UPDATE_SA_VAR_CURRENT;

SELECT * FROM SA_Variable_Map;
SELECT * FROM SA_VAR_CURRENT;
select max(Revision) as MAXREV from SA_Variable_Map where SA_ID='SA-Test1';

-------------------------------Test for stp_ImportBPData
DECLARE @PlanName NVARCHAR(100)='TEST-BP1';
DECLARE @SAID NVARCHAR(100)='B21-60-001';
DECLARE @Quantity NVARCHAR(100)='100'
DECLARE @PARAS NVARCHAR(100) = ',,,,';--'L,LS,A,TB,H'
--DECLARE @L_para NVARCHAR(100)='';
--DECLARE @LS_para NVARCHAR(100)='';
--DECLARE @A_para NVARCHAR(100)='24';
DECLARE @Revision NVARCHAR(100)='0';
DECLARE @CRTIME NVARCHAR(100)=GETDATE();

EXEC stp_ImportBPData2 @PlanName,@SAID,@Quantity,@PARAS,@Revision,@CRTIME;
--EXEC stp_ImportBPData @PlanName,@SAID,@Quantity,@L_para,@LS_para,@A_para,@Revision,@CRTIME;

SELECT * FROM BOM_Plan;
DELETE FROM BOM_PLAN;

SELECT dbo.FUN_VAR_EXISTS('IS1-60-001','L','10');

-------------------------------Test for stp_ImportSTDData

DECLARE @STDImportName NVARCHAR(100)='STD-TEST';
DECLARE @PartsName NVARCHAR(100)='TEST1'
DECLARE @Spec NVARCHAR(100)='SFTMH-23T Ø1-7/16"';
DECLARE @STDWeight NVARCHAR(100)='22';
DECLARE @Revision NVARCHAR(100)='0';
DECLARE @CRTIME NVARCHAR(100)=GETDATE();

EXEC stp_ImportSTDData @PartsName,@Spec,@STDWeight,@Revision,@CRTIME;

SELECT * FROM STD_Parts;
-----------------------------------------------------------------------------------------------------

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