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
    , col_varchar            varchar2(20)
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

-- =======================================================================================
CREATE SEQUENCE seq_sequence01;

-- ---------------------------------------------------------------------------------------
CREATE TABLE tr_auto_number (
      key03                  number             DEFAULT seq_sequence01.NEXTVAL PRIMARY KEY
    , col_bool               number(1)          NOT NULL
    , col_int                number(5)
    , col_dec                number(10,2)
    , col_varchar            varchar2(20)
    , create_by              varchar2(30)       NOT NULL
    , create_dt              timestamp          NOT NULL
    , update_by              varchar2(30)       NOT NULL
    , update_dt              timestamp          NOT NULL
);
COMMENT ON TABLE tr_auto_number IS 'テストテーブル - オートナンバー';
COMMENT ON COLUMN tr_auto_number.key03              IS 'プライマリキー';
COMMENT ON COLUMN tr_auto_number.col_bool           IS 'bool型の列';
COMMENT ON COLUMN tr_auto_number.col_int            IS 'int型の列';
COMMENT ON COLUMN tr_auto_number.col_dec            IS 'decimal型の列';
COMMENT ON COLUMN tr_auto_number.col_varchar        IS 'varchar型の列';
COMMENT ON COLUMN tr_auto_number.create_by          IS '作成者';
COMMENT ON COLUMN tr_auto_number.create_dt          IS '作成日時';
COMMENT ON COLUMN tr_auto_number.update_by          IS '更新者';
COMMENT ON COLUMN tr_auto_number.update_dt          IS '更新日時';
