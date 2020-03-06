using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaPocoAppFx
{
    public class ProcsSelect
    {
        public ProcsSelect()
        {

            //typeTrCompositKey = typeof(Database.PostgreSql.TrCompositeKey);

        }

        public void ExecutePostgreSql()
        {
            Database.PostgreSql.Config.ConnectionString = Program.ConnectionString;
            using (var db = new Database.PostgreSql.DB())
            {
                var recs1 = db.Fetch<Database.PostgreSql.TrCompositeKey>();
                var recs2 = db.Fetch<Database.PostgreSql.TrAutoNumber>();
            }
        }

    }
}
