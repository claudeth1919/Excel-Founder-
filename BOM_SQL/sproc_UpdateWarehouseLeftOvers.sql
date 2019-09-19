
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE sproc_UpdateWarehouseLeftOvers
	@Code VARCHAR(100),
	@OriginalCode VARCHAR(100),
	@Total DECIMAL(8,2),
	@KTL VARCHAR(100),
	@ProviderName VARCHAR(100),
	@Location VARCHAR(100)
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @MaterialId UNIQUEIDENTIFIER = (SELECT Id FROM Enum_Material WHERE Code = @Code)

	BEGIN TRANSACTION UpdateWarehouseLeftOvers
	BEGIN TRY

		IF @MaterialId IS NOT NULL
		BEGIN
			DECLARE @IsInTheSamePlace INT = (SELECT 1 FROM Warehouse WHERE Material_FK =  @MaterialId AND Location = @Location AND kLT = @KTL);
			IF @IsInTheSamePlace IS NOT NULL
			BEGIN
				UPDATE Warehouse SET [StockTotal] = [StockTotal] + @Total WHERE [Material_FK] = @MaterialId
			END
			ELSE
			BEGIN

			END
		END
		ELSE
		BEGIN
			SET @MaterialId = NEWID()
			INSERT INTO [Enum_Material] (Id, [Code]) VALUES(@MaterialId, @Code)
			INSERT INTO Warehouse ([Material_FK], [StockTotal]) VALUES(@MaterialId, @Total)
		END
	COMMIT TRANSACTION InsertPreOrderTransaction;
	 END TRY
	 BEGIN CATCH
	   SELECT 'ERROR'
	   ROLLBACK TRANSACTION UpdateWarehouseLeftOvers;
	  END CATCH;
END
GO
