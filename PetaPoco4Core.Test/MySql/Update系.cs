using MySql.Data.MySqlClient;
using System;
using Xunit;

namespace PetaPoco4Core.Test.MySql
{
    public partial class MySqlTest
    {
        [Fact]
        public void Update系_Execute_1件更新DDL()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var recbefore = db.SingleById<PtTable01>("10");

                var sql = PetaPoco.Sql.Builder
                    .Append("UPDATE PtTable01")
                    .Append("   SET ColBool = @0", false)
                    .Append("     , ColInt = @0", 555)
                    .Append("     , ColDec = @0", 5555.55m)
                    .Append("     , ColVarchar = @0", "Update'テスト'ABC")
                    .Append("     , UpdateBy = @0", "pt_test002")
                    .Append("     , UpdateDt = @0", DateTime.Parse("2018/12/19 11:11:11"))
                    .Append(" WHERE Key01 = @0", "10");
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
        public void Update系_Execute_1件更新Entity()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

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
        public void Update系_Execute_5件更新DDL()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var sql = PetaPoco.Sql.Builder
                    .Append("UPDATE PtTable01")
                    .Append("   SET ColVarchar = @0", "Update'テスト'ABC")
                    .Append(" WHERE Key01 >= @0", "01")
                    .Append("   AND Key01 <= @0", "05");
                db.Execute(sql);

                _output.WriteLine(db.LastCommand);

