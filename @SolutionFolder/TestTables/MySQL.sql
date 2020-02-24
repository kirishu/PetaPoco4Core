-- =======================================================================================
-- CREATE
CREATE TABLE IF NOT EXISTS TestCompositeKey (
      Key01                 VARCHAR(2)        NOT NULL      COMMENT 'プライマリキーその１'
    , Key02                 int               NOT NULL      COMMENT 'プライマリキーその２'
    , ColBool               bool              NOT NULL      COMMENT 'bool型の列'
    , ColInt                int                             COMMENT 'int型の列'
    , ColDec                decimal(10,2)                   COMMENT 'decimal型の列'
    , ColVarchar            VARCHAR(20)                     COMMENT 'varchar型の列'
    , CreateBy              VARCHAR(30)       NOT NULL      COMMENT '作成者'
    , CreateDt              DATETIME          NOT NULL      COMMENT '作成日時'
    , UpdateBy              VARCHAR(30)       NOT NULL      COMMENT '更新者'
    , UpdateDt              DATETIME          NOT NULL      COMMENT '更新日時'
    , CONSTRAINT PK_TestCompositeKey PRIMARY KEY (Key01, Key02)
)
COMMENT='テストテーブル - 複合キー';

-- データINSERT
INSERT INTO TestCompositeKey VALUES ('01',01,true,  999,123456.78,'北海道','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('02',02,false, 999,123456.78,'青森県','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('03',03,true,  999,123456.78,'岩手県','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('04',04,false, 999,123456.78,'宮城県','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('05',05,true,  999,123456.78,'秋田県','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('06',06,false, 999,123456.78,'山形県','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('07',07,false, 999,123456.78,'福島県','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('08',08,true,  999,123456.78,'茨城県','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('09',09,false, 999,123456.78,'栃木県','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('10',10,true,  999,123456.78,'群馬県','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('11',11,false, 999,123456.78,'埼玉県','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('12',12,true,  999,123456.78,'千葉県','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('13',13,false, 999,123456.78,'東京都','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('14',14,true,  999,123456.78,'神奈川県','system',NOW(),'system',NOW());
INSERT INTO TestCompositeKey VALUES ('15',15,false, 999,123456.78,'新潟県','system',NOW(),'system',NOW());

-- =======================================================================================
-- CREATE
CREATE TABLE IF NOT EXISTS TestAutoNumber (
      Key03                 int               NOT NULL AUTO_INCREMENT COMMENT 'プライマリキー'
    , ColBool               bool              NOT NULL      COMMENT 'bool型の列'
    , ColInt                int                             COMMENT 'int型の列'
    , ColDec                decimal(10,2)                   COMMENT 'decimal型の列'
    , ColVarchar            VARCHAR(20)                     COMMENT 'varchar型の列'
    , CreateBy              VARCHAR(30)       NOT NULL      COMMENT '作成者'
    , CreateDt              DATETIME          NOT NULL      COMMENT '作成日時'
    , UpdateBy              VARCHAR(30)       NOT NULL      COMMENT '更新者'
    , UpdateDt              DATETIME          NOT NULL      COMMENT '更新日時'
    , CONSTRAINT PK_TestAutoNumber PRIMARY KEY (Key03)
)
COMMENT='テストテーブル - オートナンバー';




