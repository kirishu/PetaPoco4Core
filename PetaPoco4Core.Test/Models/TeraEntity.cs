using System;
using PetaPoco;

namespace PetaPoco4Core.Test.Models
{
    [TableName("TblTeraEntity")]
    [PrimaryKey("Id1,Id2,Id3", AutoIncrement = false)]
    [ExplicitColumns]
    public class TeraEntity
    {
        [Column("Id1")]
        public int TheId1 { get; set; }

        [Column("Id2")]
        public string TheId2 { get; set; }

        [Column("Id3")]
        public int TheId3 { get; set; }

        [Column("another_column")]
        public string AnotherColumn { get; set; }

        [ResultColumn("result_column")]
        public string ResultColumn { get; set; }

        public string NotAColumn { get; set; }

        [Ignore]
        public DateTime Created { get; set; }
    }
}
