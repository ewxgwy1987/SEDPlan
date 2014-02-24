CREATE DATABASE SEDPLAN;
GO
USE SEDPLAN;
GO
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

CREATE TABLE SA_Variable_Map
(
	SA_ID NVARCHAR(50),
	VAR_Type NVARCHAR(5), --The type of variable: A L LS
	VAR_Value NVARCHAR(50),
	VAR_Weight FLOAT,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_SA_Variable_Map PRIMARY KEY (SA_ID,VAR_TYPE,VAR_VALUE,Revision)
);


CREATE TABLE SA_Component
(
	SA_ID NVARCHAR(50),
	Parts_Name NVARCHAR(100),
	Specification NVARCHAR(100),
	LHS INT,
	RHS INT,
	PCE INT,
	IsVariable BIT, -- Inidcate whether this part has a variable
	VAR_Type NVARCHAR(5),
	IsStandard BIT, -- Inidate whether this part is a standard part
	Parts_Weight FLOAT,
	Process_ID BIGINT,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_SA_Component PRIMARY KEY (SA_ID, Parts_Name,Specification,Revision),
	CONSTRAINT FK_SA_Component FOREIGN KEY (Process_ID) REFERENCES SA_Process(Process_ID),
);



CREATE TABLE BOM_Plan
(
	Plan_Name NVARCHAR(100),
	SA_ID NVARCHAR(50),
	VAR_Type NVARCHAR(5),
	VAR_Value NVARCHAR(50),
	Quantity int,
	IsMain bit, -- Inidate whether the variable of this SA is a primary variable
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_BOM_Plan PRIMARY KEY (Plan_Name, SA_ID, VAR_Type, VAR_Value,Revision),
);

CREATE TABLE STD_Parts
(
	STD_ImportName NVARCHAR(100),
	Parts_Name NVARCHAR(100),
	Specification NVARCHAR(100),
	STD_Weight FLOAT,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_STD_Parts PRIMARY KEY (Parts_Name, Specification,Revision)
);


CREATE TABLE BOM_Detail
(
	Plan_Name NVARCHAR(100),
	Parts_Name NVARCHAR(100),
	Specification NVARCHAR(100),
	LHS INT,
	RHS INT,
	PCE INT,
	Total_Weight FLOAT,
	Process_ID bigint,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_BOM_Detail PRIMARY KEY (Plan_Name, Parts_Name,Specification,Revision),
	CONSTRAINT FK_BOM_Detail FOREIGN KEY (Process_ID) REFERENCES SA_Process(Process_ID),
);

DROP TABLE SA_Process;
DROP TABLE SA_Variable_Map;
DROP TABLE SA_Component;
DROP TABLE BOM_Plan;
DROP TABLE STD_Parts;
DROP TABLE BOM_Detail;
