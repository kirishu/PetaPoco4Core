﻿
// This file was automatically generated by the PetaPoco T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String:      `Server=CentOSdb;Port=5432;Database=petapoco_sample;Encoding=UTF8;User Id=testman;Password=**zapped**;`
//     Provider:               `Npgsql`
//     Schema:                 ``
//     Include Views:          `True`
//     Genetated:              `2020/03/07 00:05:30`

using System;
using PetaPoco;

namespace PetaPocoAppFx.Database.PostgreSql
{
    /// <summary>
    /// petapoco_sample 接続文字列
    /// </summary>
    public static class Config
    {
        /// <summary>デフォルトの接続文字列</summary>
        public static string ConnectionString { get; set; }
    }

    /// <summary>
    /// petapoco_sample Database Object
    /// </summary>
    public class DB : DatabaseExtension
    {
        /// <summary>
        /// petapoco_sample Database Object
        /// </summary>
        public DB() : base(Config.ConnectionString, RDBType.PostgreSql)
        {
            CommandTimeout = 30;
            UseA5Mk2Params = true;
        }
    }


    /// <summary>テストテーブル - オートナンバー</summary>
    [TableName("tr_auto_number")]
    [PrimaryKey("key03", AutoIncrement=true, SequenceName="seq_a01")]
    [ExplicitColumns]
    public class TrAutoNumber: PetaPoco.PetaPocoRecord<TrAutoNumber>
    {
        /// <summary>オートナンバーキー</summary>
        [Column("key03")] public long Key03 { get { return _Key03; } set { _Key03 = value; MarkColumnModified("key03"); } } long _Key03;
        /// <summary>プライマリキーその１</summary>
        [Column("key01")] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("key01"); } } string _Key01;
        /// <summary>intの列</summary>
        [Column("col_int")] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("col_int"); } } int? _ColInt;
    }

    /// <summary>テストテーブル - 列の型テスト</summary>
    [TableName("tr_columns")]
    [PrimaryKey("key01", AutoIncrement=false)]
    [ExplicitColumns]
    public class TrColumn: PetaPoco.PetaPocoRecord<TrColumn>
    {
        /// <summary>プライマリキーその１</summary>
        [Column("key01")] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("key01"); } } string _Key01;
        /// <summary>varcharの列</summary>
        [Column("col_varchar")] public string ColVarchar { get { return _ColVarchar; } set { _ColVarchar = value; MarkColumnModified("col_varchar"); } } string _ColVarchar;
        /// <summary>charの列</summary>
        [Column("col_char")] public string ColChar { get { return _ColChar; } set { _ColChar = value; MarkColumnModified("col_char"); } } string _ColChar;
        /// <summary>bigintの列</summary>
        [Column("col_bigint")] public long? ColBigint { get { return _ColBigint; } set { _ColBigint = value; MarkColumnModified("col_bigint"); } } long? _ColBigint;
        /// <summary>intの列</summary>
        [Column("col_int")] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("col_int"); } } int? _ColInt;
        /// <summary>smallintの列</summary>
        [Column("col_smallint")] public short? ColSmallint { get { return _ColSmallint; } set { _ColSmallint = value; MarkColumnModified("col_smallint"); } } short? _ColSmallint;
        /// <summary>booleanの列</summary>
        [Column("col_bool")] public bool ColBool { get { return _ColBool; } set { _ColBool = value; MarkColumnModified("col_bool"); } } bool _ColBool;
        /// <summary>boolの列</summary>
        [Column("col_bool2")] public bool? ColBool2 { get { return _ColBool2; } set { _ColBool2 = value; MarkColumnModified("col_bool2"); } } bool? _ColBool2;
        /// <summary>decimalの列</summary>
        [Column("col_decimal")] public decimal? ColDecimal { get { return _ColDecimal; } set { _ColDecimal = value; MarkColumnModified("col_decimal"); } } decimal? _ColDecimal;
        /// <summary>numericの列</summary>
        [Column("col_numeric")] public decimal? ColNumeric { get { return _ColNumeric; } set { _ColNumeric = value; MarkColumnModified("col_numeric"); } } decimal? _ColNumeric;
        /// <summary>realの列</summary>
        [Column("col_real")] public float? ColReal { get { return _ColReal; } set { _ColReal = value; MarkColumnModified("col_real"); } } float? _ColReal;
        /// <summary>double precisionの列</summary>
        [Column("col_double")] public double? ColDouble { get { return _ColDouble; } set { _ColDouble = value; MarkColumnModified("col_double"); } } double? _ColDouble;
        /// <summary>dateの列</summary>
        [Column("col_date")] public DateTime? ColDate { get { return _ColDate; } set { _ColDate = value; MarkColumnModified("col_date"); } } DateTime? _ColDate;
        /// <summary>timeの列</summary>
        [Column("col_time")] public TimeSpan? ColTime { get { return _ColTime; } set { _ColTime = value; MarkColumnModified("col_time"); } } TimeSpan? _ColTime;
        /// <summary>timestampの列</summary>
        [Column("col_timestamp")] public DateTime ColTimestamp { get { return _ColTimestamp; } set { _ColTimestamp = value; MarkColumnModified("col_timestamp"); } } DateTime _ColTimestamp;
        /// <summary>timestamp(3)の列</summary>
        [Column("col_timestamp3")] public DateTime ColTimestamp3 { get { return _ColTimestamp3; } set { _ColTimestamp3 = value; MarkColumnModified("col_timestamp3"); } } DateTime _ColTimestamp3;
        /// <summary>textの列</summary>
        [Column("col_text")] public string ColText { get { return _ColText; } set { _ColText = value; MarkColumnModified("col_text"); } } string _ColText;
        /// <summary>byteaの列</summary>
        [Column("col_bytea")] public byte[] ColBytea { get { return _ColBytea; } set { _ColBytea = value; MarkColumnModified("col_bytea"); } } byte[] _ColBytea;
    }

    /// <summary>テストテーブル - 複合キー</summary>
    [TableName("tr_composite_key")]
    [PrimaryKey("key01,key02", AutoIncrement=false)]
    [ExplicitColumns]
    public class TrCompositeKey: PetaPoco.PetaPocoRecord<TrCompositeKey>
    {
        /// <summary>プライマリキーその１</summary>
        [Column("key01")] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("key01"); } } string _Key01;
        /// <summary>プライマリキーその２</summary>
        [Column("key02")] public int Key02 { get { return _Key02; } set { _Key02 = value; MarkColumnModified("key02"); } } int _Key02;
        /// <summary>boolの列</summary>
        [Column("col_bool")] public bool ColBool { get { return _ColBool; } set { _ColBool = value; MarkColumnModified("col_bool"); } } bool _ColBool;
        /// <summary>intの列</summary>
        [Column("col_int")] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("col_int"); } } int? _ColInt;
        /// <summary>decimalの列</summary>
        [Column("col_dec")] public decimal? ColDec { get { return _ColDec; } set { _ColDec = value; MarkColumnModified("col_dec"); } } decimal? _ColDec;
        /// <summary>文字列型の列</summary>
        [Column("col_varchar")] public string ColVarchar { get { return _ColVarchar; } set { _ColVarchar = value; MarkColumnModified("col_varchar"); } } string _ColVarchar;
        /// <summary>更新者</summary>
        [Column("update_by")] public string UpdateBy { get { return _UpdateBy; } set { _UpdateBy = value; MarkColumnModified("update_by"); } } string _UpdateBy;
        /// <summary>更新日時</summary>
        [Column("update_dt")] public DateTime UpdateDt { get { return _UpdateDt; } set { _UpdateDt = value; MarkColumnModified("update_dt"); } } DateTime _UpdateDt;
    }

    /// <summary>テストビュー その1</summary>
    [TableName("vi_hoge_fuga")]
    [ExplicitColumns]
    public class ViHogeFuga
    {
        /// <summary>ほげ</summary>
        [Column("tbl1_key01")] public string Tbl1Key01 { get; set; }
        /// <summary>ふが</summary>
        [Column("tbl1_key02")] public int? Tbl1Key02 { get; set; }
        /// <summary></summary>
        [Column("tbl1_col_bool")] public bool? Tbl1ColBool { get; set; }
        /// <summary></summary>
        [Column("tbl1_col_int")] public int? Tbl1ColInt { get; set; }
        /// <summary></summary>
        [Column("tbl1_col_dec")] public decimal? Tbl1ColDec { get; set; }
        /// <summary></summary>
        [Column("tbl1_col_varchar")] public string Tbl1ColVarchar { get; set; }
        /// <summary></summary>
        [Column("tbl1_update_by")] public string Tbl1UpdateBy { get; set; }
        /// <summary></summary>
        [Column("tbl1_update_dt")] public DateTime? Tbl1UpdateDt { get; set; }
        /// <summary></summary>
        [Column("tbl2_col_int")] public int? Tbl2ColInt { get; set; }
    }
}

