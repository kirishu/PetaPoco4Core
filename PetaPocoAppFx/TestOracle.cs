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
                var rec6result = db.Insert(rec6);
                System.Diagnostics.Debug.WriteLine(rec6result);

                var rec7 = db.SingleOrDefaultById<Database.Oracle.TrColumn>("AB");
                System.Diagnostics.Debug.WriteLine(rec7.Key01);
                System.Diagnostics.Debug.WriteLine(rec7.ColNumber5);
                System.Diagnostics.Debug.WriteLine(rec7.ColBinaryDouble);
                System.Diagnostics.Debug.WriteLine(rec7.ColBinaryFloat);
                System.Diagnostics.Debug.WriteLine(rec7.ColDate);
                System.Diagnostics.Debug.WriteLine(rec7.ColTimestamp);
                System.Diagnostics.Debug.WriteLine(rec7.ColChar);
                System.Diagnostics.Debug.WriteLine(rec7.ColNvarchar2);
                System.Diagnostics.Debug.WriteLine(rec7.ColVarchar2);

                db.AbortTransaction();
            }
        }
    }
}
