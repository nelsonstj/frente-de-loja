
IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'produto_complemento' AND COLUMN_NAME = 'Perfil')
BEGIN
	ALTER TABLE produto_complemento 
	ADD Perfil Decimal (18,2) default (0)
END 

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'produto_complemento' AND COLUMN_NAME = 'Aro')
BEGIN
	ALTER TABLE produto_complemento 
	ADD Aro Decimal (18,2) default (0)
END 

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'produto_complemento' AND COLUMN_NAME = 'Carga')
BEGIN
	ALTER TABLE produto_complemento 
	ADD Carga varchar (18,2) default (0)
END

IF NOT EXISTS(SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'produto_complemento' AND COLUMN_NAME = 'Indice')
BEGIN
	ALTER TABLE produto_complemento 
	ADD Indice varchar (18,2) default (0)
END 

IF EXISTS (SELECT * FROM SYS.TABLES WHERE object_id = OBJECT_ID(N'[dbo].[VW_VEICULOS]'))
BEGIN
EXEC dbo.sp_executesql @statement = N'DROP TABLE [dbo].[VW_VEICULOS]'
END

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[VW_VEICULOS]'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[VW_VEICULOS] AS SELECT id, Marca, Modelo, Versao, VersaoMotor, InicioProducao, FinalProducao FROM Fraga_Dev.dbo.VW_VEICULOS'
END				

IF EXISTS (SELECT * FROM SYS.TABLES WHERE object_id = OBJECT_ID(N'[dbo].[VW_VEICULO_PRODUTOS]'))
BEGIN
EXEC dbo.sp_executesql @statement = N'DROP TABLE [dbo].[VW_VEICULO_PRODUTOS]'
END																	 

IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[VW_VEICULO_PRODUTOS]'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE VIEW [dbo].[VW_VEICULO_PRODUTOS] AS SELECT Veiculo_Id, PartNumber, GrupoProduto, DescricaoProduto, MarcaProduto FROM Fraga_Dev.dbo.VW_VEICULO_PRODUTOS'
END