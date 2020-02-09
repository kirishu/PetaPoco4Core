using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;
using MySql.Data.MySqlClient;

namespace PetaPoco4Core.Test.MySql
{
    public class 例外系: TestBase
    {
        public 例外系(ITestOutputHelper output): base(output, TestCommon.Instance) { }


        [Fact]
        public void XCP001_SyntacError()
        {

            var ex = Assert.Throws<MySqlException>(() =>
            {
                // DB接続
                using (var db = new DB())
                {
                    var rec = db.SingleOrDefault<PtTable01>("WEHEREWHERE Key01 = 1");
                }
            });
            Assert.Equal(1064, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void XCP002_オブジェクト無し()
        {
            var ex = Assert.Throws<MySqlException>(() =>
            {
                using (var db = new DB())
                {
                    var rec = db.SingleOrDefault<PtTable01>("SELECT * FROM pt_table123123 WHERE Key01 = 1");
                }
            });
            Assert.Equal(1146, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void XCP003_DuplicateInsert()
        {
            var ex = Assert.Throws<MySqlException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();

                    var cnt = db.Execute("INSERT INTO PtTable01 values ('01',true, 123,9999.99,'Insert''テスト''その１','pt_test001','2018/12/18 00:00:00','pt_test001','2018/12/18 18:00:00')");
                }
            });
            Assert.Equal(1062, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void PT008_クエリタイムアウト_Timeout3秒()
        {
            var ex = Assert.Throws<MySqlException>(() =>
            {
                using (var db = new DB())
                {
                    db.CommandTimeout = 3;
                    var result = db.Execute("SELECT SLEEP(30);");
                }
            });
            Assert.Equal(0, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void PT009_クエリタイムアウト_Timeout10秒()
        {
            var ex = Assert.Throws<MySqlException>(() =>
            {
                using (var db = new DB())
                {
                    db.CommandTimeout = 10;
                    var result = db.Execute("SELECT SLEEP(30);");
                }
            });
            Assert.Equal(0, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void XCP005_接続タイムアウト_Timeout10秒()
        {
            var ex = Assert.Throws<MySqlException>(() =>
            {
                var constr = "Server=8.8.8.8;Connection Timeout=10;Database=employees;uid=testman;pwd=testman;SslMode=None;";
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.DBType.MySql))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            Assert.Equal(1042, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void XCP005_接続タイムアウト_Timeout30秒()
        {
            var ex = Assert.Throws<MySqlException>(() =>
            {
                var constr = "Server=8.8.8.8;Connection Timeout=30;Database=employees;uid=testman;pwd=testman;SslMode=None;";
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.DBType.MySql))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            Assert.Equal(1042, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void XCP006_マッピング型エラー()
        {
            var ex = Assert.Throws<System.InvalidCastException>(() =>
            {
                using (var db = new DB())
                {
                    var rec = db.SingleOrDefault<PtTable01>("SELECT Key01, 'STRING' AS col_dec FROM PtTable01 WHERE Key01 = '01'");
                }
            });
            _output.WriteLine(ex.ToString());
        }

        [Fact]
        public void PT002_SQLインジェクション_数値攻撃()
        {
            var ex = Assert.Throws<MySqlException>(() =>
            {
                using (var db = new DB())
                {
                    var recs = db.Fetch<PtTable01>(PetaPoco.Sql.Builder
                        .Append("WHERE col_int = @0", "0 OR 1 = 1")
                        );
                    //_output.WriteLine(db.LastCommand);
                }
            });
            Assert.Equal(1054, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void PT002_SQLインジェクション_文字列攻撃()
        {
            using (var db = new DB())
            {
                var recs = db.Fetch<PtTable01>(PetaPoco.Sql.Builder
                    .Append("WHERE Key01 = @0", "01' OR 'A' = 'A")
                    );
                _output.WriteLine(db.LastCommand);

                Assert.Empty(recs);
            }
        }

        [Fact]
        public void PT004_接続文字列_サービスダウンサーバ()
        {
            var constr = "Server=localhost;Database=employees;uid=testman;pwd=testman;SslMode=None;";

            var ex = Assert.Throws<MySqlException>(() =>
            {
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.DBType.MySql))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            Assert.Equal(1042, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void PT005_接続文字列_認証不可アカウント()
        {
            var rx = new Regex(";(pwd|password)=(.*?);", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var constr = rx.Replace(DB.Constr, ";$1=**zapped**;");

            var ex = Assert.Throws<MySqlException>(() =>
            {
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.DBType.MySql))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            Assert.Equal(0, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }



    }
}
