using System;

namespace PetaPocoAppFx
{
    public class TestSQLServer : ITest
    {
        public void Execute()
        {
            Database.SQLServer.Config.ConnectionString = Program.ConnectionString;
            using (var db = new Database.SQLServer.DB())
            {
                var pk1 = new
                {
                    Key01 = "13",
                    Key02 = 13,
                };
                var rec1 = db.SingleOrDefaultById<Database.SQLServer.TrCompositeKey>(pk1);
                var rec2 = db.SingleOrDefaultById<Database.SQLServer.TrAutoNumber>(123);
                var rec3 = db.SingleOrDefaultById<Database.SQLServer.TrColumn>("AA");

                db.BeginTransaction();

                var rec4 = new Database.SQLServer.TrAutoNumber
                {
                    Key01 = "AA",
                    ColInt = 999,
                };
                db.Insert(rec4);

                var rec5 = new Database.SQLServer.TrCompositeKey
                {
                    Key01 = "13",
                    Key02 = 13,
                    ColVarchar = "ほげほげ",
                    UpdateDt = DateTime.Now,
                };
                db.Update(rec5);

                var rec6 = new Database.SQLServer.TrColumn
                {
                    Key01 = "",
                    ColBit = true,
                    ColBigInt = long.MaxValue,
                    ColTime = new TimeSpan(12, 24, 56),
                    ColDate = DateTime.Now,
                    ColDateTime = DateTime.Now,
                    ColDateTime2 = DateTime.Now,
                    ColSmallDateTime = DateTime.Now,
                    ColText = string.Empty,
                    ColImage = new byte[64],
                    ColVarBinary = new byte[64],
                };
                db.Insert(rec6);

                db.AbortTransaction();
            }
        }
    }
}
