USE [BOM]
GO
/****** Object:  StoredProcedure [dbo].[sproc_GetPreOrderById]    Script Date: 22/02/2019 08:17:42 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sproc_GetPreOrderById]
	@OrderId BIGINT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT d.WantedAmount, r.AvailableAmount, o.Id, w.Location, w.KLT, m.OriginalCode, m.[Description], d.Unit BOMUnit, m.Unit WarehouseUnit,eo.Name OriginName, w.StockTotal, m.ProviderName, o.ExcelPath
	 FROM [Order] o
	 JOIN Rel_Warehouse_Order r ON (r.Order_FK = o.Id)
	 JOIN Warehouse w ON (w.Id = r.Warehouse_FK)
	 JOIN Enum_Material m ON (m.Id = w.Material_FK)
	 JOIN DataBOM d ON (d.Id = r.DataBOM_FK)
	 JOIN Enum_Origin eo ON (eo.Id = w.Origin_FK)
	 WHERE o.Id = @OrderId
	 ORDER BY m.OriginalCode, r.WantedAmount DESC

END
