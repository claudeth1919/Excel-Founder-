USE [BOM]
GO
/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 22/02/2019 08:12:31 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[Split]
(
	@List VARCHAR(MAX),
	@SplitOn VARCHAR(5)
)
RETURNS @RtnValue TABLE
(

	ID INT identity(1,1),
	Value VARCHAR(MAX)
)
AS
BEGIN
	WHILE (Charindex(@SplitOn,@List)>0)
	BEGIN
		INSERT INTO
			@RtnValue (value)
		SELECT
		    	Value = ltrim(rtrim(Substring(@List,1,Charindex(@SplitOn,@List)-1)))

		SET @List = Substring(@List,Charindex(@SplitOn,@List)+len(@SplitOn),len(@List))
	END

	INSERT INTO
		@RtnValue (Value)
	    SELECT
		Value = ltrim(rtrim(@List))

	    RETURN
END
