-- =======================================================================================
-- usage : sqlite3 PetaPocoSample.sqlite3
-- =======================================================================================
-- ---------------------------------------------------------------------------------------
-- CREATE OBJECTS
-- ---------------------------------------------------------------------------------------
DROP VIEW IF EXISTS ViHogeFuga;
DROP TABLE IF EXISTS TrAutoNumber;
DROP TABLE IF EXISTS TrCompositeKey;
DROP TABLE IF EXISTS TrColumns;

-- ---------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS TrCompositeKey (
      Key01                 nvarchar(2)       NOT NULL
    , Key02                 integer           NOT NULL
    , ColBool               boolean           NOT NULL
    , ColInt                integer
    , ColDec                decimal(10,2)
    , ColVarchar            nvarchar(20)
    , UpdateBy              nvarchar(30)      NOT NULL
    , UpdateDt              datetime          NOT NULL
    , PRIMARY KEY (Key01, Key02)
);

-- data for TrCompositeKey
INSERT INTO TrCompositeKey VALUES ('01',01,true,  999,123456.78,'北海道','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('02',02,false, 999,123456.78,'青森県','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('03',03,true,  999,123456.78,'岩手県','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('04',04,false, 999,123456.78,'宮城県','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('05',05,true,  999,123456.78,'秋田県','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('06',06,false, 999,123456.78,'山形県','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('07',07,false, 999,123456.78,'福島県','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('08',08,true,  999,123456.78,'茨城県','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('09',09,false, 999,123456.78,'栃木県','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('10',10,true,  999,123456.78,'群馬県','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('11',11,false, 999,123456.78,'埼玉県','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('12',12,true,  999,123456.78,'千葉県','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('13',13,false, 999,123456.78,'東京都','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('14',14,true,  999,123456.78,'神奈川県','hogeman',datetime('now', 'localtime'));
INSERT INTO TrCompositeKey VALUES ('15',15,false, 999,123456.78,'新潟県','hogeman',datetime('now', 'localtime'));

-- ---------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS TrAutoNumber (
      Key03                 integer           PRIMARY KEY AUTOINCREMENT NOT NULL
    , Key01                 nvarchar(2)       NOT NULL
    , ColInt                integer
);

-- ---------------------------------------------------------------------------------------
CREATE VIEW IF NOT EXISTS ViHogeFuga AS
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

-- ---------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS TrColumns (
      Key01                 nvarchar(2) PRIMARY KEY NOT NULL
    , ColBigInt             bigint
    , ColInt                integer
    , ColIntU               unsigned integer
    , ColSmallInt           smallint
    , ColTinyInt            tinyint
    , ColBool               bool
    , ColDecimal            decimal(10,2)
    , ColNumeric            numeric(12,3)
    , ColDouble             double(6,2)
    , ColFloat              float(7,3)
    , ColDate               date
    , ColTime               time
    , ColDateTime           datetime
    , ColTimeStamp          timestamp
    , ColChar               char(5)
    , ColVarchar            varchar(50)
    , ColBlob               blob
);
