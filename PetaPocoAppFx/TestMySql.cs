using System;

namespace PetaPocoAppFx
{
    public class TestMySql : ITest
    {
        public void Execute()
        {
            Database.MySql.Config.ConnectionString = Program.ConnectionString;
            using (var db = new Database.MySql.DB())
            {
                var pk1 = new
                {
                    Key01 = "13",
                    Key02 = 13,
                };
                var rec1 = db.SingleOrDefaultById<Database.MySql.TrCompositeKey>(pk1);
                var rec2 = db.SingleOrDefaultById<Database.MySql.TrAutoNumber>(123);
                var rec3 = db.SingleOrDefaultById<Database.MySql.TrColumn>("AA");

                db.BeginTransaction();

                var rec4 = new Database.MySql.TrAutoNumber
                {
                    Key01 = "AA",
                    ColInt = 999,
                };
                db.Insert(rec4);

                var rec5 = new Database.MySql.TrCompositeKey
                {
                    Key01 = "13",
                    Key02 = 13,
                    ColVarchar = "ほげほげ",
                    UpdateDt = DateTime.Now,
                };
                db.Update(rec5);

                var rec6 = new Database.MySql.TrColumn
                {
                    Key01 = "",
                    ColBool = true,
                    ColBigInt = long.MaxValue,
                    ColBit = true,
                    ColTime = new TimeSpan(12, 24, 56),
                    ColDate = DateTime.Now,
                    ColTimeStamp = DateTime.Now,
                    ColLongBlob = new byte[64],
                };
                db.Insert(rec6);

                db.AbortTransaction();
            }
        }
    }
}
