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
                var rec1 = db.SingleOrDefaultById<Database.PostgreSql.TrCompositeKey>(pk1);
                var rec2 = db.SingleOrDefaultById<Database.PostgreSql.TrAutoNumber>(123);
                var rec3 = db.SingleOrDefaultById<Database.PostgreSql.TrColumn>("AA");

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
                    Key01 = "AA",
                    ColVarchar =  "ほげ",
                    ColChar = "123",
                    ColBigint = 123123123,
                    ColInt = int.MaxValue,
                    ColSmallint = short.MaxValue,
                    ColBool = true,
                    ColBool2 = false,
                    ColDecimal = 12345678.12m,
                    ColNumeric = 1234567890123m,
                    ColReal = 123123.123f,
                    ColDouble = 123123.123d,
                    ColDate = DateTime.Now,
                    ColTime = new TimeSpan(12, 24, 56),
                    ColTimestamp = DateTime.Now,
                    ColTimestamp3 = DateTime.Now,
                    ColText = "ほげÄÄÄÄ",
                    ColBytea = new byte[64],
                };
                db.Insert(rec6);

                db.AbortTransaction();
            }
        }
    }
}
