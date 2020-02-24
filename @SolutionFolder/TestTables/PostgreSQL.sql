-- =======================================================================================
-- CREATE
CREATE TABLE test_composite_key (
      key01                  varchar(2)        NOT NULL
    , key02                  int               NOT NULL
    , col_bool               bool              NOT NULL
    , col_int                int
    , col_dec                decimal(10,2)
    , col_varchar            varchar(20)
    , create_by              varchar(30)       NOT NULL
    , create_dt              timestamp(3)      NOT NULL
    , update_by              varchar(30)       NOT NULL
    , update_dt              timestamp(3)      NOT NULL
    , PRIMARY KEY (key01, key02)
);

COMMENT ON TABLE test_composite_key IS 'テストテーブル - 複合キー';
COMMENT ON COLUMN test_composite_key.key01              IS 'プライマリキーその１';
COMMENT ON COLUMN test_composite_key.key02              IS 'プライマリキーその２';
COMMENT ON COLUMN test_composite_key.col_bool           IS 'bool型の列';
COMMENT ON COLUMN test_composite_key.col_int            IS 'int型の列';
COMMENT ON COLUMN test_composite_key.col_dec            IS 'decimal型の列';
COMMENT ON COLUMN test_composite_key.col_varchar        IS 'varchar型の列';
COMMENT ON COLUMN test_composite_key.create_by          IS '作成者';
COMMENT ON COLUMN test_composite_key.create_dt          IS '作成日時';
COMMENT ON COLUMN test_composite_key.update_by          IS '更新者';
COMMENT ON COLUMN test_composite_key.update_dt          IS '更新日時';

-- データINSERT
INSERT INTO test_composite_key VALUES ('01',01,true,  999,123456.78,'北海道','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('02',02,false, 999,123456.78,'青森県','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('03',03,true,  999,123456.78,'岩手県','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('04',04,false, 999,123456.78,'宮城県','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('05',05,true,  999,123456.78,'秋田県','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('06',06,false, 999,123456.78,'山形県','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('07',07,false, 999,123456.78,'福島県','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('08',08,true,  999,123456.78,'茨城県','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('09',09,false, 999,123456.78,'栃木県','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('10',10,true,  999,123456.78,'群馬県','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('11',11,false, 999,123456.78,'埼玉県','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('12',12,true,  999,123456.78,'千葉県','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('13',13,false, 999,123456.78,'東京都','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('14',14,true,  999,123456.78,'神奈川県','system',NOW(),'system',NOW());
INSERT INTO test_composite_key VALUES ('15',15,false, 999,123456.78,'新潟県','system',NOW(),'system',NOW());

-- =======================================================================================
CREATE SEQUENCE test_sequence01 AS bigint;

-- CREATE
CREATE TABLE IF NOT EXISTS test_auto_number (
      key03                  int               NOT NULL DEFAULT nextval('test_sequence01')
    , col_bool               bool              NOT NULL
    , col_int                int
    , col_dec                decimal(10,2)
    , col_varchar            varchar(20)
    , create_by              varchar(30)       NOT NULL
    , create_dt              timestamp(3)      NOT NULL
    , update_by              varchar(30)       NOT NULL
    , update_dt              timestamp(3)      NOT NULL
    , PRIMARY KEY (key03)
);

COMMENT ON TABLE test_auto_number IS 'テストテーブル - オートナンバー';
COMMENT ON COLUMN test_auto_number.key03              IS 'プライマリキー';
COMMENT ON COLUMN test_auto_number.col_bool           IS 'bool型の列';
COMMENT ON COLUMN test_auto_number.col_int            IS 'int型の列';
COMMENT ON COLUMN test_auto_number.col_dec            IS 'decimal型の列';
COMMENT ON COLUMN test_auto_number.col_varchar        IS 'varchar型の列';
COMMENT ON COLUMN test_auto_number.create_by          IS '作成者';
COMMENT ON COLUMN test_auto_number.create_dt          IS '作成日時';
COMMENT ON COLUMN test_auto_number.update_by          IS '更新者';
COMMENT ON COLUMN test_auto_number.update_dt          IS '更新日時';




