using System;
using PetaPoco;

namespace PetaPoco4Core.Test.PostgreSql
{
    /// <summary>PT テストテーブル01（PK 1列）</summary>
    [TableName("pt_table01")]
    [PrimaryKey("key01", AutoIncrement = false)]
    [ExplicitColumns]
    public class PtTable01 : PetaPoco.PetaPocoRecord<PtTable01>
    {
        /// <summary></summary>
        [Column(Name = "key01")] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("key01"); } }
        string _Key01;
        /// <summary></summary>
        [Column(Name = "col_bool")] public bool ColBool { get { return _ColBool; } set { _ColBool = value; MarkColumnModified("col_bool"); } }
        bool _ColBool;
        /// <summary></summary>
        [Column(Name = "col_int")] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("col_int"); } }
        int? _ColInt;
        /// <summary></summary>
        [Column(Name = "col_dec")] public decimal? ColDec { get { return _ColDec; } set { _ColDec = value; MarkColumnModified("col_dec"); } }
        decimal? _ColDec;
        /// <summary></summary>
        [Column(Name = "col_varchar")] public string ColVarchar { get { return _ColVarchar; } set { _ColVarchar = value; MarkColumnModified("col_varchar"); } }
        string _ColVarchar;
        /// <summary></summary>
        [Column(Name = "create_by")] public string CreateBy { get { return _CreateBy; } set { _CreateBy = value; MarkColumnModified("create_by"); } }
        string _CreateBy;
        /// <summary></summary>
        [Column(Name = "create_dt")] public DateTime CreateDt { get { return _CreateDt; } set { _CreateDt = value; MarkColumnModified("create_dt"); } }
        DateTime _CreateDt;
        /// <summary></summary>
        [Column(Name = "update_by")] public string UpdateBy { get { return _UpdateBy; } set { _UpdateBy = value; MarkColumnModified("update_by"); } }
        string _UpdateBy;
        /// <summary></summary>
        [Column(Name = "update_dt")] public DateTime UpdateDt { get { return _UpdateDt; } set { _UpdateDt = value; MarkColumnModified("update_dt"); } }
        DateTime _UpdateDt;
    }

    /// <summary>PT テストテーブル02（PK 2列）</summary>
    [TableName("pt_table02")]
    [PrimaryKey("key01,key02", AutoIncrement = false)]
    [ExplicitColumns]
    public class PtTable02 : PetaPoco.PetaPocoRecord<PtTable02>
    {
        /// <summary></summary>
        [Column(Name = "key01")] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("key01"); } }
        string _Key01;
        /// <summary></summary>
        [Column(Name = "key02")] public int Key02 { get { return _Key02; } set { _Key02 = value; MarkColumnModified("key02"); } }
        int _Key02;
        /// <summary></summary>
        [Column(Name = "col_bool")] public bool ColBool { get { return _ColBool; } set { _ColBool = value; MarkColumnModified("col_bool"); } }
        bool _ColBool;
        /// <summary></summary>
        [Column(Name = "col_int")] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("col_int"); } }
        int? _ColInt;
        /// <summary></summary>
        [Column(Name = "col_dec")] public decimal? ColDec { get { return _ColDec; } set { _ColDec = value; MarkColumnModified("col_dec"); } }
        decimal? _ColDec;
        /// <summary></summary>
        [Column(Name = "col_varchar")] public string ColVarchar { get { return _ColVarchar; } set { _ColVarchar = value; MarkColumnModified("col_varchar"); } }
        string _ColVarchar;
        /// <summary></summary>
        [Column(Name = "create_by")] public string CreateBy { get { return _CreateBy; } set { _CreateBy = value; MarkColumnModified("create_by"); } }
        string _CreateBy;
        /// <summary></summary>
        [Column(Name = "create_dt")] public DateTime CreateDt { get { return _CreateDt; } set { _CreateDt = value; MarkColumnModified("create_dt"); } }
        DateTime _CreateDt;
        /// <summary></summary>
        [Column(Name = "update_by")] public string UpdateBy { get { return _UpdateBy; } set { _UpdateBy = value; MarkColumnModified("update_by"); } }
        string _UpdateBy;
        /// <summary></summary>
        [Column(Name = "update_dt")] public DateTime UpdateDt { get { return _UpdateDt; } set { _UpdateDt = value; MarkColumnModified("update_dt"); } }
        DateTime _UpdateDt;
    }

    /// <summary>結合クエリ用</summary>
    public class JoinTest
    {
        public string Key01 { get; set; }
        public bool ColBool { get; set; }
        public int? ColInt { get; set; }
        public decimal? ColDec { get; set; }
        public string ColVarchar { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDt { get; set; }

        public string ColVarchar02 { get; set; }
        public DateTime CreateDt02 { get; set; }
    }

    /// <summary>情報が不完全なpost</summary>
    public class PtTableDefective : PetaPoco.PetaPocoRecord<PtTableDefective>
    {
        /// <summary></summary>
        [Column(Name = "key01")] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("key01"); } }
        string _Key01;
        /// <summary></summary>
        [Column(Name = "key02")] public int Key02 { get { return _Key02; } set { _Key02 = value; MarkColumnModified("key02"); } }
        int _Key02;
        /// <summary></summary>
        [Column(Name = "col_bool")] public bool ColBool { get { return _ColBool; } set { _ColBool = value; MarkColumnModified("col_bool"); } }
        bool _ColBool;
        /// <summary></summary>
        [Column(Name = "col_int")] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("col_int"); } }
        int? _ColInt;
        /// <summary></summary>
        [Column(Name = "col_dec")] public decimal? ColDec { get { return _ColDec; } set { _ColDec = value; MarkColumnModified("col_dec"); } }
        decimal? _ColDec;
        /// <summary></summary>
        [Column(Name = "col_varchar")] public string ColVarchar { get { return _ColVarchar; } set { _ColVarchar = value; MarkColumnModified("col_varchar"); } }
        string _ColVarchar;
        /// <summary></summary>
        [Column(Name = "create_by")] public string CreateBy { get { return _CreateBy; } set { _CreateBy = value; MarkColumnModified("create_by"); } }
        string _CreateBy;
        /// <summary></summary>
        [Column(Name = "create_dt")] public DateTime CreateDt { get { return _CreateDt; } set { _CreateDt = value; MarkColumnModified("create_dt"); } }
        DateTime _CreateDt;
        /// <summary></summary>
        [Column(Name = "update_by")] public string UpdateBy { get { return _UpdateBy; } set { _UpdateBy = value; MarkColumnModified("update_by"); } }
        string _UpdateBy;
        /// <summary></summary>
        [Column(Name = "update_dt")] public DateTime UpdateDt { get { return _UpdateDt; } set { _UpdateDt = value; MarkColumnModified("update_dt"); } }
        DateTime _UpdateDt;
    }
}
