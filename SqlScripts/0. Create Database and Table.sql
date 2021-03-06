CREATE DATABASE SEDPLAN;
GO
USE SEDPLAN;
GO

DROP TABLE SA_INFO;
CREATE TABLE SA_INFO
(
	SA_ID NVARCHAR(50),
	SA_DESCRIPTION NVARCHAR(100),
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_SA_INFO PRIMARY KEY (SA_ID,Revision)
)

DROP TABLE DD_COLOR;
CREATE TABLE DD_COLOR
(
	DD_ID NVARCHAR(50),
	COLOR NVARCHAR(50),
	CONSTRAINT PK_DD_COLOR PRIMARY KEY (DD_ID),
)

drop table DD_Variable_Map
CREATE TABLE DD_Variable_Map
(
	DD_ID NVARCHAR(50),
	--The type of variable: A for angle degree, L for length, LS for length ??, F for Fix
	VAR_Type NVARCHAR(5),
	VAR_Value NVARCHAR(50),
	VAR_Weight FLOAT,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_SA_Variable_Map PRIMARY KEY (DD_ID,VAR_TYPE,VAR_VALUE,Revision)
);

DROP TABLE STD_Parts;
CREATE TABLE STD_Parts
(
	Parts_Name NVARCHAR(100),
	Specification NVARCHAR(100),
	STD_Weight FLOAT,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_STD_Parts PRIMARY KEY (Parts_Name, Specification,Revision)
);

DROP TABLE DD_TYPES;
CREATE TABLE DD_TYPES
(
	SA_ID NVARCHAR(50),
	Parts_Name NVARCHAR(100),
	Para_Type NVARCHAR(5),
	Para_Value NVARCHAR(50),
	DD_ID NVARCHAR(50),
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_DD_TYPES PRIMARY KEY (SA_ID, Parts_Name, Para_Type, Para_Value, Revision)
);

DROP TABLE SA_Component;
CREATE TABLE SA_Component
(
	SA_ID NVARCHAR(50),
	Parts_Name NVARCHAR(100),
	Specification NVARCHAR(100),
	LHS INT,
	RHS INT,
	PCE INT,
	IsVariable BIT, -- Indicate whether this part has a variable
	VAR_Type NVARCHAR(5),--The type of variable: A for angle degree, L for length, LS for length ??
	IsStandard BIT, -- Indicate whether this part is a standard part
	IsSubSA BIT, -- Indicate whether this part is a sub SA component - Added by Guo Wenyu 2014/06/18
	Para_Type NVARCHAR(5), -- The type of parameter: TB and H
	Process_ID BIGINT,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_SA_Component PRIMARY KEY (SA_ID, Parts_Name,Specification,Process_ID,Revision),
	CONSTRAINT FK_SA_Component FOREIGN KEY (Process_ID) REFERENCES SA_Process(Process_ID),
	CONSTRAINT CHK_SA_Component_PROCESS CHECK (DBO.FUN_SC_PROCESS_EXISTS(Process_ID)=CAST(1 AS BIT)),
	CONSTRAINT CHK_SA_Component_VAR CHECK (DBO.FUN_SC_VAR_EXISTS(Specification,VAR_Type,IsVariable,Para_Type)=CAST(1 AS BIT)),
	CONSTRAINT CHK_SA_Component_STD CHECK (DBO.FUN_SC_STD_EXISTS(Parts_Name,Specification,IsStandard)=CAST(1 AS BIT)),
	CONSTRAINT CHK_SA_Component_PARA CHECK (DBO.FUN_SC_PARA_EXISTS(SA_ID,Parts_Name,Para_Type,IsSubSA)=CAST(1 AS BIT)),
);


