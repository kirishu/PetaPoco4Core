using System;
using System.Linq;
using Xunit;

namespace PetaPoco4Core.Test.MySql
{
    public partial class MySqlTest
    {
        [Fact]
        public void Select系_SingleById_PK1列()
        {
            using (var db = new DB())
            {
                var rec = db.SingleById<PtTable01>("03");
                _output.WriteLine(db.LastCommand);

                Assert.NotNull(rec);
                Assert.True(rec.ColBool);
                Assert.Equal(999, rec.ColInt.Value);
                Assert.Equal(123456.78m, rec.ColDec.Value);
                Assert.Equal("岩手県", rec.ColVarchar);
                Assert.Equal("system", rec.CreateBy);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);

                Assert.Contains("WHERE `Key01` = @0", db.LastCommand);
                Assert.Contains("@0 [String] = \"03\"", db.LastCommand);
            }
        }

        [Fact]
        public void Select系_SingleById_PK2列()
        {
            using (var db = new DB())
            {
                var pk = new
                {
                    Key01 = "03",
                    Key02 = 3,
                };
                var rec = db.SingleById<PtTable02>(pk);
                _output.WriteLine(db.LastCommand);

                Assert.NotNull(rec);
                Assert.True(rec.ColBool);
                Assert.Equal(999, rec.ColInt.Value);
                Assert.Equal(123456.78m, rec.ColDec.Value);
                Assert.Equal("岩手県", rec.ColVarchar);
                Assert.Equal("system", rec.CreateBy);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);

                Assert.Contains("WHERE `Key01` = @0 AND `Key02` = @1", db.LastCommand);
                Assert.Contains("@0 [String] = \"03\"", db.LastCommand);
                Assert.Contains("@1 [Int32] = \"3\"", db.LastCommand);
            }
        }


        [Fact]
        public void Select系_SingleById_PK1列ヒットなし()
        {
            // InvalidOperationExceptionが発生したらOK
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                using (var db = new DB())
                {
                    var rec = db.SingleById<PtTable01>("999");
                }
            });
            _output.WriteLine(ex.ToString());

        }

        [Fact]
        public void Select系_SingleById_PK2列ヒットなし()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                // InvalidOperationExceptionが発生したらOK
                using (var db = new DB())
                {
                    var pk = new
                    {
                        Key01 = "999",
                        Key02 = 999,
                    };
                    var rec = db.SingleById<PtTable02>(pk);
                }
            });
            _output.WriteLine(ex.ToString());
        }

        [Fact]
        public void Select系_SingleOrDefaultById_PK1列()
        {
            using (var db = new DB())
            {
                var rec = db.SingleOrDefaultById<PtTable01>("04");
                _output.WriteLine(db.LastCommand);

                Assert.NotNull(rec);
                Assert.True(rec.ColBool);
                Assert.Equal(999, rec.ColInt.Value);
                Assert.Equal(123456.78m, rec.ColDec.Value);
                Assert.Equal("宮城県", rec.ColVarchar);
                Assert.Equal("system", rec.CreateBy);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [Fact]
        public void Select系_SingleOrDefaultById_PK2列()
        {
            using (var db = new DB())
            {
                var pk = new
                {
                    Key01 = "04",
                    Key02 = 4,
                };

                var rec = db.SingleOrDefaultById<PtTable02>(pk);
                _output.WriteLine(db.LastCommand);

                Assert.NotNull(rec);
                Assert.True(rec.ColBool);
                Assert.Equal(999, rec.ColInt.Value);
                Assert.Equal(123456.78m, rec.ColDec.Value);
                Assert.Equal("宮城県", rec.ColVarchar);
                Assert.Equal("system", rec.CreateBy);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [Fact]
        public void Select系_SingleOrDefaultById_PK1列ヒットなし()
        {
            using (var db = new DB())
            {
                // 0件でOK
                var rec = db.SingleOrDefaultById<PtTable01>("999");
                _output.WriteLine(db.LastCommand);

                Assert.Null(rec);
            }
        }

        [Fact]
        public void Select系_SingleOrDefaultById_PK2列ヒットなし()
        {
            using (var db = new DB())
            {
                var pk = new
                {
                    Key01 = "999",
                    Key02 = 4,
                };
                var rec = db.SingleOrDefaultById<PtTable02>(pk);
                _output.WriteLine(db.LastCommand);

                Assert.Null(rec);
            }
        }

        [Fact]
        public void Select系_Single_条件1つを指定して1件取得()
        {
            using (var db = new DB())
            {
                var rec = db.SingleOrDefault<PtTable01>("WHERE Key01 = @0", "03");
                _output.WriteLine(db.LastCommand);

                Assert.NotNull(rec);
                Assert.True(rec.ColBool);
                Assert.Equal(999, rec.ColInt.Value);
                Assert.Equal(123456.78m, rec.ColDec.Value);
                Assert.Equal("岩手県", rec.ColVarchar);
                Assert.Equal("system", rec.CreateBy);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [Fact]
        public void Select系_Single_条件2つを指定して1件取得()
        {
            using (var db = new DB())
            {
                var rec = db.SingleOrDefault<PtTable02>("WHERE Key01=@0 AND Key02=@1", "03", 3);
                _output.WriteLine(db.LastCommand);

                Assert.NotNull(rec);
                Assert.True(rec.ColBool);
                Assert.Equal(999, rec.ColInt.Value);
                Assert.Equal(123456.78m, rec.ColDec.Value);
                Assert.Equal("岩手県", rec.ColVarchar);
                Assert.Equal("system", rec.CreateBy);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        //[Fact]
        //public void Select系_Single_条件を指定してヒットなしエラー()
        //{
        //    // InvalidOperationExceptionが発生したらOK
        //    var ex = Assert.Throws<InvalidOperationException>(() =>
        //    {
        //        using (var db = new DB())
        //        {
        //            var rec = db.Single<PtTable01>("WHERE Key01 = @0", "999");
        //            _output.WriteLine(db.LastCommand);
        //        }
        //    });
        //    _output.WriteLine(ex.ToString());
        //}


        [Fact]
        public void Select系_SingleOrDefault_条件1つを指定して1件取得()
        {
            using (var db = new DB())
            {
                var rec = db.SingleOrDefault<PtTable01>("WHERE Key01 = @0", "03");
                _output.WriteLine(db.LastCommand);

                Assert.NotNull(rec);
                Assert.True(rec.ColBool);
                Assert.Equal(999, rec.ColInt.Value);
                Assert.Equal(123456.78m, rec.ColDec.Value);
                Assert.Equal("岩手県", rec.ColVarchar);
                Assert.Equal("system", rec.CreateBy);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [Fact]
        public void Select系_SingleOrDefault_条件2つを指定して1件取得()
        {
            using (var db = new DB())
            {
                var rec = db.SingleOrDefault<PtTable02>("WHERE Key01=@0 AND Key02=@1", "03", 3);
                _output.WriteLine(db.LastCommand);

                Assert.NotNull(rec);
                Assert.True(rec.ColBool);
                Assert.Equal(999, rec.ColInt.Value);
                Assert.Equal(123456.78m, rec.ColDec.Value);
                Assert.Equal("岩手県", rec.ColVarchar);
                Assert.Equal("system", rec.CreateBy);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [Fact]
        public void Select系_SingleOrDefault_条件を指定して1件ヒットなし()
        {
            using (var db = new DB())
            {
                var rec = db.SingleOrDefault<PtTable01>("WHERE Key01 = @0", "999");
                _output.WriteLine(db.LastCommand);

                Assert.Null(rec);
            }
        }

        [Fact]
        public void Select系_SingleOrDefault_複数ヒットエラー()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                // InvalidOperationExceptionが発生したらOK
                using (var db = new DB())
                {
                    var rec = db.SingleOrDefault<PtTable01>("WHERE CreateBy = @0", "system");
                    _output.WriteLine(db.LastCommand);

                }
            });
            _output.WriteLine(ex.ToString());
        }


        [Fact]
        public void Select系_First_該当する先頭行を取得()
        {
            using (var db = new DB())
            {
                var rec = db.First<PtTable01>("WHERE Key01 < @0 ORDER BY Key01 DESC", "06");
                _output.WriteLine(db.LastCommand);

                Assert.NotNull(rec);
                Assert.True(rec.ColBool);
                Assert.Equal(999, rec.ColInt.Value);
                Assert.Equal(123456.78m, rec.ColDec.Value);
                Assert.Equal("秋田県", rec.ColVarchar);
                Assert.Equal("system", rec.CreateBy);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [Fact]
        public void Select系_First_ヒットなしエラー()
        {
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                // InvalidOperationExceptionが発生したらOK
                using (var db = new DB())
                {
                    db.First<PtTable01>("WHERE Key01 > @0 ORDER BY Key01 DESC", "9999");
                    _output.WriteLine(db.LastCommand);

                }
            });
            _output.WriteLine(ex.ToString());
        }

        [Fact]
        public void Select系_FirstOrDefault_該当する先頭行を取得()
        {
            using (var db = new DB())
            {
                var rec = db.FirstOrDefault<PtTable01>("WHERE Key01 < @0 ORDER BY Key01 DESC", "06");
                _output.WriteLine(db.LastCommand);

                Assert.NotNull(rec);
                Assert.True(rec.ColBool);
                Assert.Equal(999, rec.ColInt.Value);
                Assert.Equal(123456.78m, rec.ColDec.Value);
                Assert.Equal("秋田県", rec.ColVarchar);
                Assert.Equal("system", rec.CreateBy);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
            }
        }

        [Fact]
        public void Select系_FirstOrDefault_ヒットなし()
        {
            using (var db = new DB())
            {
                var rec = db.FirstOrDefault<PtTable01>("WHERE Key01 > @0 ORDER BY Key01 DESC", "9999");
                _output.WriteLine(db.LastCommand);

                Assert.Null(rec);
            }
        }

        [Fact]
        public void Select系_ExecuteScalar_string型()
        {
            using (var db = new DB())
            {
                var res = db.ExecuteScalar<string>("SELECT ColVarchar FROM PtTable01 WHERE Key01 = @0", "05");
                _output.WriteLine(db.LastCommand);

                Assert.Equal("秋田県", res);
            }
        }

        [Fact]
        public void Select系_ExecuteScalar_int型()
        {
            using (var db = new DB())
            {
                var res = db.ExecuteScalar<int>("SELECT COUNT(*) FROM PtTable01");
                Assert.Equal(15, res);
                _output.WriteLine(db.LastCommand);

            }
        }


        [Fact]
        public void Select系_ExecuteScalar_DateTime型()
        {
            using (var db = new DB())
            {
                var res = db.ExecuteScalar<DateTime>("SELECT UpdateDt FROM PtTable01 WHERE Key01 = @0", "05");
                _output.WriteLine(db.LastCommand);

                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), res);
            }
        }


        [Fact]
        public void Select系_Single_内部結合クエリ()
        {
            using (var db = new DB())
            {
                var rec = db.SingleOrDefault<JoinTest>(PetaPoco.Sql.Builder
                    .Append("SELECT  t01.Key01")
                    .Append("       ,t01.ColVarchar")
                    .Append("       ,t01.CreateBy")
                    .Append("       ,t01.CreateDt")
                    .Append("       ,t02.ColVarchar AS ColVarchar02")
                    .Append("       ,t02.CreateDt AS CreateDt02")
                    .Append("  FROM PtTable01 AS t01")
                    .Append("       INNER JOIN PtTable02 AS t02")
                    .Append("               ON t01.Key01 = t02.Key01")
                    .Append("              AND t02.Key02 = @0", 2)
                    .Append(" WHERE t01.Key01 = @0", "02")
                    );
                _output.WriteLine(db.LastCommand);

                Assert.NotNull(rec);
                Assert.False(rec.ColBool);
                Assert.Null(rec.ColInt);
                Assert.Null(rec.ColDec);
                Assert.Equal("青森県", rec.ColVarchar);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
                Assert.Equal("青森県", rec.ColVarchar02);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt02);
            }
        }

        [Fact]
        public void Select系_Single_外部結合クエリ()
        {
            using (var db = new DB())
            {
                var rec = db.SingleOrDefault<JoinTest>(PetaPoco.Sql.Builder
                    .Append("SELECT  t01.Key01")
                    .Append("       ,t01.ColVarchar")
                    .Append("       ,t01.CreateBy")
                    .Append("       ,t01.CreateDt")
                    .Append("       ,t02.ColVarchar AS ColVarchar02")
                    .Append("       ,t02.CreateDt AS CreateDt02")
                    .Append("  FROM PtTable01 AS t01")
                    .Append("       LEFT JOIN PtTable02 AS t02")
                    .Append("              ON t01.Key01 = t02.Key01")
                    .Append("             AND t02.Key02 = @0", 0)       // table02はヒットなし
                    .Append(" WHERE t01.Key01 = @0", "13")
                    );

                _output.WriteLine(db.LastCommand);

                Assert.NotNull(rec);
                Assert.Equal("東京都", rec.ColVarchar);
                Assert.Equal(DateTime.Parse("2018/10/24 12:00:00"), rec.CreateDt);
                Assert.Null(rec.ColVarchar02);
                Assert.Equal(DateTime.Parse("0001/01/01 00:00:00"), rec.CreateDt02);
            }
        }

        [Fact]
        public void Select系_Fetch_単一テーブルヒットあり()
        {
            using (var db = new DB())
            {
                var recs = db.Fetch<PtTable01>("ORDER BY Key01");
                _output.WriteLine(db.LastCommand);


                Assert.Equal(15, recs.Count);
                Assert.Equal("北海道", recs.ElementAt(0).ColVarchar);
                Assert.Equal("青森県", recs.ElementAt(1).ColVarchar);
                Assert.Equal("東京都", recs.ElementAt(12).ColVarchar);
            }
        }

        [Fact]
        public void Select系_Fetch_単一テーブルヒットなし()
        {
            using (var db = new DB())
            {
                var recs = db.Fetch<PtTable01>("WHERE Key01 = @0 ORDER BY Key01", "999");
                _output.WriteLine(db.LastCommand);


                Assert.Empty(recs);
            }
        }

        [Fact]
        public void Select系_Fetch_外部結合クエリ()
        {
            using (var db = new DB())
            {
                var recs = db.Fetch<JoinTest>(PetaPoco.Sql.Builder
                    .Append("SELECT  t02.Key01")
                    .Append("       ,t02.ColVarchar")
                    .Append("       ,t02.CreateBy")
                    .Append("       ,t01.ColVarchar AS ColVarchar02")
                    .Append("       ,t01.CreateDt AS CreateDt02")
                    .Append("  FROM PtTable02 AS t02")
                    .Append("       LEFT JOIN PtTable01 AS t01")
                    .Append("              ON t02.Key01 = t01.Key01")
                    .Append(" ORDER BY t02.Key01, t02.Key02")
                    );

                _output.WriteLine(db.LastCommand);

                Assert.Equal(15, recs.Count);
                Assert.Equal("宮城県", recs.ElementAt(3).ColVarchar);
                Assert.Equal("秋田県", recs.ElementAt(4).ColVarchar);
                Assert.Equal("山形県", recs.ElementAt(5).ColVarchar);
            }
        }

        [Fact]
        public void Select系_Dictionary_その1()
        {
            using (var db = new DB())
            {
                var recs = db.Dictionary<string, string>("SELECT Key01, ColVarchar FROM PtTable01 ORDER BY Key01");

                _output.WriteLine(db.LastCommand);

                Assert.Equal(15, recs.Count);
                Assert.Equal("北海道", recs["01"]);
                Assert.Equal("埼玉県", recs["11"]);
            }
        }

    }
}
