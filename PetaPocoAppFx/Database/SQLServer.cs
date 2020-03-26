﻿
// This file was automatically generated by the PetaPoco T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String:      `Data Source=192.168.1.210;Database=PetaPocoSample;Integrated Security=False;User ID=testman;Password=**zapped**;Pooling=true;`
//     Provider:               `System.Data.SqlClient`
//     Schema:                 ``
//     Include Views:          `True`
//     Genetated:              `2020/03/07 00:05:41`

using System;
using PetaPoco;

namespace PetaPocoAppFx.Database.SQLServer
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
        public DB() : base(Config.ConnectionString, RDBType.SqlServer)
        {
            CommandTimeout = 30;
            UseA5Mk2Params = false;
        }
    }


    /// <summary>テストテーブル - オートナンバー</summary>
    [TableName("TrAutoNumber")]
    [PrimaryKey("Key03", AutoIncrement=true)]
    [ExplicitColumns]
    public class TrAutoNumber: PetaPoco.PetaPocoRecord<TrAutoNumber>
    {
        /// <summary>オートナンバーキー</summary>
        [Column] public int Key03 { get { return _Key03; } set { _Key03 = value; MarkColumnModified("Key03"); } } int _Key03;
        /// <summary>プライマリキーその１</summary>
        [Column] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("Key01"); } } string _Key01;
        /// <summary>int型の列</summary>
        [Column] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("ColInt"); } } int? _ColInt;
    }

    /// <summary>テストテーブル - 列の型テスト</summary>
    [TableName("TrColumns")]
    [PrimaryKey("Key01", AutoIncrement=false)]
    [ExplicitColumns]
    public class TrColumn: PetaPoco.PetaPocoRecord<TrColumn>
    {
        /// <summary></summary>
        [Column] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("Key01"); } } string _Key01;
        /// <summary>bigintの列</summary>
        [Column] public long? ColBigInt { get { return _ColBigInt; } set { _ColBigInt = value; MarkColumnModified("ColBigInt"); } } long? _ColBigInt;
        /// <summary>intの列</summary>
        [Column] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("ColInt"); } } int? _ColInt;
        /// <summary>smallintの列</summary>
        [Column] public short? ColSmallInt { get { return _ColSmallInt; } set { _ColSmallInt = value; MarkColumnModified("ColSmallInt"); } } short? _ColSmallInt;
        /// <summary>tinyintの列</summary>
        [Column] public byte? ColTinyInt { get { return _ColTinyInt; } set { _ColTinyInt = value; MarkColumnModified("ColTinyInt"); } } byte? _ColTinyInt;
        /// <summary>bitの列</summary>
        [Column] public bool? ColBit { get { return _ColBit; } set { _ColBit = value; MarkColumnModified("ColBit"); } } bool? _ColBit;
        /// <summary>decimal(10,2)の列</summary>
        [Column] public decimal? ColDecimal { get { return _ColDecimal; } set { _ColDecimal = value; MarkColumnModified("ColDecimal"); } } decimal? _ColDecimal;
        /// <summary>numeric(12,0)の列</summary>
        [Column] public decimal? ColNumeric { get { return _ColNumeric; } set { _ColNumeric = value; MarkColumnModified("ColNumeric"); } } decimal? _ColNumeric;
        /// <summary>moneyの列</summary>
        [Column] public decimal? ColMoney { get { return _ColMoney; } set { _ColMoney = value; MarkColumnModified("ColMoney"); } } decimal? _ColMoney;
        /// <summary>floatの列</summary>
        [Column] public double? ColFloat { get { return _ColFloat; } set { _ColFloat = value; MarkColumnModified("ColFloat"); } } double? _ColFloat;
        /// <summary>realの列</summary>
        [Column] public float? ColReal { get { return _ColReal; } set { _ColReal = value; MarkColumnModified("ColReal"); } } float? _ColReal;
        /// <summary>datetimeの列</summary>
        [Column] public DateTime? ColDateTime { get { return _ColDateTime; } set { _ColDateTime = value; MarkColumnModified("ColDateTime"); } } DateTime? _ColDateTime;
        /// <summary>datetime2の列</summary>
        [Column] public DateTime? ColDateTime2 { get { return _ColDateTime2; } set { _ColDateTime2 = value; MarkColumnModified("ColDateTime2"); } } DateTime? _ColDateTime2;
        /// <summary>smalldatetimeの列</summary>
        [Column] public DateTime? ColSmallDateTime { get { return _ColSmallDateTime; } set { _ColSmallDateTime = value; MarkColumnModified("ColSmallDateTime"); } } DateTime? _ColSmallDateTime;
        /// <summary>dateの列</summary>
        [Column] public DateTime? ColDate { get { return _ColDate; } set { _ColDate = value; MarkColumnModified("ColDate"); } } DateTime? _ColDate;
        /// <summary>timeの列</summary>
        [Column] public TimeSpan? ColTime { get { return _ColTime; } set { _ColTime = value; MarkColumnModified("ColTime"); } } TimeSpan? _ColTime;
        /// <summary>char(5)の列</summary>
        [Column] public string ColChar { get { return _ColChar; } set { _ColChar = value; MarkColumnModified("ColChar"); } } string _ColChar;
        /// <summary>nchar(5)の列</summary>
        [Column] public string ColNchar { get { return _ColNchar; } set { _ColNchar = value; MarkColumnModified("ColNchar"); } } string _ColNchar;
        /// <summary>varchar(50)の列</summary>
        [Column] public string ColVarChar { get { return _ColVarChar; } set { _ColVarChar = value; MarkColumnModified("ColVarChar"); } } string _ColVarChar;
        /// <summary>nvarchar(50)の列</summary>
        [Column] public string ColNvarChar { get { return _ColNvarChar; } set { _ColNvarChar = value; MarkColumnModified("ColNvarChar"); } } string _ColNvarChar;
        /// <summary>textの列</summary>
        [Column] public string ColText { get { return _ColText; } set { _ColText = value; MarkColumnModified("ColText"); } } string _ColText;
        /// <summary>ntextの列</summary>
        [Column] public string ColNText { get { return _ColNText; } set { _ColNText = value; MarkColumnModified("ColNText"); } } string _ColNText;
        /// <summary>imageの列</summary>
        [Column] public byte[] ColImage { get { return _ColImage; } set { _ColImage = value; MarkColumnModified("ColImage"); } } byte[] _ColImage;
        /// <summary>varbinaryの列</summary>
        [Column] public byte[] ColVarBinary { get { return _ColVarBinary; } set { _ColVarBinary = value; MarkColumnModified("ColVarBinary"); } } byte[] _ColVarBinary;
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
        /// <summary>bit型の列</summary>
        [Column] public bool ColBool { get { return _ColBool; } set { _ColBool = value; MarkColumnModified("ColBool"); } } bool _ColBool;
        /// <summary>int型の列</summary>
        [Column] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("ColInt"); } } int? _ColInt;
        /// <summary>decimal型の列</summary>
        [Column] public decimal? ColDec { get { return _ColDec; } set { _ColDec = value; MarkColumnModified("ColDec"); } } decimal? _ColDec;
        /// <summary>varchar型の列</summary>
        [Column] public string ColVarchar { get { return _ColVarchar; } set { _ColVarchar = value; MarkColumnModified("ColVarchar"); } } string _ColVarchar;
        /// <summary>作成者</summary>
        [Column] public string CreateBy { get { return _CreateBy; } set { _CreateBy = value; MarkColumnModified("CreateBy"); } } string _CreateBy;
        /// <summary>作成日時</summary>
        [Column] public DateTime CreateDt { get { return _CreateDt; } set { _CreateDt = value; MarkColumnModified("CreateDt"); } } DateTime _CreateDt;
        /// <summary>更新者</summary>
        [Column] public string UpdateBy { get { return _UpdateBy; } set { _UpdateBy = value; MarkColumnModified("UpdateBy"); } } string _UpdateBy;
        /// <summary>更新日時</summary>
        [Column] public DateTime UpdateDt { get { return _UpdateDt; } set { _UpdateDt = value; MarkColumnModified("UpdateDt"); } } DateTime _UpdateDt;
    }

    /// <summary>テストビュー その1</summary>
    [TableName("ViHogeFuga")]
    [ExplicitColumns]
    public class ViHogeFuga
    {
        /// <summary></summary>
        [Column] public string Tbl1Key01 { get; set; }
        /// <summary></summary>
        [Column] public int Tbl1Key02 { get; set; }
        /// <summary></summary>
        [Column] public bool Tbl1ColBool { get; set; }
        /// <summary></summary>
        [Column] public int? Tbl1ColInt { get; set; }
        /// <summary></summary>
        [Column] public decimal? Tbl1ColDec { get; set; }
        /// <summary></summary>
        [Column] public string Tbl1ColVarchar { get; set; }
        /// <summary></summary>
        [Column] public string Tbl1UpdateBy { get; set; }
        /// <summary></summary>
        [Column] public DateTime Tbl1UpdateDt { get; set; }
        /// <summary></summary>
        [Column] public int? Tbl2ColInt { get; set; }
    }
}

