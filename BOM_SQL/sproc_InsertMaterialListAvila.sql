
GO
/****** Object:  StoredProcedure [dbo].[sproc_InsertMaterialAvila]    Script Date: 30/01/2019 11:46:41 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sproc_InsertMaterialAvila]
 @Code VARCHAR(100),
 @OriginalCode VARCHAR(100),
 @Total DECIMAL(8,2),
 @KTL VARCHAR(100),
 @ProviderName VARCHAR(100),
 @Location VARCHAR(100),
 @Unit VARCHAR(20),
 @Description VARCHAR(MAX)
AS
BEGIN
 SET NOCOUNT ON;
 DECLARE @MaterialId  UNIQUEIDENTIFIER = (SELECT Id FROM [Enum_Material] WHERE Code = @Code)
 DECLARE @ORIGIN_AVILA INT = 2
 BEGIN TRANSACTION InserMaterialTransactionAvila
 BEGIN TRY

   IF @MaterialId IS NULL
   BEGIN
     SET @MaterialId = NEWID()
     INSERT INTO [Enum_Material] ([Id], Code, [ProviderName], [OriginalCode], Unit,[Description]) VALUES(@MaterialId,@Code, @ProviderName,@OriginalCode, @Unit,@Description);
     INSERT INTO [Warehouse] ([Material_FK], [StockTotal], [Location], [KLT], Origin_FK) VALUES (@MaterialId, @Total, @Location,@KTL,@ORIGIN_AVILA);
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
      INSERT INTO [Warehouse] ([Material_FK], [StockTotal], [Location], [KLT], Origin_FK) VALUES (@MaterialId, @Total, @Location,@KTL,@ORIGIN_AVILA);
     END
   END

 COMMIT TRANSACTION InserMaterialTransactionAvila;
 END TRY
 BEGIN CATCH
 SELECT
'ERROR'
     ,ERROR_NUMBER() AS ErrorNumber
    ,ERROR_MESSAGE() AS ErrorMessage;
   ROLLBACK TRANSACTION InserMaterialTransactionAvila;
  END CATCH;

END
