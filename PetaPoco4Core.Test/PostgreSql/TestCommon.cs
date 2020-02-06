using System;
using PetaPoco;

namespace PetaPoco4Core.Test.PostgreSql
{
    /// <summary>
    /// Database Object
    /// </summary>
    public class DB : PetaPoco.DatabaseExtension
    {
        public static readonly string Constr = "Server=localhost;Port=5432;Database=dvdrental;Encoding=UTF8;User Id=testman;Password=testpwd;";

        /// <summary>
        /// employees Database Object
        /// </summary>
        public DB() : base(Constr, DBType.PostgreSQL)
        {
        }
    }

    /// <summary>
    /// テスト共通
    /// </summary>
    public sealed class TestCommon: ITestCommon
    {
        // singleton instance
        private static readonly TestCommon _instance = new TestCommon();

        public static TestCommon Instance {  get { return _instance; } }

        private TestCommon() { }

        /// <summary>
        /// 初期処理
        /// </summary>
        public void Initialize()
        {
            // DB接続
            using (var db = new DB())
            {
                // pt_table01の作成
                CreateTable01(db);
                // pt_table02の作成
                CreateTable02(db);
            }
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        public void Cleanup()
        {
            // DB接続
            using (var db = new DB())
            {
                // 存在していたらDROP
                db.Execute("DROP TABLE IF EXISTS pt_table01");
                db.Execute("DROP TABLE IF EXISTS pt_table02");
                //var checktable01 = db.ExecuteScalar<string>("SELECT relname FROM pg_class WHERE relkind = 'r' AND relname = @0", "pt_table01");
                //if (!string.IsNullOrWhiteSpace(checktable01))
                //{
                //    db.Execute("DROP TABLE pt_table01");
                //}

                //var checktable02 = db.ExecuteScalar<string>("SELECT relname FROM pg_class WHERE relkind = 'r' AND relname = @0", "pt_table02");
                //if (!string.IsNullOrWhiteSpace(checktable02))
                //{
                //    db.Execute("DROP TABLE pt_table02");
                //}
            }
        }

        private void CreateTable01(DB db)
        {
            //// 存在していたらDROP
            //db.Execute("DROP TABLE IF EXISTS pt_table01");

            // 存在していたら作成しない
            var tablenm = db.ExecuteScalar<string>("SELECT relname FROM pg_class WHERE relkind = 'r' AND relname = @0", "pt_table01");
            if (!string.IsNullOrWhiteSpace(tablenm))
            {
                return;
            }

            // CREATE
            var sql = PetaPoco.Sql.Builder
                .Append("CREATE TABLE pt_table01 (")
                .Append("      key01                  VARCHAR(2)        NOT NULL")
                .Append("    , col_bool               bool              NOT NULL")
                .Append("    , col_int                int")
                .Append("    , col_dec                decimal(10,2)")
                .Append("    , col_varchar            VARCHAR(20)")
                .Append("    , create_by              VARCHAR(30)       NOT NULL")
                .Append("    , create_dt              TIMESTAMP(3)      NOT NULL")
                .Append("    , update_by              VARCHAR(30)       NOT NULL")
                .Append("    , update_dt              TIMESTAMP(3)      NOT NULL")
                .Append("    , CONSTRAINT PK_pt_table01 PRIMARY KEY (key01)")
                .Append(");");
            db.Execute(sql);

            // データINSERT
            db.Execute("INSERT INTO pt_table01 values ('01',true,999,123456.78,'北海道','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('02',true,999,123456.78,'青森県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('03',true,999,123456.78,'岩手県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('04',true,999,123456.78,'宮城県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('05',true,999,123456.78,'秋田県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('06',true,999,123456.78,'山形県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('07',true,999,123456.78,'福島県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('08',true,999,123456.78,'茨城県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('09',true,999,123456.78,'栃木県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('10',true,999,123456.78,'群馬県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('11',true,999,123456.78,'埼玉県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('12',true,999,123456.78,'千葉県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('13',true,999,123456.78,'東京都','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('14',true,999,123456.78,'神奈川県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table01 values ('15',true,999,123456.78,'新潟県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");

        }

        private void CreateTable02(DB db)
        {
            //// 存在していたらDROP
            //db.Execute("DROP TABLE IF EXISTS pt_table02");

            // 存在していたら作成しない
            var tablenm = db.ExecuteScalar<string>("SELECT relname FROM pg_class WHERE relkind = 'r' AND relname = @0", "pt_table02");
            if (!string.IsNullOrWhiteSpace(tablenm))
            {
                return;
            }

            // CREATE
            var sql = PetaPoco.Sql.Builder
                .Append("CREATE TABLE pt_table02 (")
                .Append("      key01                  VARCHAR(2)        NOT NULL")
                .Append("    , key02                  int               NOT NULL")
                .Append("    , col_bool               bool              NOT NULL")
                .Append("    , col_int                int")
                .Append("    , col_dec                decimal(10,2)")
                .Append("    , col_varchar            VARCHAR(20)")
                .Append("    , create_by              VARCHAR(30)       NOT NULL")
                .Append("    , create_dt              TIMESTAMP(3)      NOT NULL")
                .Append("    , update_by              VARCHAR(30)       NOT NULL")
                .Append("    , update_dt              TIMESTAMP(3)      NOT NULL")
                .Append("    , CONSTRAINT PK_pt_table02 PRIMARY KEY (key01, key02)")
                .Append(");");
            db.Execute(sql);

            // データINSERT
            db.Execute("INSERT INTO pt_table02 values ('01',01,true,999,123456.78,'北海道','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('02',02,true,999,123456.78,'青森県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('03',03,true,999,123456.78,'岩手県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('04',04,true,999,123456.78,'宮城県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('05',05,true,999,123456.78,'秋田県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('06',06,true,999,123456.78,'山形県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('07',07,true,999,123456.78,'福島県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('08',08,true,999,123456.78,'茨城県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('09',09,true,999,123456.78,'栃木県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('10',10,true,999,123456.78,'群馬県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('11',11,true,999,123456.78,'埼玉県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('12',12,true,999,123456.78,'千葉県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('13',13,true,999,123456.78,'東京都','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('14',14,true,999,123456.78,'神奈川県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO pt_table02 values ('15',15,true,999,123456.78,'新潟県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");

        }


    }

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
