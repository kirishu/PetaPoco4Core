-- =======================================================================================
-- CREATE
CREATE TABLE TestCompositeKey (
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
    , CONSTRAINT pk_TestCompositeKey PRIMARY KEY (Key01, Key02)
);


EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestCompositeKey',@value=N'テストテーブル - 複合キー';

EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestCompositeKey',@level2type=N'COLUMN',@level2name=N'Key01'           ,@value=N'プライマリキーその１'    ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestCompositeKey',@level2type=N'COLUMN',@level2name=N'Key02'           ,@value=N'プライマリキーその２'    ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestCompositeKey',@level2type=N'COLUMN',@level2name=N'ColBool'         ,@value=N'bit型の列'               ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestCompositeKey',@level2type=N'COLUMN',@level2name=N'ColInt'          ,@value=N'int型の列'               ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestCompositeKey',@level2type=N'COLUMN',@level2name=N'ColDec'          ,@value=N'decimal型の列'           ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestCompositeKey',@level2type=N'COLUMN',@level2name=N'ColVarchar'      ,@value=N'varchar型の列'           ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestCompositeKey',@level2type=N'COLUMN',@level2name=N'CreateBy'        ,@value=N'作成者'                  ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestCompositeKey',@level2type=N'COLUMN',@level2name=N'CreateDt'        ,@value=N'作成日時'                ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestCompositeKey',@level2type=N'COLUMN',@level2name=N'UpdateBy'        ,@value=N'更新者'                  ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestCompositeKey',@level2type=N'COLUMN',@level2name=N'UpdateDt'        ,@value=N'更新日時'                ;


-- データINSERT
INSERT INTO TestCompositeKey VALUES ('01',01,1,  999,123456.78,'北海道','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('02',02,0, 999,123456.78,'青森県','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('03',03,1,  999,123456.78,'岩手県','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('04',04,0, 999,123456.78,'宮城県','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('05',05,1,  999,123456.78,'秋田県','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('06',06,0, 999,123456.78,'山形県','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('07',07,0, 999,123456.78,'福島県','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('08',08,1,  999,123456.78,'茨城県','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('09',09,0, 999,123456.78,'栃木県','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('10',10,1,  999,123456.78,'群馬県','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('11',11,0, 999,123456.78,'埼玉県','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('12',12,1,  999,123456.78,'千葉県','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('13',13,0, 999,123456.78,'東京都','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('14',14,1,  999,123456.78,'神奈川県','system',GETDATE(),'system',GETDATE());
INSERT INTO TestCompositeKey VALUES ('15',15,0, 999,123456.78,'新潟県','system',GETDATE(),'system',GETDATE());

-- =======================================================================================
-- CREATE
CREATE TABLE TestAutoNumber (
      Key03                 int               NOT NULL IDENTITY (1, 1)
    , ColBool               bit               NOT NULL
    , ColInt                int                       
    , ColDec                decimal(10,2)             
    , ColVarchar            nvarchar(20)              
    , CreateBy              nvarchar(30)      NOT NULL
    , CreateDt              datetime2         NOT NULL
    , UpdateBy              nvarchar(30)      NOT NULL
    , UpdateDt              datetime2         NOT NULL
    , CONSTRAINT pk_TestAutoNumber PRIMARY KEY (Key03)
);

EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestAutoNumber',@value=N'テストテーブル - オートナンバー';

EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestAutoNumber',@level2type=N'COLUMN',@level2name=N'Key03'           ,@value=N'プライマリキー'    ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestAutoNumber',@level2type=N'COLUMN',@level2name=N'ColBool'         ,@value=N'bit型の列'               ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestAutoNumber',@level2type=N'COLUMN',@level2name=N'ColInt'          ,@value=N'int型の列'               ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestAutoNumber',@level2type=N'COLUMN',@level2name=N'ColDec'          ,@value=N'decimal型の列'           ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestAutoNumber',@level2type=N'COLUMN',@level2name=N'ColVarchar'      ,@value=N'varchar型の列'           ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestAutoNumber',@level2type=N'COLUMN',@level2name=N'CreateBy'        ,@value=N'作成者'                  ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestAutoNumber',@level2type=N'COLUMN',@level2name=N'CreateDt'        ,@value=N'作成日時'                ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestAutoNumber',@level2type=N'COLUMN',@level2name=N'UpdateBy'        ,@value=N'更新者'                  ;
EXEC sys.sp_addextendedproperty  @name=N'MS_Description',@level0type=N'SCHEMA',@level0name=N'dbo',@level1type=N'TABLE',@level1name=N'TestAutoNumber',@level2type=N'COLUMN',@level2name=N'UpdateDt'        ,@value=N'更新日時'                ;





