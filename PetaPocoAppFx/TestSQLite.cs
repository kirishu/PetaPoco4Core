using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace PetaPocoAppFx
{
    public class TestSQLite : ITest
    {
        public void Execute()
        {
            Database.SQLite.Config.ConnectionString = Program.ConnectionString;

            using (var db = new Database.SQLite.DB())
            {

                var pk1 = new
                {
                    Key01 = "13",
                    Key02 = 13,
                };
                var rec1 = db.SingleOrDefaultById<Database.SQLite.TrCompositeKey>(pk1);
                System.Diagnostics.Debug.WriteLine(rec1.ColVarchar);

                var rec2 = db.SingleOrDefaultById<Database.SQLite.TrAutoNumber>(123);
                var rec3 = db.SingleOrDefaultById<Database.SQLite.TrColumn>("AA");

                db.BeginTransaction();

                var rec4 = new Database.SQLite.TrAutoNumber
                {
                    Key01 = "AA",
                    ColInt = 999,
                };
                db.Insert(rec4);

                var rec5 = new Database.SQLite.TrCompositeKey
                {
                    Key01 = "13",
                    Key02 = 13,
                    ColVarchar = "ほげほげ",
                    UpdateDt = DateTime.Now,
                };
                db.Update(rec5);

                var rec6 = new Database.SQLite.TrColumn
                {
                    Key01 = "AB",
                    ColBigInt = long.MaxValue,
                    ColInt = int.MaxValue,
                    ColIntU = uint.MaxValue,
                    ColSmallInt = short.MaxValue,
                    ColTinyInt = sbyte.MaxValue,
                    ColBool = true,
                    ColDecimal = 123123.12m,
                    ColNumeric = 333333.333m,
                    ColDouble = 1231.12d,
                    ColFloat = 1231.123f,
                    ColDate = DateTime.Now,
                    ColTime = new DateTime(),
                    ColBlob = new byte[64],
                };
                db.Insert(rec6);

                var rec7 = db.SingleOrDefaultById<Database.SQLite.TrColumn>("AB");
                System.Diagnostics.Debug.WriteLine(rec7.Key01);
                System.Diagnostics.Debug.WriteLine(rec7.ColBigInt);
                System.Diagnostics.Debug.WriteLine(rec7.ColInt);
                System.Diagnostics.Debug.WriteLine(rec7.ColIntU);
                System.Diagnostics.Debug.WriteLine(rec7.ColSmallInt);
                System.Diagnostics.Debug.WriteLine(rec7.ColTinyInt);
                System.Diagnostics.Debug.WriteLine(rec7.ColBool);
                System.Diagnostics.Debug.WriteLine(rec7.ColDecimal);
                System.Diagnostics.Debug.WriteLine(rec7.ColNumeric);
                System.Diagnostics.Debug.WriteLine(rec7.ColDouble);
                System.Diagnostics.Debug.WriteLine(rec7.ColFloat);
                System.Diagnostics.Debug.WriteLine(rec7.ColDate);
                System.Diagnostics.Debug.WriteLine(rec7.ColTime);

                db.AbortTransaction();
            }
        }
    }
}
