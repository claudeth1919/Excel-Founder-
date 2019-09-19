USE [BOM]
GO
/****** Object:  StoredProcedure [dbo].[sproc_AssignOrder]    Script Date: 22/02/2019 08:13:15 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sproc_AssignOrder]
@WarehouseIds VARCHAR(MAX),
@MaterialAmounts VARCHAR(MAX),
@OrderId BIGINT
AS
BEGIN

DECLARE @SplitOn VARCHAR(5) =';'
CREATE TABLE #WarehouseIds(
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Value UNIQUEIDENTIFIER
);
CREATE TABLE #Amount(
  Id INT IDENTITY(1,1) PRIMARY KEY,
  Value DECIMAL(8,2)
);
BEGIN TRANSACTION AssignOrderTransaction
BEGIN TRY

INSERT INTO #WarehouseIds (Value) SELECT  CAST(Value AS UNIQUEIDENTIFIER) FROM  [dbo].Split(@WarehouseIds, @SplitOn);
INSERT INTO #Amount (Value) SELECT Value FROM  [dbo].Split( @MaterialAmounts, @SplitOn);

DECLARE @Index INT = 1
DECLARE @LENGHT INT = (SELECT COUNT(*) FROM #WarehouseIds)
WHILE @Index <= @LENGHT
BEGIN

  DECLARE @Amount DECIMAL(8,2) = (SELECT Value FROM #Amount WHERE Id = @Index)
  DECLARE @WarehouseId UNIQUEIDENTIFIER = (SELECT Value FROM [#WarehouseIds] WHERE Id = @Index)
  DECLARE @Total DECIMAL(8,2) = (SELECT StockTotal FROM [Warehouse] WHERE Id = @WarehouseId)

  IF @Total != 0 AND @Total IS NOT NULL
  BEGIN

   UPDATE [Rel_Warehouse_Order] SET [ChosenAmount] = @Amount, [IsSelected] = 1
   WHERE [Warehouse_FK] = @WarehouseId AND [Order_FK] = @OrderId

   UPDATE [Warehouse] SET StockTotal = StockTotal - @Amount WHERE Id = @WarehouseId

  END

  SET @Index = @Index + 1
END
UPDATE [Order] SET IsAssigned = 1, WhenAssigned = GETDATE() WHERE Id = @OrderId



COMMIT TRANSACTION AssignOrderTransaction;
SELECT SUM(r.[ChosenAmount]) ChosenAmount, m.OriginalCode, m.[Description], o.ExcelPath, o.Id, m.Code, m.Unit
FROM [Warehouse] w
JOIN [Rel_Warehouse_Order] r ON (w.Id = r.[Warehouse_FK])
JOIN [Order] o ON (o.Id = r.Order_FK)
JOIN Enum_Material m ON (m.Id = w.Material_FK)
WHERE [IsSelected] = 1
AND r.Order_FK = @OrderId
GROUP BY m.OriginalCode, m.[Description], o.ExcelPath, o.Id, m.Code, m.Unit
END TRY
BEGIN CATCH
  SELECT 'ERROR'
  ROLLBACK TRANSACTION AssignOrderTransaction;
 END CATCH;
END
