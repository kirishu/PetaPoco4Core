using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PetaPoco4Core.Test.Fx.PostgreSql
{
    [TestClass]
    public class Select系
    {
        /// <summary>
        /// 初期処理
        /// </summary>
        /// <param name="context"></param>
        [ClassInitialize]
        public static void AssemblyInit(TestContext context)
        {
            //// DBテスト共通前処理
            //TestCommon.Initialize();
        }

        /// <summary>
        /// 終了処理
        /// </summary>
        [ClassCleanup]
        public static void AssemblyCleanup()
        {
            // アセンブリ内のすべてのテストが実行された後、アセンブリによって取得されたリソースを開放するために使用されるコードを含むメソッドを識別します。
        }

        [TestMethod]
        public void PT001_SingleById_PK1列()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var rec = db.SingleById<PtTable01>("03");
                Console.WriteLine(db.LastCommand);

                Assert.IsNotNull(rec);
                Assert.IsTrue(rec.ColBool);
                Assert.AreEqual(999, rec.ColInt.Value);
                Assert.AreEqual(123456.78m, rec.ColDec.Value);
                Assert.AreEqual("岩手県", rec.ColVarchar);
                Assert.AreEqual("system", rec.CreateBy);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);

                Assert.IsTrue(db.LastCommand.Contains("WHERE \"key01\" = @0"));
                Assert.IsTrue(db.LastCommand.Contains("@0 [String] = \"03\""));
            }
        }

        [TestMethod]
        public void PT002_SingleById_PK2列()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable02(db);

                var pk = new
                {
                    key01 = "03",
                    key02 = 3,
                };
                var rec = db.SingleById<PtTable02>(pk);
                Console.WriteLine(db.LastCommand);

                Assert.IsNotNull(rec);
                Assert.IsTrue(rec.ColBool);
                Assert.AreEqual(999, rec.ColInt.Value);
                Assert.AreEqual(123456.78m, rec.ColDec.Value);
                Assert.AreEqual("岩手県", rec.ColVarchar);
                Assert.AreEqual("system", rec.CreateBy);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);

                Assert.IsTrue(db.LastCommand.Contains("WHERE \"key01\" = @0 AND \"key02\" = @1"));
                Assert.IsTrue(db.LastCommand.Contains("@0 [String] = \"03\""));
                Assert.IsTrue(db.LastCommand.Contains("@1 [Int32] = \"3\""));
            }
        }


        [TestMethod]
        public void PT003_SingleById_PK1列ヒットなし()
        {
            // InvalidOperationExceptionが発生したらOK
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    var rec = db.SingleById<PtTable01>("999");
                }
            });
            Console.WriteLine(ex.ToString());

        }

        [TestMethod]
        public void PT004_SingleById_PK2列ヒットなし()
        {
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                // InvalidOperationExceptionが発生したらOK
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable02(db);

                    var pk = new
                    {
                        key01 = "999",
                        key02 = 999,
                    };
                    var rec = db.SingleById<PtTable02>(pk);
                }
            });
            Console.WriteLine(ex.ToString());
        }

        [TestMethod]
        public void PT005_SingleOrDefaultById_PK1列()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var rec = db.SingleOrDefaultById<PtTable01>("04");
                Console.WriteLine(db.LastCommand);

                Assert.IsNotNull(rec);
                Assert.IsTrue(rec.ColBool);
                Assert.AreEqual(999, rec.ColInt.Value);
                Assert.AreEqual(123456.78m, rec.ColDec.Value);
                Assert.AreEqual("宮城県", rec.ColVarchar);
                Assert.AreEqual("system", rec.CreateBy);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [TestMethod]
        public void PT006_SingleOrDefaultById_PK2列()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable02(db);

                var pk = new
                {
                    key01 = "04",
                    key02 = 4,
                };

                var rec = db.SingleOrDefaultById<PtTable02>(pk);
                Console.WriteLine(db.LastCommand);

                Assert.IsNotNull(rec);
                Assert.IsTrue(rec.ColBool);
                Assert.AreEqual(999, rec.ColInt.Value);
                Assert.AreEqual(123456.78m, rec.ColDec.Value);
                Assert.AreEqual("宮城県", rec.ColVarchar);
                Assert.AreEqual("system", rec.CreateBy);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [TestMethod]
        public void PT007_SingleOrDefaultById_PK1列ヒットなし()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                // 0件でOK
                var rec = db.SingleOrDefaultById<PtTable01>("999");
                Console.WriteLine(db.LastCommand);

                Assert.IsNull(rec);
            }
        }

        [TestMethod]
        public void PT008_SingleOrDefaultById_PK2列ヒットなし()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable02(db);

                var pk = new
                {
                    key01 = "999",
                    key02 = 4,
                };
                var rec = db.SingleOrDefaultById<PtTable02>(pk);
                Console.WriteLine(db.LastCommand);

                Assert.IsNull(rec);
            }
        }

        [TestMethod]
        public void PT009_Single_条件1つを指定して1件取得()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var rec = db.Single<PtTable01>("WHERE key01 = @0", "03");
                Console.WriteLine(db.LastCommand);

                Assert.IsNotNull(rec);
                Assert.IsTrue(rec.ColBool);
                Assert.AreEqual(999, rec.ColInt.Value);
                Assert.AreEqual(123456.78m, rec.ColDec.Value);
                Assert.AreEqual("岩手県", rec.ColVarchar);
                Assert.AreEqual("system", rec.CreateBy);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [TestMethod]
        public void PT010_Single_条件2つを指定して1件取得()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable02(db);

                var rec = db.Single<PtTable02>("WHERE key01=@0 AND key02=@1", "03", 3);
                Console.WriteLine(db.LastCommand);

                Assert.IsNotNull(rec);
                Assert.IsTrue(rec.ColBool);
                Assert.AreEqual(999, rec.ColInt.Value);
                Assert.AreEqual(123456.78m, rec.ColDec.Value);
                Assert.AreEqual("岩手県", rec.ColVarchar);
                Assert.AreEqual("system", rec.CreateBy);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [TestMethod]
        public void PT011_Single_条件を指定してヒットなしエラー()
        {
            // InvalidOperationExceptionが発生したらOK
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    var rec = db.Single<PtTable01>("WHERE key01 = @0", "999");
                    Console.WriteLine(db.LastCommand);
                }
            });
            Console.WriteLine(ex.ToString());
        }


        [TestMethod]
        public void PT012_SingleOrDefault_条件1つを指定して1件取得()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var rec = db.SingleOrDefault<PtTable01>("WHERE key01 = @0", "03");
                Console.WriteLine(db.LastCommand);

                Assert.IsNotNull(rec);
                Assert.IsTrue(rec.ColBool);
                Assert.AreEqual(999, rec.ColInt.Value);
                Assert.AreEqual(123456.78m, rec.ColDec.Value);
                Assert.AreEqual("岩手県", rec.ColVarchar);
                Assert.AreEqual("system", rec.CreateBy);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [TestMethod]
        public void PT013_SingleOrDefault_条件2つを指定して1件取得()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable02(db);

                var rec = db.SingleOrDefault<PtTable02>("WHERE key01=@0 AND key02=@1", "03", 3);
                Console.WriteLine(db.LastCommand);

                Assert.IsNotNull(rec);
                Assert.IsTrue(rec.ColBool);
                Assert.AreEqual(999, rec.ColInt.Value);
                Assert.AreEqual(123456.78m, rec.ColDec.Value);
                Assert.AreEqual("岩手県", rec.ColVarchar);
                Assert.AreEqual("system", rec.CreateBy);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [TestMethod]
        public void PT014_SingleOrDefault_条件を指定して1件ヒットなし()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var rec = db.SingleOrDefault<PtTable01>("WHERE key01 = @0", "999");
                Console.WriteLine(db.LastCommand);

                Assert.IsNull(rec);
            }
        }

        [TestMethod]
        public void PT015_SingleOrDefault_複数ヒットエラー()
        {
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                // InvalidOperationExceptionが発生したらOK
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    var rec = db.SingleOrDefault<PtTable01>("WHERE create_by = @0", "system");
                    Console.WriteLine(db.LastCommand);

                }
            });
            Console.WriteLine(ex.ToString());
        }


        [TestMethod]
        public void PT016_First_該当する先頭行を取得()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var rec = db.First<PtTable01>("WHERE key01 < @0 ORDER BY key01 DESC", "06");
                Console.WriteLine(db.LastCommand);

                Assert.IsNotNull(rec);
                Assert.IsTrue(rec.ColBool);
                Assert.AreEqual(999, rec.ColInt.Value);
                Assert.AreEqual(123456.78m, rec.ColDec.Value);
                Assert.AreEqual("秋田県", rec.ColVarchar);
                Assert.AreEqual("system", rec.CreateBy);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [TestMethod]
        public void PT017_First_ヒットなしエラー()
        {
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                // InvalidOperationExceptionが発生したらOK
                using (var db = new DB())
                {
                    db.BeginTransaction();
                    TestCommon.CreateTempTable01(db);

                    db.First<PtTable01>("WHERE key01 > @0 ORDER BY key01 DESC", "9999");
                    Console.WriteLine(db.LastCommand);

                }
            });
            Console.WriteLine(ex.ToString());
        }

        [TestMethod]
        public void PT018_FirstOrDefault_該当する先頭行を取得()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var rec = db.FirstOrDefault<PtTable01>("WHERE key01 < @0 ORDER BY key01 DESC", "06");
                Console.WriteLine(db.LastCommand);

                Assert.IsNotNull(rec);
                Assert.IsTrue(rec.ColBool);
                Assert.AreEqual(999, rec.ColInt.Value);
                Assert.AreEqual(123456.78m, rec.ColDec.Value);
                Assert.AreEqual("秋田県", rec.ColVarchar);
                Assert.AreEqual("system", rec.CreateBy);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [TestMethod]
        public void PT019_FirstOrDefault_ヒットなし()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var rec = db.FirstOrDefault<PtTable01>("WHERE key01 > @0 ORDER BY key01 DESC", "9999");
                Console.WriteLine(db.LastCommand);

                Assert.IsNull(rec);
            }
        }

        [TestMethod]
        public void PT020_ExecuteScalar_string型()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var res = db.ExecuteScalar<string>("SELECT col_varchar FROM pt_table01 WHERE key01 = @0", "05");
                Console.WriteLine(db.LastCommand);

                Assert.AreEqual("秋田県", res);
            }
        }

        [TestMethod]
        public void PT021_ExecuteScalar_int型()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var res = db.ExecuteScalar<int>("SELECT COUNT(*) FROM pt_table01");
                Assert.AreEqual(15, res);
                Console.WriteLine(db.LastCommand);

            }
        }


        [TestMethod]
        public void PT022_ExecuteScalar_DateTime型()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var res = db.ExecuteScalar<DateTime>("SELECT update_dt FROM pt_table01 WHERE key01 = @0", "05");
                Console.WriteLine(db.LastCommand);

                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), res);
            }
        }


        [TestMethod]
        public void PT023_Single_内部結合クエリ()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);
                TestCommon.CreateTempTable02(db);

                var rec = db.Single<JoinTest>(PetaPoco.Sql.Builder
                    .Append("SELECT  t01.key01 AS Key01")
                    .Append("       ,t01.col_varchar AS ColVarchar")
                    .Append("       ,t01.create_by AS CreateBy")
                    .Append("       ,t01.create_dt AS CreateDt")
                    .Append("       ,t02.col_varchar AS ColVarchar02")
                    .Append("       ,t02.create_dt AS CreateDt02")
                    .Append("  FROM pt_table01 AS t01")
                    .Append("       INNER JOIN pt_table02 AS t02")
                    .Append("               ON t01.key01 = t02.key01")
                    .Append("              AND t02.key02 = @0", 2)
                    .Append(" WHERE t01.key01 = @0", "02")
                    );
                Console.WriteLine(db.LastCommand);

                Assert.IsNotNull(rec);
                Assert.IsFalse(rec.ColBool);
                Assert.IsNull(rec.ColInt);
                Assert.IsNull(rec.ColDec);
                Assert.AreEqual("青森県", rec.ColVarchar);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
                Assert.AreEqual("青森県", rec.ColVarchar02);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt02);
            }
        }

        [TestMethod]
        public void PT024_Single_外部結合クエリ()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);
                TestCommon.CreateTempTable02(db);

                var rec = db.Single<JoinTest>(PetaPoco.Sql.Builder
                    .Append("SELECT  t01.key01 AS Key01")
                    .Append("       ,t01.col_varchar AS ColVarchar")
                    .Append("       ,t01.create_by AS CreateBy")
                    .Append("       ,t01.create_dt AS CreateDt")
                    .Append("       ,t02.col_varchar AS ColVarchar02")
                    .Append("       ,t02.create_dt AS CreateDt02")
                    .Append("  FROM pt_table01 AS t01")
                    .Append("       LEFT JOIN pt_table02 AS t02")
                    .Append("              ON t01.key01 = t02.key01")
                    .Append("             AND t02.key02 = @0", 0)       // table02はヒットなし
                    .Append(" WHERE t01.key01 = @0", "13")
                    );

                Console.WriteLine(db.LastCommand);

                Assert.IsNotNull(rec);
                Assert.AreEqual("東京都", rec.ColVarchar);
                Assert.AreEqual(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
                Assert.IsNull(rec.ColVarchar02);
                Assert.AreEqual(DateTime.Parse("0001/01/01 00:00:00"), rec.CreateDt02);
            }
        }

        [TestMethod]
        public void PT025_Fetch_単一テーブルヒットあり()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var recs = db.Fetch<PtTable01>("ORDER BY key01");
                Console.WriteLine(db.LastCommand);


                Assert.AreEqual(15, recs.Count);
                Assert.AreEqual("北海道", recs.ElementAt(0).ColVarchar);
                Assert.AreEqual("青森県", recs.ElementAt(1).ColVarchar);
                Assert.AreEqual("東京都", recs.ElementAt(12).ColVarchar);
            }
        }

        [TestMethod]
        public void PT026_Fetch_単一テーブルヒットなし()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var recs = db.Fetch<PtTable01>("WHERE key01 = @0 ORDER BY key01", "999");
                Console.WriteLine(db.LastCommand);


                Assert.AreEqual(recs.Count, 0);
            }
        }

        [TestMethod]
        public void PT027_Fetch_外部結合クエリ()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);
                TestCommon.CreateTempTable02(db);

                var recs = db.Fetch<JoinTest>(PetaPoco.Sql.Builder
                    .Append("SELECT  t02.key01 AS Key01")
                    .Append("       ,t02.col_varchar AS ColVarchar")
                    .Append("       ,t02.create_by AS CreateBy")
                    .Append("       ,t01.col_varchar AS ColVarchar02")
                    .Append("       ,t01.create_dt AS CreateDt02")
                    .Append("  FROM pt_table02 AS t02")
                    .Append("       LEFT JOIN pt_table01 AS t01")
                    .Append("              ON t02.key01 = t01.key01")
                    .Append(" ORDER BY t02.key01, t02.key02")
                    );

                Console.WriteLine(db.LastCommand);

                Assert.AreEqual(15, recs.Count);
                Assert.AreEqual("宮城県", recs.ElementAt(3).ColVarchar);
                Assert.AreEqual("秋田県", recs.ElementAt(4).ColVarchar);
                Assert.AreEqual("山形県", recs.ElementAt(5).ColVarchar);
            }
        }

        [TestMethod]
        public void PT028_Dictionary_その1()
        {
            using (var db = new DB())
            {
                db.BeginTransaction();
                TestCommon.CreateTempTable01(db);

                var recs = db.Dictionary<string, string>("SELECT key01, col_varchar FROM pt_table01 ORDER BY key01");

                Console.WriteLine(db.LastCommand);

                Assert.AreEqual(15, recs.Count);
                Assert.AreEqual("北海道", recs["01"]);
                Assert.AreEqual("埼玉県", recs["11"]);
            }
        }

    }
}
