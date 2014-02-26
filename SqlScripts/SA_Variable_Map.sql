GO
USE [SEDPLAN];
GO

CREATE PROCEDURE DBO.UPDATE_SA_VAR_CURRENT
AS
BEGIN
	TRUNCATE TABLE SA_VAR_CURRENT;

	INSERT INTO SA_VAR_CURRENT
	SELECT SA_ID,VAR_Type,VAR_Value,VAR_Weight
	FROM SA_Variable_Map SVM
	WHERE SVM.Revision=(SELECT MAX(Revision) 
						FROM SA_Variable_Map SVM2
						WHERE SVM2.SA_ID=SVM.SA_ID);

END