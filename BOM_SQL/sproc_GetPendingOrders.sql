USE [BOM]
GO
/****** Object:  StoredProcedure [dbo].[sproc_GetPendingOrders]    Script Date: 22/02/2019 08:16:31 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sproc_GetPendingOrders]
AS
BEGIN
	SET NOCOUNT ON;

	EXEC sproc_UpdateOrderAmounts;

	SELECT o.[Id]
      ,o.[WhenStarted]
      ,o.[WhenAssigned]
      ,o.[IsAssigned]
	--  ,'['+STUFF(( SELECT ','+''''+Id+'''' FROM Rel_Warehouse_Order r JOIN Warehouse w ON ( r.Warehouse_FK= w.Id)  WHERE r.Order_FK = o.Id FOR XML PATH('')  ),1,1,'')+']' Material
	  FROM [Order] o
		WHERE o.[IsAssigned] =0
		AND EXISTS(SELECT 1 FROM Rel_Warehouse_Order r WHERE r.Order_FK = o.Id)
END
