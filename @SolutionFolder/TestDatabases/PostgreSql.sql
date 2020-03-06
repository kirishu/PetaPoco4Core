-- =======================================================================================
-- usage : psql -U postgres < PostgreSql.sql
-- =======================================================================================
-- ---------------------------------------------------------------------------------------
-- CREATE DATABASE
-- ---------------------------------------------------------------------------------------
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

CREATE DATABASE petapoco_sample ENCODING 'UTF8' LC_COLLATE 'C' LC_CTYPE 'C' TEMPLATE template0;

\connect petapoco_sample;

CREATE ROLE testman WITH LOGIN PASSWORD 'testpwd';
CREATE SCHEMA testman authorization testman;
GRANT CONNECT ON DATABASE petapoco_sample TO testman;

\connect - testman;

SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;

-- ---------------------------------------------------------------------------------------
-- CREATE OBJECTS
-- ---------------------------------------------------------------------------------------
DROP VIEW IF EXISTS vi_hoge_fuga;
DROP TABLE IF EXISTS tr_auto_number;
DROP TABLE IF EXISTS tr_composite_key;

-- ---------------------------------------------------------------------------------------
CREATE TABLE tr_composite_key (
      key01                  varchar(2)        NOT NULL
    , key02                  int               NOT NULL
    , col_bool               boolean           NOT NULL DEFAULT FALSE
    , col_int                int
    , col_dec                decimal(10,2)
    , col_varchar            varchar(20)
    , update_by              varchar(30)       NOT NULL
    , update_dt              timestamp(3)      NOT NULL DEFAULT NOW()
    , PRIMARY KEY (key01, key02)
);

COMMENT ON TABLE tr_composite_key IS 'テストテーブル - 複合キー';
COMMENT ON COLUMN tr_composite_key.key01              IS 'プライマリキーその１';
COMMENT ON COLUMN tr_composite_key.key02              IS 'プライマリキーその２';
COMMENT ON COLUMN tr_composite_key.col_bool           IS 'boolの列';
COMMENT ON COLUMN tr_composite_key.col_int            IS 'intの列';
COMMENT ON COLUMN tr_composite_key.col_dec            IS 'decimalの列';
COMMENT ON COLUMN tr_composite_key.col_varchar        IS '文字列型の列';
COMMENT ON COLUMN tr_composite_key.update_by          IS '更新者';
COMMENT ON COLUMN tr_composite_key.update_dt          IS '更新日時';

-- data for tr_composite_key
INSERT INTO tr_composite_key VALUES ('01',01,true,  999,123456.78,'北海道','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('02',02,false, 999,123456.78,'青森県','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('03',03,true,  999,123456.78,'岩手県','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('04',04,false, 999,123456.78,'宮城県','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('05',05,true,  999,123456.78,'秋田県','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('06',06,false, 999,123456.78,'山形県','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('07',07,false, 999,123456.78,'福島県','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('08',08,true,  999,123456.78,'茨城県','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('09',09,false, 999,123456.78,'栃木県','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('10',10,true,  999,123456.78,'群馬県','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('11',11,false, 999,123456.78,'埼玉県','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('12',12,true,  999,123456.78,'千葉県','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('13',13,false, 999,123456.78,'東京都','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('14',14,true,  999,123456.78,'神奈川県','hogeman',NOW());
INSERT INTO tr_composite_key VALUES ('15',15,false, 999,123456.78,'新潟県','hogeman',NOW());

-- ---------------------------------------------------------------------------------------
CREATE SEQUENCE seq_a01 AS bigint;
CREATE TABLE IF NOT EXISTS tr_auto_number (
      key03                  bigint            NOT NULL DEFAULT nextval('seq_a01')
    , key01                  varchar(2)
    , col_int                int
    , PRIMARY KEY (key03)
);

COMMENT ON TABLE tr_auto_number IS 'テストテーブル - オートナンバー';
COMMENT ON COLUMN tr_auto_number.key03              IS 'オートナンバーキー';
COMMENT ON COLUMN tr_auto_number.key02              IS 'プライマリキーその２';
COMMENT ON COLUMN tr_auto_number.col_int            IS 'intの列';

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
         LEFT JOIN tr_auto_number tbl2 ON tbl1.key02 = tbl2.key02;

COMMENT ON VIEW vi_hoge_fuga IS 'テストビュー その1';
COMMENT ON COLUMN vi_hoge_fuga.tbl1_key01              IS 'ほげ';
COMMENT ON COLUMN vi_hoge_fuga.tbl1_key02              IS 'ふが';

-- ---------------------------------------------------------------------------------------
CREATE TABLE IF NOT EXISTS tr_columns (
      key01                  varchar(2)        NOT NULL
    , col_varchar            varchar(50)
    , col_char               char(3)
    , col_bigint             bigint
    , col_int                int
    , col_smallint           smallint
    , col_bool               boolean           NOT NULL DEFAULT FALSE
    , col_decimal            decimal(10,2)
    , col_numeric            decimal(13,0)
    , col_date               date
    , col_time               time
    , col_timestamp          timestamp         NOT NULL
    , col_text               text
    , col_bytea              bytea
    , col_timestamp3         timestamp(3)      NOT NULL
    , PRIMARY KEY (key01)
);
COMMENT ON TABLE tr_columns IS 'テストテーブル - 列の型テスト';
COMMENT ON COLUMN tr_columns.key01              IS 'プライマリキーその１';
COMMENT ON COLUMN tr_columns.col_varchar        IS 'varcharの列';
COMMENT ON COLUMN tr_columns.col_char           IS 'charの列';
COMMENT ON COLUMN tr_columns.col_bigint         IS 'bigintの列';
COMMENT ON COLUMN tr_columns.col_int            IS 'intの列';
COMMENT ON COLUMN tr_columns.col_smallint       IS 'smallintの列';
COMMENT ON COLUMN tr_columns.col_bool           IS 'boolの列';
COMMENT ON COLUMN tr_columns.col_decimal        IS 'decimalの列';
COMMENT ON COLUMN tr_columns.col_numeric        IS 'numericの列';
COMMENT ON COLUMN tr_columns.col_date           IS 'dateの列';
COMMENT ON COLUMN tr_columns.col_time           IS 'timeの列';
COMMENT ON COLUMN tr_columns.col_timestamp      IS 'timestampの列';
COMMENT ON COLUMN tr_columns.col_text           IS 'textの列';
COMMENT ON COLUMN tr_columns.col_bytea          IS 'byteaの列';
COMMENT ON COLUMN tr_columns.col_timestamp3     IS 'timestamp(3)の列';