DROP TABLE BOM_Plan;
CREATE TABLE BOM_Plan
(
	Project_No NVARCHAR(20),
	Plan_Name NVARCHAR(100),
	SA_ID NVARCHAR(50),
	VAR_Type NVARCHAR(5),
	VAR_Value NVARCHAR(50),
	Quantity int,
	COLOR NVARCHAR(50),
	IsMain bit, -- Inidate whether the variable of this SA is a primary variable
	SAUID BIGINT,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_BOM_Plan PRIMARY KEY (Project_No, Plan_Name, SA_ID, VAR_Type, VAR_Value,COLOR,Revision),
	CONSTRAINT FK_BOM_Plan FOREIGN KEY (Project_No) REFERENCES ProjectInfo(ProjectNo),
	--CONSTRAINT CHK_BOM_Plan_SAID CHECK (DBO.FUN_BP_SAID_EXISTS(SA_ID)=CAST(1 AS BIT)),
	--CONSTRAINT CHK_BOM_Plan_VAR CHECK (DBO.FUN_BP_VAR_EXISTS(SA_ID,VAR_Type,VAR_Value)=CAST(1 AS BIT)),
);

DROP TABLE BOM_Detail;
CREATE TABLE BOM_Detail
(
	Project_No NVARCHAR(20),
	Plan_Name NVARCHAR(100),
	Parts_Name NVARCHAR(100),
	Specification NVARCHAR(100),
	VAR_TYPE NVARCHAR(5),
	VAR_VALUE NVARCHAR(50),
	LHS INT,
	RHS INT,
	PCE INT,
	Total_Weight FLOAT,
	Process_ID bigint,
	COLOR NVARCHAR(50),
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_BOM_Detail PRIMARY KEY (Project_No, Plan_Name, Parts_Name,Specification,VAR_TYPE,VAR_VALUE,Process_ID,COLOR,Revision),
	CONSTRAINT FK_BOM_Detail_PRSSID FOREIGN KEY (Process_ID) REFERENCES SA_Process(Process_ID),
	CONSTRAINT FK_BOM_Detail_PROJNO FOREIGN KEY (Project_No) REFERENCES ProjectInfo(ProjectNo),
);

drop table ProjectInfo
CREATE TABLE ProjectInfo
(
	ProjectNo NVARCHAR(20),
	ProjectName NVARCHAR(200),
	CONSTRAINT PK_ProjectInfo PRIMARY KEY (ProjectNo)
);
INSERT INTO ProjectInfo VALUES ('S32A1305700','Oklahoma International Airport, US');
INSERT INTO ProjectInfo VALUES ('PTERIS11111','TEST FOR TEST, SG');

drop table SA_Process
CREATE TABLE SA_Process
(
	Process_ID BIGINT,
	Process_Name NVARCHAR(100),
	CONSTRAINT PK_SA_Process PRIMARY KEY (Process_ID)
);

INSERT INTO SA_Process VALUES (1001,'PURCHASING');
INSERT INTO SA_Process VALUES (1002,'MACHINING');
INSERT INTO SA_Process VALUES (1003,'FABRICATION');
INSERT INTO SA_Process VALUES (1004,'ASSEMBLY');
INSERT INTO SA_Process VALUES (1005,'SITE JOINT FASTENER');
INSERT INTO SA_Process VALUES (1006,'PROCUREMENT');
INSERT INTO SA_Process VALUES (1007,'IN-HOUSE FASTENER');

drop table SA_Paras
CREATE TABLE SA_Paras
(
	Para_Name NVARCHAR(100),
	CONSTRAINT PK_SA_Paras PRIMARY KEY (Para_Name)
);

INSERT INTO SA_Paras VALUES ('TB');
INSERT INTO SA_Paras VALUES ('H');
INSERT INTO SA_Paras VALUES ('BR');

CREATE TABLE [dbo].[PICTURES](
	[PIC_NAME] [varchar](20) NOT NULL,
	[PIC_TITLE] [varchar](100) NOT NULL,
	[PIC_DESC] [nvarchar](100) NULL,
	[PIC_IMAGE] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_PICTURES] PRIMARY KEY CLUSTERED 
