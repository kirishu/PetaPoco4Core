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

-- コメントを変更する場合
-- EXEC sys.sp_updateextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrCompositeKey',@value=N'テストテーブル - 複合キー';

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
    , Key01                 nvarchar(2)       NOT NULL
    , ColInt                int
    , CONSTRAINT pk_TrAutoNumber PRIMARY KEY (Key03)
);
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@value=N'テストテーブル - オートナンバー';
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@level2type=N'COLUMN',@level2name=N'Key03'           ,@value=N'オートナンバーキー' ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@level2type=N'COLUMN',@level2name=N'Key01'           ,@value=N'プライマリキーその１';
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrAutoNumber',@level2type=N'COLUMN',@level2name=N'ColInt'          ,@value=N'int型の列'         ;

-- ---------------------------------------------------------------------------------------
CREATE VIEW ViHogeFuga AS
  SELECT tbl1.Key01             AS Tbl1Key01
       , tbl1.Key02             AS Tbl1Key02
       , tbl1.ColBool           AS Tbl1ColBool
       , tbl1.ColInt            AS Tbl1ColInt
       , tbl1.ColDec            AS Tbl1ColDec
       , tbl1.ColVarchar        AS Tbl1ColVarchar
       , tbl1.UpdateBy          AS Tbl1UpdateBy
       , tbl1.UpdateDt          AS Tbl1UpdateDt
       , tbl2.ColInt            AS Tbl2ColInt
    FROM TrCompositeKey tbl1
         LEFT JOIN TrAutoNumber tbl2 ON tbl1.Key01 = tbl2.Key01;

EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'VIEW',@level1name=N'ViHogeFuga',@value=N'テストビュー その1';
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'VIEW',@level1name=N'ViHogeFuga',@level2type=N'COLUMN',@level2name=N'Tbl1Key01'           ,@value=N'ほげ' ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'VIEW',@level1name=N'ViHogeFuga',@level2type=N'COLUMN',@level2name=N'Tbl1Key02'           ,@value=N'ふが';

-- ---------------------------------------------------------------------------------------
CREATE TABLE TrColumns (
      Key01                 nvarchar(2)       NOT NULL
    , ColBigInt             bigint
    , ColInt                int
    , ColSmallInt           smallint
    , ColTinyInt            tinyint
    , ColBit                bit
    , ColDecimal            decimal(10,2)
    , ColNumeric            numeric(12,0)
    , ColMoney              money
    , ColFloat              float
    , ColReal               real
    , ColDateTime           datetime
    , ColDateTime2          datetime2
    , ColSmallDateTime      smalldatetime
    , ColDate               date
    , ColTime               time
    , ColChar               char(5)
    , ColNchar              nchar(5)
    , ColVarChar            varchar(50)
    , ColNvarChar           nvarchar(50)
    , ColText               text
    , ColNText              ntext
    , ColImage              image
    , ColVarBinary          varbinary
    , CONSTRAINT pk_TrColumns PRIMARY KEY (Key01)
);
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@value=N'テストテーブル - 列の型テスト';
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColBigInt'             ,@value=N'bigintの列'              ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColInt'                ,@value=N'intの列'                 ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColSmallInt'           ,@value=N'smallintの列'            ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColTinyInt'            ,@value=N'tinyintの列'             ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColBit'                ,@value=N'bitの列'                 ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColDecimal'            ,@value=N'decimal(10,2)の列'       ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColNumeric'            ,@value=N'numeric(12,0)の列'       ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColMoney'              ,@value=N'money(10,2)の列'         ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColFloat'              ,@value=N'floatの列'               ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColReal'               ,@value=N'realの列'                ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColDateTime'           ,@value=N'datetimeの列'            ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColDateTime2'          ,@value=N'datetime2の列'           ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColSmallDateTime'      ,@value=N'smalldatetimeの列'       ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColDate'               ,@value=N'dateの列'                ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColTime'               ,@value=N'timeの列'                ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColChar'               ,@value=N'char(5)の列'             ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColNchar'              ,@value=N'nchar(5)の列'            ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColVarChar'            ,@value=N'varchar(50)の列'         ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColNvarChar'           ,@value=N'nvarchar(50)の列'        ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColText'               ,@value=N'textの列'                ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColNText'              ,@value=N'ntextの列'               ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColImage'              ,@value=N'imageの列'               ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TrColumns',@level2type=N'COLUMN',@level2name=N'ColVarBinary'          ,@value=N'varbinaryの列'           ;
