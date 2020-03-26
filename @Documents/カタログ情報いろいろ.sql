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
-- MySQL
-- =======================================================================================
SELECT table_schema
     , table_name
     , table_type
     , table_comment
  FROM information_schema.tables
 WHERE table_schema IN (SELECT database())
   AND table_name = 'TrAutonumber'
 ORDER BY by table_name;

-- 列情報
SELECT table_schema
     , table_name
     , column_name
     , ordinal_position
     , is_nullable
     , column_default
     , data_type
     , column_type
     , character_maximum_length
     , numeric_precision
     , numeric_scale
     , column_key
     , extra
     , column_comment
  FROM information_schema.columns
 WHERE table_schema = (SELECT database())
   AND table_name = 'TrAutonumber'
 ORDER BY ordinal_position;

-- =======================================================================================
-- SQL Server
-- =======================================================================================





