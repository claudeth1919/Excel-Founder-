CREATE VIEW MaterialList AS SELECT w.[Id]
              ,m.[Name]
              ,m.[Code]
              ,m.[OriginalCode]
              ,m.[Description]
              ,m.[ProviderName]
              ,w.StockTotal
              ,m.[Unit]
              ,w.KLT
              ,w.Location
              ,w.IsActive
              ,w.Origin_FK
              FROM [Enum_Material] m JOIN Warehouse w ON(m.Id = w.Material_FK)
              WHERE IsActive = 1;
              AND w.StockTotal>0
