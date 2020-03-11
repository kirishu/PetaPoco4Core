using System;

namespace PetaPocoAppFx
{
    public class Program
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public static string ConnectionString;

        public static void Main(string[] args)
        {
            PetaPoco.Database.RDBType rdbType = PetaPoco.Database.RDBType.NotSet;

            ITest test = null;

            if (args != null && args.Length > 0)
            {
                switch (args[0].ToUpper())
                {
                    case "MYSQL":
                        rdbType = PetaPoco.Database.RDBType.MySql;
                        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MySql"].ConnectionString;
                        test = new TestMySql();
                        break;
                    case "POSTGRESQL":
                        rdbType = PetaPoco.Database.RDBType.PostgreSql;
                        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["PostgreSql"].ConnectionString;
                        test = new TestPostgreSql();
                        break;
                    case "SQLSERVER":
                        rdbType = PetaPoco.Database.RDBType.SqlServer;
                        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLServer"].ConnectionString;
                        test = new TestSQLServer();
                        break;
                    case "SQLITE":
                        rdbType = PetaPoco.Database.RDBType.SQLite;
                        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SQLite"].ConnectionString;
                        test = new TestSQLite();
                        break;
                    case "ORACLE":
                        rdbType = PetaPoco.Database.RDBType.Oracle;
                        ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString;
                        test = new TestOracle();
                        break;
                }
            }

            if (rdbType == PetaPoco.Database.RDBType.NotSet)
            {
                Console.WriteLine("Require Arguments");
                Console.WriteLine("    MySql");
                Console.WriteLine("    PostgreSql");
                Console.WriteLine("    SqlServer");
                Console.WriteLine("    SQLite");
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
