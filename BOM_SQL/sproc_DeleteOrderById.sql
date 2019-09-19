USE [BOM]
GO
/****** Object:  StoredProcedure [dbo].[sproc_DeleteOrderById]    Script Date: 22/02/2019 08:14:00 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sproc_DeleteOrderById]
	@OrderId BIGINT
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRANSACTION DeleteOrderById
	 BEGIN TRY
		DELETE FROM [Rel_Warehouse_Order] WHERE Order_FK = @OrderId;
		DELETE FROM [Order] WHERE Id = @OrderId;
	COMMIT TRANSACTION DeleteOrderById;
 END TRY
 BEGIN CATCH
   SELECT 'ERROR'
   ROLLBACK TRANSACTION DeleteOrderById;
  END CATCH;
END
