using System;
using Xunit;
using Xunit.Abstractions;

namespace PetaPoco4Core.Test.PostgreSql
{
    public class Delete系: TestBase
    {
        public Delete系(ITestOutputHelper output) : base(output, TestCommon.Instance) { }


        [Fact]
        public void DEL001_Execute_1件削除DDL()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var recbefore = db.SingleById<PtTable01>("10");

                var sql = PetaPoco.Sql.Builder
                    .Append("DELETE FROM pt_table01")
                    .Append(" WHERE key01 = @0", "10");
                db.Execute(sql);

                _output.WriteLine(db.LastCommand);

                var recafter = db.SingleOrDefaultById<PtTable01>("10");

                Assert.Null(recafter);
            }
        }

        [Fact]
        public void DEL002_Execute_5件削除DDL()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var sql = PetaPoco.Sql.Builder
                    .Append("DELETE FROM pt_table01")
                    .Append(" WHERE key01 >= @0", "01")
                    .Append("   AND key01 <= @0", "05");
                var cnt = db.Execute(sql);
                _output.WriteLine(db.LastCommand);
                Assert.Equal(5, cnt);

                var recs = db.Fetch<PtTable01>("WHERE key01 >= @0 AND key01 <= @1 ORDER BY key01", "01", "05");
                Assert.Empty(recs);

            }
        }

        [Fact]
        public void DEL003_削除対象無し_DDL発行()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var sql = PetaPoco.Sql.Builder
                    .Append("DELETE FROM pt_table01")
                    .Append(" WHERE key01 = @0", "99");
                var cnt = db.Execute(sql);
                Assert.Equal(0, cnt);

                _output.WriteLine(db.LastCommand);

                // pt_table01 の key01 = 99を取得する
                var recafter = db.SingleOrDefaultById<PtTable01>("99");
                Assert.Null(recafter);
            }
        }

        [Fact]
        public void DEL004_Delete_1件削除Entity_1列PK()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var recbefore = db.SingleOrDefaultById<PtTable01>("11");
                Assert.NotNull(recbefore);

                var cnt = db.Delete<PtTable01>("WHERE Key01=@0", "11");
                _output.WriteLine(db.LastCommand);
                Assert.Equal(1, cnt);

                var recafter = db.SingleOrDefaultById<PtTable01>("11");
                Assert.Null(recafter);
            }
        }

        [Fact]
        public void DEL005_Delete_1件削除Entity_2列PK()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var pk = new
                {
                    key01 = "12",
                    key02 = 12,
                };

                var recbefore = db.SingleOrDefaultById<PtTable02>(pk);
                Assert.NotNull(recbefore);

                var cnt = db.Delete<PtTable02>("WHERE Key01=@0 AND Key02=@1", "12", 12);
                _output.WriteLine(db.LastCommand);
                Assert.Equal(1, cnt);

                var recafter = db.SingleOrDefaultById<PtTable02>(pk);
                Assert.Null(recafter);
            }
        }

        [Fact]
        public void DEL006_Delete_1件削除Entity_2列PK_ヒットなし()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var pk = new
                {
                    key01 = "12",
                    key02 = 999,
                };

                var recbefore = db.SingleOrDefaultById<PtTable02>(pk);
                Assert.Null(recbefore);

                var cnt = db.Delete<PtTable02>("WHERE Key01=@0 AND Key02=@1", "12", 999);
                _output.WriteLine(db.LastCommand);
                Assert.Equal(0, cnt);
            }
        }

        [Fact]
        public void DEL007_Delete_5件削除Entity()
        {
            var pk = new
            {
                key01 = "12",
                key02 = 999,
            };

            using (var db = new DB())
            {
                db.BeginTransaction();

                var cnt = db.Delete<PtTable02>("WHERE key01 >= @0 AND key01 <= @1", "01", "05");
                _output.WriteLine(db.LastCommand);
                Assert.Equal(5, cnt);

                var recafter = db.SingleOrDefaultById<PtTable02>(pk);
                Assert.Null(recafter);
            }
        }

        [Fact]
        public void DEL008_Delete_5件削除Entity_SQLオブジェクト()
        {
            var pk = new
            {
                key01 = "12",
                key02 = 999,
            };

            using (var db = new DB())
            {
                db.BeginTransaction();

                var cnt = db.Delete<PtTable02>(PetaPoco.Sql.Builder
                    .Append(" WHERE key01 >= @0", "01")
                    .Append("   AND key01 <= @0", "05"));
                _output.WriteLine(db.LastCommand);
                Assert.Equal(5, cnt);

                var recafter = db.SingleOrDefaultById<PtTable02>(pk);
                Assert.Null(recafter);
            }
        }

        [Fact]
        public void DEL009_Transaction_Commit()
        {
            using (var db = new DB())
            {
                // pt_table02 の key01=14 を取得して確保する
                var pk = new
                {
                    key01 = "14",
                    key02 = 14,
                };
                var recbefore = db.SingleOrDefaultById<PtTable02>(pk);
                Assert.NotNull(recbefore);

                // Transactionを開始する
                db.BeginTransaction();

                // 下記Deleteを実行する
                var cnt = db.Delete<PtTable02>("WHERE Key01=@0 AND Key02=@1", "14", 14);
                Assert.Equal(1, cnt);

                // TransactionをCommitする
                db.CompleteTransaction();

                // pt_table02 の key01=14 を取得して内容を確認する
                var recafter = db.SingleOrDefaultById<PtTable02>(pk);
                Assert.Null(recafter);

            }
        }

        [Fact]
        public void DEL010_Transaction_Rollback()
        {
            using (var db = new DB())
            {
                // pt_table02 の key01=12 を取得して確保する
                var pk = new
                {
                    key01 = "12",
                    key02 = 12,
                };
                var recbefore = db.SingleOrDefaultById<PtTable02>(pk);
                Assert.NotNull(recbefore);

                // Transactionを開始する
                db.BeginTransaction();

                // 下記Deleteを実行する
                var cnt = db.Delete<PtTable02>("WHERE Key01=@0 AND Key02=@1", "12", 12);
                Assert.Equal(1, cnt);

                // TransactionをRollbakする
                db.AbortTransaction();

                // pt_table02 の key01=12 を取得して内容を確認する
                var recafter = db.SingleOrDefaultById<PtTable02>(pk);
                Assert.NotNull(recafter);

            }
        }


        [Fact]
        public void DEL011_Delete_1件読んで削除_PK1()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var rec = db.SingleOrDefaultById<PtTable01>("12");

                var cnt = db.Delete(rec);
                _output.WriteLine(db.LastCommand);
                Assert.Equal(1, cnt);

                var recafter = db.SingleOrDefaultById<PtTable01>("12");
                Assert.Null(recafter);

            }
        }

        [Fact]
        public void DEL012_Delete_1件読んで削除_PK2()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var pk = new
                {
                    key01 = "12",
                    key02 = 12,
                };

                var rec = db.SingleOrDefaultById<PtTable02>(pk);

                var cnt = db.Delete(rec);
                _output.WriteLine(db.LastCommand);
                Assert.Equal(1, cnt);

                var recafter = db.SingleOrDefaultById<PtTable02>(pk);
                Assert.Null(recafter);
            }
        }


    }
}
