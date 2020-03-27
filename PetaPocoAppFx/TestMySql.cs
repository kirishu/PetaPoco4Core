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
                    Key01 = "AA",
                    ColBigint =  long.MaxValue,
                    ColInt = int.MaxValue,
                    ColSmallint = short.MaxValue,
                    ColTinyint = sbyte.MaxValue,
                    ColBool = false,
                    ColDecimal = 1234567890.12m,
                    ColFloat = 1231.123f,
                    ColDouble = 1231.12d,
                    ColDate = DateTime.Now,
                    ColTime = new TimeSpan(12, 24, 56),
                    ColDateTime = DateTime.Now,
                    ColChar = "あいうえお",
                    ColVarchar = "あいうえおほほほほ",
                    ColText = "色は匂へど 散りぬるを 我が世誰そ 常ならむ 有為の奥山 今日越えて 浅き夢見じ 酔ひもせず",
                    ColBlob = new byte[64],

                    ColBigintU = ulong.MaxValue,
                    ColIntU = uint.MaxValue,
                    ColMediumint = short.MaxValue,
                    ColMediumintU = ushort.MaxValue,
                    ColSmallintU = ushort.MaxValue,
                    ColTinyintU = byte.MaxValue,
                    ColBit = true,
                    ColNumeric = 9876543210.321m,
                    ColTimeStamp = DateTime.Now,
                    
                };
                db.Insert(rec6);

                var rec7 = db.SingleById<Database.MySql.TrColumn>("AA");
                System.Diagnostics.Debug.WriteLine(rec7.ColBigint);
                System.Diagnostics.Debug.WriteLine(rec7.ColInt);
                System.Diagnostics.Debug.WriteLine(rec7.ColSmallint);
                System.Diagnostics.Debug.WriteLine(rec7.ColTinyint);
                System.Diagnostics.Debug.WriteLine(rec7.ColBool);
                System.Diagnostics.Debug.WriteLine(rec7.ColDecimal);
                System.Diagnostics.Debug.WriteLine(rec7.ColFloat);
                System.Diagnostics.Debug.WriteLine(rec7.ColDouble);
                System.Diagnostics.Debug.WriteLine(rec7.ColDate);
                System.Diagnostics.Debug.WriteLine(rec7.ColTime);
                System.Diagnostics.Debug.WriteLine(rec7.ColDateTime);
                System.Diagnostics.Debug.WriteLine(rec7.ColChar);
                System.Diagnostics.Debug.WriteLine(rec7.ColVarchar);
                System.Diagnostics.Debug.WriteLine(rec7.ColText);
                System.Diagnostics.Debug.WriteLine(rec7.ColBlob);

                System.Diagnostics.Debug.WriteLine(rec7.ColBigintU);
                System.Diagnostics.Debug.WriteLine(rec7.ColIntU);
                System.Diagnostics.Debug.WriteLine(rec7.ColMediumint);
                System.Diagnostics.Debug.WriteLine(rec7.ColMediumintU);
                System.Diagnostics.Debug.WriteLine(rec7.ColSmallintU);
                System.Diagnostics.Debug.WriteLine(rec7.ColTinyintU);
                System.Diagnostics.Debug.WriteLine(rec7.ColBit);
                System.Diagnostics.Debug.WriteLine(rec7.ColNumeric);
                System.Diagnostics.Debug.WriteLine(rec7.ColTimeStamp);

                db.AbortTransaction();
            }
        }
    }
}
