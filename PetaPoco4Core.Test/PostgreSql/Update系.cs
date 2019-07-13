﻿using System;
using Xunit;
using Xunit.Abstractions;

namespace PetaPoco4Core.Test.PostgreSql
{
    public class Update系: BaseTestClass
    {
        public Update系(ITestOutputHelper output) : base(output) { }


        [Fact]
        public void PT001_Execute_1件更新DDL()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var recbefore = db.SingleById<PtTable01>("10");

                var sql = PetaPoco.Sql.Builder
                    .Append("UPDATE pt_table01")
                    .Append("  SET col_bool = @0", false)
                    .Append("    , col_int = @0", 555)
                    .Append("    , col_dec = @0", 5555.55m)
                    .Append("    , col_varchar = @0", "Update'テスト'ABC")
                    .Append("    , update_by = @0", "pt_test002")
                    .Append("    , update_dt = @0", DateTime.Parse("2018/12/19 11:11:11"))
                    .Append(" WHERE key01 = @0", "10");
                db.Execute(sql);

                _output.WriteLine(db.LastCommand);

                var recafter = db.SingleById<PtTable01>("10");

                Assert.False(recafter.ColBool);
                Assert.Equal(555, recafter.ColInt.Value);
                Assert.Equal(5555.55m, recafter.ColDec.Value);
                Assert.Equal("Update'テスト'ABC", recafter.ColVarchar);
                Assert.Equal("pt_test002", recafter.UpdateBy);
                Assert.Equal(DateTime.Parse("2018/12/19 11:11:11"), recafter.UpdateDt);

