USE [BOM]
GO
/****** Object:  StoredProcedure [dbo].[sproc_UpdateWarehouseMaterialForDummies]    Script Date: 18/02/2019 09:30:18 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sproc_UpdateWarehouseMaterialForDummies]
	@Code VARCHAR(MAX),
	@OriginalCode VARCHAR(MAX),
	@Description VARCHAR(200),
	@ProviderName VARCHAR(100),
	@Total DECIMAL(8,2),
	@Unit VARCHAR(20),
	@Location VARCHAR(200),
	@KLT VARCHAR(50),
	@MaterialName VARCHAR(100)
AS
BEGIN

	SET NOCOUNT ON;

     BEGIN TRANSACTION UpdateForDummies
			BEGIN TRY
				DECLARE @ORIGIN_PROJECTS_LEFTOVERS INT = 4
				DECLARE @MaterialId UNIQUEIDENTIFIER = NEWID ()
				INSERT INTO Enum_Material (Id,OriginalCode,Code,[Description],ProviderName,Unit,Name) VALUES(@MaterialId,@OriginalCode,@Code,@Description,@ProviderName,@Unit,@MaterialName);
				INSERT INTO Warehouse (Material_FK,Location,KLT,StockTotal,Origin_FK) VALUES(@MaterialId,@Location,@KLT,@Total,@ORIGIN_PROJECTS_LEFTOVERS)

			COMMIT TRANSACTION UpdateForDummies;
		END TRY
	BEGIN CATCH
	SELECT
	'ERROR'
			 ,ERROR_NUMBER() AS ErrorNumber
			,ERROR_MESSAGE() AS ErrorMessage;
		ROLLBACK TRANSACTION UpdateForDummies;
	END CATCH;

END
