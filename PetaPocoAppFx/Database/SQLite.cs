using System;
using PetaPoco;

namespace PetaPocoAppFx.Database.SQLite
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
        public DB() : base(Config.ConnectionString, RDBType.SQLite)
        {
            CommandTimeout = 30;
            UseA5Mk2Params = true;
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
        /// <summary>プライマリキーその１</summary>
        [Column] public string Key01 { get; set; }
        /// <summary>bigintの列</summary>
        [Column] public long? ColBigInt { get; set; }
        /// <summary>intの列</summary>
        [Column] public int? ColInt { get; set; }
        /// <summary>int unsignedの列</summary>
        [Column] public long? ColIntU { get; set; }
        /// <summary>smallintの列</summary>
        [Column] public short? ColSmallInt { get; set; }
        /// <summary>tinyintの列</summary>
        [Column] public short? ColTinyInt { get; set; }
        /// <summary>boolの列</summary>
        [Column] public bool? ColBool { get; set; }
        /// <summary>decimalの列</summary>
        [Column] public decimal? ColDecimal { get; set; }
        /// <summary>numericの列</summary>
        [Column] public decimal? ColNumeric { get; set; }
        /// <summary>doubleの列</summary>
        [Column] public double? ColDouble { get; set; }
        /// <summary>floatの列</summary>
        [Column] public float? ColFloat { get; set; }
        /// <summary>dateの列</summary>
        [Column] public DateTime? ColDate { get; set; }
        /// <summary>timeの列</summary>
        [Column] public DateTime? ColTime { get; set; }
        /// <summary>datetimeの列</summary>
        [Column] public DateTime? ColDateTime { get; set; }
        /// <summary>timestampの列</summary>
        [Column] public DateTime ColTimeStamp { get; set; }
        /// <summary>char(5)の列</summary>
        [Column] public string ColChar { get; set; }
        /// <summary>varchar(50)の列</summary>
        [Column] public string ColVarchar { get; set; }
        /// <summary>longblobの列</summary>
        [Column] public byte[] ColBlob { get; set; }
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
        [Column] public long Key02 { get { return _Key02; } set { _Key02 = value; MarkColumnModified("Key02"); } } long _Key02;
        /// <summary>bit型の列</summary>
        [Column] public bool ColBool { get { return _ColBool; } set { _ColBool = value; MarkColumnModified("ColBool"); } } bool _ColBool;
        /// <summary>int型の列</summary>
        [Column] public long? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("ColInt"); } } long? _ColInt;
        /// <summary>decimal型の列</summary>
        [Column] public decimal? ColDec { get { return _ColDec; } set { _ColDec = value; MarkColumnModified("ColDec"); } } decimal? _ColDec;
        /// <summary>varchar型の列</summary>
        [Column] public string ColVarchar { get { return _ColVarchar; } set { _ColVarchar = value; MarkColumnModified("ColVarchar"); } } string _ColVarchar;
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


