﻿using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace PetaPoco4Core.Test.SQLServer
{
    public partial class SQLServer
    {
        [Fact]
        public void 例外系_SyntacError()
        {

            var ex = Assert.Throws<System.Data.SqlClient.SqlException>(() =>
            {
                // DB接続
                using (var db = new DB())
                {
                    var rec = db.SingleOrDefault<PtTable01>("WEHEREWHERE Key01 = 1");
                }
            });
            Assert.Equal(102, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void 例外系_オブジェクト無し()
        {
            var ex = Assert.Throws<System.Data.SqlClient.SqlException>(() =>
            {
                using (var db = new DB())
                {
                    var rec = db.SingleOrDefault<PtTable01>("SELECT * FROM pt_table123123 WHERE Key01 = 1");
                }
            });
            Assert.Equal(208, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void 例外系_DuplicateInsert()
        {
            var ex = Assert.Throws<System.Data.SqlClient.SqlException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();

                    var cnt = db.Execute("INSERT INTO PtTable01 values ('01','true', 123,9999.99,'Insert''テスト''その１','pt_test001','2018/12/18 00:00:00','pt_test001','2018/12/18 18:00:00')");
                }
            });
            Assert.Equal(2627, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void 例外系_クエリタイムアウト_Timeout3秒()
        {
            var ex = Assert.Throws<System.Data.SqlClient.SqlException>(() =>
            {
                using (var db = new DB())
                {
                    db.CommandTimeout = 3;
                    var result = db.Execute("SELECT SLEEP(30);");
                }
            });
            Assert.Equal(195, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void 例外系_クエリタイムアウト_Timeout10秒()
        {
            var ex = Assert.Throws<System.Data.SqlClient.SqlException>(() =>
            {
                using (var db = new DB())
                {
                    db.CommandTimeout = 10;
                    var result = db.Execute("SELECT SLEEP(30);");
                }
            });
            Assert.Equal(195, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void 例外系_接続タイムアウト_Timeout10秒()
        {
            var ex = Assert.Throws<System.Data.SqlClient.SqlException>(() =>
            {
                var constr = @"Data Source=8.8.8.8;Database=Northwind;Integrated Security=False;User ID=testman;Password=testpwd;Pooling=true;Connection Timeout=10;";
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.RDBType.SqlServer))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            Assert.Equal(53, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void 例外系_接続タイムアウト_Timeout30秒()
        {
            var ex = Assert.Throws<System.Data.SqlClient.SqlException>(() =>
            {
                var constr = @"Data Source=8.8.8.8;Database=Northwind;Integrated Security=False;User ID=testman;Password=testpwd;Pooling=true;Connection Timeout=30;";
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.RDBType.SqlServer))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            Assert.Equal(53, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void 例外系_マッピング型エラー()
        {
            var ex = Assert.Throws<System.FormatException>(() =>
            {
                using (var db = new DB())
                {
                    var rec = db.SingleOrDefault<PtTable01>("SELECT Key01, 'STRING' AS col_dec FROM PtTable01 WHERE Key01 = '01'");
                }
            });
            _output.WriteLine(ex.ToString());
        }

        [Fact]
        public void 例外系_SQLインジェクション_数値攻撃()
        {
            var ex = Assert.Throws<System.Data.SqlClient.SqlException>(() =>
            {
                using (var db = new DB())
                {
                    var recs = db.Fetch<PtTable01>(PetaPoco.Sql.Builder
                        .Append("WHERE col_int = @0", "0 OR 1 = 1")
                        );
                    //_output.WriteLine(db.LastCommand);
                }
            });
            Assert.Equal(207, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void 例外系_SQLインジェクション_文字列攻撃()
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
        public void 例外系_接続文字列_サービスダウンサーバ()
        {
            var constr = @"Data Source=localhost;Database=Northwind;Integrated Security=False;User ID=testman;Password=testpwd;Pooling=true;";

            var ex = Assert.Throws<System.Data.SqlClient.SqlException>(() =>
            {
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.RDBType.SqlServer))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            Assert.Equal(2, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }

        [Fact]
        public void 例外系_接続文字列_認証不可アカウント()
        {
            var rx = new Regex(";(pwd|password)=(.*?);", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            var constr = rx.Replace(DB.Constr, ";$1=**zapped**;");

            var ex = Assert.Throws<System.Data.SqlClient.SqlException>(() =>
            {
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.RDBType.SqlServer))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            Assert.Equal(18456, ex.Number);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
        }



    }
}
