CREATE DATABASE SEDPLAN;

CREATE TABLE SA_Variable_Map
(
	SA_ID VARCHAR(50),
	VAR_Type VARCHAR(5), --The type of variable: A L LS
	VAR_Value VARCHAR(50),
	VAR_Weight FLOAT,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_SA_Variable_Map PRIMARY KEY (SA_ID,VAR_TYPE,VAR_VALUE)
);


CREATE TABLE SA_Component
(
	SA_ID VARCHAR(50),
	Parts_Name VARCHAR(100),
	Specification VARCHAR(100),
	LHS INT,
	RHS INT,
	PCE INT,
	IsVariable BIT, -- Inidcate whether this part has a variable
	VAR_Type VARCHAR(5),
	IsStandard BIT, -- Inidate whether this part is a standard part
	Parts_Weight FLOAT,
	Process_ID BIGINT,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_SA_Component PRIMARY KEY (SA_ID, Parts_Name,Specification),
	CONSTRAINT FK_SA_Component FOREIGN KEY (Process_ID) REFERENCES SA_Process(Process_ID),
);

CREATE TABLE SA_Process
(
	Process_ID BIGINT,
	Process_Name VARCHAR(100),
	CONSTRAINT PK_SA_Process PRIMARY KEY (Process_ID)
);

CREATE TABLE BOM_Plan
(
	Plan_Name varchar(100),
	SA_ID varchar(50),
	VAR_Type varchar(5),
	VAR_Value varchar(50),
	Quantity int,
	IsMain bit, -- Inidate whether the variable of this SA is a primary variable
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_BOM_Plan PRIMARY KEY (Plan_Name, SA_ID, VAR_Type, VAR_Value),
);

CREATE TABLE STD_Parts
(
	Parts_Name varchar(100),
	Specification varchar(100),
	STD_Weight float,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_STD_Parts PRIMARY KEY (Parts_Name, Specification)
);
);

CREATE TABLE BOM_Detail
(
	Plan_Name varchar(100),
	Parts_Name VARCHAR(100),
	Specification VARCHAR(100),
	LHS INT,
	RHS INT,
	PCE INT,
	Total_Weight FLOAT,
	Process_ID bigint,
	Revision int,
	Created_Time Datetime,
	timestamp,
	CONSTRAINT PK_BOM_Detail PRIMARY KEY (Plan_Name, Parts_Name,Specification),
	CONSTRAINT FK_BOM_Detail FOREIGN KEY (Process_ID) REFERENCES SA_Process(Process_ID),
);