(
	[PIC_NAME] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

--Views for SEDPlan
GO
CREATE VIEW vw_BOM_Detail AS
	SELECT	* 
	FROM	BOM_Detail BD
	WHERE	BD.Revision=(SELECT MAX(Revision) FROM BOM_Detail BD2 WHERE BD2.Plan_Name=BD.Plan_Name AND BD2.Project_No=BD.Project_No)
GO
CREATE VIEW vw_BOM_Plan AS
	SELECT	* 
	FROM	BOM_Plan BP
	WHERE	BP.Revision=(SELECT MAX(Revision) FROM BOM_Plan BP2 WHERE BP2.Plan_Name=BP.Plan_Name AND BP2.Project_No=BP.Project_No);
GO
CREATE VIEW vw_DD_TYPES AS
	SELECT	* 
	FROM	DD_TYPES DDT
	WHERE	DDT.Revision=(SELECT MAX(Revision) FROM DD_TYPES DDT2 WHERE DDT2.SA_ID=DDT.SA_ID);
GO
CREATE VIEW vw_SA_Component AS
	SELECT	* 
	FROM	SA_Component SC
	WHERE	SC.Revision=(SELECT MAX(Revision) FROM SA_Component SC2 WHERE SC2.SA_ID=SC.SA_ID);
GO
CREATE VIEW vw_STD_Parts AS
	SELECT	* 
	FROM	STD_Parts STD
	WHERE	STD.Revision=(SELECT MAX(Revision) FROM STD_Parts STD2 WHERE STD.Parts_Name=STD2.Specification AND STD.Specification=STD2.Specification);
GO
CREATE VIEW vw_Fixed_Weight AS
	SELECT	* 
	FROM	DD_Variable_Map DDV
	WHERE	DDV.VAR_Type='F'
		AND DDV.Revision=(SELECT MAX(Revision) FROM DD_Variable_Map DDV2 WHERE DDV2.DD_ID=DDV.DD_ID AND DDV2.VAR_Type='F');
GO
CREATE VIEW vw_DD_Variable_Map AS
	SELECT	* 
	FROM	DD_Variable_Map DDV
	WHERE	DDV.VAR_Type!='F'
		AND DDV.Revision=(SELECT MAX(Revision) FROM DD_Variable_Map DDV2 WHERE DDV2.DD_ID=DDV.DD_ID);
GO
CREATE VIEW vw_SA_INFO AS
	SELECT	* 
	FROM	SA_INFO SI
	WHERE	SI.Revision=(SELECT MAX(Revision) FROM SA_INFO SI2 WHERE SI2.SA_ID=SI.SA_ID)
GO
--DROP TABLE ProjectInfo;
--DROP TABLE SA_Process;
--DROP TABLE DD_Variable_Map;
--DROP TABLE STD_Parts;
--DROP TABLE SA_Component;
--DROP TABLE DD_TYPES;
--DROP TABLE BOM_Plan;
--DROP TABLE BOM_Detail;
--DROP TABLE DD_COLOR
--DROP TABLE SA_INFO

SELECT * FROM BOM_Detail;
SELECT * FROM BOM_Plan;
SELECT * FROM DD_COLOR
SELECT * FROM DD_TYPES
SELECT * FROM DD_Variable_Map
SELECT * FROM SA_Component
SELECT * FROM SA_INFO
SELECT * FROM STD_Parts

SELECT * FROM PICTURES
SELECT * FROM ProjectInfo
SELECT * FROM SA_Process

TRUNCATE TABLE BOM_Detail;
TRUNCATE TABLE BOM_Plan;
TRUNCATE TABLE DD_COLOR
TRUNCATE TABLE DD_TYPES
TRUNCATE TABLE DD_Variable_Map
TRUNCATE TABLE SA_Component
TRUNCATE TABLE SA_INFO
TRUNCATE TABLE STD_Parts
DELETE FROM SA_Process

DROP DATABASE SEDPLAN;

-- Report: Fabrication List 
DECLARE @BOMPlan NVARCHAR(100)='FAB01-B3-02';
DECLARE @Project NVARCHAR(50)='S32A1305700';
select	BD.Parts_Name,BD.Specification,BD.VAR_TYPE,BD.VAR_VALUE
		,SUM(BD.LHS) AS LHS,SUM(BD.RHS) AS RHS,SUM(BD.PCE) PCE,SUM(BD.Total_Weight) AS Total_Weight
		,SP.Process_Name,BD.COLOR
from	BOM_Detail BD, SA_Process SP
WHERE	BD.Project_No=@Project 
	AND BD.Plan_Name IN (@BOMPlan) 
	AND BD.Process_ID IN (@ProcessName)
	AND BD.Process_ID=SP.Process_ID 
	AND BD.Revision=(SELECT MAX(Revision) FROM BOM_Detail BD2 WHERE BD2.Plan_Name=BD.Plan_Name AND BD2.Project_No=BD.Project_No)
GROUP BY BD.Parts_Name,BD.Specification,BD.VAR_TYPE,BD.VAR_VALUE,SP.Process_Name,BD.COLOR;

-- Report: Sub Assembly List


--SELECT *

--SELECT SI.SA_DESCRIPTION,ALLBP.SA_ID,L_VAL,LS_VAL,A_VAL,TB_VAL,H_VAL,SUM(QUANTITY)AS QUANTITY,COLOR 
--FROM(
--	SELECT 
--	BP.SA_ID,
--	BP_L.VAR_VALUE AS L_VAL,
--	BP_LS.VAR_VALUE AS LS_VAL,
--	BP_A.VAR_VALUE AS A_VAL,
--	BP_TB.VAR_VALUE AS TB_VAL, 
--	BP_H.VAR_VALUE AS H_VAL,
--	BP.QUANTITY,
--	BP.COLOR
--	FROM BOM_Plan BP
--	LEFT JOIN BOM_PLAN BP_L ON BP.SAUID=BP_L.SAUID AND BP.Revision=BP_L.Revision AND BP_L.VAR_TYPE='L'
--	LEFT JOIN BOM_PLAN BP_LS ON BP.SAUID=BP_LS.SAUID AND BP.Revision=BP_LS.Revision AND BP_LS.VAR_TYPE='LS'
--	LEFT JOIN BOM_PLAN BP_A ON BP.SAUID=BP_A.SAUID AND BP.Revision=BP_A.Revision AND BP_A.VAR_TYPE='A��'
--	LEFT JOIN BOM_PLAN BP_H ON BP.SAUID=BP_H.SAUID AND BP.Revision=BP_H.Revision AND BP_H.VAR_TYPE='H'
--	LEFT JOIN BOM_PLAN BP_TB ON BP.SAUID=BP_TB.SAUID AND BP.Revision=BP_TB.Revision AND BP_TB.VAR_TYPE='TB'
--	WHERE BP.Project_No=@Project AND BP.Plan_Name IN (@FabricationList) AND BP.ISMAIN=1
--	AND BP.Revision=(SELECT MAX(Revision) FROM BOM_Plan BP2 WHERE BP2.Plan_Name=BP.Plan_Name AND BP2.Project_No=BP.Project_No)
--) AS ALLBP
--LEFT JOIN SA_INFO SI 
--	ON ALLBP.SA_ID=SI.SA_ID
--	AND SI.Revision=(SELECT MAX(Revision) FROM SA_INFO SI2 WHERE SI2.SA_ID=SI.SA_ID)
--GROUP BY SI.SA_DESCRIPTION,ALLBP.SA_ID,L_VAL,LS_VAL,A_VAL,TB_VAL,H_VAL,COLOR;

  
