using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaPocoAppFx
{
    public class ProcsSelect
    {
        PetaPoco.DatabaseExtension _db;

        Type T1;
        


        public ProcsSelect()
        {
            Database.PostgreSql.Config.ConnectionString = Program.ConnectionString;
            _db = new Database.PostgreSql.DB();

            //typeTrCompositKey = typeof(Database.PostgreSql.TrCompositeKey);

        }

        public void Execute()
        {
            var recs = _db.Fetch<Database.PostgreSql.TrCompositeKey>();
            Console.WriteLine(recs.Count);

        }

    }
}
