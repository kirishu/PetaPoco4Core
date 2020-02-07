using System;
using PetaPoco;

namespace PetaPoco4Core.Test.MySql
{
    /// <summary>PT テストテーブル01（PK 1列）</summary>
    [TableName("PtTable01")]
    [PrimaryKey("Key01", AutoIncrement = false)]
    [ExplicitColumns]
    public class PtTable01 : PetaPoco.PetaPocoRecord<PtTable01>
    {
        /// <summary></summary>
        [Column] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("Key01"); } }
        string _Key01;
        /// <summary></summary>
        [Column] public bool ColBool { get { return _ColBool; } set { _ColBool = value; MarkColumnModified("ColBool"); } }
        bool _ColBool;
        /// <summary></summary>
        [Column] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("ColInt"); } }
        int? _ColInt;
        /// <summary></summary>
        [Column] public decimal? ColDec { get { return _ColDec; } set { _ColDec = value; MarkColumnModified("ColDec"); } }
        decimal? _ColDec;
        /// <summary></summary>
        [Column] public string ColVarchar { get { return _ColVarchar; } set { _ColVarchar = value; MarkColumnModified("ColVarchar"); } }
        string _ColVarchar;
        /// <summary></summary>
        [Column] public string CreateBy { get { return _CreateBy; } set { _CreateBy = value; MarkColumnModified("CreateBy"); } }
        string _CreateBy;
        /// <summary></summary>
        [Column] public DateTime CreateDt { get { return _CreateDt; } set { _CreateDt = value; MarkColumnModified("CreateDt"); } }
        DateTime _CreateDt;
        /// <summary></summary>
        [Column] public string UpdateBy { get { return _UpdateBy; } set { _UpdateBy = value; MarkColumnModified("UpdateBy"); } }
        string _UpdateBy;
        /// <summary></summary>
        [Column] public DateTime UpdateDt { get { return _UpdateDt; } set { _UpdateDt = value; MarkColumnModified("UpdateDt"); } }
        DateTime _UpdateDt;
    }

    /// <summary>PT テストテーブル02（PK 2列）</summary>
    [TableName("PtTable02")]
    [PrimaryKey("Key01,Key02", AutoIncrement = false)]
    [ExplicitColumns]
    public class PtTable02 : PetaPoco.PetaPocoRecord<PtTable02>
    {
        /// <summary></summary>
        [Column] public string Key01 { get { return _Key01; } set { _Key01 = value; MarkColumnModified("Key01"); } }
        string _Key01;
        /// <summary></summary>
        [Column] public int Key02 { get { return _Key02; } set { _Key02 = value; MarkColumnModified("Key02"); } }
        int _Key02;
        /// <summary></summary>
        [Column] public bool ColBool { get { return _ColBool; } set { _ColBool = value; MarkColumnModified("ColBool"); } }
        bool _ColBool;
        /// <summary></summary>
        [Column] public int? ColInt { get { return _ColInt; } set { _ColInt = value; MarkColumnModified("ColInt"); } }
        int? _ColInt;
        /// <summary></summary>
        [Column] public decimal? ColDec { get { return _ColDec; } set { _ColDec = value; MarkColumnModified("ColDec"); } }
        decimal? _ColDec;
        /// <summary></summary>
        [Column] public string ColVarchar { get { return _ColVarchar; } set { _ColVarchar = value; MarkColumnModified("ColVarchar"); } }
        string _ColVarchar;
        /// <summary></summary>
        [Column] public string CreateBy { get { return _CreateBy; } set { _CreateBy = value; MarkColumnModified("CreateBy"); } }
        string _CreateBy;
        /// <summary></summary>
        [Column] public DateTime CreateDt { get { return _CreateDt; } set { _CreateDt = value; MarkColumnModified("CreateDt"); } }
        DateTime _CreateDt;
        /// <summary></summary>
        [Column] public string UpdateBy { get { return _UpdateBy; } set { _UpdateBy = value; MarkColumnModified("UpdateBy"); } }
        string _UpdateBy;
        /// <summary></summary>
        [Column] public DateTime UpdateDt { get { return _UpdateDt; } set { _UpdateDt = value; MarkColumnModified("UpdateDt"); } }
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
}
