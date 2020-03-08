﻿
// This file was automatically generated by the PetaPoco T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String:      `server=CentOSdb;database=PetaPocoSample;uid=testman;pwd=**zapped**;SslMode=None;`
//     Provider:               `MySql.Data.MySqlClient`
//     Schema:                 ``
//     Include Views:          `True`
//     Genetated:              `2020/03/07 00:07:50`

using System;
using PetaPoco;

namespace PetaPocoAppFx.Database.MySql
{
    /// <summary>
    /// PetaPocoSample 接続文字列
    /// </summary>
    public static class Config
    {
        /// <summary>デフォルトの接続文字列</summary>
        public static string ConnectionString { get; set; }
    }

    /// <summary>
    /// PetaPocoSample Database Object
    /// </summary>
    public class DB : DatabaseExtension
    {
        /// <summary>
        /// PetaPocoSample Database Object
        /// </summary>
        public DB() : base(Config.ConnectionString, RDBType.MySql)
        {
        }
    }


    /// <summary>テストテーブル - オートナンバー</summary>
    [TableName("TrAutoNumber")]
    [PrimaryKey("Key03", AutoIncrement=true)]
    [ExplicitColumns]
    public class TrAutoNumber: PetaPoco.PetaPocoRecord<TrAutoNumber>
    {
        /// <summary>オートナンバーキー</summary>
        [Column] public long Key03 { get { return _Key03; } set { _Key03 = value; MarkColumnModified("Key03"); } } long _Key03;
        /// <summary>プライマリキーその１</summary>
        [Column] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("Key01"); } } string _Key01;
        /// <summary>intの列</summary>
        [Column] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("ColInt"); } } int? _ColInt;
    }

    /// <summary>テストテーブル - 列の型テスト</summary>
    [TableName("TrColumns")]
    [PrimaryKey("Key01", AutoIncrement=false)]
    [ExplicitColumns]
    public class TrColumn: PetaPoco.PetaPocoRecord<TrColumn>
    {
        /// <summary>プライマリキーその１</summary>
        [Column] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("Key01"); } } string _Key01;
        /// <summary>bigintの列</summary>
        [Column] public long? ColBigInt { get { return _ColBigInt; } set { _ColBigInt = value; MarkColumnModified("ColBigInt"); } } long? _ColBigInt;
        /// <summary>bigint unsignedの列</summary>
        [Column] public ulong? ColBigIntU { get { return _ColBigIntU; } set { _ColBigIntU = value; MarkColumnModified("ColBigIntU"); } } ulong? _ColBigIntU;
        /// <summary>intの列</summary>
        [Column] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("ColInt"); } } int? _ColInt;
        /// <summary>int unsignedの列</summary>
        [Column] public uint? ColIntU { get { return _ColIntU; } set { _ColIntU = value; MarkColumnModified("ColIntU"); } } uint? _ColIntU;
        /// <summary>mediumintの列</summary>
        [Column] public int? ColMediumInt { get { return _ColMediumInt; } set { _ColMediumInt = value; MarkColumnModified("ColMediumInt"); } } int? _ColMediumInt;
        /// <summary>mediumint unsignedの列</summary>
        [Column] public uint? ColMediumIntU { get { return _ColMediumIntU; } set { _ColMediumIntU = value; MarkColumnModified("ColMediumIntU"); } } uint? _ColMediumIntU;
        /// <summary>smallintの列</summary>
        [Column] public short? ColSmallInt { get { return _ColSmallInt; } set { _ColSmallInt = value; MarkColumnModified("ColSmallInt"); } } short? _ColSmallInt;
        /// <summary>smallint unsignedの列</summary>
        [Column] public ushort? ColSmallIntU { get { return _ColSmallIntU; } set { _ColSmallIntU = value; MarkColumnModified("ColSmallIntU"); } } ushort? _ColSmallIntU;
        /// <summary>tinyintの列</summary>
        [Column] public sbyte? ColTinyInt { get { return _ColTinyInt; } set { _ColTinyInt = value; MarkColumnModified("ColTinyInt"); } } sbyte? _ColTinyInt;
        /// <summary>tinyint unsignedの列</summary>
        [Column] public byte? ColTinyIntU { get { return _ColTinyIntU; } set { _ColTinyIntU = value; MarkColumnModified("ColTinyIntU"); } } byte? _ColTinyIntU;
        /// <summary>bitの列</summary>
        [Column] public bool? ColBit { get { return _ColBit; } set { _ColBit = value; MarkColumnModified("ColBit"); } } bool? _ColBit;
        /// <summary>boolの列</summary>
        [Column] public bool? ColBool { get { return _ColBool; } set { _ColBool = value; MarkColumnModified("ColBool"); } } bool? _ColBool;
        /// <summary>decimalの列</summary>
        [Column] public decimal? ColDecimal { get { return _ColDecimal; } set { _ColDecimal = value; MarkColumnModified("ColDecimal"); } } decimal? _ColDecimal;
        /// <summary>numericの列</summary>
        [Column] public decimal? ColNumeric { get { return _ColNumeric; } set { _ColNumeric = value; MarkColumnModified("ColNumeric"); } } decimal? _ColNumeric;
        /// <summary>doubleの列</summary>
        [Column] public double? ColDouble { get { return _ColDouble; } set { _ColDouble = value; MarkColumnModified("ColDouble"); } } double? _ColDouble;
        /// <summary>floatの列</summary>
        [Column] public float? ColFloat { get { return _ColFloat; } set { _ColFloat = value; MarkColumnModified("ColFloat"); } } float? _ColFloat;
        /// <summary>dateの列</summary>
        [Column] public DateTime? ColDate { get { return _ColDate; } set { _ColDate = value; MarkColumnModified("ColDate"); } } DateTime? _ColDate;
        /// <summary>timeの列</summary>
        [Column] public TimeSpan? ColTime { get { return _ColTime; } set { _ColTime = value; MarkColumnModified("ColTime"); } } TimeSpan? _ColTime;
        /// <summary>datetimeの列</summary>
        [Column] public DateTime? ColDateTime { get { return _ColDateTime; } set { _ColDateTime = value; MarkColumnModified("ColDateTime"); } } DateTime? _ColDateTime;
        /// <summary>timestampの列</summary>
        [Column] public DateTime ColTimeStamp { get { return _ColTimeStamp; } set { _ColTimeStamp = value; MarkColumnModified("ColTimeStamp"); } } DateTime _ColTimeStamp;
        /// <summary>char(5)の列</summary>
        [Column] public string ColChar { get { return _ColChar; } set { _ColChar = value; MarkColumnModified("ColChar"); } } string _ColChar;
        /// <summary>varchar(50)の列</summary>
        [Column] public string ColVarchar { get { return _ColVarchar; } set { _ColVarchar = value; MarkColumnModified("ColVarchar"); } } string _ColVarchar;
        /// <summary>longtextの列</summary>
        [Column] public string ColLongText { get { return _ColLongText; } set { _ColLongText = value; MarkColumnModified("ColLongText"); } } string _ColLongText;
        /// <summary>longblobの列</summary>
        [Column] public byte[] ColLongBlob { get { return _ColLongBlob; } set { _ColLongBlob = value; MarkColumnModified("ColLongBlob"); } } byte[] _ColLongBlob;
    }

    /// <summary>テストテーブル - 複合キー</summary>
    [TableName("TrCompositeKey")]
    [PrimaryKey("Key01,Key02", AutoIncrement=false)]
    [ExplicitColumns]
    public class TrCompositeKey: PetaPoco.PetaPocoRecord<TrCompositeKey>
    {
        /// <summary>プライマリキーその１</summary>
        [Column] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("Key01"); } } string _Key01;
        /// <summary>プライマリキーその２</summary>
        [Column] public int Key02 { get { return _Key02; } set { _Key02 = value; MarkColumnModified("Key02"); } } int _Key02;
        /// <summary>boolの列</summary>
        [Column] public bool ColBool { get { return _ColBool; } set { _ColBool = value; MarkColumnModified("ColBool"); } } bool _ColBool;
        /// <summary>intの列</summary>
        [Column] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("ColInt"); } } int? _ColInt;
        /// <summary>decimalの列</summary>
        [Column] public decimal? ColDec { get { return _ColDec; } set { _ColDec = value; MarkColumnModified("ColDec"); } } decimal? _ColDec;
        /// <summary>varcharの列</summary>
        [Column] public string ColVarchar { get { return _ColVarchar; } set { _ColVarchar = value; MarkColumnModified("ColVarchar"); } } string _ColVarchar;
        /// <summary>更新者</summary>
        [Column] public string UpdateBy { get { return _UpdateBy; } set { _UpdateBy = value; MarkColumnModified("UpdateBy"); } } string _UpdateBy;
        /// <summary>更新日時</summary>
        [Column] public DateTime UpdateDt { get { return _UpdateDt; } set { _UpdateDt = value; MarkColumnModified("UpdateDt"); } } DateTime _UpdateDt;
    }

    /// <summary>VIEW</summary>
    [TableName("ViHogeFuga")]
    [ExplicitColumns]
    public class ViHogeFuga
    {
        /// <summary>プライマリキーその１</summary>
        [Column] public string Tbl1Key01 { get; set; }
        /// <summary>プライマリキーその２</summary>
        [Column] public int Tbl1Key02 { get; set; }
        /// <summary>boolの列</summary>
        [Column] public bool Tbl1ColBool { get; set; }
        /// <summary>intの列</summary>
        [Column] public int? Tbl1ColInt { get; set; }
        /// <summary>decimalの列</summary>
        [Column] public decimal? Tbl1ColDec { get; set; }
        /// <summary>varcharの列</summary>
        [Column] public string Tbl1ColVarchar { get; set; }
        /// <summary>更新者</summary>
        [Column] public string Tbl1UpdateBy { get; set; }
        /// <summary>更新日時</summary>
        [Column] public DateTime Tbl1UpdateDt { get; set; }
        /// <summary>intの列</summary>
        [Column] public int? Tbl2ColInt { get; set; }
    }
}


