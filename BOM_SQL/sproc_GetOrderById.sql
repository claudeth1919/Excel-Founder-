
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE sproc_GetOrderById
	@OrderId BIGINT
AS
BEGIN
	SELECT SUM(r.[ChosenAmount]) ChosenAmount, m.OriginalCode, m.[Description], o.ExcelPath, o.Id, m.Code, m.Unit
	FROM [Warehouse] w
	JOIN [Rel_Warehouse_Order] r ON (w.Id = r.[Warehouse_FK])
	JOIN [Order] o ON (o.Id = r.Order_FK)
	JOIN Enum_Material m ON (m.Id = w.Material_FK)
	WHERE [IsSelected] = 1
	AND r.Order_FK = @OrderId
	GROUP BY m.OriginalCode, m.[Description], o.ExcelPath, o.Id, m.Code, m.Unit
END
GO
