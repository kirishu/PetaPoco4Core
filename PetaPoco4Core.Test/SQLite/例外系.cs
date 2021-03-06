﻿using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace PetaPoco4Core.Test.SQLite
{
    public partial class SQLite
    {
        [Fact]
        public void 例外系_SyntacError()
        {

            var ex = Assert.Throws<System.Data.SQLite.SQLiteException>(() =>
            {
                // DB接続
                using (var db = new DB())
                {
                    var rec = db.SingleOrDefault<PtTable01>("WEHEREWHERE Key01 = 1");
                }
            });
            Assert.Equal(1, ex.ErrorCode);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.ErrorCode.ToString());
        }

        [Fact]
        public void 例外系_オブジェクト無し()
        {
            var ex = Assert.Throws<System.Data.SQLite.SQLiteException>(() =>
            {
                using (var db = new DB())
                {
                    var rec = db.SingleOrDefault<PtTable01>("SELECT * FROM pt_table123123 WHERE Key01 = 1");
                }
            });
            Assert.Equal(1, ex.ErrorCode);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.ErrorCode.ToString());
        }

        [Fact]
        public void 例外系_DuplicateInsert()
        {
            var ex = Assert.Throws<System.Data.SQLite.SQLiteException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();

                    var cnt = db.Execute("INSERT INTO PtTable01 values ('01','true', 123,9999.99,'Insert''テスト''その１','pt_test001','2018/12/18 00:00:00','pt_test001','2018/12/18 18:00:00')");
                }
            });
            Assert.Equal(19, ex.ErrorCode);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.ErrorCode.ToString());
        }

        [Fact]
        public void 例外系_クエリタイムアウト_Timeout3秒()
        {
            var ex = Assert.Throws<System.Data.SQLite.SQLiteException>(() =>
            {
                using (var db = new DB())
                {
                    db.CommandTimeout = 3;
                    var result = db.Execute("SELECT SLEEP(30);");
                }
            });
            Assert.Equal(1, ex.ErrorCode);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.ErrorCode.ToString());
        }

        [Fact]
        public void 例外系_クエリタイムアウト_Timeout10秒()
        {
            var ex = Assert.Throws<System.Data.SQLite.SQLiteException>(() =>
            {
                using (var db = new DB())
                {
                    db.CommandTimeout = 10;
                    var result = db.Execute("SELECT SLEEP(30);");
                }
            });
            Assert.Equal(1, ex.ErrorCode);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.ErrorCode.ToString());
        }

        [Fact]
        public void 例外系_接続タイムアウト_Timeout10秒()
        {
            var ex = Assert.Throws<System.Data.SQLite.SQLiteException>(() =>
            {
                var constr = @"Data Source=hoge.sqlite3";
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.RDBType.SQLite))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            Assert.Equal(1, ex.ErrorCode);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.ErrorCode.ToString());
        }

        [Fact]
        public void 例外系_接続タイムアウト_Timeout30秒()
        {
            var ex = Assert.Throws<System.Data.SQLite.SQLiteException>(() =>
            {
                var constr = @"Data Source=hoge.sqlite3";
                using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.RDBType.SQLite))
                {
                    var rec = db.SingleOrDefaultById<PtTable01>("01");
                }
            });
            Assert.Equal(1, ex.ErrorCode);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.ErrorCode.ToString());
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
            var ex = Assert.Throws<System.Data.SQLite.SQLiteException>(() =>
            {
                using (var db = new DB())
                {
                    var recs = db.Fetch<PtTable01>(PetaPoco.Sql.Builder
                        .Append("WHERE col_int = @0", "0 OR 1 = 1")
                        );
                    //_output.WriteLine(db.LastCommand);
                }
            });
            Assert.Equal(1, ex.ErrorCode);
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.ErrorCode.ToString());
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

        //[Fact]
        //public void 例外系_接続文字列_認証不可アカウント()
        //{
        //    var rx = new Regex(";(pwd|password)=(.*?);", RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase);
        //    var constr = rx.Replace(DB.Constr, ";$1=**zapped**;");

        //    var ex = Assert.Throws<System.Data.SQLite.SQLiteException>(() =>
        //    {
        //        using (var db = new PetaPoco.DatabaseExtension(constr, PetaPoco.Database.RDBType.SQLite))
        //        {
        //            var rec = db.SingleOrDefaultById<PtTable01>("01");
        //        }
        //    });
        //    Assert.Equal(18456, ex.ErrorCode);
        //    _output.WriteLine(ex.ToString());
        //    _output.WriteLine(ex.ErrorCode.ToString());
        //}



    }
}
