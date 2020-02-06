using System;
using PetaPoco;

namespace PetaPoco4Core.Test.MySql
{
    /// <summary>
    /// Database Object
    /// </summary>
    public class DB : PetaPoco.DatabaseExtension
    {
        public static readonly string Constr = "Server=localhost;Database=employees;uid=testman;pwd=testpwd;SslMode=None;";

        /// <summary>
        /// employees Database Object
        /// </summary>
        public DB() : base(Constr, DBType.MySql)
        {
        }
    }

    public class MySqlEnv: ITestCommon
    {
        // singleton instance
        private static readonly MySqlEnv _instance = new MySqlEnv();

        public static MySqlEnv Instance {  get { return _instance; } }

        private MySqlEnv() { }

        /// <summary>
        /// 初期処理
        /// </summary>
        public void Initialize()
        {
            // DB接続
            using (var db = new DB())
            {
                // PtTable01の作成
                CreateTable01(db);
                // PtTable02の作成
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
                //var checktable01 = db.ExecuteScalar<string>("SELECT relname FROM pg_class WHERE relkind = 'r' AND relname = @0", "PtTable01");
                //if (!string.IsNullOrWhiteSpace(checktable01))
                //{
                //    db.Execute("DROP TABLE PtTable01");
                //}

                //var checktable02 = db.ExecuteScalar<string>("SELECT relname FROM pg_class WHERE relkind = 'r' AND relname = @0", "PtTable02");
                //if (!string.IsNullOrWhiteSpace(checktable02))
                //{
                //    db.Execute("DROP TABLE PtTable02");
                //}
                db.Execute("DROP TABLE IF EXISTS PtTable01");
                db.Execute("DROP TABLE IF EXISTS PtTable02");
            }
        }

        private  void CreateTable01(DB db)
        {
            //// 存在していたらDROP
            //db.Execute("DROP TABLE IF EXISTS PtTable01");

            // CREATE
            var sql = PetaPoco.Sql.Builder
                .Append("CREATE TABLE IF NOT EXISTS PtTable01 (")
                .Append("      Key01                 VARCHAR(2)        NOT NULL")
                .Append("    , ColBool               bool              NOT NULL")
                .Append("    , ColInt                int")
                .Append("    , ColDec                decimal(10,2)")
                .Append("    , ColVarchar            VARCHAR(20)")
                .Append("    , CreateBy              VARCHAR(30)       NOT NULL")
                .Append("    , CreateDt              DATETIME          NOT NULL")
                .Append("    , UpdateBy              VARCHAR(30)       NOT NULL")
                .Append("    , UpdateDt              DATETIME          NOT NULL")
                .Append("    , CONSTRAINT PK_PtTable01 PRIMARY KEY (Key01)")
                .Append(");");
            db.Execute(sql);

            // データINSERT
            db.Execute("INSERT INTO PtTable01 values ('01',true,999,123456.78,'北海道','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('02',true,999,123456.78,'青森県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('03',true,999,123456.78,'岩手県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('04',true,999,123456.78,'宮城県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('05',true,999,123456.78,'秋田県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('06',true,999,123456.78,'山形県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('07',true,999,123456.78,'福島県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('08',true,999,123456.78,'茨城県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('09',true,999,123456.78,'栃木県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('10',true,999,123456.78,'群馬県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('11',true,999,123456.78,'埼玉県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('12',true,999,123456.78,'千葉県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('13',true,999,123456.78,'東京都','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('14',true,999,123456.78,'神奈川県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('15',true,999,123456.78,'新潟県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");

        }

        private void CreateTable02(DB db)
        {
            // 存在していたらDROP
            db.Execute("DROP TABLE IF EXISTS PtTable02");

            // CREATE
            var sql = PetaPoco.Sql.Builder
                .Append("CREATE TABLE IF NOT EXISTS PtTable02 (")
                .Append("      Key01                 VARCHAR(2)        NOT NULL")
                .Append("    , Key02                 int               NOT NULL")
                .Append("    , ColBool               bool              NOT NULL")
                .Append("    , ColInt                int")
                .Append("    , ColDec                decimal(10,2)")
                .Append("    , ColVarchar            VARCHAR(20)")
                .Append("    , CreateBy              VARCHAR(30)       NOT NULL")
                .Append("    , CreateDt              DATETIME          NOT NULL")
                .Append("    , UpdateBy              VARCHAR(30)       NOT NULL")
                .Append("    , UpdateDt              DATETIME          NOT NULL")
                .Append("    , CONSTRAINT PK_PtTable02 PRIMARY KEY (Key01, Key02)")
                .Append(");");
            db.Execute(sql);

            // データINSERT
            db.Execute("INSERT INTO PtTable02 values ('01',01,true,999,123456.78,'北海道','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('02',02,true,999,123456.78,'青森県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('03',03,true,999,123456.78,'岩手県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('04',04,true,999,123456.78,'宮城県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('05',05,true,999,123456.78,'秋田県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('06',06,true,999,123456.78,'山形県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('07',07,true,999,123456.78,'福島県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('08',08,true,999,123456.78,'茨城県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('09',09,true,999,123456.78,'栃木県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('10',10,true,999,123456.78,'群馬県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('11',11,true,999,123456.78,'埼玉県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('12',12,true,999,123456.78,'千葉県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('13',13,true,999,123456.78,'東京都','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('14',14,true,999,123456.78,'神奈川県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('15',15,true,999,123456.78,'新潟県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");

        }


    }

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
