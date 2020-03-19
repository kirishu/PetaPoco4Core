-- =======================================================================================
-- usage : mysql --user root < MySQL.sql
-- =======================================================================================
-- ---------------------------------------------------------------------------------------
-- CREATE DATABASE
-- ---------------------------------------------------------------------------------------
DROP DATABASE IF EXISTS PetaPocoSample;

CREATE DATABASE IF NOT EXISTS PetaPocoSample
       CHARACTER SET utf8mb4
       COLLATE utf8mb4_general_ci;

CREATE USER 'testman'@'%' IDENTIFIED BY 'testpwd';
GRANT ALL ON PetaPocoSample.* TO 'testman'@'%';
FLUSH PRIVILEGES;

USE PetaPocoSample;

-- ---------------------------------------------------------------------------------------
-- CREATE OBJECTS
-- ---------------------------------------------------------------------------------------
DROP VIEW IF EXISTS ViHogeFuga;
DROP TABLE IF EXISTS TrCompositeKey;
DROP TABLE IF EXISTS TrAutoNumber;
DROP TABLE IF EXISTS TrColumns;

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
      Key03                 BIGINT            NOT NULL AUTO_INCREMENT COMMENT 'オートナンバーキー'
    , Key01                 VARCHAR(2)        NOT NULL      COMMENT 'プライマリキーその１'
    , ColInt                INT                             COMMENT 'intの列'
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
       , tbl2.ColInt            AS Tbl2ColInt
    FROM TrCompositeKey tbl1
         LEFT JOIN TrAutoNumber tbl2 ON tbl1.Key01 = tbl2.Key01;

-- ※MySqlのViewには、コメントは付けられにゃい

-- ---------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS TrColumns (
      Key01                 varchar(2)        NOT NULL      COMMENT 'プライマリキーその１'
    , ColBigInt             bigint                          COMMENT 'bigintの列'
    , ColBigIntU            bigint unsigned                 COMMENT 'bigint unsignedの列'
    , ColInt                int                             COMMENT 'intの列'
    , ColIntU               int unsigned                    COMMENT 'int unsignedの列'
    , ColMediumInt          mediumint                       COMMENT 'mediumintの列'
    , ColMediumIntU         mediumint unsigned              COMMENT 'mediumint unsignedの列'
    , ColSmallInt           smallint                        COMMENT 'smallintの列'
    , ColSmallIntU          smallint unsigned               COMMENT 'smallint unsignedの列'
    , ColTinyInt            tinyint                         COMMENT 'tinyintの列'
    , ColTinyIntU           tinyint unsigned                COMMENT 'tinyint unsignedの列'
    , ColBit                bit                             COMMENT 'bitの列'
    , ColBool               bool                            COMMENT 'boolの列'
    , ColDecimal            decimal(10,2)                   COMMENT 'decimalの列'
    , ColNumeric            numeric(12,3)                   COMMENT 'numericの列'
    , ColDouble             double(6,2)                     COMMENT 'doubleの列'
    , ColFloat              float(7,3)                      COMMENT 'floatの列'
    , ColDate               date                            COMMENT 'dateの列'
    , ColTime               time                            COMMENT 'timeの列'
    , ColDateTime           datetime                        COMMENT 'datetimeの列'
    , ColTimeStamp          timestamp                       COMMENT 'timestampの列'
    , ColChar               char(5)                         COMMENT 'char(5)の列'
    , ColVarchar            varchar(50)                     COMMENT 'varchar(50)の列'
    , ColLongText           longtext                        COMMENT 'longtextの列'
    , ColLongBlob           longblob                        COMMENT 'longblobの列'
    , CONSTRAINT PRIMARY KEY (Key01)
)
COMMENT='テストテーブル - 列の型テスト';
