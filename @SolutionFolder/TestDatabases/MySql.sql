-- =======================================================================================
-- usage : 
-- =======================================================================================
-- ---------------------------------------------------------------------------------------
-- CREATE DATABASE
-- ---------------------------------------------------------------------------------------

-- ---------------------------------------------------------------------------------------
-- CREATE OBJECTS
-- ---------------------------------------------------------------------------------------
DROP VIEW IF EXISTS ViHogeFuga;
DROP TABLE IF EXISTS TrCompositeKey;
DROP TABLE IF EXISTS TrAutoNumber;

-- ---------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS TrCompositeKey (
      Key01                 VARCHAR(2)        NOT NULL      COMMENT 'プライマリキーその１'
    , Key02                 INT               NOT NULL      COMMENT 'プライマリキーその２'
    , ColBool               BOOL              NOT NULL      COMMENT 'boolの列'
    , ColInt                INT                             COMMENT 'intの列'
    , ColDec                DECIMAL(10,2)                   COMMENT 'decimalの列'
    , ColVarchar            VARCHAR(20)                     COMMENT 'varcharの列'
    , UpdateBy              VARCHAR(30)       NOT NULL      COMMENT '更新者'
    , UpdateDt              DATETIME          NOT NULL      COMMENT '更新日時'
    , CONSTRAINT PRIMARY KEY (Key01, Key02)
)
COMMENT='テストテーブル - 複合キー';

-- data for tr_composite_key
INSERT INTO TrCompositeKey VALUES ('01',01,true,  999,123456.78,'北海道','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('02',02,false, 999,123456.78,'青森県','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('03',03,true,  999,123456.78,'岩手県','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('04',04,false, 999,123456.78,'宮城県','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('05',05,true,  999,123456.78,'秋田県','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('06',06,false, 999,123456.78,'山形県','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('07',07,false, 999,123456.78,'福島県','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('08',08,true,  999,123456.78,'茨城県','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('09',09,false, 999,123456.78,'栃木県','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('10',10,true,  999,123456.78,'群馬県','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('11',11,false, 999,123456.78,'埼玉県','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('12',12,true,  999,123456.78,'千葉県','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('13',13,false, 999,123456.78,'東京都','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('14',14,true,  999,123456.78,'神奈川県','hogeman',NOW());
INSERT INTO TrCompositeKey VALUES ('15',15,false, 999,123456.78,'新潟県','hogeman',NOW());

-- ---------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS TrAutoNumber (
      Key03                 BIGINT            NOT NULL AUTO_INCREMENT COMMENT 'プライマリキー'
    , ColVarchar            VARCHAR(50)                               COMMENT 'varcharの列'
    , ColChar               CHAR(3)                                   COMMENT 'charの列'
    , ColBigint             BIGINT                                    COMMENT 'bigintの列'
    , ColInt                INT                                       COMMENT 'intの列'
    , ColSmallint           SMALLINT                                  COMMENT 'smallintの列'
    , ColBool               BIT              NOT NULL DEFAULT FALSE   COMMENT 'bitの列'
    , ColDecimal            DECIMAL(10,2)                             COMMENT 'decimalの列'
    , ColNumeric            NUMERIC(13,0)                             COMMENT 'numericの列'
    , ColDate               DATE                                      COMMENT 'dateの列'
    , ColTime               TIME                                      COMMENT 'timeの列'
    , ColTimestamp          DATETIME         NOT NULL DEFAULT NOW()   COMMENT 'datetimeの列'
    , ColText               LONGTEXT                                  COMMENT 'longtextの列'
    , ColBytea              LONGBLOB                                  COMMENT 'longblobの列'
    , ColUBigint            BIGINT UNSIGNED                           COMMENT 'bigint(UNSIGNED)の列'
    , ColUInt               INT UNSIGNED                              COMMENT 'int(UNSIGNED)の列'
    , ColUSmallint          SMALLINT UNSIGNED                         COMMENT 'smallint(UNSIGNED)の列'
    , ColUTinyInt           TINYINT UNSIGNED                          COMMENT 'tinyint(UNSIGNED)の列'
    , CONSTRAINT PRIMARY KEY (Key03)
)
COMMENT='テストテーブル - オートナンバー';

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
       , tbl2.ColBool           AS Tbl2ColBool
       , tbl2.ColVarchar        AS Tbl2ColVarchar
       , tbl2.ColTimestamp      AS Tbl2ColTimestamp
    FROM TrCompositeKey tbl1
         LEFT JOIN TrAutoNumber tbl2 ON tbl1.ColInt = tbl2.ColInt;

-- ※MySqlのViewには、コメントは付けられにゃい
