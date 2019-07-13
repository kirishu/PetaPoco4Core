using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace PetaPoco4Core.Test.PostgreSql
{
    public class 例外系: BaseTestClass
    {
        public 例外系(ITestOutputHelper output): base(output) { }


        [Fact]
        public void PT2_001_SyntacError()
        {

            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                // DB接続
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    var rec = db.SingleOrDefault<PtTable01>("WEHEREWHERE key01 = 1");
                }
            });
            ex.SqlState.Equals("42601");
            ex.Message.Contains("Npgsql.PostgresException: 42601: syntax error").Equals(true);
            _output.WriteLine(ex.ToString());

            /*
                Message: Test method DevSupport.Tests.Database.例外テスト系.PT2_001_SyntacError threw exception:
                Npgsql.PostgresException: 42601: syntax error at or near "key01"
            * */
        }

        [Fact]
        public void PT2_002_オブジェクト無し()
        {
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    var rec = db.SingleOrDefault<PtTable01>("SELECT * FROM pt_table123123 WHERE key01 = 1");
                }
            });
            ex.SqlState.Equals("42P01");
            ex.Message.Contains("Npgsql.PostgresException: 42P01:").Equals(true);
            _output.WriteLine(ex.ToString());
            /*
                Message: Test method DevSupport.Tests.Database.例外テスト系.PT2_002_オブジェクト無し threw exception:
                Npgsql.PostgresException: 42P01: relation "pt_table123123" does not exist
             * */
        }

        [Fact]
        public void PT2_003_DuplicateInsert()
        {
            // Npgsql.PostgresExceptionが発生したらOK
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    var cnt = db.Execute("INSERT INTO pt_table01 values ('01',true, 123,9999.99,'Insert''テスト''その１','pt_test001','2018/12/18 00:00:00','pt_test001','2018/12/18 18:00:00')");
                }
            });
            ex.SqlState.Equals("23505");
            ex.Message.Contains("Npgsql.PostgresException: 23505:").Equals(true);
            _output.WriteLine(ex.ToString());

            /*
                Message: Test method DevSupport.Tests.Database.例外テスト系.PT2_003_DuplicateInsert threw exception:
                Npgsql.PostgresException: 23505: duplicate key value violates unique constraint "pk_pt_table01"
            */
        }

        [Fact]
        public void PT008_クエリタイムアウト_Timeout3秒()
        {
            var ex = Assert.Throws<Npgsql.NpgsqlException>(() =>
            {
                using (var db = new DB())
                {
                    db.CommandTimeout = 3;
                    var result = db.Execute("SELECT CURRENT_TIMESTAMP;SELECT pg_sleep(30); SELECT CURRENT_TIMESTAMP;");
                }
            });
            _output.WriteLine(ex.ToString());
        }

        [Fact]
        public void PT009_クエリタイムアウト_Timeout10秒()
        {
            var ex = Assert.Throws<Npgsql.NpgsqlException>(() =>
            {
                using (var db = new DB())
                {
                    db.CommandTimeout = 10;
                    var result = db.Execute("SELECT CURRENT_TIMESTAMP;SELECT pg_sleep(30); SELECT CURRENT_TIMESTAMP;");
                }
            });
            _output.WriteLine(ex.ToString());
        }

        ////[TestMethod]
        ////[ExpectedException(typeof(Npgsql.NpgsqlException))]
        //[Fact]
        //public void PT2_004_クエリタイムアウト_Timeout30秒()
        //{
        //    // DB接続
        //    using (var db = new DB())
        //    {
        //        db.CommandTimeout = 30;
        //        var result = db.Execute("SELECT CURRENT_TIMESTAMP;SELECT pg_sleep(40); SELECT CURRENT_TIMESTAMP;");
        //    }
        //    /*
        //     * 10秒も30秒も同じException
        //        Message: Test method DevSupport.Tests.Database.例外テスト系.PT2_004_クエリタイムアウト_Timeout30秒 threw exception:
        //        Npgsql.NpgsqlException: Exception while reading from stream
        //        ---> System.IO.IOException: 転送接続からデータを読み取れません: 接続済みの呼び出し先が一定の時間を過ぎても正しく応答しなかったため、接続できませんでした。または接続済みのホストが応答しなかったため、確立された接続は失敗しました。。 
        //        ---> System.Net.Sockets.SocketException: 接続済みの呼び出し先が一定の時間を過ぎても正しく応答しなかったため、接続できませんでした。または接続済みのホストが応答しなかったため、確立された接続は失敗しました。

        //     *
        //     * */
        //}

        [Fact]
        public void PT2_005_接続タイムアウト_Timeout10秒()
        {
            var ex = Assert.Throws<System.TimeoutException>(() =>
            {
                var constr = "Server=8.8.8.8;Port=5432;Timeout=10;Database=dev_support;Encoding=UTF8;User Id=dev_support;Password=dev_support;";
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.DBType.PostgreSQL))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            _output.WriteLine(ex.ToString());

            /*
                Message: Test method DevSupport.Tests.Database.例外テスト系.PT2_005_接続タイムアウト_Timeout10秒 threw exception:
                System.TimeoutException: 操作がタイムアウトしました。
            * */
        }
        [Fact]
        public void PT2_005_接続タイムアウト_Timeout30秒()
        {
            var ex = Assert.Throws<System.Net.Sockets.SocketException>(() =>
            {
                var constr = "Server=8.8.8.8;Port=5432;Timeout=30;Database=dev_support;Encoding=UTF8;User Id=dev_support;Password=dev_support;";
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.DBType.PostgreSQL))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            _output.WriteLine(ex.ToString());

            /*
                Message: Test method DevSupport.Tests.Database.例外テスト系.PT2_005_接続タイムアウト_Timeout30秒 threw exception:
                System.Net.Sockets.SocketException: 接続済みの呼び出し先が一定の時間を過ぎても正しく応答しなかったため、接続できませんでした。または接続済みのホストが応答しなかったため、確立された接続は失敗しました。
             * */
        }

        [Fact]
        public void PT2_006_マッピング型エラー()
        {
            var ex = Assert.Throws<System.InvalidCastException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    var rec = db.SingleOrDefault<PtTable01>("SELECT key01, 'STRING' AS col_dec FROM pt_table01 WHERE key01 = '01'");
                }
            });
            _output.WriteLine(ex.ToString());

            /*
                Message: Test method DevSupport.Tests.Database.例外テスト系.PT2_006_マッピング型エラー threw exception:
                System.FormatException: 入力文字列の形式が正しくありません。
            * */
        }

        [Fact]
        public void PT002_SQLインジェクション_数値攻撃()
        {
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    // Npgsql.PostgresExceptionが発生したらOK
                    var recs = db.Fetch<PtTable01>(PetaPoco.Sql.Builder
                        .Append("WHERE col_int = @0", "0 OR 1 = 1")
                        );
                    //_output.WriteLine(db.LastCommand);
                }
            });
            ex.SqlState.Equals("42883");
            _output.WriteLine(ex.ToString());
        }

        [Fact]
        public void PT002_SQLインジェクション_文字列攻撃()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var recs = db.Fetch<PtTable01>(PetaPoco.Sql.Builder
                    .Append("WHERE key01 = @0", "01' OR 'A' = 'A")
                    );
                _output.WriteLine(db.LastCommand);

                Assert.Empty(recs);
            }
        }

        [Fact]
        public void PT004_接続文字列_サービスダウンサーバ()
        {
            var constr = "Server=localhost;Port=5432;Database=dev_support;Encoding=UTF8;User Id=dev_support;Password=dev_support;";

            var ex = Assert.Throws<System.Net.Sockets.SocketException>(() =>
            {
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.DBType.PostgreSQL))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            _output.WriteLine(ex.ToString());
        }

        [Fact]
        public void PT005_接続文字列_認証不可アカウント()
        {
            var rx = new Regex(";(pwd|password)=(.*?);", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var constr = rx.Replace(DB.Constr, ";$1=**zapped**;");

            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.DBType.PostgreSQL))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            ex.SqlState.Equals("28P01");
            _output.WriteLine(ex.ToString());
        }



    }
}
