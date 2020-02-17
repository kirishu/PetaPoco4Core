﻿using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace PetaPoco4Core.Test.PostgreSql
{
    public class Insert系: TestBase
    {
        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="output"></param>
        public Insert系(ITestOutputHelper output) : base(output, TestCommon.Instance) { }


        [Fact]
        public void INS001_Execute_1件挿入DDL()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var cnt = db.Execute("INSERT INTO pt_table01 values ('91',true, 123,9999.99,'Insert''テスト''その１','pt_test001','2018/12/18 00:00:00','pt_test001','2018/12/18 18:00:00')");

                Assert.Equal(1, cnt);

                var rec = db.SingleById<PtTable01>("91");
                Assert.True(rec.ColBool);
                Assert.Equal(123, rec.ColInt.Value);
                Assert.Equal(9999.99m, rec.ColDec.Value);
                Assert.Equal("Insert'テスト'その１", rec.ColVarchar);
                Assert.Equal("pt_test001", rec.CreateBy);
                Assert.Equal(DateTime.Parse("2018/12/18 00:00:00"), rec.CreateDt);
            }
        }

        [Fact]
        public void INS002_Insert_1件挿入Entity()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var rec = new PtTable02
                {
                    Key01 = "92",
                    Key02 = 92,
                    ColBool = true,
                    ColInt = 123,
                    ColDec = 987654.32m,
                    ColVarchar = "Insert'テスト'その２",
                    CreateBy = "pt_test001",
                    CreateDt = DateTime.Parse("2018/12/18 18:00:00"),
                    UpdateBy = "pt_test001",
                    UpdateDt = DateTime.Parse("2018/12/18 18:00:00"),
                };
                var result = db.Insert(rec);
                _output.WriteLine(db.LastSQL);
                _output.WriteLine(result.ToString());

                Assert.Equal("-1", result.ToString());      // AutoIncrementでない場合は-1が返る

                var rec2 = db.SingleById<PtTable02>(new { Key01 = "92", Key02 = 92 });
                Assert.True(rec2.ColBool);
                Assert.Equal(123, rec2.ColInt.Value);
                Assert.Equal(987654.32m, rec2.ColDec.Value);
                Assert.Equal("Insert'テスト'その２", rec2.ColVarchar);
                Assert.Equal("pt_test001", rec2.CreateBy);
                Assert.Equal(DateTime.Parse("2018/12/18 18:00:00"), rec2.CreateDt);
            }

        }

        [Fact]
        public void INS003_Execute_キー重複エラーDDL()
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
            Assert.Equal("23505", ex.SqlState);
        }

        [Fact]
        public void INS004_Insert_キー重複エラーEntity()
        {
            // Npgsql.PostgresExceptionが発生したらOK
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();

                    var rec = new PtTable01
                    {
                        Key01 = "02",
                        ColBool = true,
                        ColInt = 123,
                        ColDec = 987654.32m,
                        ColVarchar = "Insert'テスト'その２",
                        CreateBy = "pt_test001",
                        CreateDt = DateTime.Now,
                        UpdateBy = "pt_test001",
                        UpdateDt = DateTime.Now,
                    };
                    db.Insert(rec);
                }
            });
            Assert.Equal("23505", ex.SqlState);
        }


        [Fact]
        public void INS005_Execute_サイズオーバーエラーDDL()
        {
            // Npgsql.PostgresExceptionが発生したらOK
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();

                    var cnt = db.Execute("INSERT INTO pt_table01 values ('93',true, 123,9999.99, '12345678901234567890123', 'pt_test001','2018/12/18 00:00:00','pt_test001','2018/12/18 18:00:00')");
                }
            });
            Assert.Equal("22001", ex.SqlState);
        }

        [Fact]
        public void INS006_Insert_サイズオーバーエラーEntity()
        {
            // Npgsql.PostgresExceptionが発生したらOK
            var ex = Assert.Throws<Npgsql.PostgresException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();

                    var rec = new PtTable01
                    {
                        Key01 = "94",
                        ColBool = true,
                        ColInt = 123,
                        ColDec = 987654.32m,
                        ColVarchar = "12345678901234567890123",
                        CreateBy = "pt_test001",
                        CreateDt = DateTime.Now,
                        UpdateBy = "pt_test001",
                        UpdateDt = DateTime.Now,
                    };
                    db.Insert(rec);
                }
            });
            Assert.Equal("22001", ex.SqlState);
        }

        [Fact]
        public void INS007_Transaction_Commit()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var rec = new PtTable01
                {
                    Key01 = "95",
                    ColBool = true,
                    ColInt = 123,
                    ColDec = 987654.32m,
                    ColVarchar = "Transactionテスト",
                    CreateBy = "pt_test001",
                    CreateDt = DateTime.Parse("2018/12/18 18:00:00"),
                    UpdateBy = "pt_test001",
                    UpdateDt = DateTime.Parse("2018/12/18 18:00:00"),
                };
                db.Insert(rec);

                //db.CompleteTransaction();
                var rec2 = db.SingleById<PtTable01>("95");
                Assert.True(rec2.ColBool);
                Assert.Equal(123, rec2.ColInt.Value);
                Assert.Equal(987654.32m, rec2.ColDec.Value);
                Assert.Equal("Transactionテスト", rec2.ColVarchar);
                Assert.Equal("pt_test001", rec2.CreateBy);
                Assert.Equal(DateTime.Parse("2018/12/18 18:00:00"), rec2.CreateDt);
            }
        }

        [Fact]
        public void INS008_Transaction_Rollback()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var rec = new PtTable01
                {
                    Key01 = "96",
                    ColBool = true,
                    ColInt = 123,
                    ColDec = 987654.32m,
                    ColVarchar = "Transactionテスト",
                    CreateBy = "pt_test001",
                    CreateDt = DateTime.Parse("2018/12/18 18:00:00"),
                    UpdateBy = "pt_test001",
                    UpdateDt = DateTime.Parse("2018/12/18 18:00:00"),
                };
                db.Insert(rec);

                db.AbortTransaction();

                var rec2 = db.SingleOrDefaultById<PtTable01>("96");
                Assert.Null(rec2);
            }
        }

        [Fact]
        public void INS009_AutoInclementKey()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();

                var rec = new PtTable03
                {
                    Key03 = 100,        // キー値をわざと指定しても、INSERT文には含まれない・・・というテスト
                    ColBool = true,
                    ColInt = 123,
                    ColDec = 987654.32m,
                    ColVarchar = "AutoIncrementテスト",
                    CreateBy = "pt_test001",
                    CreateDt = DateTime.Parse("2018/12/18 18:00:00"),
                    UpdateBy = "pt_test001",
                    UpdateDt = DateTime.Parse("2018/12/18 18:00:00"),
                };
                var newkey = db.Insert(rec);
                var sql = db.LastSQL;
                _output.WriteLine(sql);
                _output.WriteLine(newkey.ToString());

                Assert.NotNull(newkey);


                // SQLに含まれてはいけない文字列
                Assert.DoesNotContain("Key03", sql);


                var recafter = db.SingleOrDefaultById<PtTable03>(newkey);
                Assert.Equal("AutoIncrementテスト", recafter.ColVarchar);

            }
        }


    }
}
