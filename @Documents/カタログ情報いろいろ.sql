-- =======================================================================================
-- PostgreSQL
-- =======================================================================================
-- テーブル情報
SELECT istb.table_name
     , istb.table_schema
     , istb.table_type
     , obj_description(istb.table_name::regclass) AS table_comment
  FROM information_schema.tables istb
 WHERE istb.table_type IN ('BASE TABLE', 'VIEW')
   AND istb.table_schema IN (SELECT DISTINCT schemaname FROM pg_stat_user_tables)
--   AND istb.table_name = '@tableName'
 ORDER BY istb.table_schema
        , istb.table_name;

-- 列情報
SELECT isco.table_name
     , isco.column_name
     , isco.is_nullable
     , isco.udt_name
     , isco.column_default
     , isco.ordinal_position
     , isco.character_maximum_length
     , isco.numeric_precision
     , isco.numeric_precision_radix
     , isco.numeric_scale
     , isco.datetime_precision
     , col_description(isco.table_name::regclass, isco.ordinal_position) AS column_comment
  FROM information_schema.columns ifco
 WHERE isco.table_name = '@tableName'
--   AND isco.table_schema IN (SELECT DISTINCT schemaname FROM pg_stat_user_tables)
 ORDER BY isco.ordinal_position;


-- =======================================================================================
-- =======================================================================================
SELECT TABLE_SCHEMA
     , TABLE_NAME
     , TABLE_TYPE
     , TABLE_COMMENT
  FROM INFORMATION_SCHEMA.TABLES
 WHERE TABLE_SCHEMA IN (SELECT database())
   AND TABLE_NAME = 'TrAutoNumber'
 ORDER BY TABLE_NAME;

-- 列情報
SELECT TABLE_SCHEMA
     , TABLE_NAME
     , COLUMN_NAME
     , ORDINAL_POSITION
     , IS_NULLABLE
     , COLUMN_DEFAULT
     , DATA_TYPE
     , COLUMN_TYPE
     , CHARACTER_MAXIMUM_LENGTH
     , NUMERIC_PRECISION
     , NUMERIC_SCALE
     , COLUMN_KEY
     , EXTRA
     , COLUMN_COMMENT
  FROM INFORMATION_SCHEMA.COLUMNS
 WHERE TABLE_SCHEMA IN (SELECT database())
   AND TABLE_NAME = 'TrAutoNumber'
--   AND TABLE_SCHEMA = [DB名]
 ORDER BY ORDINAL_POSITION;







