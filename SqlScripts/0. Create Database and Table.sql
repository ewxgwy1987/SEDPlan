CREATE DATABASE SEDPLAN;
GO
USE SEDPLAN;
GO
--drop table SA_Process
CREATE TABLE SA_Process
(
	Process_ID BIGINT,
	Process_Name NVARCHAR(100),
	CONSTRAINT PK_SA_Process PRIMARY KEY (Process_ID)
);

INSERT INTO SA_Process VALUES (1001,'FOR PURCHASING');
INSERT INTO SA_Process VALUES (1002,'FOR MACHINING');
INSERT INTO SA_Process VALUES (1003,'FOR FABRICATION');
INSERT INTO SA_Process VALUES (1004,'FOR ASSEMBLY');
INSERT INTO SA_Process VALUES (1005,'FOR SITE JOINT');

--drop table SA_Variable_Map
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

--DROP TABLE SA_Component;
CREATE TABLE SA_Component
(
	SA_ID NVARCHAR(50),
	Parts_Name NVARCHAR(100),
	Specification NVARCHAR(100),
	LHS INT,
	RHS INT,
	PCE INT,
	IsVariable BIT, -- Inidcate whether this part has a variable
	VAR_Type NVARCHAR(5),--The type of variable: A for angle degree, L for length, LS for length ??
	IsStandard BIT, -- Inidate whether this part is a standard part
	Para_Type NVARCHAR(5), -- The type of parameter: TB and H
	Process_ID BIGINT,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_SA_Component PRIMARY KEY (SA_ID, Parts_Name,Specification,Process_ID,Revision),
	--CONSTRAINT FK_SA_Component FOREIGN KEY (Process_ID) REFERENCES SA_Process(Process_ID),
	CONSTRAINT CHK_SA_Component_PROCESS CHECK (DBO.FUN_SC_PROCESS_EXISTS(Process_ID)=CAST(1 AS BIT)),
	CONSTRAINT CHK_SA_Component_VAR CHECK (DBO.FUN_SC_VAR_EXISTS(Specification,VAR_Type,IsStandard,Para_Type)=CAST(1 AS BIT)),
	CONSTRAINT CHK_SA_Component_STD CHECK (DBO.FUN_SC_STD_EXISTS(Parts_Name,Specification,IsStandard)=CAST(1 AS BIT)),
	CONSTRAINT CHK_SA_Component_PARA CHECK (DBO.FUN_SC_PARA_EXISTS(SA_ID,Parts_Name,Para_Type)=CAST(1 AS BIT)),
);



--DROP TABLE BOM_Plan;
CREATE TABLE BOM_Plan
(
	Plan_Name NVARCHAR(100),
	SA_ID NVARCHAR(50),
	VAR_Type NVARCHAR(5),
	VAR_Value NVARCHAR(50),
	Quantity int,
	IsMain bit, -- Inidate whether the variable of this SA is a primary variable
	SAUID BIGINT,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_BOM_Plan PRIMARY KEY (Plan_Name, SA_ID, VAR_Type, VAR_Value,Revision),
	--CONSTRAINT FK_BOM_Plan FOREIGN KEY (SA_ID,VAR_TYPE,VAR_VALUE) REFERENCES SA_VAR_CURRENT(SA_ID,VAR_TYPE,VAR_VALUE),
	CONSTRAINT CHK_BOM_Plan_SAID CHECK (DBO.FUN_BP_SAID_EXISTS(SA_ID)=CAST(1 AS BIT)),
	CONSTRAINT CHK_BOM_Plan_VAR CHECK (DBO.FUN_BP_VAR_EXISTS(SA_ID,VAR_Type,VAR_Value)=CAST(1 AS BIT)),
);

--DROP TABLE BOM_Detail;
CREATE TABLE BOM_Detail
(
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
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_BOM_Detail PRIMARY KEY (Plan_Name, Parts_Name,Specification,VAR_TYPE,VAR_VALUE,Process_ID,Revision),
	CONSTRAINT FK_BOM_Detail FOREIGN KEY (Process_ID) REFERENCES SA_Process(Process_ID),
);

DROP TABLE SA_Process;
DROP TABLE DD_Variable_Map;
DROP TABLE STD_Parts;
DROP TABLE SA_Component;
DROP TABLE DD_TYPES;
DROP TABLE BOM_Plan;
DROP TABLE BOM_Detail;

DROP DATABASE SEDPLAN;
