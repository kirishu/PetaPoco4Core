using System;
using PetaPoco;

namespace PetaPoco4Core.Test.Models
{
    [TableName("yotta_entities")]
    [PrimaryKey("Id", AutoIncrement = true, SequenceName = "SEQ_1")]
    public class YottaEntity
    {
        [Column]
        public int Id { get; set; }

        [Column]
        public string AnotherColumn { get; set; }

        [ResultColumn]
        public string ResultColumn { get; set; }

        [Ignore]
        public string NotAColumn { get; set; }

        
        public DateTime Creater { get; set; }
    }
}
