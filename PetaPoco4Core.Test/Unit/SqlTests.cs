// <copyright company="PetaPoco - CollaboratingPlatypus">
//      Apache License, Version 2.0 https://github.com/CollaboratingPlatypus/PetaPoco/blob/master/LICENSE.txt
// </copyright>
// <author>PetaPoco - CollaboratingPlatypus</author>
// <date>2018/07/02</date>

using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace PetaPoco4Core.Test.Unit
{
    [RequiresCleanUp]
    public class SqlTests: TestBase
    {
        private PetaPoco.Sql _sql;

        public SqlTests(ITestOutputHelper output) : base(output)
        {
            _sql = new PetaPoco.Sql();
        }

        [Fact]
        public void Append_GivenSimpleStrings()
        {
            _sql.Append("LINE 1");
            _sql.Append("LINE 2");
            _sql.Append("LINE 3");

            Assert.Equal("LINE 1\nLINE 2\nLINE 3", _sql.SQL);
            Assert.Empty(_sql.Arguments);
        }

        [Fact]
        public void Append_GivenSignleArgument()
        {
            _sql.Append("arg @0", "a1");

            Assert.Equal("arg @0", _sql.SQL);
            Assert.Single(_sql.Arguments);
            Assert.Equal("a1", _sql.Arguments[0]);
        }

        [Fact]
        public void Append_GivenMultipleArguments()
        {
            _sql.Append("arg @0 @1", "a1", "a2");

            Assert.Equal("arg @0 @1", _sql.SQL);
            Assert.Equal(2, _sql.Arguments.Length);
            Assert.Equal("a1", _sql.Arguments[0]);
            Assert.Equal("a2", _sql.Arguments[1]);
        }

        [Fact]
        [Description("Question: should this not throw?")]
        public void Append_GivenUnusedArguments()
        {
            _sql.Append("arg @0 @2", "a1", "a2", "a3");

            Assert.Equal("arg @0 @1", _sql.SQL);
            Assert.Equal(2, _sql.Arguments.Length);
            Assert.Equal("a1", _sql.Arguments[0]);
            Assert.Equal("a3", _sql.Arguments[1]);
        }

        [Fact]
        public void Append_GivenUnorderedArguments()
        {
            _sql.Append("arg @2 @1", "a1", "a2", "a3");

            Assert.Equal("arg @0 @1", _sql.SQL);
            Assert.Equal(2, _sql.Arguments.Length);
            Assert.Equal("a3", _sql.Arguments[0]);
            Assert.Equal("a2", _sql.Arguments[1]);
        }

        [Fact]
        public void Append_GivenRepeatedArguments()
        {
            _sql.Append("arg @0 @1 @0 @1", "a1", "a2");

            //Assert.Equal("arg @0 @1 @2 @3", _sql.SQL);
            //Assert.Equal(4, _sql.Arguments.Length);
            Assert.Equal("arg @0 @1 @0 @1", _sql.SQL);
            Assert.Equal(2, _sql.Arguments.Length);
            Assert.Equal("a1", _sql.Arguments[0]);
            Assert.Equal("a2", _sql.Arguments[1]);
            //Assert.Equal("a1", _sql.Arguments[2]);
            //Assert.Equal("a2", _sql.Arguments[3]);
        }

        [Fact]
        public void Append_GivenMySqlUserVariables()
        {
            _sql.Append("arg @@user1 @2 @1 @@@system1", "a1", "a2", "a3");

            Assert.Equal("arg @@user1 @0 @1 @@@system1", _sql.SQL);
            Assert.Equal(2,_sql.Arguments.Length);
            Assert.Equal("a3", _sql.Arguments[0]);
            Assert.Equal("a2", _sql.Arguments[1]);
        }

        [Fact]
        public void Append_GivenNameArguments()
        {
            _sql.Append("arg @name @password", new { name = "n", password = "p" });

            Assert.Equal("arg @0 @1", _sql.SQL);
            Assert.Equal(2, _sql.Arguments.Length);
            Assert.Equal("n", _sql.Arguments[0]);
            Assert.Equal("p", _sql.Arguments[1]);
        }

        [Fact]
        public void Append_GivenMixedNameAndNumberArguments()
        {
            _sql.Append("arg @0 @name @1 @password @2", "a1", "a2", "a3", new { name = "n", password = "p" });

            Assert.Equal("arg @0 @1 @2 @3 @4", _sql.SQL);
            Assert.Equal(5, _sql.Arguments.Length);
            Assert.Equal("a1", _sql.Arguments[0]);
            Assert.Equal("n", _sql.Arguments[1]);
            Assert.Equal("a2", _sql.Arguments[2]);
            Assert.Equal("p", _sql.Arguments[3]);
            Assert.Equal("a3", _sql.Arguments[4]);
        }

        [Fact]
        public void Append_GivenConsecutiveArguments()
        {
            _sql.Append("l1 @0", "a0");
            _sql.Append("l2 @0", "a1");
            _sql.Append("l3 @0", "a2");

            Assert.Equal("l1 @0\nl2 @1\nl3 @2", _sql.SQL);
            Assert.Equal(3, _sql.Arguments.Length);
            Assert.Equal("a0", _sql.Arguments[0]);
            Assert.Equal("a1", _sql.Arguments[1]);
            Assert.Equal("a2", _sql.Arguments[2]);
        }

        [Fact]
        public void Append_GivenConsecutiveComplexArguments()
        {
            _sql.Append("l1");
            _sql.Append("l2 @0 @1", "a1", "a2");
            _sql.Append("l3 @0", "a3");

            Assert.Equal("l1\nl2 @0 @1\nl3 @2", _sql.SQL);
            Assert.Equal(3, _sql.Arguments.Length);
            Assert.Equal("a1", _sql.Arguments[0]);
            Assert.Equal("a2", _sql.Arguments[1]);
            Assert.Equal("a3", _sql.Arguments[2]);
        }

        [Fact]
        public void Append_GivenInvalidNumberOfArguments_ShouldThrow()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                _sql.Append("arg @0 @1", "a0");
                Assert.Equal("arg @0 @1", _sql.SQL);
            });
        }

        [Fact]
        public void Append_GivenInvalidArgumentNames_ShouldThrow()
        {
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                _sql.Append("arg @name1 @name2", new { x = 1, y = 2 });
                Assert.Equal("arg @0 @1", _sql.SQL);
            });
        }

        [Fact]
        public void Append_GivenSqLInstance()
        {
            _sql = new PetaPoco.Sql("l0 @0", "a0");
            var sql1 = new PetaPoco.Sql("l1 @0", "a1");
            var sql2 = new PetaPoco.Sql("l2 @0", "a2");

            Assert.Equal(_sql, _sql.Append(sql1));
            Assert.Equal(_sql, _sql.Append(sql2));

            Assert.Equal("l0 @0\nl1 @1\nl2 @2", _sql.SQL);
            Assert.Equal(3, _sql.Arguments.Length);
            Assert.Equal("a0", _sql.Arguments[0]);
            Assert.Equal("a1", _sql.Arguments[1]);
            Assert.Equal("a2", _sql.Arguments[2]);
        }

        [Fact]
        public void Append_GivenConsecutiveSets()
        {
            _sql = new PetaPoco.Sql()
                .Append("UPDATE blah");

            _sql.Append("SET a = 1");
            _sql.Append("SET b = 2");

            Assert.Equal("UPDATE blah\nSET a = 1\n, b = 2", _sql.SQL);
        }

        [Fact]
        public void Append_GivenConsecutiveSetsAndWheres()
        {
            _sql = new PetaPoco.Sql()
                .Append("UPDATE blah");

            _sql.Append("SET a = 1");
            _sql.Append("SET b = 2");
            _sql.Append("WHERE x");
            _sql.Append("WHERE y");

            Assert.Equal("UPDATE blah\nSET a = 1\n, b = 2\nWHERE x\nAND y", _sql.SQL);
        }

        [Fact]
        public void Append_GivenConsecutiveWheres()
        {
            _sql = new PetaPoco.Sql()
                .Append("SELECT * FROM blah");

            _sql.Append("WHERE x");
            _sql.Append("WHERE y");

            Assert.Equal("SELECT * FROM blah\nWHERE x\nAND y", _sql.SQL);
        }

        [Fact]
        public void Append_GivenConsecutiveOrderBys()
        {
            _sql = new PetaPoco.Sql()
                .Append("SELECT * FROM blah");

            _sql.Append("ORDER BY x");
            _sql.Append("ORDER BY y");

            Assert.Equal("SELECT * FROM blah\nORDER BY x\n, y", _sql.SQL);
        }

        [Fact]
        public void Append_GivenArrayAndValue()
        {
            // Simple collection parameter expansion
            _sql = PetaPoco.Sql.Builder.Append("@0 IN (@1) @2", 20, new int[] { 1, 2, 3 }, 30);

            Assert.Equal("@0 IN (@1,@2,@3) @4", _sql.SQL);
            Assert.Equal(5, _sql.Arguments.Length);
            Assert.Equal(20, _sql.Arguments[0]);
            Assert.Equal(1, _sql.Arguments[1]);
            Assert.Equal(2, _sql.Arguments[2]);
            Assert.Equal(3, _sql.Arguments[3]);
            Assert.Equal(30, _sql.Arguments[4]);
        }

        [Fact]
        public void Append_GivenArray()
        {
            _sql = PetaPoco.Sql.Builder.Append("IN (@numbers)", new { numbers = (new[] { 10, 20, 30 }) });

            Assert.Equal("IN (@0,@1,@2)", _sql.SQL);
            Assert.Equal(3, _sql.Arguments.Length);
            Assert.Equal(10, _sql.Arguments[0]);
            Assert.Equal(20, _sql.Arguments[1]);
            Assert.Equal(30, _sql.Arguments[2]);
        }


        [Fact]
        public void Append_GivenList()
        {
            var chars = new List<string> { "い", "ろ", "は", };

            _sql = PetaPoco.Sql.Builder.Append("IN (@0)", chars);

            Assert.Equal("IN (@0,@1,@2)", _sql.SQL);
            Assert.Equal(3, _sql.Arguments.Length);
            Assert.Equal("い", _sql.Arguments[0]);
            Assert.Equal("ろ", _sql.Arguments[1]);
            Assert.Equal("は", _sql.Arguments[2]);
        }


        [Fact]
        public void Append_GivenArrayString()
        {
            var keys = new string[] { "10", "20", "30" };
            //_sql = PetaPoco.Sql.Builder.Append("IN (@0)", keys);   ←これはダメ。エラーになっちゃう
            _sql = PetaPoco.Sql.Builder.Append("IN (@1)", null, keys);

            Assert.Equal("IN (@0,@1,@2)", _sql.SQL);
            Assert.Equal(3, _sql.Arguments.Length);
            Assert.Equal("10", _sql.Arguments[0]);
            Assert.Equal("20", _sql.Arguments[1]);
            Assert.Equal("30", _sql.Arguments[2]);
        }

        [Fact]
        public void Append_GivenListString()
        {
            var keys = new List<string> { "い", "ろ", "は", };
            _sql = PetaPoco.Sql.Builder.Append("IN (@0)", keys);

            Assert.Equal("IN (@0,@1,@2)", _sql.SQL);
            Assert.Equal(3, _sql.Arguments.Length);
            Assert.Equal("い", _sql.Arguments[0]);
            Assert.Equal("ろ", _sql.Arguments[1]);
            Assert.Equal("は", _sql.Arguments[2]);
        }

        [Fact]
        public void Append_GivenTwoArrays()
        {
            // Out of order expansion
            _sql = PetaPoco.Sql.Builder.Append("IN (@3) (@1)", null, new[] { 10, 20, 30 }, null, new[] { 40, 50, 60 });

            Assert.Equal("IN (@0,@1,@2) (@3,@4,@5)", _sql.SQL);
            Assert.Equal(6, _sql.Arguments.Length);
            Assert.Equal(40, _sql.Arguments[0]);
            Assert.Equal(50, _sql.Arguments[1]);
            Assert.Equal(60, _sql.Arguments[2]);
            Assert.Equal(10, _sql.Arguments[3]);
            Assert.Equal(20, _sql.Arguments[4]);
            Assert.Equal(30, _sql.Arguments[5]);
        }

        [Fact]
        public void Join_GivenTable()
        {
            _sql = PetaPoco.Sql.Builder
                .Select("*")
                .From("articles")
                .LeftJoin("comments").On("articles.article_id=comments.article_id");

            Assert.Equal("SELECT *\nFROM articles\nLEFT JOIN comments\nON articles.article_id=comments.article_id", _sql.SQL);
        }

        [Fact]
        [Description("Investigation of reported bug #123")]
        public void Append_GivenMultipleAppends()
        {
            var resource = new
            {
                ResourceName = "p1",
                ResourceDescription = "p2",
                ResourceContent = "p3",
                ResourceData = "p4",
                ResourceGUID = Guid.Parse("C32B630F-FCFE-49FF-A27C-2E4105D4003E"),
                LaunchPath = "p5",
                ResourceType = Models.OrderStatus.Deleted,
                ContentType = "p5",
                SchoolID = "p5",
                DistrictID = "p5",
                UpdatedBy = 87,
                UpdatedDate = new DateTime(2000, 1, 1, 1, 1, 1, DateTimeKind.Utc),
                IsActive = true,
                Extension = "p9",
                ResourceID = 99,
            };

            _sql.Append("UPDATE [Resource] SET ")
                .Append("[ResourceName] = @0", resource.ResourceName)
                .Append(",[ResourceDescription] = @0", resource.ResourceDescription)
                .Append(",[ResourceContent] = @0", resource.ResourceContent)
                .Append(",[ResourceData] = @0", resource.ResourceData)
                .Append(",[ResourceGUID] = @0", resource.ResourceGUID)
                .Append(",[LaunchPath] = @0", resource.LaunchPath)
                .Append(",[ResourceType] = @0", (int)resource.ResourceType)
                .Append(",[ContentType] = @0", resource.ContentType)
                .Append(",[SchoolID] = @0", resource.SchoolID)
                .Append(",[DistrictID] = @0", resource.DistrictID)
                .Append(",[IsActive] = @0", resource.IsActive)
                .Append(",[UpdatedBy] = @0", resource.UpdatedBy)
                .Append(",[UpdatedDate] = @0", resource.UpdatedDate)
                .Append(",[Extension] = @0", resource.Extension).Append(" WHERE ResourceID=@0", resource.ResourceID);

            var result = @"UPDATE [Resource]"
                       + " SET [ResourceName] = @0"
                       + ",[ResourceDescription] = @1"
                       + ",[ResourceContent] = @2"
                       + ",[ResourceData] = @3"
                       + ",[ResourceGUID] = @4"
                       + ",[LaunchPath] = @5"
                       + ",[ResourceType] = @6"
                       + ",[ContentType] = @5"  // same "p5"
                       + ",[SchoolID] = @5"     // same "p5"
                       + ",[DistrictID] = @5"   // same "p5"
                       + ",[IsActive] = @7"
                       + ",[UpdatedBy] = @8"
                       + ",[UpdatedDate] = @9"
                       + ",[Extension] = @10"
                       + " WHERE ResourceID=@11";

            Assert.Equal(result, _sql.SQL.Replace("\n", "").Replace("\r", ""));

            //Assert.Equal(@"UPDATE [Resource] SET [ResourceName] = @0,[ResourceDescription] = @1,[ResourceContent] = @2,[ResourceData] = @3,[ResourceGUID] = @4,[LaunchPath] = @5,[ResourceType] = @6,[ContentType] = @7,[SchoolID] = @8,[DistrictID] = @9,[IsActive] = @10,[UpdatedBy] = @11,[UpdatedDate] = @12,[Extension] = @13 WHERE ResourceID=@14",
            //    _sql.SQL.Replace("\n", "").Replace("\r", ""));
        }

        [Fact]
        [Description("Investigation of reported bug #106")]
        public void Sql_CacheShouldBeResetAfterAdditionalChanges()
        {
            _sql.Select("field");
            var sqlCapture1 = _sql.SQL;
            _sql.From("myTable");
            var sqlCapture2 = _sql.SQL;
            _sql.Where("id = @0", 1);
            var sqlCapture3 = _sql.SQL;

            Assert.Equal("SELECT field", sqlCapture1.Replace("\n", " "));
            Assert.Equal("SELECT field FROM myTable", sqlCapture2.Replace("\n", " "));
            Assert.Equal("SELECT field FROM myTable WHERE (id = @0)", sqlCapture3.Replace("\n", " "));
        }

        [Fact]
        public void 別インスタンスSQLの結合()
        {
            var sqlSelect = PetaPoco.Sql.Builder.Append("SELECT * ");
            var sqlFrom = PetaPoco.Sql.Builder.Append("FROM employees");
            var sqlWhere = PetaPoco.Sql.Builder
                .Where("emp_no = @0", 10023)
                .Where("birth_date >= @0 AND birth_date <= @1", DateTime.Parse("1953/01/01"), DateTime.Parse("1953/12/31"))
                .Where("first_name = @0", "Bojan");


            _sql = PetaPoco.Sql.Builder
                .Append(sqlSelect.SQL)
                .Append(sqlFrom.SQL)
                .Append(new PetaPoco.Sql(sqlWhere.SQL, sqlWhere.Arguments));

            _output.WriteLine(_sql.SQL);

            Assert.Equal(4, _sql.Arguments.Length);
        }

        [Fact]
        public void Clone()
        {
            var where = PetaPoco.Sql.Builder
                .Where("emp_no = @0", 10023)
                .Where("birth_date >= @0 AND birth_date <= @1", DateTime.Parse("1953/01/01"), DateTime.Parse("1953/12/31"))
                .Where("first_name = @0", "Bojan");


            _sql = where.Clone();

            _sql.Where("last_name = @0", "Montemayor");

            _output.WriteLine(_sql.SQL);

            Assert.Equal(4, where.Arguments.Length);
            Assert.Equal(5, _sql.Arguments.Length);
        }

    }
}