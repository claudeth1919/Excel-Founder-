
-- DROP TABLE Enum_Material, Warehouse, [Order], Rel_Warehouse_Order, Enum_Origin, DataBOM

CREATE TABLE Enum_Origin(
	Id INT PRIMARY KEY,
	Name VARCHAR(100)
);

CREATE TABLE Error(
	Id INT IDENTITY(1,1)  PRIMARY KEY,
	WhenHappened DATETIME DEFAULT GETDATE(),
	Number INT,
	Name VARCHAR(MAX)
);

CREATE TABLE Enum_Supplier(
	Id INT IDENTITY(1,1)  PRIMARY KEY,
	Name VARCHAR(100)
);

CREATE TABLE Enum_Unit(
	Id INT IDENTITY(1,1)  PRIMARY KEY,
	Name VARCHAR(100)
);
-- INSERT INTO Enum_Supplier (Name) SELECT UPPER([ProviderName]) FROM [dbo].[Enum_Material] WHERE [ProviderName]!= '' GROUP BY [ProviderName];
INSERT INTO Enum_Origin (Id, Name) VALUES (1, 'Almacén Pontón');
INSERT INTO Enum_Origin (Id, Name) VALUES (2, 'Actuador');
INSERT INTO Enum_Origin (Id, Name) VALUES (3, 'Almacén No Productivo');
INSERT INTO Enum_Origin (Id, Name) VALUES (4, 'Sobrante proyecto');


INSERT INTO Enum_Unit (Name) VALUES ('Unidad');
INSERT INTO Enum_Unit (Name) VALUES ('Paquete');
INSERT INTO Enum_Unit (Name) VALUES ('Metro');
INSERT INTO Enum_Unit (Name) VALUES ('Pieza');

CREATE TABLE Enum_Material(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT (NEWID()),
	Name VARCHAR(100),
	Code VARCHAR(100) NOT NULL,
	OriginalCode VARCHAR(300) NOT NULL,
	Description VARCHAR(200),
	ProviderName VARCHAR(100),
	Unit VARCHAR(20)
);

CREATE TABLE Enum_MaterialType(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Name VARCHAR(100)
);

INSERT INTO Enum_MaterialType (Name) VALUES('Machine');
INSERT INTO Enum_MaterialType (Name) VALUES('Consumible');

CREATE TABLE Warehouse(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT (NEWID()),
	Material_FK UNIQUEIDENTIFIER REFERENCES  Enum_Material(Id) NOT NULL,
	StockTotal DECIMAL(8,2) NOT NULL,
	Location VARCHAR(200),
  KLT VARCHAR(50),
	Origin_FK INT REFERENCES Enum_Origin(Id) NOT NULL DEFAULT 1,
	IsActive BIT DEFAULT 1
);

CREATE TABLE [Order](
	Id BIGINT IDENTITY(1,1) PRIMARY KEY ,
	WhenStarted DATETIME DEFAULT GETDATE() NOT NULL,
	WhenAssigned DATETIME NULL,
	IsAssigned BIT NOT NULL DEFAULT 0,
	ExcelPath VARCHAR(500)
);



CREATE TABLE DataBOM(
	Id BIGINT IDENTITY(1,1) PRIMARY KEY,
	WantedAmount DECIMAL(8,2) NOT NULL,
	Unit VARCHAR(20),
	Sheet VARCHAR(100) NULL
	Row VARCHAR(4) NULL
	Col VARCHAR(3) NULL
);


CREATE TABLE Rel_Warehouse_Order(
	Warehouse_FK UNIQUEIDENTIFIER REFERENCES  Warehouse(Id),
	Order_FK BIGINT REFERENCES  [Order](Id),
	AvailableAmount DECIMAL(8,2) NOT NULL,
  WantedAmount DECIMAL(8,2) NOT NULL,
  ChosenAmount DECIMAL(8,2),
	DataBOM_FK BIGINT REFERENCES DataBOM(Id) NOT NULL,
  IsSelected BIT NOT NULL DEFAULT 0
);


ALTER TABLE Enum_Material ALTER COLUMN OriginalCode VARCHAR (MAX);
ALTER TABLE Enum_Material ALTER COLUMN Code VARCHAR (880);
CREATE NONCLUSTERED  INDEX index_Code_NONclustered ON Enum_Material(Code);
--DROP INDEX index_Code_NONclustered ON Enum_Material;

-- Alter Staments

ALTER TABLE DataBOM ADD Sheet VARCHAR(100) NULL;
ALTER TABLE DataBOM ADD Row VARCHAR(3) NULL;
ALTER TABLE Enum_Material ADD MaterialType_FK INT REFERENCES  Enum_MaterialType(Id) NULL;
