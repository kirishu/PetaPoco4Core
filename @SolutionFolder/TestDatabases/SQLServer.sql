-- =======================================================================================
-- usage : sqlcmd -S {server} -U sa -i SQLServer.sql -o results.txt
-- =======================================================================================
-- ---------------------------------------------------------------------------------------
-- CREATE DATABASE
-- ---------------------------------------------------------------------------------------
SET NOCOUNT ON
go
USE [master]
go
if exists (select * from sysdatabases where name='PetaPocoSample')
     DROP DATABASE [PetaPocoSample]
go
if exists (select * from syslogins where name='testman')
    DROP LOGIN [testman]
go

CREATE  DATABASE [PetaPocoSample]
go

CREATE LOGIN [testman] WITH PASSWORD = 'testpwd'
       , DEFAULT_DATABASE = [PetaPocoSample]
       , CHECK_POLICY = OFF
       , CHECK_EXPIRATION = OFF
go

USE [PetaPocoSample]
go
CREATE USER [testman]
go

exec sp_addrolemember 'db_owner', 'testman'
go

-- ---------------------------------------------------------------------------------------
-- CREATE OBJECTS
-- ---------------------------------------------------------------------------------------
CREATE TABLE TrCompositeKey (
      Key01                 nvarchar(2)       NOT NULL
    , Key02                 int               NOT NULL
    , ColBool               bit               NOT NULL
    , ColInt                int
    , ColDec                decimal(10,2)
    , ColVarchar            nvarchar(20)
    , CreateBy              nvarchar(30)      NOT NULL
    , CreateDt              datetime2         NOT NULL
    , UpdateBy              nvarchar(30)      NOT NULL
    , UpdateDt              datetime2         NOT NULL
    , CONSTRAINT pk_TrCompositeKey PRIMARY KEY (Key01, Key02)
);
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrCompositeKey',@value=N'テストテーブル - 複合キー';
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrCompositeKey',@level2type=N'COLUMN',@level2name=N'Key01'           ,@value=N'プライマリキーその１'    ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrCompositeKey',@level2type=N'COLUMN',@level2name=N'Key02'           ,@value=N'プライマリキーその２'    ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrCompositeKey',@level2type=N'COLUMN',@level2name=N'ColBool'         ,@value=N'bit型の列'               ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrCompositeKey',@level2type=N'COLUMN',@level2name=N'ColInt'          ,@value=N'int型の列'               ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrCompositeKey',@level2type=N'COLUMN',@level2name=N'ColDec'          ,@value=N'decimal型の列'           ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrCompositeKey',@level2type=N'COLUMN',@level2name=N'ColVarchar'      ,@value=N'varchar型の列'           ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrCompositeKey',@level2type=N'COLUMN',@level2name=N'CreateBy'        ,@value=N'作成者'                  ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrCompositeKey',@level2type=N'COLUMN',@level2name=N'CreateDt'        ,@value=N'作成日時'                ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrCompositeKey',@level2type=N'COLUMN',@level2name=N'UpdateBy'        ,@value=N'更新者'                  ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrCompositeKey',@level2type=N'COLUMN',@level2name=N'UpdateDt'        ,@value=N'更新日時'                ;

-- data for TrCompositeKey
INSERT INTO TrCompositeKey VALUES ('01',01,1, 999,123456.78,'北海道','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('02',02,0, 999,123456.78,'青森県','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('03',03,1, 999,123456.78,'岩手県','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('04',04,0, 999,123456.78,'宮城県','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('05',05,1, 999,123456.78,'秋田県','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('06',06,0, 999,123456.78,'山形県','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('07',07,0, 999,123456.78,'福島県','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('08',08,1, 999,123456.78,'茨城県','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('09',09,0, 999,123456.78,'栃木県','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('10',10,1, 999,123456.78,'群馬県','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('11',11,0, 999,123456.78,'埼玉県','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('12',12,1, 999,123456.78,'千葉県','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('13',13,0, 999,123456.78,'東京都','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('14',14,1, 999,123456.78,'神奈川県','hogeman',GETDATE(),'hogeman',GETDATE());
INSERT INTO TrCompositeKey VALUES ('15',15,0, 999,123456.78,'新潟県','hogeman',GETDATE(),'hogeman',GETDATE());

-- ---------------------------------------------------------------------------------------
CREATE TABLE TrAutoNumber (
      Key03                 int               NOT NULL IDENTITY (1, 1)
    , ColBool               bit               NOT NULL
    , ColInt                int
    , ColDec                decimal(10,2)
    , ColVarchar            nvarchar(20)
    , CreateBy              nvarchar(30)      NOT NULL
    , CreateDt              datetime2         NOT NULL
    , UpdateBy              nvarchar(30)      NOT NULL
    , UpdateDt              datetime2         NOT NULL
    , CONSTRAINT pk_TrAutoNumber PRIMARY KEY (Key03)
);
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@value=N'テストテーブル - オートナンバー';
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@level2type=N'COLUMN',@level2name=N'Key03'           ,@value=N'プライマリキー'    ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@level2type=N'COLUMN',@level2name=N'ColBool'         ,@value=N'bit型の列'         ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@level2type=N'COLUMN',@level2name=N'ColInt'          ,@value=N'int型の列'         ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@level2type=N'COLUMN',@level2name=N'ColDec'          ,@value=N'decimal型の列'     ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@level2type=N'COLUMN',@level2name=N'ColVarchar'      ,@value=N'varchar型の列'     ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@level2type=N'COLUMN',@level2name=N'CreateBy'        ,@value=N'作成者'            ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@level2type=N'COLUMN',@level2name=N'CreateDt'        ,@value=N'作成日時'          ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@level2type=N'COLUMN',@level2name=N'UpdateBy'        ,@value=N'更新者'            ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@level2type=N'COLUMN',@level2name=N'UpdateDt'        ,@value=N'更新日時'          ;
