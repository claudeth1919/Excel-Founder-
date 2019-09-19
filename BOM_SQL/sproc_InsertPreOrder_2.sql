USE [BOM]
GO
/****** Object:  StoredProcedure [dbo].[sproc_InsertPreOrder]    Script Date: 26/02/2019 04:54:54 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sproc_InsertPreOrder]
 @MaterialCodes VARCHAR(MAX),
 @MaterialAmounts VARCHAR(MAX),
 @MaterialUnits VARCHAR(MAX),
 @ExcelPath VARCHAR(500)
AS
BEGIN
 DECLARE @OrderId BIGINT
 DECLARE @SplitOn VARCHAR(5) =';'
 DECLARE @PROJECT_LEFTOVER INT = 4
 DECLARE @PONTON_WAREHOUSE INT = 1
 CREATE TABLE #Code(
   Id INT IDENTITY(1,1) PRIMARY KEY,
   Value VARCHAR(MAX)
 );
 CREATE TABLE #Amount(
   Id INT IDENTITY(1,1) PRIMARY KEY,
   Value DECIMAL(8,2)
 );
 CREATE TABLE #Unit(
   Id INT IDENTITY(1,1) PRIMARY KEY,
   Value VARCHAR(20)
 );
 BEGIN TRANSACTION InsertPreOrderTransaction
 BEGIN TRY

 INSERT INTO #Code (Value) SELECT Value FROM  [dbo].Split(@MaterialCodes, @SplitOn);
 INSERT INTO #Amount (Value) SELECT Value FROM  [dbo].Split( @MaterialAmounts, @SplitOn);
 INSERT INTO #Unit (Value) SELECT Value FROM  [dbo].Split( @MaterialUnits, @SplitOn);

 DECLARE @Index INT = 1
 DECLARE @LENGHT INT = (SELECT COUNT(*) FROM #Code)
 WHILE @Index <= @LENGHT
 BEGIN
   DECLARE @Code VARCHAR(MAX) = (SELECT Value FROM #Code WHERE Id = @Index)
   DECLARE @Unit VARCHAR(20) = (SELECT Value FROM #Unit WHERE Id = @Index)
   DECLARE @Amount DECIMAL(8,2) = (SELECT Value FROM #Amount WHERE Id = @Index)

   DECLARE @MaterialId UNIQUEIDENTIFIER  = (SELECT Id FROM Enum_Material WHERE Code = @Code)

   IF @MaterialId IS NOT NULL
   BEGIN

   IF OBJECT_ID('dbo.#AvailableLocations', 'U') IS NOT NULL
      BEGIN
        DROP TABLE #AvailableLocations
      END

    IF OBJECT_ID('tempdb.dbo.#AvailableLocations', 'U') IS NOT NULL
       BEGIN
         DROP TABLE #AvailableLocations
       END

	 CREATE TABLE #AvailableLocations(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		WarehouseId UNIQUEIDENTIFIER,
		Amount DECIMAL(8,2)
	 );
	 INSERT INTO #AvailableLocations (WarehouseId,Amount) SELECT Id, StockTotal FROM [Warehouse] WHERE Material_FK = @MaterialId AND StockTotal <> 0 AND IsActive = 1
	 DECLARE @Location_Len INT = (SELECT COUNT (*) FROM #AvailableLocations)
	 DECLARE @InterIndex INT = 1

	 DECLARE @BomId BIGINT

     IF @Location_Len != 0 AND @Location_Len IS NOT NULL
     BEGIN
       IF @OrderId IS NULL
       BEGIN
         INSERT INTO [Order] (WhenStarted, ExcelPath) VALUES(GETDATE(), @ExcelPath)
         SET @OrderId = SCOPE_IDENTITY()
       END

			INSERT INTO DataBOM (WantedAmount, Unit) VALUES(@Amount, @Unit)
			SET @BomId = SCOPE_IDENTITY()

       WHILE @InterIndex <= @Location_Len
       BEGIN
        DECLARE @WarehouseId UNIQUEIDENTIFIER = (SELECT WarehouseId FROM #AvailableLocations WHERE Id = @InterIndex)
        DECLARE @ItemAmount DECIMAL(8,2) = (SELECT Amount FROM #AvailableLocations WHERE Id = @InterIndex)
        IF @ItemAmount > 0
        BEGIN
          DECLARE @AvailableAmount DECIMAL(8,2) = (SELECT IIF(@ItemAmount>=@Amount, @Amount, @ItemAmount) )
          INSERT INTO [Rel_Warehouse_Order] ([Warehouse_FK], [Order_FK], [AvailableAmount], WantedAmount,DataBOM_FK) VALUES(@WarehouseId, @OrderId,  @AvailableAmount,@Amount,@BomId)
          SET @Amount = @Amount - @AvailableAmount
        END
        IF @Amount <= 0
        BEGIN
          BREAK;
        END
        SET @InterIndex = @InterIndex + 1
       END

     END
   END

   SET @Index = @Index + 1
 END

 SELECT d.WantedAmount, r.AvailableAmount, o.Id, w.Location, w.KLT, m.OriginalCode, m.[Description], d.Unit BOMUnit, m.Unit WarehouseUnit,eo.Name OriginName, w.StockTotal, m.ProviderName
 FROM [Order] o
 JOIN Rel_Warehouse_Order r ON (r.Order_FK = o.Id)
 JOIN Warehouse w ON (w.Id = r.Warehouse_FK)
 JOIN Enum_Material m ON (m.Id = w.Material_FK)
 JOIN DataBOM d ON (d.Id = r.DataBOM_FK)
 JOIN Enum_Origin eo ON (eo.Id = w.Origin_FK)
 WHERE o.Id = @OrderId
 ORDER BY m.OriginalCode, r.WantedAmount DESC




 COMMIT TRANSACTION InsertPreOrderTransaction;
 END TRY
 BEGIN CATCH
   --SELECT 'ERROR'
   SELECT
		'ERROR'
        ,ERROR_NUMBER() AS ErrorNumber
       ,ERROR_MESSAGE() AS ErrorMessage;

	INSERT INTO Error (Number, Name) VALUES(ERROR_NUMBER(), ERROR_MESSAGE());
   ROLLBACK TRANSACTION InsertPreOrderTransaction;
  END CATCH;
END
