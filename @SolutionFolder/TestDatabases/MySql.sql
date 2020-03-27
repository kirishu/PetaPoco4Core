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
      Key01                 varchar(2)    NOT NULL                   COMMENT 'プライマリキーその１'
    , ColBigint             bigint                                   COMMENT 'bigintの列'
    , ColInt                int                                      COMMENT 'intの列'
    , ColSmallint           smallint                                 COMMENT 'smallintの列'
    , ColTinyint            tinyint                                  COMMENT 'tinyintの列'
    , ColBool               bool          NOT NULL DEFAULT FALSE     COMMENT 'boolの列'
    , ColDecimal            decimal(14,2)                            COMMENT 'decimal(14,2)の列'
    , ColFloat              float                                    COMMENT 'floatの列'
    , ColDouble             double                                   COMMENT 'doubleの列'
    , ColDate               date                                     COMMENT 'dateの列'
    , ColTime               time                                     COMMENT 'timeの列'
    , ColDateTime           datetime      NOT NULL DEFAULT NOW()     COMMENT 'datetimeの列'
    , ColChar               char(5)                                  COMMENT 'charの列'
    , ColVarchar            varchar(255)                             COMMENT 'varcharの列'
    , ColText               longtext                                 COMMENT 'longtextの列'
    , ColBlob               longblob                                 COMMENT 'longblobの列'

    , ColBigintU            bigint unsigned                          COMMENT 'bigint unsignedの列'
    , ColIntU               int unsigned                             COMMENT 'int unsignedの列'
    , ColMediumint          mediumint                                COMMENT 'mediumintの列'
    , ColMediumintU         mediumint unsigned                       COMMENT 'mediumint unsignedの列'
    , ColSmallintU          smallint unsigned                        COMMENT 'smallint unsignedの列'
    , ColTinyintU           tinyint unsigned                         COMMENT 'tinyint unsignedの列'
    , ColBit                bit                                      COMMENT 'bitの列'
    , ColNumeric            numeric(13,3)                            COMMENT 'numeric(13,3)の列'
    , ColTimeStamp          timestamp                                COMMENT 'timestampの列'
    , CONSTRAINT PRIMARY KEY (Key01)
)
COMMENT='テストテーブル - 列の型テスト';
