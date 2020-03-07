using System;

namespace PetaPocoAppFx
{
    public class Program
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static string ConnectionString;

        public static void Main(string[] args)
        {
            PetaPoco.Database.DBType dbType = PetaPoco.Database.DBType.NotSet;

            ITest test = null;

            if (args != null && args.Length > 0)
            {
                switch (args[0].ToUpper())
                {
                    case "MYSQL":
                        dbType = PetaPoco.Database.DBType.MySql;
                        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
                        test = new TestMySql();
                        break;
                    case "POSTGRESQL":
                        dbType = PetaPoco.Database.DBType.PostgreSql;
                        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PostgreSql"].ConnectionString;
                        test = new TestPostgreSql();
                        break;
                    case "SQLSERVER":
                        dbType = PetaPoco.Database.DBType.SqlServer;
                        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                        test = new TestSQLServer();
                        break;
                    case "ORACLE":
                        dbType = PetaPoco.Database.DBType.Oracle;
                        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                        test = new TestOracle();
                        break;
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

            //try
            //{
            if (test != null)
            {
                test.Execute();
            }
            //}
            //catch (Exception ex)
            //{
            //    _logger.Error(ex.ToString());

            //}
        }
    }
}
