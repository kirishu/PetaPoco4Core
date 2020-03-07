using System;

namespace PetaPocoAppFx
{
    public class TestPostgreSql : ITest
    {
        public void Execute()
        {
            Database.PostgreSql.Config.ConnectionString = Program.ConnectionString;
            using (var db = new Database.PostgreSql.DB())
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

                var rec4 = new Database.PostgreSql.TrAutoNumber
                {
                    Key01 = "AA",
                    ColInt = 999,
                };
                db.Insert(rec4);

                var rec5 = new Database.PostgreSql.TrCompositeKey
                {
                    Key01 = "13",
                    Key02 = 13,
                    ColVarchar = "ほげほげ",
                    UpdateDt = DateTime.Now,
                };
                db.Update(rec5);

                var rec6 = new Database.PostgreSql.TrColumn
                {
                    Key01 = "",
                    ColBool = true,
                    ColBool2 = false,
                    ColTime = new TimeSpan(12, 24, 56),
                    ColDate = DateTime.Now,
                    ColTimestamp = DateTime.Now,
                    ColTimestamp3 = DateTime.Now,
                    ColBytea = new byte[64],
                };
                db.Insert(rec6);

                db.AbortTransaction();
            }
        }
    }
}
