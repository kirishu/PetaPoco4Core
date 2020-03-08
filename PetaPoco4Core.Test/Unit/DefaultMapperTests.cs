// <copyright company="PetaPoco - CollaboratingPlatypus">
//      Apache License, Version 2.0 https://github.com/CollaboratingPlatypus/PetaPoco/blob/master/LICENSE.txt
// </copyright>
// <author>PetaPoco - CollaboratingPlatypus</author>
// <date>2018/07/02</date>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace PetaPoco4Core.Test.Unit
{
    public class DefaultMapperTests : TestBase
    {
        public DefaultMapperTests(ITestOutputHelper output) : base(output) { }

        /// <summary>
        /// Attributeあり
        /// </summary>
        [Fact]
        public void DMP001_GetTableInfo_GivenEntityWithTableAttribute()
        {
            var teraEntity = new PetaPoco.Database.PocoData(typeof(Models.TeraEntity)).TableInfo;
            Assert.Equal("TblTeraEntity", teraEntity.TableName);
            Assert.Equal("Id1,Id2,Id3", teraEntity.PrimaryKey);
            Assert.False(teraEntity.AutoIncrement);
            Assert.Null(teraEntity.SequenceName);

            var yottaEntity = new PetaPoco.Database.PocoData(typeof(Models.YottaEntity)).TableInfo;
            Assert.Equal("yotta_entities", yottaEntity.TableName);
            Assert.Equal("Id", yottaEntity.PrimaryKey);
            Assert.True(yottaEntity.AutoIncrement);
            Assert.Equal("SEQ_1", yottaEntity.SequenceName);
        }

        /// <summary>
        /// AttributeなしただのPoco
        /// </summary>
        [Fact]
        public void DMP002_GetTableInfo_GivenEntityWithOutTableAttribute()
        {
            var ti = new PetaPoco.Database.PocoData(typeof(Models.ExaEntity)).TableInfo;
            Assert.Equal("ExaEntity", ti.TableName);
            Assert.Equal("ID", ti.PrimaryKey);      // 勝手に"ID"という列がPK・・・としちゃう（ver5と違う）
            Assert.False(ti.AutoIncrement);          // AutoIncrementはfalse
            Assert.Null(ti.SequenceName);
        }

        /// <summary>
        /// [ExplicitColumns]がついて*いる*Entityの列情報
        /// </summary>
        [Fact]
        public void DMP003_GetColumnInfo_GivenPropertyWithColumnAttributeAndEntityWithExplicitAttribute()
        {
            var poco = new PetaPoco.Database.PocoData(typeof(Models.TeraEntity));

            Assert.Equal(5, poco.Columns.Count);

            var colA = poco.Columns.Values.Single(x => x.PropertyInfo.Name == nameof(Models.TeraEntity.TheId2));
            Assert.Equal("Id2", colA.ColumnName);
            Assert.False(colA.ResultColumn);
            Assert.False(colA.VersionColumn);

            var colB = poco.Columns.Values.Single(x => x.PropertyInfo.Name == nameof(Models.TeraEntity.ResultColumn));
            Assert.Equal("result_column", colB.ColumnName);
            Assert.True(colB.ResultColumn);
            Assert.False(colB.VersionColumn);

            var colC = poco.Columns.Values.SingleOrDefault(x => x.PropertyInfo.Name == nameof(Models.TeraEntity.NotAColumn));
            Assert.Null(colC);
        }

        /// <summary>
        /// [ExplicitColumns]がついて*いない*Entityの列情報
        /// </summary>
        [Fact]
        public void DMP004_GetColumnInfo_GivenPropertyWithColumnAttributeAndEntityWithOutExplicitAttribute()
        {
            var poco = new PetaPoco.Database.PocoData(typeof(Models.YottaEntity));

            // ついていなくてもとれる

            Assert.Equal(4, poco.Columns.Count);

            var colA = poco.Columns.Values.Single(x => x.PropertyInfo.Name == nameof(Models.YottaEntity.AnotherColumn));
            Assert.Equal("AnotherColumn", colA.ColumnName);
            Assert.False(colA.ResultColumn);
            Assert.False(colA.VersionColumn);

            var colB = poco.Columns.Values.Single(x => x.PropertyInfo.Name == nameof(Models.YottaEntity.ResultColumn));
            Assert.Equal("ResultColumn", colB.ColumnName);
            Assert.True(colB.ResultColumn);
            Assert.False(colB.VersionColumn);

            var colC = poco.Columns.Values.SingleOrDefault(x => x.PropertyInfo.Name == nameof(Models.YottaEntity.NotAColumn));
            Assert.Null(colC);

            var colD = poco.Columns.Values.Single(x => x.PropertyInfo.Name == nameof(Models.YottaEntity.Creater));
            Assert.Equal("Creater", colD.ColumnName);
            Assert.False(colD.ResultColumn);
            Assert.False(colD.VersionColumn);
        }

        /// <summary>
        /// PK
        /// </summary>
        [Fact]
        public void DMP005_GetPrimaryKeyValues_Mono()
        {
            var db = new PetaPoco.Database("test", PetaPoco.Database.RDBType.MySql);

            // Invoke refrectionを使ってprivateメソッドを実行する
            MethodInfo methodInfo = typeof(PetaPoco.Database).GetMethod("GetPrimaryKeyValues", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters =
                {
                    PetaPoco.Database.PocoData.ForType(typeof(Models.YottaEntity)).TableInfo.PrimaryKey,
                    123,
                };
            var result = (Dictionary<string, object>)methodInfo.Invoke(db, parameters);

            Assert.Single(result);

            Assert.Equal(123, result["Id"]);
        }

        /// <summary>
        /// 複合PK
        /// </summary>
        [Fact]
        public void DMP006_GetPrimaryKeyValues_Composite()
        {
            var pk = new
            {
                Id1 = 100,
                Id2 = "Id2_value",
                Id3 = 300,
            };

            var db = new PetaPoco.Database("test", PetaPoco.Database.RDBType.MySql);

            // Invoke refrectionを使ってprivateメソッドを実行する
            MethodInfo methodInfo = typeof(PetaPoco.Database).GetMethod("GetPrimaryKeyValues", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = 
                {
                    PetaPoco.Database.PocoData.ForType(typeof(Models.TeraEntity)).TableInfo.PrimaryKey,
                    pk,
                };
            var result = (Dictionary<string, object>)methodInfo.Invoke(db, parameters);

            Assert.Equal(3, result.Count);

            Assert.Equal(100, result["Id1"]);
            Assert.Equal("Id2_value", result["Id2"]);
            Assert.Equal(300, result["Id3"]);
        }
    }
}