using System;

namespace PetaPocoAppFx
{
    public class TestOracle : ITest
    {
        public void Execute()
        {
            Database.Oracle.Config.ConnectionString = Program.ConnectionString;
            using (var db = new Database.Oracle.DB())
            {
                var recs1 = db.Fetch<Database.Oracle.TrCompositeKey>();
                var recs2 = db.Fetch<Database.Oracle.TrAutoNumber>();
                var recs3 = db.Fetch<Database.Oracle.TrColumn>();

                db.BeginTransaction();

                var rec4 = new Database.Oracle.TrAutoNumber
                {
                    Key01 = "AA",
                    ColInt = 999,
                };
                db.Insert(rec4);

                var rec5 = new Database.Oracle.TrCompositeKey
                {
                    Key01 = "13",
                    Key02 = 13,
                    ColVarchar = "ほげほげ",
                    UpdateDt = DateTime.Now,
                };
                db.Update(rec5);

                var rec6 = new Database.Oracle.TrColumn
                {
                    Key01 = "AB",
                    ColNumber5 = 12345,
                    ColBinaryDouble = 1234567.123,
                    ColBinaryFloat = 1234,
                    ColDate = DateTime.Now,
                    ColTimestamp = DateTime.Now,
                    ColChar = "A",
                    ColNvarchar2 = "ほげほげ",
                    ColVarchar2 = "ほげほげ",
                    ColBlob = new byte[64],
                };
                db.Insert(rec6);

                db.AbortTransaction();
            }
        }
    }
}
