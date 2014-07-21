GO
USE [SEDPLAN];
GO

CREATE FUNCTION DBO.FUN_CALBP_LRNUM(@MODE VARCHAR(3), @LHS INT, @RHS INT, @MOTORPOS VARCHAR(5))
RETURNS INT
--DBO.FUN_CALBP_LRNUM: Get the quantity of LHS or RHS according to motor position of SA
--Some SA's have asymmetrical quantity of LHS and RHS. The quantities of them depend on the motor postion of SA
--If the motor of SA is on the left side, then the quantity of LHS and RHS DO NOT be swapped.
--If the motor of SA is on the right side, then the quantity of LHS and RHS SHOULD be swapped.
--All the data imported into DB is based on left hand side.
AS
BEGIN

	DECLARE @LRNUM INT = 0;

	IF @LHS=@RHS
	BEGIN
		SET @LRNUM = @LHS;
	END
	ELSE
	BEGIN
		IF @MODE='LHS'
		BEGIN
			IF LTRIM(RTRIM(@MOTORPOS))='L'
			BEGIN
				SET @LRNUM=@LHS;
			END

			ELSE IF LTRIM(RTRIM(@MOTORPOS))='R'
			BEGIN
				SET @LRNUM=@RHS;
			END
		END

		ELSE IF @MODE='RHS'
		BEGIN
			IF LTRIM(RTRIM(@MOTORPOS))='L'
			BEGIN
				SET @LRNUM=@RHS;
			END

			ELSE IF LTRIM(RTRIM(@MOTORPOS))='R'
			BEGIN
				SET @LRNUM=@LHS;
			END
		END
	END

	RETURN @LRNUM;
END