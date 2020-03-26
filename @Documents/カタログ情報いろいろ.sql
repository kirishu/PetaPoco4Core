/*
[TABLE]
  schema_name
  table_name
  comment

[COLUMN]








*/

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
SELECT tbl.name AS TableName
     , scm.name AS SchemaName
     , eps.value AS Comment
  FROM sys.tables AS tbl
        LEFT JOIN sys.schemas AS scm
               ON tbl.schema_id = scm.schema_id
        LEFT JOIN sys.extended_properties eps
               ON tbl.object_id = eps.major_id
              AND eps.minor_id = 0
 WHERE tbl.type = 'U'
-- Viewは sys.tablesの代わりに sys.views

SELECT cif.table_catalog AS [Database]
     , cif.table_schema AS [Owner]
     , cif.table_name AS TableName
     , cif.column_name AS ColumnName
     , cif.ordinal_position AS OrdinalPosition
     , cif.column_default AS DefaultSetting
     , cif.is_nullable AS IsNullable
     , cif.data_type AS DataType
     , cif.character_maximum_length AS [MaxLength]
     , cif.datetime_precision AS DatePrecision
     , COLUMNPROPERTY(object_id('[' + table_schema + '].[' + table_name + ']'), column_name, 'IsIdentity') AS IsIdentity
     , COLUMNPROPERTY(object_id('[' + table_schema + '].[' + table_name + ']'), column_name, 'IsComputed') AS IsComputed
     , exp.value AS Commnet
  FROM information_schema.columns AS cif
        LEFT JOIN sys.tables AS tbl
               ON cif.table_name = tbl.name
        LEFT JOIN sys.columns AS col
               ON tbl.object_id = col.object_id
              AND cif.column_name = col.name
        LEFT JOIN sys.extended_properties AS exp
               ON col.object_id = exp.major_id
              AND col.column_id = exp.minor_id
 WHERE cif.table_name = @tablename
   AND cif.table_schema = @schemaname
 ORDER BY cif.ordinal_position ASC;

