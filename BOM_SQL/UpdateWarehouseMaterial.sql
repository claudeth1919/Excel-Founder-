USE [BOM]
GO
/****** Object:  StoredProcedure [dbo].[sproc_UpdateWarehouseMaterial]    Script Date: 12/02/2019 08:57:05 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sproc_UpdateWarehouseMaterial]
	@WarehouseId VARCHAR(100),
	@Code VARCHAR(100),
	@OriginalCode VARCHAR(100),
	@Description VARCHAR(200),
	@ProviderName VARCHAR(100),
	@Total DECIMAL(8,2),
	@Unit VARCHAR(20),
	@Location VARCHAR(200),
	@KLT VARCHAR(50),
	@MaterialName VARCHAR(100),
	@IsActive BIT
AS
BEGIN

	SET NOCOUNT ON;

     BEGIN TRANSACTION UpdateWarehouseMaterial
		BEGIN TRY
			IF @WarehouseId IS NULL OR @WarehouseId = ''
			BEGIN
				DECLARE @MaterialId UNIQUEIDENTIFIER = (SELECT Id FROM Enum_Material WHERE Code = @Code)
				IF @MaterialId IS NULL
				BEGIN
					SET @MaterialId = NEWID ()
					INSERT INTO Enum_Material (Id,OriginalCode,Code,[Description],ProviderName,Unit,Name) VALUES(@MaterialId,@OriginalCode,@Code,@Description,@ProviderName,@Unit,@MaterialName);
				END
				INSERT INTO Warehouse (Material_FK,Location,KLT,StockTotal) VALUES(@MaterialId,@Location,@KLT,@Total);
			END
			ELSE
			BEGIN
				UPDATE Enum_Material SET OriginalCode = @OriginalCode, [Description] = @Description, ProviderName = @ProviderName, Unit = @Unit, Name = @MaterialName, Code = @Code
				WHERE Id = (SELECT Material_FK FROM Warehouse WHERE Id = @WarehouseId)

				UPDATE Warehouse SET Location = @Location, KLT = @KLT, StockTotal = @Total, IsActive = @IsActive
				WHERE Id = @WarehouseId
			END


			COMMIT TRANSACTION UpdateWarehouseMaterial;
		END TRY
	BEGIN CATCH
	SELECT
	'ERROR'
			 ,ERROR_NUMBER() AS ErrorNumber
			,ERROR_MESSAGE() AS ErrorMessage;
		ROLLBACK TRANSACTION UpdateWarehouseMaterial;
	END CATCH;

END