                var recs = db.Fetch<PtTable01>("WHERE Key01 >= @0 AND Key01 <= @1 ORDER BY Key01", "01", "05");
                foreach (var rec in recs)
                {
                    Assert.Equal("Update'テスト'ABC", rec.ColVarchar);
                }

            }
        }

        [Fact]
        public void Update系_更新対象無し_DDL発行()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var sql = PetaPoco.Sql.Builder
                    .Append("UPDATE PtTable01")
                    .Append("   SET ColVarchar = @0", "Update'テスト'ABC")
                    .Append(" WHERE Key01 = @0", "99");
                var cnt = db.Execute(sql);

                _output.WriteLine(db.LastCommand);

                // PtTable01 の Key01 = 99を取得する
                var recafter = db.SingleOrDefaultById<PtTable01>("99");
                Assert.Null(recafter);
            }
        }

        //[Fact]
        //public void Update系_カラムサイズオーバーエラー_DDL発行()
        //{
        //    // MySqlExceptionが発生したらOK
        //    var ex = Assert.Throws<MySqlException>(() =>
        //    {
        //        using (var db = new DB())
        //        {
        //            db.BeginTransaction();

        //            var sql = PetaPoco.Sql.Builder
        //                .Append("UPDATE PtTable01")
        //                .Append("   SET ColVarchar = @0", "1234567890123456789012345")
        //                .Append(" WHERE Key01 = @0", "11");
        //            db.Execute(sql);
        //            _output.WriteLine(db.LastCommand);
        //        }
        //    });
        //    _output.WriteLine(ex.ToString());
        //    _output.WriteLine(ex.Number.ToString());
        //    Assert.Equal(0, ex.Number);
        //}

        //[Fact]
        //public void Update系_カラムサイズオーバーエラー_Entity利用()
        //{
        //    // MySqlExceptionが発生したらOK
        //    var ex = Assert.Throws<MySqlException>(() =>
        //    {
        //        using (var db = new DB())
        //        {
        //            db.BeginTransaction();

        //            var rec = db.SingleById<PtTable01>("11");
        //            rec.ColVarchar = "1234567890123456789012345";
        //            db.Update(rec);
        //            _output.WriteLine(db.LastCommand);
        //        }
        //    });
        //    _output.WriteLine(ex.ToString());
        //    _output.WriteLine(ex.Number.ToString());
        //    Assert.Equal(0, ex.Number);
        //}

        //[Fact]
        //public void Update系_カラムサイズオーバーエラー_INT()
        //{
        //    // MySqlExceptionが発生したらOK
        //    var ex = Assert.Throws<MySqlException>(() =>
        //    {
        //        using (var db = new DB())
        //        {
        //            db.BeginTransaction();

        //            var sql = PetaPoco.Sql.Builder
        //                .Append("UPDATE PtTable01")
        //                .Append("   SET ColInt = @0", 1234567890123456789012345m)
        //                .Append(" WHERE Key01 = @0", "11");
        //            db.Execute(sql);
        //            _output.WriteLine(db.LastCommand);
        //        }
        //    });
        //    _output.WriteLine(ex.ToString());
        //    _output.WriteLine(ex.Number.ToString());
        //    Assert.Equal(0, ex.Number);
        //}

        [Fact]
        public void Update系_NULL制約エラー()
        {
            var ex = Assert.Throws<MySqlException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();

                    var sql = PetaPoco.Sql.Builder
                        .Append("UPDATE PtTable01")
                        .Append("   SET CreateBy = NULL")
                        .Append(" WHERE Key01 = @0", "11");
                    db.Execute(sql);
                    _output.WriteLine(db.LastCommand);
                }
            });
            _output.WriteLine(ex.ToString());
            _output.WriteLine(ex.Number.ToString());
            Assert.Equal(1048, ex.Number);
        }

        [Fact]
        public void Update系_更新列のみ更新_事前読込あり()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var rec = db.SingleById<PtTable01>("11");
                rec.ColVarchar = "1234567890";
                var cnt = db.Update(rec);
                _output.WriteLine(db.LastCommand);

                Assert.Equal(1, cnt);

                var sql = db.LastSQL;

                // これは含まれるべき文字列
                Assert.Contains("ColVarchar", sql);

                // 含まれてはいけない文字列
                Assert.DoesNotContain("ColBool", sql);
                Assert.DoesNotContain("ColInt", sql);
                Assert.DoesNotContain("ColDec", sql);
                Assert.DoesNotContain("CreateBy", sql);
                Assert.DoesNotContain("UpdateDt", sql);

            }
        }

        [Fact]
        public void Update系_変更列のみ更新_NewRec_PK引数あり_列引数なし()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                // 更新する値
                var rec = new PtTable01
                {
                    ColVarchar = "1234567890",
                };

                var cnt = db.Update(rec, "13");   // PKを指定するメソッド
                _output.WriteLine(db.LastCommand);

                Assert.Equal(1, cnt);

                var sql = db.LastSQL;

                // これは含まれるべき文字列
                Assert.Contains("ColVarchar", sql);

                // 含まれてはいけない文字列
                Assert.DoesNotContain("ColBool", sql);
                Assert.DoesNotContain("ColInt", sql);
                Assert.DoesNotContain("ColDec", sql);
                Assert.DoesNotContain("CreateBy", sql);
                Assert.DoesNotContain("UpdateDt", sql);
            }
        }

        [Fact]
        public void Update系_変更列のみ更新_NewRec_PK引数なし_列引数なし()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                // レコード内にPK値も持つ
                var rec = new PtTable01
                {
                    Key01 = "13",
                    ColVarchar = "1234567890",
                };

                var cnt = db.Update(rec);       // PKはレコード内にあるパターン
                _output.WriteLine(db.LastCommand);

                Assert.Equal(1, cnt);

                var sql = db.LastSQL;

                // これは含まれるべき文字列
                Assert.Contains("ColVarchar", sql);

                // 含まれてはいけない文字列
                Assert.DoesNotContain("ColBool", sql);
                Assert.DoesNotContain("ColInt", sql);
                Assert.DoesNotContain("ColDec", sql);
                Assert.DoesNotContain("CreateBy", sql);
                Assert.DoesNotContain("UpdateDt", sql);
            }
        }

        [Fact]
        public void Update系_変更列のみ更新_NewRec_PK引数あり2_列引数なし()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                // 更新する値
                var rec = new PtTable02
                {
                    ColVarchar = "1234567890",
                };

                // PK
                var pk = new
                {
                    Key01 = "13",
                    Key02 = 13,
                };

                var cnt = db.Update(rec, pk);   // PKを指定するメソッド
                _output.WriteLine(db.LastCommand);

                Assert.Equal(1, cnt);

                var sql = db.LastSQL;

                // これは含まれるべき文字列
                Assert.Contains("ColVarchar", sql);

                // 含まれてはいけない文字列
                Assert.DoesNotContain("ColBool", sql);
                Assert.DoesNotContain("ColInt", sql);
                Assert.DoesNotContain("ColDec", sql);
                Assert.DoesNotContain("CreateBy", sql);
                Assert.DoesNotContain("UpdateDt", sql);
            }
        }

        [Fact]
        public void Update系_変更列のみ更新_NewRec_PK引数なし2_列引数なし()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

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
                Assert.Contains("ColVarchar", sql);

                // 含まれてはいけない文字列
                Assert.DoesNotContain("ColBool", sql);
                Assert.DoesNotContain("ColInt", sql);
                Assert.DoesNotContain("ColDec", sql);
                Assert.DoesNotContain("CreateBy", sql);
                Assert.DoesNotContain("UpdateDt", sql);
            }
        }

        [Fact]
        public void Update系_変更列のみ更新_NewRec_PK引数なし_列引数あり()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                // 更新する値
                var rec = new PtTable02
                {
                    Key01 = "13",
                    Key02 = 13,
                    ColVarchar = "1234567890",
                    CreateBy = "hogehoge",
                    CreateDt = DateTime.Now,
                    UpdateBy = "hogehoge",
                    UpdateDt = DateTime.Now,
                };
                // 更新する列だけを別に指定
                var updatingCols = new string[]
                {
                    "ColVarchar",
                    "UpdateBy",
                };

                var cnt = db.Update(rec, updatingCols);   // 更新列を指定するメソッド
                _output.WriteLine(db.LastCommand);

                Assert.Equal(1, cnt);

                var sql = db.LastSQL;

                // これは含まれるべき文字列
                Assert.Contains("ColVarchar", sql);
                Assert.Contains("UpdateBy", sql);

                // 含まれてはいけない文字列
                Assert.DoesNotContain("ColBool", sql);
                Assert.DoesNotContain("ColInt", sql);
                Assert.DoesNotContain("ColDec", sql);
                Assert.DoesNotContain("CreateBy", sql);
                Assert.DoesNotContain("UpdateDt", sql);
            }
        }

        [Fact]
        public void Update系_変更列のみ更新_NewRec_PK引数あり_列引数あり()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                // 更新する値
                var rec = new PtTable02
                {
                    ColVarchar = "1234567890",
                    CreateBy = "hogehoge",
                    CreateDt = DateTime.Now,
                    UpdateBy = "hogehoge",
                    UpdateDt = DateTime.Now,
                };
                // 更新する列だけを別に指定
                var updatingCols = new string[]
                {
                    "ColVarchar",
                    "UpdateBy",
                };

                // PK
                var pk = new
                {
                    Key01 = "13",
                    Key02 = 13,
                };

                var cnt = db.Update(rec, pk, updatingCols);   // PKと更新列を指定するメソッド
                _output.WriteLine(db.LastCommand);

                Assert.Equal(1, cnt);

                var sql = db.LastSQL;

                // これは含まれるべき文字列
                Assert.Contains("ColVarchar", sql);
                Assert.Contains("UpdateBy", sql);

                // 含まれてはいけない文字列
                Assert.DoesNotContain("ColBool", sql);
                Assert.DoesNotContain("ColInt", sql);
                Assert.DoesNotContain("ColDec", sql);
                Assert.DoesNotContain("CreateBy", sql);
                Assert.DoesNotContain("UpdateDt", sql);
            }
        }


        [Fact]
        public void Update系_Transaction_Commit()
        {
            using (var db = new DB())
            {
                // PtTable02 の Key01=12 を取得して確保する
                var pk = new
                {
                    Key01 = "12",
                    Key02 = 12,
                };
                var recbefore = db.SingleById<PtTable02>(pk);
                Assert.Equal("千葉県", recbefore.ColVarchar);

                // Transactionを開始する
                db.BeginTransaction();

                // 下記Updateを実行する
                var recafter = db.SingleById<PtTable02>(pk);
                recafter.ColVarchar = "Update-Commitテスト";
                db.Update(recafter);

                // PtTable02 の Key01=12 を取得して内容を確認する
                var rec_03 = db.SingleById<PtTable02>(pk);
                Assert.Equal("Update-Commitテスト", rec_03.ColVarchar);

                // TransactionをCommitする
                db.CompleteTransaction();

                // PtTable02 の Key01=12 を取得して内容を確認する
                var rec_04 = db.SingleById<PtTable02>(pk);
                Assert.Equal("Update-Commitテスト", rec_03.ColVarchar);
            }
        }

        [Fact]
        public void Update系_Transaction_Rollback()
        {
            using (var db = new DB())
            {
                // PtTable02 の Key01=13 を取得して確保する
                var pk = new
                {
                    Key01 = "13",
                    Key02 = 13,
                };
                var recbefore = db.SingleById<PtTable02>(pk);
                string before_char = recbefore.ColVarchar;

                // Transactionを開始する
                db.BeginTransaction();

                // 下記Updateを実行する
                var recafter = db.SingleById<PtTable02>(pk);
                recafter.ColVarchar = "Update-Rollbackテスト";
                db.Update(recafter);

                // PtTable02 の Key01=13 を取得して内容を確認する
                var rec_03 = db.SingleById<PtTable02>(pk);
                Assert.Equal("Update-Rollbackテスト", rec_03.ColVarchar);

                // TransactionをRollbackする
                db.AbortTransaction();

                // PtTable02 の Key01=12 を取得して内容を確認する
                var rec_04 = db.SingleById<PtTable02>(pk);
                Assert.Equal(rec_04.ColVarchar, before_char);
            }
        }

    }
}
