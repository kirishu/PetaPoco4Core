using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaPocoAppFx
{
    public class Program
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static string ConnectionString;

        public static void Main(string[] args)
        {
            PetaPoco.Database.DBType dbType = PetaPoco.Database.DBType.NotSet;

            if (args != null && args.Length > 0)
            {
                switch (args[0].ToUpper())
                {
                    case "MYSQL":
                        dbType = PetaPoco.Database.DBType.MySql;
                        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
                        break;
                    case "POSTGRESQL":
                        dbType = PetaPoco.Database.DBType.PostgreSql;
                        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PostgreSql"].ConnectionString;
                        break;
                        //ConstrSQLServer = System.Configuration.ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                        //ConstrOracle = System.Configuration.ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                }
            }

            if (dbType == PetaPoco.Database.DBType.NotSet)
            {
                Console.WriteLine("Require Arguments");
                Console.WriteLine("    MySql");
                Console.WriteLine("    PostgreSql");
                Console.WriteLine("    SqlServer");
                Console.WriteLine("    Oracle");
                return;
            }

            var select = new ProcsSelect();
            select.Execute();

        }
    }
}