                Assert.NotEqual(recbefore.ColVarchar, recafter.ColVarchar);

            }
        }

        [Fact]
        public void PT002_Execute_1件更新Entity()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var recbefore = db.SingleById<PtTable01>("11");

                var rec = db.SingleById<PtTable01>("11");
                rec.ColBool = false;
                rec.ColInt = 555;
                rec.ColDec = 5555.55m;
                rec.ColVarchar = "Update'テスト'ABC";
                rec.UpdateBy = "pt_test002";
                rec.UpdateDt = DateTime.Parse("2018/12/19 11:11:11");
                db.Update(rec);

                _output.WriteLine(db.LastCommand);

                var recafter = db.SingleById<PtTable01>("11");
                Assert.False(recafter.ColBool);
                Assert.Equal(555, recafter.ColInt.Value);
                Assert.Equal(5555.55m, recafter.ColDec.Value);
                Assert.Equal("Update'テスト'ABC", recafter.ColVarchar);
                Assert.Equal("pt_test002", recafter.UpdateBy);
                Assert.Equal(DateTime.Parse("2018/12/19 11:11:11"), recafter.UpdateDt);

                Assert.NotEqual(recbefore.ColVarchar, recafter.ColVarchar);

            }
        }

        [Fact]
        public void PT003_Execute_5件更新DDL()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var sql = PetaPoco.Sql.Builder
                    .Append("UPDATE pt_table01")
                    .Append("  SET col_varchar = @0", "Update'テスト'ABC")
                    .Append(" WHERE key01 >= @0", "01")
                    .Append("   AND key01 <= @0", "05");
                db.Execute(sql);

                _output.WriteLine(db.LastCommand);

                var recs = db.Fetch<PtTable01>("WHERE key01 >= @0 AND key01 <= @1 ORDER BY key01", "01", "05");
                foreach (var rec in recs)
                {
                    Assert.Equal("Update'テスト'ABC", rec.ColVarchar);
                }

            }
        }

        [Fact]
        public void PT004_更新対象無し_DDL発行()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var sql = PetaPoco.Sql.Builder
                    .Append("UPDATE pt_table01")
                    .Append("  SET col_varchar = @0", "Update'テスト'ABC")
                    .Append(" WHERE key01 = @0", "99");
                var cnt = db.Execute(sql);

                _output.WriteLine(db.LastCommand);

                // pt_table01 の key01 = 99を取得する
                var recafter = db.SingleOrDefaultById<PtTable01>("99");
                Assert.Null(recafter);
            }
        }

        [Fact]
        public void PT005_カラムサイズオーバーエラー_DDL発行()
        {
            // Npgsql.PostgresExceptionが発生したらOK
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    var sql = PetaPoco.Sql.Builder
                        .Append("UPDATE pt_table01")
                        .Append("  SET col_varchar = @0", "1234567890123456789012345")
                        .Append(" WHERE key01 = @0", "11");
                    db.Execute(sql);
                    _output.WriteLine(db.LastCommand);
                }
            });
            ex.SqlState.Equals("22001");
            _output.WriteLine(ex.ToString());
        }

        [Fact]
        public void PT006_カラムサイズオーバーエラー_Entity利用()
        {
            // Npgsql.PostgresExceptionが発生したらOK
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    var rec = db.SingleById<PtTable01>("11");
                    rec.ColVarchar = "1234567890123456789012345";
                    db.Update(rec);
                    _output.WriteLine(db.LastCommand);
                }
            });
            ex.SqlState.Equals("22001");
            _output.WriteLine(ex.ToString());
        }

        [Fact]
        public void PT005_カラムサイズオーバーエラー_INT()
        {
            // Npgsql.PostgresExceptionが発生したらOK
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    var sql = PetaPoco.Sql.Builder
                        .Append("UPDATE pt_table01")
                        .Append("  SET col_int = @0", 1234567890123456789012345m)
                        .Append(" WHERE key01 = @0", "11");
                    db.Execute(sql);
                    _output.WriteLine(db.LastCommand);
                }
            });
            // 22003: integerの範囲外です
            ex.SqlState.Equals("22003");
            _output.WriteLine(ex.ToString());
        }

        [Fact]
        public void PT006_NULL制約エラー()
        {
            // Npgsql.PostgresExceptionが発生したらOK
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    var sql = PetaPoco.Sql.Builder
                        .Append("UPDATE pt_table01")
                        .Append("  SET create_by = NULL")
                        .Append(" WHERE key01 = @0", "11");
                    db.Execute(sql);
                    _output.WriteLine(db.LastCommand);
                }
            });
            // 23502: 列"create_by"内のNULL値はNOT NULL制約違反です
            ex.SqlState.Equals("23502");
            _output.WriteLine(ex.ToString());
        }

        [Fact]
        public void PT007_更新列のみ更新_事前読込あり()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var rec = db.SingleById<PtTable01>("11");
                rec.ColVarchar = "1234567890";
                var cnt = db.Update(rec);
                _output.WriteLine(db.LastCommand);

                Assert.Equal(1, cnt);

                var sql = db.LastSQL;

                // これは含まれるべき文字列
                Assert.Contains("col_varchar", sql);

                // これ以降は含まれてはいけない文字列
                Assert.DoesNotContain("col_bool", sql);
                Assert.DoesNotContain("col_int", sql);
                Assert.DoesNotContain("col_dec", sql);
                Assert.DoesNotContain("create_by", sql);
                Assert.DoesNotContain("update_dt", sql);

            }
        }

        [Fact]
        public void PT007_更新列のみ更新_事前読込なし()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable02(db);

                // 更新する値
                var rec = new PtTable02
                {
                    ColVarchar = "1234567890",
                };

                // PK
                var pk = new
                {
                    key01 = "13",
                    key02 = 13,
                };

                var cnt = db.Update(rec, pk);   // PKを指定するメソッド
                _output.WriteLine(db.LastCommand);

                Assert.Equal(1, cnt);

                var sql = db.LastSQL;

                // これは含まれるべき文字列
                Assert.Contains("col_varchar", sql);

                // これ以降は含まれてはいけない文字列
                Assert.DoesNotContain("col_bool", sql);
                Assert.DoesNotContain("col_int", sql);
                Assert.DoesNotContain("col_dec", sql);
                Assert.DoesNotContain("create_by", sql);
                Assert.DoesNotContain("update_dt", sql);
            }
        }

        [Fact]
        public void PT007_更新列のみ更新_事前読込なし_2()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable02(db);

                // レコード内にPK値も持つ
                var rec = new PtTable02
                {
                    Key01 = "13",
                    Key02 = 13,
                    ColVarchar = "1234567890",
                };

                var cnt = db.Update(rec);       // PKはレコード内にあるパターン
                _output.WriteLine(db.LastCommand);

                Assert.Equal(1, cnt);

                var sql = db.LastSQL;

                // これは含まれるべき文字列
                Assert.Contains("col_varchar", sql);

                // これ以降は含まれてはいけない文字列
                Assert.DoesNotContain("col_bool", sql);
                Assert.DoesNotContain("col_int", sql);
                Assert.DoesNotContain("col_dec", sql);
                Assert.DoesNotContain("create_by", sql);
                Assert.DoesNotContain("update_dt", sql);
            }
        }

        //[Fact]
        //public void PT008_Transaction_Commit()
        //{
        //    using (var db = new DB())
        //    {
        //        db.BeginTransaction();
        //        TestCommon.CreateTempTable01(db);
        //        TestCommon.CreateTempTable02(db);

        //        // pt_table02 の key01=12 を取得して確保する
        //        var pk = new
        //        {
        //            key01 = "12",
        //            key02 = 12,
        //        };
        //        var recbefore = db.SingleById<PtTable02>(pk);
        //        Assert.Equal("千葉県", recbefore.ColVarchar);

        //        //// Transactionを開始する
        //        //db.BeginTransaction();

        //        // 下記Updateを実行する
        //        var recafter = db.SingleById<PtTable02>(pk);
        //        recafter.ColVarchar = "Update-Commitテスト";
        //        db.Update(recafter);

        //        // pt_table02 の key01=12 を取得して内容を確認する
        //        var rec_03 = db.SingleById<PtTable02>(pk);
        //        Assert.Equal(rec_03.ColVarchar, "Update-Commitテスト");

        //        //// TransactionをCommitする
        //        //db.CompleteTransaction();

        //        // pt_table02 の key01=12 を取得して内容を確認する
        //        var rec_04 = db.SingleById<PtTable02>(pk);
        //        Assert.Equal(rec_03.ColVarchar, "Update-Commitテスト");
        //    }
        //}

        //[Fact]
        //public void PT009_Transaction_Rollback()
        //{
        //    using (var db = new DB())
        //    {
        //        db.BeginTransaction();
        //        TestCommon.CreateTempTable01(db);
        //        TestCommon.CreateTempTable02(db);

        //        // pt_table02 の key01=13 を取得して確保する
        //        var pk = new
        //        {
        //            key01 = "13",
        //            key02 = 13,
        //        };
        //        var recbefore = db.SingleById<PtTable02>(pk);
        //        string before_char = recbefore.ColVarchar;

        //        // Transactionを開始する
        //        db.BeginTransaction();

        //        // 下記Updateを実行する
        //        var recafter = db.SingleById<PtTable02>(pk);
        //        recafter.ColVarchar = "Update-Rollbackテスト";
        //        db.Update(recafter);

        //        // pt_table02 の key01=13 を取得して内容を確認する
        //        var rec_03 = db.SingleById<PtTable02>(pk);
        //        Assert.Equal(rec_03.ColVarchar, "Update-Rollbackテスト");

        //        // TransactionをRollbackする
        //        db.AbortTransaction();

        //        // pt_table02 の key01=12 を取得して内容を確認する
        //        var rec_04 = db.SingleById<PtTable02>(pk);
        //        Assert.Equal(rec_04.ColVarchar, before_char);
        //    }
        //}

    }
}
