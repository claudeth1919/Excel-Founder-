
GO
/****** Object:  StoredProcedure [dbo].[sproc_InsertMaterial]    Script Date: 30/01/2019 11:46:41 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sproc_InsertMaterial]
 @Code VARCHAR(100),
 @OriginalCode VARCHAR(100),
 @Total DECIMAL(8,2),
 @KTL VARCHAR(100),
 @ProviderName VARCHAR(100),
 @Location VARCHAR(100),
 @Unit VARCHAR(20)
AS
BEGIN
 SET NOCOUNT ON;
 DECLARE @MaterialId  UNIQUEIDENTIFIER = (SELECT Id FROM [Enum_Material] WHERE Code = @Code)

 BEGIN TRANSACTION InserMaterialTransaction
 BEGIN TRY

   IF @MaterialId IS NULL
   BEGIN
     SET @MaterialId = NEWID()
     INSERT INTO [Enum_Material] ([Id], Code, [ProviderName], [OriginalCode], Unit) VALUES(@MaterialId,@Code, @ProviderName,@OriginalCode, @Unit);
     INSERT INTO [Warehouse] ([Material_FK], [StockTotal], [Location], [KLT]) VALUES (@MaterialId, @Total, @Location,@KTL);
   END
   ELSE
   BEGIN
     DECLARE @IsInTheSamePlace INT = (SELECT 1 FROM Warehouse WHERE Material_FK =  @MaterialId AND Location = @Location AND kLT = @KTL);
     IF @IsInTheSamePlace IS NOT NULL
     BEGIN
      UPDATE [Warehouse] SET [StockTotal] = [StockTotal] + @Total WHERE [Material_FK] = @MaterialId AND Location = @Location AND kLT = @KTL
     END
     ELSE
     BEGIN
      INSERT INTO [Warehouse] ([Material_FK], [StockTotal], [Location], [KLT]) VALUES (@MaterialId, @Total, @Location,@KTL);
     END
   END

 COMMIT TRANSACTION InserMaterialTransaction;
 END TRY
 BEGIN CATCH
 SELECT
'ERROR'
     ,ERROR_NUMBER() AS ErrorNumber
    ,ERROR_MESSAGE() AS ErrorMessage;
   ROLLBACK TRANSACTION InserMaterialTransaction;
  END CATCH;

END
