
	SET ANSI_NULLS ON
	GO
	SET QUOTED_IDENTIFIER ON
	GO

	ALTER PROCEDURE sproc_UpdateOrderAmounts
		-- @OrderId BIGINT
	AS
	BEGIN
		SET NOCOUNT ON;

	    BEGIN TRANSACTION UpdateOrderAmounts
		BEGIN TRY
			DECLARE @WithoutRelations INT =   0
			CREATE TABLE #PendingOrder(
				Id BIGINT
			);

			INSERT INTO #PendingOrder (Id) SELECT Id FROM [Order] WHERE IsAssigned = 0

			DELETE r FROM Rel_Warehouse_Order r WHERE EXISTS(SELECT 1 FROM Warehouse w WHERE r.Warehouse_FK = w.Id AND w.StockTotal = 0) AND r.Order_FK IN (SELECT Id FROM #PendingOrder);

			DELETE o FROM [Order] o WHERE EXISTS (SELECT COUNT(*) FROM Rel_Warehouse_Order r WHERE o.Id = r.Order_FK  HAVING COUNT(*) = @WithoutRelations) AND Id IN (SELECT Id FROM #PendingOrder)

			UPDATE r SET r.AvailableAmount = (IIF(r.AvailableAmount<=w.StockTotal,r.AvailableAmount,w.StockTotal))
			FROM  Rel_Warehouse_Order r
			JOIN 	Warehouse w ON (r.Warehouse_FK = w.Id)
			WHERE r.Order_FK IN (SELECT Id FROM #PendingOrder)


			COMMIT TRANSACTION UpdateOrderAmounts;
		 END TRY
		 BEGIN CATCH
			 SELECT 'ERROR'
			 ROLLBACK TRANSACTION UpdateOrderAmounts;
			END CATCH;
	END
	GO
