USE [BOM]
GO
/****** Object:  StoredProcedure [dbo].[sproc_GetMaterialByOrderId]    Script Date: 22/02/2019 08:16:20 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sproc_GetMaterialByOrderId]
	@OrderId BIGINT
AS
BEGIN
	SET NOCOUNT ON;


  SELECT m.Code, r.AvailableAmount, w.StockTotal, m.OriginalCode, r.Warehouse_FK, w.Location, w.KLT, d.WantedAmount, d.Unit BOMUnit, m.Unit WarehouseUnit, eo.Name OriginName
	FROM [Order] o
	JOIN Rel_Warehouse_Order r ON (o.Id = r.Order_FK)
	JOIN Warehouse w ON (w.Id = r.Warehouse_FK)
	JOIN Enum_Origin eo ON (eo.Id = w.Origin_FK)
	JOIN Enum_Material m ON (m.Id = w.Material_FK)
	JOIN DataBOM d ON (d.Id = r.DataBOM_FK)
	WHERE o.Id = @OrderId
	ORDER BY m.Code, r.WantedAmount DESC
END
