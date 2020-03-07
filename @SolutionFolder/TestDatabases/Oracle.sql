-- =======================================================================================
-- !! Require Oracle version 12c or later !!
-- usage : sqlplus system/{password}@{server}
-- ---------------------------------------------------------------------------------------
-- CREATE SCHEMA
-- ---------------------------------------------------------------------------------------
alter session set container=XEPDB1;
GRANT CONNECT,RESOURCE,UNLIMITED TABLESPACE TO testman IDENTIFIED BY testpwd;
GRANT DBA TO testman;

connect testman/testpwd@{server}/XEPDB1
-- ---------------------------------------------------------------------------------------
-- CREATE OBJECTS
-- ---------------------------------------------------------------------------------------
CREATE TABLE tr_composite_key (
      key01                  varchar2(2)        NOT NULL
    , key02                  number(3)          NOT NULL
    , col_bool               number(1)          NOT NULL
    , col_int                number(5)
    , col_dec                number(10,2)
    , col_varchar            nvarchar2(20)
    , create_by              varchar2(30)       NOT NULL
    , create_dt              timestamp          NOT NULL
    , update_by              varchar2(30)       NOT NULL
    , update_dt              timestamp          NOT NULL
    , PRIMARY KEY (key01, key02)
);
COMMENT ON TABLE tr_composite_key IS 'テストテーブル - 複合キー';
COMMENT ON COLUMN tr_composite_key.key01              IS 'プライマリキーその１';
COMMENT ON COLUMN tr_composite_key.key02              IS 'プライマリキーその２';
COMMENT ON COLUMN tr_composite_key.col_bool           IS 'bool型の列';
COMMENT ON COLUMN tr_composite_key.col_int            IS 'int型の列';
COMMENT ON COLUMN tr_composite_key.col_dec            IS 'decimal型の列';
COMMENT ON COLUMN tr_composite_key.col_varchar        IS 'varchar型の列';
COMMENT ON COLUMN tr_composite_key.create_by          IS '作成者';
COMMENT ON COLUMN tr_composite_key.create_dt          IS '作成日時';
COMMENT ON COLUMN tr_composite_key.update_by          IS '更新者';
COMMENT ON COLUMN tr_composite_key.update_dt          IS '更新日時';

-- data for tr_composite_key
INSERT INTO tr_composite_key VALUES ('01',01,1, 999,123456.78,'北海道','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('02',02,0, 999,123456.78,'青森県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('03',03,1, 999,123456.78,'岩手県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('04',04,0, 999,123456.78,'宮城県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('05',05,1, 999,123456.78,'秋田県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('06',06,0, 999,123456.78,'山形県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('07',07,0, 999,123456.78,'福島県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('08',08,1, 999,123456.78,'茨城県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('09',09,0, 999,123456.78,'栃木県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('10',10,1, 999,123456.78,'群馬県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('11',11,0, 999,123456.78,'埼玉県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('12',12,1, 999,123456.78,'千葉県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('13',13,0, 999,123456.78,'東京都','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('14',14,1, 999,123456.78,'神奈川県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);
INSERT INTO tr_composite_key VALUES ('15',15,0, 999,123456.78,'新潟県','hogeman',SYSTIMESTAMP,'hogeman',SYSTIMESTAMP);

-- ---------------------------------------------------------------------------------------
CREATE SEQUENCE seq_sequence01;

-- ---------------------------------------------------------------------------------------
CREATE TABLE tr_auto_number (
      key03                  number             DEFAULT seq_sequence01.NEXTVAL PRIMARY KEY
    , key01                  varchar2(2)
    , col_int                number(5)
);
COMMENT ON TABLE tr_auto_number IS 'テストテーブル - オートナンバー';
COMMENT ON COLUMN tr_auto_number.key03              IS 'オートナンバーキー';
COMMENT ON COLUMN tr_auto_number.key01              IS 'プライマリキーその１';
COMMENT ON COLUMN tr_auto_number.col_int            IS 'int型の列';

-- ---------------------------------------------------------------------------------------
CREATE VIEW vi_hoge_fuga AS
  SELECT tbl1.key01              AS tbl1_key01
       , tbl1.key02              AS tbl1_key02
       , tbl1.col_bool           AS tbl1_col_bool
       , tbl1.col_int            AS tbl1_col_int
       , tbl1.col_dec            AS tbl1_col_dec
       , tbl1.col_varchar        AS tbl1_col_varchar
       , tbl1.update_by          AS tbl1_update_by
       , tbl1.update_dt          AS tbl1_update_dt
       , tbl2.col_int            AS tbl2_col_int
    FROM tr_composite_key tbl1
         LEFT JOIN tr_auto_number tbl2 ON tbl1.key01 = tbl2.key01;

COMMENT ON TABLE vi_hoge_fuga IS 'テストビュー その1';
COMMENT ON COLUMN vi_hoge_fuga.tbl1_key01              IS 'ほげ';
COMMENT ON COLUMN vi_hoge_fuga.tbl1_key02              IS 'ふが';

-- ---------------------------------------------------------------------------------------
DROP TABLE tr_columns;
CREATE TABLE tr_columns (
      key01                  varchar2(2)        NOT NULL PRIMARY KEY
    , col_number             number(10,3)
    , col_number5            number(5)
    , col_char               char(5)
    , col_nchar              nchar(5)
    , col_varchar2           varchar2(50)
    , col_nvarchar2          nvarchar2(50)
    , col_date               date               DEFAULT sysdate NOT NULL
    , col_timestamp          timestamp          DEFAULT systimestamp NOT NULL
    , col_binary_float       binary_float
    , col_binary_double      binary_double
    , col_raw                raw(1024)
    , col_long_raw           long raw
    , col_blob               blob
);
COMMENT ON TABLE tr_columns IS 'テストテーブル - 列の型テスト';
COMMENT ON COLUMN tr_columns.key01                   IS 'プライマリキーその１';
COMMENT ON COLUMN tr_columns.col_number              IS 'number(10,3)の列';
COMMENT ON COLUMN tr_columns.col_number5             IS 'number(5)の列';
COMMENT ON COLUMN tr_columns.col_char                IS 'char(5)の列';
COMMENT ON COLUMN tr_columns.col_nchar               IS 'nchar(5)の列';
COMMENT ON COLUMN tr_columns.col_varchar2            IS 'varchar2(50)の列';
COMMENT ON COLUMN tr_columns.col_nvarchar2           IS 'nvarchar2(50)の列';
COMMENT ON COLUMN tr_columns.col_date                IS 'dateの列';
COMMENT ON COLUMN tr_columns.col_timestamp           IS 'timestampの列';
COMMENT ON COLUMN tr_columns.col_binary_float        IS 'binary_floatの列';
COMMENT ON COLUMN tr_columns.col_binary_double       IS 'binary_doubleの列';
COMMENT ON COLUMN tr_columns.col_raw                 IS 'raw(1024)の列';
COMMENT ON COLUMN tr_columns.col_long_raw            IS 'long rawの列';
COMMENT ON COLUMN tr_columns.col_blob                IS 'blobの列';
