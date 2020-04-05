using System.Text.RegularExpressions;
using Xunit;

namespace PetaPoco4Core.Test.PostgreSql
{
    public partial class PostgreSqlTest
    {
        [Fact]
        public void 例外系_SyntacError()
        {

            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                // DB接続
                using (var db = new DB())
                {
                    var rec = db.SingleOrDefault<PtTable01>("WEHEREWHERE key01 = 1");
                }
            });
            _output.WriteLine(ex.ToString());
            Assert.Equal("42601", ex.SqlState);

            /*
                Message: Test method DevSupport.Tests.Database.例外テスト系.PT2_001_SyntacError threw exception:
                Npgsql.PostgresException: 42601: syntax error at or near "key01"
            * */
        }

        [Fact]
        public void 例外系_オブジェクト無し()
        {
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    var rec = db.SingleOrDefault<PtTable01>("SELECT * FROM pt_table123123 WHERE key01 = 1");
                }
            });
            _output.WriteLine(ex.ToString());
            Assert.Equal("42P01", ex.SqlState);
            /*
                Message: Test method DevSupport.Tests.Database.例外テスト系.PT2_002_オブジェクト無し threw exception:
                Npgsql.PostgresException: 42P01: relation "pt_table123123" does not exist
             * */
        }

        [Fact]
        public void 例外系_DuplicateInsert()
        {
            // Npgsql.PostgresExceptionが発生したらOK
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();

                    var cnt = db.Execute("INSERT INTO pt_table01 values ('01',true, 123,9999.99,'Insert''テスト''その１','pt_test001','2018/12/18 00:00:00','pt_test001','2018/12/18 18:00:00')");
                }
            });
            _output.WriteLine(ex.ToString());
            Assert.Equal("23505", ex.SqlState);

            /*
                Message: Test method DevSupport.Tests.Database.例外テスト系.PT2_003_DuplicateInsert threw exception:
                Npgsql.PostgresException: 23505: duplicate key value violates unique constraint "pk_pt_table01"
            */
        }

        [Fact]
        public void 例外系_クエリタイムアウト_Timeout3秒()
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
        public void 例外系_クエリタイムアウト_Timeout10秒()
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
        //public void 例外系_クエリタイムアウト_Timeout30秒()
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
        public void 例外系_接続タイムアウト_Timeout10秒()
        {
            var ex = Assert.Throws<Npgsql.NpgsqlException>(() =>
            {
                var constr = "Server=8.8.8.8;Port=5432;Timeout=10;Database=dvdrental;Encoding=UTF8;User Id=testman;Password=testpwd;";
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.RDBType.PostgreSql))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.ErrorCode.ToString());
            Assert.Equal(-2147467259, ex.ErrorCode);
            /*
                Message: Test method DevSupport.Tests.Database.例外テスト系.PT2_005_接続タイムアウト_Timeout10秒 threw exception:
                System.TimeoutException: 操作がタイムアウトしました。
            * */
        }
        [Fact]
        public void 例外系_接続タイムアウト_Timeout30秒()
        {
            var ex = Assert.Throws<Npgsql.NpgsqlException>(() =>
            {
                var constr = "Server=8.8.8.8;Port=5432;Timeout=30;Database=dvdrental;Encoding=UTF8;User Id=testman;Password=testpwd;";
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.RDBType.PostgreSql))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.ErrorCode.ToString());
            Assert.Equal(-2147467259, ex.ErrorCode);

            /*
                Message: Test method DevSupport.Tests.Database.例外テスト系.PT2_005_接続タイムアウト_Timeout30秒 threw exception:
                System.Net.Sockets.SocketException: 接続済みの呼び出し先が一定の時間を過ぎても正しく応答しなかったため、接続できませんでした。または接続済みのホストが応答しなかったため、確立された接続は失敗しました。
             * */
        }

        [Fact]
        public void 例外系_マッピング型エラー()
        {
            var ex = Assert.Throws<System.FormatException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();

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
        public void 例外系_SQLインジェクション_数値攻撃()
        {
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();

                    // Npgsql.PostgresExceptionが発生したらOK
                    var recs = db.Fetch<PtTable01>(PetaPoco.Sql.Builder
                        .Append("WHERE col_int = @0", "0 OR 1 = 1")
                        );
                    //_output.WriteLine(db.LastCommand);
                }
            });
            _output.WriteLine(ex.ToString());
            Assert.Equal("42883", ex.SqlState);
        }

        [Fact]
        public void 例外系_SQLインジェクション_文字列攻撃()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var recs = db.Fetch<PtTable01>(PetaPoco.Sql.Builder
                    .Append("WHERE key01 = @0", "01' OR 'A' = 'A")
                    );
                _output.WriteLine(db.LastCommand);

                Assert.Empty(recs);
            }
        }

        [Fact]
        public void 例外系_接続文字列_サービスダウンサーバ()
        {
            var constr = "Server=localhost;Port=5432;Database=dvdrental;Encoding=UTF8;User Id=testman;Password=testpwd;";

            var ex = Assert.Throws<Npgsql.NpgsqlException>(() =>
            {
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.RDBType.PostgreSql))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            _output.WriteLine(ex.ToString());
            Assert.Equal(-2147467259, ex.ErrorCode);
        }

        [Fact]
        public void 例外系_接続文字列_認証不可アカウント()
        {
            var rx = new Regex(";(User Id)=(.*?);", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var constr = rx.Replace(DB.Constr, ";$1=**zapped**;");

            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.RDBType.PostgreSql))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            _output.WriteLine(ex.ToString());
            Assert.Equal("28P01", ex.SqlState);     // ユーザ"**zapped**"のパスワード認証に失敗しました
        }

        [Fact]
        public void 例外系_接続文字列_パスワード誤り()
        {
            var rx = new Regex(";(pwd|password)=(.*?);", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var constr = rx.Replace(DB.Constr, ";$1=**zapped**;");

            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.RDBType.PostgreSql))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            _output.WriteLine(ex.ToString());
            Assert.Equal("28P01", ex.SqlState);     // ユーザ"testman"のパスワード認証に失敗しました
        }



    }
}
