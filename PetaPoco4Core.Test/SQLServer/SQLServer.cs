using Xunit.Abstractions;

namespace PetaPoco4Core.Test.SQLServer
{
    /// <summary>
    /// Database Object
    /// </summary>
    public class DB : PetaPoco.DatabaseExtension
    {
        public static readonly string Constr = @"Data Source=192.168.1.210;Database=PetaPocoSample;Integrated Security=False;User ID=testman;Password=testpwd;Pooling=true;";

        /// <summary>
        /// Database Object
        /// </summary>
        public DB() : base(Constr, RDBType.SqlServer)
        {
        }
    }

    [RequiresCleanUp]
    public partial class SQLServer : TestBase
    {
        public SQLServer(ITestOutputHelper output) : base(output) { }


        /// <summary>
        /// 初期処理
        /// </summary>
        internal override void Initialize()
        {
            // DB接続
            using (var db = new DB())
            {
                CreateTable01(db);
                CreateTable02(db);
                CreateTable03(db);   // AutoIncrement PK
            }
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        internal override void Cleanup()
        {
            // DB接続
            using (var db = new DB())
            {
                db.Execute("IF OBJECT_ID(N'PtTable01', N'U') IS NOT NULL    DROP TABLE PtTable01");
                db.Execute("IF OBJECT_ID(N'PtTable02', N'U') IS NOT NULL    DROP TABLE PtTable02");
                db.Execute("IF OBJECT_ID(N'PtTable03', N'U') IS NOT NULL    DROP TABLE PtTable03");
            }
        }

        private void CreateTable01(DB db)
        {
            // 存在していたらDROP
            db.Execute("IF OBJECT_ID(N'PtTable01', N'U') IS NOT NULL    DROP TABLE PtTable01");

            // CREATE
            var sql = PetaPoco.Sql.Builder
                .Append("CREATE TABLE PtTable01 (")
                .Append("      Key01                 NVARCHAR(2)        NOT NULL")
                .Append("    , ColBool               bit              NOT NULL")
                .Append("    , ColInt                int")
                .Append("    , ColDec                decimal(10,2)")
                .Append("    , ColVarchar            NVARCHAR(20)")
                .Append("    , CreateBy              NVARCHAR(30)       NOT NULL")
                .Append("    , CreateDt              DATETIME          NOT NULL")
                .Append("    , UpdateBy              NVARCHAR(30)       NOT NULL")
                .Append("    , UpdateDt              DATETIME          NOT NULL")
                .Append("    , CONSTRAINT PK_PtTable01 PRIMARY KEY (Key01)")
                .Append(");");
            db.Execute(sql);

            // データINSERT
            db.Execute("INSERT INTO PtTable01 values ('01','true',999,123456.78,'北海道','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('02','true',999,123456.78,'青森県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('03','true',999,123456.78,'岩手県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('04','true',999,123456.78,'宮城県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('05','true',999,123456.78,'秋田県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('06','true',999,123456.78,'山形県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('07','true',999,123456.78,'福島県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('08','true',999,123456.78,'茨城県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('09','true',999,123456.78,'栃木県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('10','true',999,123456.78,'群馬県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('11','true',999,123456.78,'埼玉県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('12','true',999,123456.78,'千葉県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('13','true',999,123456.78,'東京都','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('14','true',999,123456.78,'神奈川県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable01 values ('15','true',999,123456.78,'新潟県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");

        }

        private void CreateTable02(DB db)
        {
            // 存在していたらDROP
            db.Execute("IF OBJECT_ID(N'PtTable02', N'U') IS NOT NULL    DROP TABLE PtTable02");

            // CREATE
            var sql = PetaPoco.Sql.Builder
                .Append("CREATE TABLE PtTable02 (")
                .Append("      Key01                 NVARCHAR(2)        NOT NULL")
                .Append("    , Key02                 int               NOT NULL")
                .Append("    , ColBool               bit              NOT NULL")
                .Append("    , ColInt                int")
                .Append("    , ColDec                decimal(10,2)")
                .Append("    , ColVarchar            NVARCHAR(20)")
                .Append("    , CreateBy              NVARCHAR(30)       NOT NULL")
                .Append("    , CreateDt              DATETIME          NOT NULL")
                .Append("    , UpdateBy              NVARCHAR(30)       NOT NULL")
                .Append("    , UpdateDt              DATETIME          NOT NULL")
                .Append("    , CONSTRAINT PK_PtTable02 PRIMARY KEY (Key01, Key02)")
                .Append(");");
            db.Execute(sql);

            // データINSERT
            db.Execute("INSERT INTO PtTable02 values ('01',01,'true',999,123456.78,'北海道','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('02',02,'true',999,123456.78,'青森県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('03',03,'true',999,123456.78,'岩手県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('04',04,'true',999,123456.78,'宮城県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('05',05,'true',999,123456.78,'秋田県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('06',06,'true',999,123456.78,'山形県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('07',07,'true',999,123456.78,'福島県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('08',08,'true',999,123456.78,'茨城県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('09',09,'true',999,123456.78,'栃木県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('10',10,'true',999,123456.78,'群馬県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('11',11,'true',999,123456.78,'埼玉県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('12',12,'true',999,123456.78,'千葉県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('13',13,'true',999,123456.78,'東京都','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('14',14,'true',999,123456.78,'神奈川県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");
            db.Execute("INSERT INTO PtTable02 values ('15',15,'true',999,123456.78,'新潟県','system','2018/10/24 12:00:00','system','2018/10/24 12:00:00');");

        }

        /// <summary>
        /// AutoIncrement table
        /// </summary>
        /// <param name="db"></param>
        private void CreateTable03(DB db)
        {
            // 存在していたらDROP
            db.Execute("IF OBJECT_ID(N'PtTable03', N'U') IS NOT NULL    DROP TABLE PtTable03");

            // CREATE
            var sql = PetaPoco.Sql.Builder
                .Append("CREATE TABLE PtTable03 (")
                .Append("      Key03                 int          IDENTITY (1, 1)  NOT NULL")
                .Append("    , ColBool               bit              NOT NULL")
                .Append("    , ColInt                int")
                .Append("    , ColDec                decimal(10,2)")
                .Append("    , ColVarchar            NVARCHAR(20)")
                .Append("    , CreateBy              NVARCHAR(30)       NOT NULL")
                .Append("    , CreateDt              DATETIME          NOT NULL")
                .Append("    , UpdateBy              NVARCHAR(30)       NOT NULL")
                .Append("    , UpdateDt              DATETIME          NOT NULL")
                .Append("    , CONSTRAINT PK_PtTable03 PRIMARY KEY (Key03)")
                .Append(");");
            db.Execute(sql);

        }


    }

}
