using Xunit.Abstractions;

namespace PetaPoco4Core.Test.PostgreSql
{
    /// <summary>
    /// Database Object
    /// </summary>
    public class DB : PetaPoco.DatabaseExtension
    {
        public static readonly string Constr = "Server=CentOSdb;Port=5432;Database=petapoco_sample;Encoding=UTF8;User Id=testman;Password=testpwd;";

        /// <summary>
        /// Database Object
        /// </summary>
        public DB() : base(Constr, RDBType.PostgreSql)
        {
        }
    }

    [RequiresCleanUp]
    public partial class PostgreSqlTest : TestBase
    {
        public PostgreSqlTest(ITestOutputHelper output) : base(output) { }


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
                CreateTable03(db);     // use Sequence PK
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
                // 存在していたらDROP
                db.Execute("DROP TABLE IF EXISTS pt_table01");
                db.Execute("DROP TABLE IF EXISTS pt_table02");
                db.Execute("DROP TABLE IF EXISTS pt_table03");
            }
        }

        private void CreateTable01(DB db)
        {
            // 存在していたらDROP
            db.Execute("DROP TABLE IF EXISTS pt_table01");

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
                .Append("    , CONSTRAINT pk_pt_table01 PRIMARY KEY (key01)")
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
            // 存在していたらDROP
            db.Execute("DROP TABLE IF EXISTS pt_table02");

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
                .Append("    , CONSTRAINT pk_pt_table02 PRIMARY KEY (key01, key02)")
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

        /// <summary>
        /// AutoIncrement table
        /// </summary>
        /// <param name="db"></param>
        private void CreateTable03(DB db)
        {
            // 存在していたらDROP
            db.Execute("DROP TABLE IF EXISTS pt_table03");

            // CREATE
            var sql = PetaPoco.Sql.Builder
                .Append("CREATE TABLE pt_table03 (")
                .Append("      key03                  SERIAL            NOT NULL")
                .Append("    , col_bool               bool              NOT NULL")
                .Append("    , col_int                int")
                .Append("    , col_dec                decimal(10,2)")
                .Append("    , col_varchar            VARCHAR(20)")
                .Append("    , create_by              VARCHAR(30)       NOT NULL")
                .Append("    , create_dt              TIMESTAMP(3)      NOT NULL")
                .Append("    , update_by              VARCHAR(30)       NOT NULL")
                .Append("    , update_dt              TIMESTAMP(3)      NOT NULL")
                .Append("    , CONSTRAINT pk_pt_table03 PRIMARY KEY (key03)")
                .Append(");");
            db.Execute(sql);
        }

    }
}
