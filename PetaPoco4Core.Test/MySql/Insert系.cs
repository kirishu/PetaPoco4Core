using System;
using Xunit;
using Xunit.Abstractions;
using MySql.Data.MySqlClient;

namespace PetaPoco4Core.Test.MySql
{
    public class Insert系: TestBase
    {
        public Insert系(ITestOutputHelper output) : base(output) { }


        [Fact]
        public void PT001_Execute_1件挿入DDL()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var cnt = db.Execute("INSERT INTO PtTable01 values ('91',true, 123,9999.99,'Insert''テスト''その１','pt_test001','2018/12/18 00:00:00','pt_test001','2018/12/18 18:00:00')");

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
        public void PT002_Insert_1件挿入Entity()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var rec = new PtTable01
                {
                    Key01 = "92",
                    ColBool = true,
                    ColInt = 123,
                    ColDec = 987654.32m,
                    ColVarchar = "Insert'テスト'その２",
                    CreateBy = "pt_test001",
                    CreateDt = DateTime.Parse("2018/12/18 18:00:00"),
                    UpdateBy = "pt_test001",
                    UpdateDt = DateTime.Parse("2018/12/18 18:00:00"),
                };
                db.Insert(rec);

                var rec2 = db.SingleById<PtTable01>("92");
                Assert.True(rec2.ColBool);
                Assert.Equal(123, rec2.ColInt.Value);
                Assert.Equal(987654.32m, rec2.ColDec.Value);
                Assert.Equal("Insert'テスト'その２", rec2.ColVarchar);
                Assert.Equal("pt_test001", rec2.CreateBy);
                Assert.Equal(DateTime.Parse("2018/12/18 18:00:00"), rec2.CreateDt);
            }

        }

        [Fact]
        public void PT003_Execute_キー重複エラーDDL()
        {
            var ex = Assert.Throws<MySqlException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    var cnt = db.Execute("INSERT INTO PtTable01 values ('01',true, 123,9999.99,'Insert''テスト''その１','pt_test001','2018/12/18 00:00:00','pt_test001','2018/12/18 18:00:00')");
                }
            });
            _output.WriteLine(ex.ToString());
            Assert.Equal(1062, ex.Number);
        }

        [Fact]
        public void PT004_Insert_キー重複エラーEntity()
        {
            var ex = Assert.Throws<MySqlException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

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
            _output.WriteLine(ex.ToString());
            Assert.Equal(1062, ex.Number);
        }


        //[Fact]
        //public void PT005_Execute_サイズオーバーエラーDDL()
        //{
        //    var ex = Assert.Throws<MySqlException>(() =>
        //    {
        //        using (var db = new DB())
        //        {
        //            db.BeginTransaction();
        //            TestCommon.CreateTempTable01(db);

        //            var cnt = db.Execute("INSERT INTO PtTable01 values ('93',true, 123,9999.99, '123456789012345678901233456789012334567890123345678901233456789012334567890123345678901233456789012334567890123', 'pt_test001','2018/12/18 00:00:00','pt_test001','2018/12/18 18:00:00')");
        //        }
        //    });
        //    _output.WriteLine(ex.ToString());
        //    _output.WriteLine(ex.Number.ToString());
        //    //Assert.Equal("22001", ex.SqlState);
        //}

        //[Fact]
        //public void PT006_Insert_サイズオーバーエラーEntity()
        //{
        //    var ex = Assert.Throws<MySqlException>(() =>
        //    {
        //        using (var db = new DB())
        //        {
        //            db.BeginTransaction();
        //            TestCommon.CreateTempTable01(db);

        //            var rec = new PtTable01
        //            {
        //                Key01 = "94",
        //                ColBool = true,
        //                ColInt = 123,
        //                ColDec = 987654.32m,
        //                ColVarchar = "12345678901234567890123",
        //                CreateBy = "pt_test001",
        //                CreateDt = DateTime.Now,
        //                UpdateBy = "pt_test001",
        //                UpdateDt = DateTime.Now,
        //            };
        //            db.Insert(rec);
        //        }
        //    });
        //    _output.WriteLine(ex.ToString());
        //    //Assert.Equal("22001", ex.SqlState);
        //    _output.WriteLine(ex.Number.ToString());

        //}

        [Fact]
        public void PT007_Transaction_Commit()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

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

        //[Fact]
        //public void PT008_Transaction_Rollback()
        //{
        //    using (var db = new DB())
        //    {
        //        db.BeginTransaction();
        //        TestCommon.CreateTempTable01(db);
        //        TestCommon.CreateTempTable02(db);

        //        var rec = new PtTable01
        //        {
        //            Key01 = "96",
        //            ColBool = true,
        //            ColInt = 123,
        //            ColDec = 987654.32m,
        //            ColVarchar = "Transactionテスト",
        //            CreateBy = "pt_test001",
        //            CreateDt = DateTime.Parse("2018/12/18 18:00:00"),
        //            UpdateBy = "pt_test001",
        //            UpdateDt = DateTime.Parse("2018/12/18 18:00:00"),
        //        };
        //        db.Insert(rec);

        //        db.AbortTransaction();

        //        var rec2 = db.SingleOrDefaultById<PtTable01>("96");
        //        Assert.IsNull(rec2);
        //    }
        //}



    }
}
