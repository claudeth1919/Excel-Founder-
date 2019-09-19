GO
/****** Object:  Trigger [dbo].[TR_InsertRowInRepPartOriginHistory]    Script Date: 29/11/2018 04:54:00 p.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[TR_InsertNewMaterial]
ON [dbo].Warehouse
 AFTER UPDATE, INSERT
AS BEGIN
SET NOCOUNT ON;
IF  UPDATE (StockTotal)
BEGIN

END
IF NOT EXISTS (SELECT * FROM deleted)
BEGIN

END
END
