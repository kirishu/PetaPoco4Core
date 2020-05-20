/* PetaPoco v4.0.3.12 - A Tiny ORMish thing for your POCO's.
 * Copyright 2011-2012 Topten Software.  All Rights Reserved.
 *
 * Apache License 2.0 - http://www.toptensoftware.com/petapoco/license
 *
 * Special thanks to Rob Conery (@robconery) for original inspiration (ie:Massive) and for
 * use of Subsonic's T4 templates, Rob Sullivan (@DataChomp) for hard core DBA advice
 * and Adam Schroder (@schotime) for lots of suggestions, improvements and Oracle support
 */
/* --------------------------------------------------------------------------
 * Modified by kirishu (zapshu@gmail.com)
 * v4.7.1.5
 * https://github.com/kirishu/PetaPoco4Core
 * -------------------------------------------------------------------------- */

#region Suppressions
#pragma warning disable CS1591  // Missing XML comment for publicly visible type or member
#pragma warning disable CA1034  // Nested types should not be visible
#pragma warning disable CA1051  // Do not declare visible instance fields
#pragma warning disable CA2227  // Collection properties should be read only
#pragma warning disable IDE0034 // default expression can be simplified
#pragma warning disable IDE0063 // 'using' statement can be simplified
#pragma warning disable IDE0066 // Convert switch statement to switch expression
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace PetaPoco
{
    /// <summary>
    /// Holds the results of a paged request.
    /// </summary>
    /// <typeparam name="T">The type of Poco in the returned result set</typeparam>
    public class Page<T>
    {
        /// <summary>The current page number contained in this page of result set</summary>
        public long CurrentPage { get; set; }
        /// <summary>The total number of pages in the full result set</summary>
        public long TotalPages { get; set; }
        /// <summary>The total number of records in the full result set</summary>
        public long TotalItems { get; set; }
        /// <summary>The number of items per page</summary>
        public long ItemsPerPage { get; set; }
        /// <summary>The actual records on this page</summary>
        public List<T> Items { get; set; }
        /// <summary>User property to hold anything.</summary>
        public object Context { get; set; }
    }

    /// <summary>
    /// Wrap strings in an instance of this class to force use of DbType.AnsiString
    /// </summary>
    public class AnsiString
    {
        /// <summary>
        /// Constructs an AnsiString
        /// </summary>
        /// <param name="str">The C# string to be converted to ANSI before being passed to the DB</param>
        public AnsiString(string str)
        {
            Value = str;
        }
        /// <summary>The string value</summary>
        public string Value { get; private set; }
    }

    /// <summary>
    /// Use by IMapper to override table bindings for an object
    /// </summary>
    public class TableInfo
    {
        /// <summary>The database table name</summary>
        public string TableName { get; set; }
        /// <summary>The name of the primary key column of the table</summary>
        public string PrimaryKey { get; set; }
        /// <summary>True if the primary key column is an auto-incrementing</summary>
        public bool AutoIncrement { get; set; }
        /// <summary>The name of the sequence used for auto-incrementing Oracle primary key fields</summary>
        public string SequenceName { get; set; }

        /// <summary>
        /// Creates and populates a TableInfo from the attributes of a POCO
        /// </summary>
        /// <param name="t">The POCO type</param>
        /// <returns>A TableInfo instance</returns>
        public static TableInfo FromPoco(Type t)
        {
            if (t == null) { throw new ArgumentNullException(nameof(t)); }

            var ti = new TableInfo();

            // Get the table name
            var a = t.GetCustomAttributes(typeof(TableNameAttribute), true);
            ti.TableName = a.Length == 0 ? t.Name : (a[0] as TableNameAttribute).Value;

            // Get the primary key
            a = t.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
            ti.PrimaryKey = a.Length == 0 ? null : (a[0] as PrimaryKeyAttribute).Value;
            ti.SequenceName = a.Length == 0 ? null : (a[0] as PrimaryKeyAttribute).SequenceName;
            ti.AutoIncrement = a.Length != 0 && (a[0] as PrimaryKeyAttribute).AutoIncrement;

            if (string.IsNullOrEmpty(ti.PrimaryKey))
            {
                var prop = t.GetProperties().FirstOrDefault(p =>
                {
                    if (p.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (p.Name.Equals(t.Name + "id", StringComparison.OrdinalIgnoreCase))
                        return true;
                    if (p.Name.Equals(t.Name + "_id", StringComparison.OrdinalIgnoreCase))
                        return true;
                    return false;
                });

                if (prop != null)
                {
                    ti.PrimaryKey = prop.Name;
                    ti.AutoIncrement = prop.PropertyType.IsValueType;
                }
            }
            return ti;
        }
    }

    /// <summary>
    /// IMapper provides a way to hook into PetaPoco's Database to POCO mapping mechanism to either
    /// customize or completely replace it.
    /// </summary>
    /// <remarks>
    /// To use this functionality, instantiate a class that implements IMapper and then pass it to
    /// PetaPoco through the static method Mappers.Register()
    /// </remarks>
    public interface IMapper
    {
        void GetTableInfo(Type t, TableInfo ti);
        bool MapPropertyToColumn(PropertyInfo pi, ref string columnName, ref bool resultColumn);
        /// <summary>
        /// Supply a function to convert a database value to the correct property value
        /// </summary>
        /// <param name="targetProperty">The target property</param>
        /// <param name="sourceType">The type of data returned by the DB</param>
        /// <returns>A Func that can do the conversion, or null for no conversion</returns>
        Func<object, object> GetFromDbConverter(PropertyInfo targetProperty, Type sourceType);

        /// <summary>
        /// Supply a function to convert a property value into a database value
        /// </summary>
        /// <param name="SourceType">The type of data returned by the DB</param>
        /// <returns>A Func that can do the conversion</returns>
        Func<object, object> GetToDbConverter(Type SourceType);
    }

    /// <summary>
    /// the default implementation of IMapper used by PetaPoco
    /// </summary>
    public class DefaultMapper : IMapper
    {
        public virtual void GetTableInfo(Type t, TableInfo ti) { }
        public virtual bool MapPropertyToColumn(PropertyInfo pi, ref string columnName, ref bool resultColumn)
        {
            return true;
        }

        /// <summary>
        /// Supply a function to convert a database value to the correct property value
        /// </summary>
        /// <param name="targetProperty">The target property</param>
        /// <param name="sourceType">The type of data returned by the DB</param>
        /// <returns>A Func that can do the conversion, or null for no conversion</returns>
        public virtual Func<object, object> GetFromDbConverter(PropertyInfo targetProperty, Type sourceType)
        {
            return null;
        }

        /// <summary>
        /// Supply a function to convert a property value into a database value
        /// </summary>
        /// <param name="sourceType">he type of data returned by the DB</param>
        /// <returns>A Func that can do the conversion</returns>
        public virtual Func<object, object> GetToDbConverter(Type sourceType)
        {
            return null;
        }
    }

    /// <summary>
    /// Specifies the database queries.
    /// </summary>
    public interface IDatabaseQuery
    {
        void OpenSharedConnection();
        void CloseSharedConnection();
        int Execute(string sql, params object[] args);
        int Execute(Sql sql);
        T ExecuteScalar<T>(string sql, params object[] args);
        T ExecuteScalar<T>(Sql sql);
        List<T> Fetch<T>();
        List<T> Fetch<T>(string sql, params object[] args);
        List<T> Fetch<T>(Sql sql);
        List<T> Fetch<T>(long page, long itemsPerPage, string sql, params object[] args);
        List<T> Fetch<T>(long page, long itemsPerPage, Sql sql);
        Page<T> Page<T>(long page, long itemsPerPage, string sql, params object[] args);
        Page<T> Page<T>(long page, long itemsPerPage, Sql sql);
        List<T> SkipTake<T>(long skip, long take, string sql, params object[] args);
        List<T> SkipTake<T>(long skip, long take, Sql sql);
        List<TRet> Fetch<T1, T2, TRet>(Func<T1, T2, TRet> cb, string sql, params object[] args);
        List<TRet> Fetch<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, string sql, params object[] args);
        List<TRet> Fetch<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, string sql, params object[] args);
        IEnumerable<TRet> Query<T1, T2, TRet>(Func<T1, T2, TRet> cb, string sql, params object[] args);
        IEnumerable<TRet> Query<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, string sql, params object[] args);
        IEnumerable<TRet> Query<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, string sql, params object[] args);
        List<TRet> Fetch<T1, T2, TRet>(Func<T1, T2, TRet> cb, Sql sql);
        List<TRet> Fetch<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, Sql sql);
        List<TRet> Fetch<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, Sql sql);
        IEnumerable<TRet> Query<T1, T2, TRet>(Func<T1, T2, TRet> cb, Sql sql);
        IEnumerable<TRet> Query<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, Sql sql);
        IEnumerable<TRet> Query<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, Sql sql);
        List<T1> Fetch<T1, T2>(string sql, params object[] args);
        List<T1> Fetch<T1, T2, T3>(string sql, params object[] args);
        List<T1> Fetch<T1, T2, T3, T4>(string sql, params object[] args);
        IEnumerable<T1> Query<T1, T2>(string sql, params object[] args);
        IEnumerable<T1> Query<T1, T2, T3>(string sql, params object[] args);
        IEnumerable<T1> Query<T1, T2, T3, T4>(string sql, params object[] args);
        IEnumerable<TRet> Query<TRet>(Type[] types, object cb, Sql sql);
        List<T1> Fetch<T1, T2>(Sql sql);
        List<T1> Fetch<T1, T2, T3>(Sql sql);
        List<T1> Fetch<T1, T2, T3, T4>(Sql sql);
        IEnumerable<T1> Query<T1, T2>(Sql sql);
        IEnumerable<T1> Query<T1, T2, T3>(Sql sql);
        IEnumerable<T1> Query<T1, T2, T3, T4>(Sql sql);
        IEnumerable<T> Query<T>(string sql, params object[] args);
        IEnumerable<T> Query<T>(Sql sql);
        T SingleById<T>(object primaryKey);
        T SingleOrDefaultById<T>(object primaryKey);
        T SingleOrDefault<T>(string sql, params object[] args);
        T First<T>(string sql, params object[] args);
        T FirstOrDefault<T>(string sql, params object[] args);
        T SingleOrDefault<T>(Sql sql);
        T First<T>(Sql sql);
        T FirstOrDefault<T>(Sql sql);
        Dictionary<TKey, TValue> Dictionary<TKey, TValue>(Sql sql);
        Dictionary<TKey, TValue> Dictionary<TKey, TValue>(string sql, params object[] args);
        bool Exists<T>(object primaryKey);
        int OneTimeCommandTimeout { get; set; }

        TRet FetchMultiple<T1, T2, TRet>(Func<List<T1>, List<T2>, TRet> cb, string sql, params object[] args);
        TRet FetchMultiple<T1, T2, T3, TRet>(Func<List<T1>, List<T2>, List<T3>, TRet> cb, string sql, params object[] args);
        TRet FetchMultiple<T1, T2, T3, T4, TRet>(Func<List<T1>, List<T2>, List<T3>, List<T4>, TRet> cb, string sql, params object[] args);
        TRet FetchMultiple<T1, T2, TRet>(Func<List<T1>, List<T2>, TRet> cb, Sql sql);
        TRet FetchMultiple<T1, T2, T3, TRet>(Func<List<T1>, List<T2>, List<T3>, TRet> cb, Sql sql);
        TRet FetchMultiple<T1, T2, T3, T4, TRet>(Func<List<T1>, List<T2>, List<T3>, List<T4>, TRet> cb, Sql sql);

        Tuple<List<T1>, List<T2>> FetchMultiple<T1, T2>(string sql, params object[] args);
        Tuple<List<T1>, List<T2>, List<T3>> FetchMultiple<T1, T2, T3>(string sql, params object[] args);
        Tuple<List<T1>, List<T2>, List<T3>, List<T4>> FetchMultiple<T1, T2, T3, T4>(string sql, params object[] args);
        Tuple<List<T1>, List<T2>> FetchMultiple<T1, T2>(Sql sql);
        Tuple<List<T1>, List<T2>, List<T3>> FetchMultiple<T1, T2, T3>(Sql sql);
        Tuple<List<T1>, List<T2>, List<T3>, List<T4>> FetchMultiple<T1, T2, T3, T4>(Sql sql);
    }

    /// <summary>
    /// Represents the core functionality of PetaPoco.
    /// </summary>
    public interface IDatabase : IDatabaseQuery
    {
        IDbConnection Connection { get; }
        IDbTransaction DbTransaction { get; }
        IDataParameter CreateParameter();
        Transaction GetTransaction();
        Transaction GetTransaction(IsolationLevel? isolationLevel);
        void BeginTransaction();
        void BeginTransaction(IsolationLevel? isolationLevel);
        void AbortTransaction();
        void CompleteTransaction();
        object Insert<T>(T poco) where T : IPetaPocoRecord<T>;
        int Update<T>(T poco) where T : IPetaPocoRecord<T>;
        int Update<T>(T poco, object primaryKey) where T : IPetaPocoRecord<T>;
        int Update<T>(T poco, IEnumerable<string> columns) where T : IPetaPocoRecord<T>;
        int Update<T>(T poco, object primaryKey, IEnumerable<string> columns) where T : IPetaPocoRecord<T>;
        int Update<T>(string sql, params object[] args) where T : IPetaPocoRecord<T>;
        int Update<T>(Sql sql) where T : IPetaPocoRecord<T>;
        int Delete<T>(string sql, params object[] args) where T : IPetaPocoRecord<T>;
        int Delete<T>(Sql sql) where T : IPetaPocoRecord<T>;
        int Delete<T>(T poco) where T : IPetaPocoRecord<T>;
    }

    /// <summary>
    /// The main PetaPoco Database class.  You can either use this class directly, or derive from it.
    /// </summary>
    public class Database : IDisposable, IDatabase
    {
        public enum RDBType
        {
            NotSet,
            SqlServer,
            MySql,
            PostgreSql,
            Oracle,
            SQLite
        }
        protected readonly RDBType _rdbType;

        /// <summary>The prefix used to delimit parameters in SQL query strings.</summary>
        public string ParamPrefix = "@";

        /// <summary>Sets the timeout value for all SQL statements.</summary>
        public int CommandTimeout { get; set; }
        /// <summary>Sets the timeout value for the next (and only next) SQL statement</summary>
        public int OneTimeCommandTimeout { get; set; }

        /// <summary>This static class manages registation of IMapper instances with PetaPoco</summary>
        public static IMapper Mapper { get; set; }
        /// <summary>True if time and date values returned through this column should be forced to UTC DateTimeKind.</summary>
        public bool ForceDateTimesToUtc { get; set; }
        /// <summary>When set to true, PetaPoco will automatically create the "SELECT columns" part of any query that looks like it</summary>        
        public bool EnableAutoSelect { get; set; }

        // Member variables
        private readonly string _connectionString;
        private readonly DbProviderFactory _dbFactory;
        private IDbConnection _sharedConnection;
        private IDbTransaction _transaction;
        private int _sharedConnectionDepth;
        private int _transactionDepth;
        private bool _transactionCancelled;
        private string _lastSql;
        private object[] _lastArgs;

        /// <summary>
        /// Constructs an instance using a supplied connections string and provider name.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="rdbType">The database type.</param>
        /// <remarks>
        /// PetaPoco will automatically close and dispose any connections it creates.
        /// </remarks>
        /// <exception cref="ArgumentException">Thrown when <paramref name="connectionString" /> is null or empty.</exception>
        public Database(string connectionString, RDBType rdbType)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(string.Format(Culture, "Connection string cannot be null or empty"), nameof(connectionString));
            }

            _connectionString = connectionString;
            _rdbType = rdbType;
            _dbFactory = GetDbFactory(rdbType);

            _transactionDepth = 0;
            ForceDateTimesToUtc = true;
            EnableAutoSelect = true;

            if (_rdbType == RDBType.MySql
                && _connectionString != null
                && _connectionString.IndexOf("Allow User Variables=true", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                ParamPrefix = "?";
            }
            else if (_rdbType == RDBType.Oracle)
            {
                ParamPrefix = ":";
            }
        }

        /// <summary>
        /// Returns the .NET conforming DbProviderFactory.
        /// </summary>
        /// <param name="rdbType">The database type.</param>
        /// <returns>The db provider factory.</returns>
        /// <exception cref="ArgumentException">Thrown when AssemblyName does not match a type.</exception>
        protected DbProviderFactory GetDbFactory(RDBType rdbType)
        {
            string[] assemblyName;

            if (rdbType == RDBType.SqlServer)
            {
                assemblyName = new string[] {
                    "System.Data.SqlClient.SqlClientFactory, System.Data, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                };
            }
            else if (rdbType == RDBType.MySql)
            {
                assemblyName = new string[] {
                    "MySql.Data.MySqlClient.MySqlClientFactory, MySql.Data, Culture=neutral, PublicKeyToken=c5687fc88969c44d",
                };
            }
            else if (rdbType == RDBType.Oracle)
            {
                assemblyName = new string[] {
                    "Oracle.ManagedDataAccess.Client.OracleClientFactory, Oracle.ManagedDataAccess, Culture=neutral, PublicKeyToken=89b483f429c47342",
                    "Oracle.DataAccess.Client.OracleClientFactory, Oracle.DataAccess",
                };
            }
            else if (rdbType == RDBType.PostgreSql)
            {
                assemblyName = new string[] {
                    "Npgsql.NpgsqlFactory, Npgsql, Culture=neutral, PublicKeyToken=5d8b90d52f46fda7",
                };
            }
            else if (rdbType == RDBType.SQLite)
            {
                assemblyName = new string[] {
                    "System.Data.SQLite.SQLiteFactory, System.Data.SQLite",
                };
            }
            else
            {
                throw new ArgumentException(string.Format(Culture, "DBType is not exist"));
            }

            Type ft = null;
            foreach (var asm in assemblyName)
            {
                ft = Type.GetType(asm);
                if (ft != null) break;
            }
            if (ft == null)
            {
                throw new ArgumentException("Could not load the " + GetType().Name + " DbProviderFactory.");
            }
            return (DbProviderFactory)ft.GetField("Instance").GetValue(null);
        }

        /// <summary>
        /// Automatically close one open shared connection
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Automatically close one open shared connection
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            // Automatically close one open connection reference
            //  (Works with KeepConnectionAlive and manually opening a shared connection)
            // added [kirishu]
            if (disposing)
            {
                if (_transaction == null)
                {
                    CloseSharedConnection();
                }
                else
                {
                    _transactionCancelled = true;
                    CleanupTransaction();
                }
            }
        }

        /// <summary>Set to true to keep the first opened connection alive until this object is disposed</summary>
        public bool KeepConnectionAlive { get; set; }

        /// <summary>
        /// Open a connection (can be nested)
        /// </summary>
        public void OpenSharedConnection()
        {
            if (_sharedConnectionDepth == 0)
            {
                _sharedConnection = _dbFactory.CreateConnection();
                _sharedConnection.ConnectionString = _connectionString;

                if (_sharedConnection.State == ConnectionState.Broken)
                    _sharedConnection.Close();
                if (_sharedConnection.State == ConnectionState.Closed)
                    _sharedConnection.Open();

                _sharedConnection = OnConnectionOpened(_sharedConnection);

                if (KeepConnectionAlive)
                    _sharedConnectionDepth++;        // Make sure you call Dispose
            }
            _sharedConnectionDepth++;
        }

        /// <summary>
        /// Close a previously opened connection
        /// </summary>
        public void CloseSharedConnection()
        {
            if (_sharedConnectionDepth > 0)
            {
                _sharedConnectionDepth--;
                if (_sharedConnectionDepth == 0)
                {
                    OnConnectionClosing(_sharedConnection);
                    _sharedConnection.Dispose();
                    _sharedConnection = null;
                }
            }
        }

        // Access to our shared connection
        public IDbConnection Connection
        {
            get { return _sharedConnection; }
        }

        public IDbTransaction DbTransaction
        {
            get { return _transaction; }
        }

        public IDataParameter CreateParameter()
        {
            using (var conn = _sharedConnection ?? _dbFactory.CreateConnection())
            using (var comm = conn.CreateCommand())
                return comm.CreateParameter();
        }

        // Helper to create a transaction scope
        public Transaction GetTransaction()
        {
            return GetTransaction(null);
        }

        public Transaction GetTransaction(IsolationLevel? isolationLevel)
        {
            return new Transaction(this, isolationLevel);
        }

        // Use by derived repo generated by T4 templates
        public virtual void OnBeginTransaction() { }
        public virtual void OnEndTransaction() { }

        public void BeginTransaction()
        {
            BeginTransaction(null);
        }

        // Start a new transaction, can be nested, every call must be
        //    matched by a call to AbortTransaction or CompleteTransaction
        // Use `using (var scope=db.Transaction) { scope.Complete(); }` to ensure correct semantics
        public void BeginTransaction(IsolationLevel? isolationLevel)
        {
            _transactionDepth++;

            if (_transactionDepth == 1)
            {
                OpenSharedConnection();
                _transaction = isolationLevel == null ? _sharedConnection.BeginTransaction() : _sharedConnection.BeginTransaction(isolationLevel.Value);
                _transactionCancelled = false;
                OnBeginTransaction();
            }

        }

        // Internal helper to cleanup transaction stuff
        void CleanupTransaction()
        {
            OnEndTransaction();


            if (_transactionCancelled)
                _transaction.Rollback();
            else
                _transaction.Commit();

            _transaction.Dispose();
            _transaction = null;

            CloseSharedConnection();
        }

        // Abort the entire outer most transaction scope
        public void AbortTransaction()
        {
            _transactionCancelled = true;
            if ((--_transactionDepth) == 0)
                CleanupTransaction();
        }

        // Complete the transaction
        public void CompleteTransaction()
        {
            if ((--_transactionDepth) == 0)
                CleanupTransaction();
        }

        // Helper to handle named parameters from object properties
        static readonly Regex rxParams = new Regex(@"(?<!@)@\w+", RegexOptions.Compiled);
        public static string ProcessParams(string sql, object[] argsSrc, List<object> argsDest)
        {
            return rxParams.Replace(sql, m =>
            {
                string param = m.Value.Substring(1);

                object arg_val;

                if (int.TryParse(param, out int paramIndex))
                {
                    // Numbered parameter
                    if (paramIndex < 0 || paramIndex >= argsSrc.Length)
                        throw new ArgumentOutOfRangeException(string.Format(Culture, "Parameter '@{0}' specified but only {1} parameters supplied (in `{2}`)", paramIndex, argsSrc.Length, sql));
                    arg_val = argsSrc[paramIndex];
                }
                else
                {
                    // Look for a property on one of the arguments with this name
                    bool found = false;
                    arg_val = null;
                    foreach (var o in argsSrc)
                    {
                        var pi = o.GetType().GetProperty(param);
                        if (pi != null)
                        {
                            arg_val = pi.GetValue(o, null);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        throw new ArgumentException(string.Format(Culture, "Parameter '@{0}' specified but none of the passed arguments have a property with this name (in '{1}')", param, sql));
                }

                // Expand collections to parameter lists
                if ((arg_val as System.Collections.IEnumerable) != null &&
                    (arg_val as string) == null &&
                    (arg_val as byte[]) == null)
                {
                    var sb = new StringBuilder();
                    foreach (var i in arg_val as System.Collections.IEnumerable)
                    {
                        var indexOfExistingValue = argsDest.IndexOf(i);
                        if (indexOfExistingValue >= 0)
                        {
                            sb.Append((sb.Length == 0 ? "@" : ",@") + indexOfExistingValue);
                        }
                        else
                        {
                            sb.Append((sb.Length == 0 ? "@" : ",@") + argsDest.Count);
                            argsDest.Add(i);
                        }
                    }
                    if (sb.Length == 0)
                    {
                        sb.AppendFormat(Culture, "select 1 /*peta_dual*/ where 1 = 0");
                    }
                    return sb.ToString();
                }
                else
                {
                    var indexOfExistingValue = argsDest.IndexOf(arg_val);
                    if (indexOfExistingValue >= 0)
                        return "@" + indexOfExistingValue;

                    argsDest.Add(arg_val);
                    return "@" + (argsDest.Count - 1).ToString(Culture);
                }
            }
            );
        }

        // Add a parameter to a DB command
        void AddParam(IDbCommand cmd, object item, string ParameterPrefix)
        {
            // Convert value to from poco type to db type
            if (Database.Mapper != null && item != null)
            {
                var fn = Database.Mapper.GetToDbConverter(item.GetType());
                if (fn != null)
                    item = fn(item);
            }

            // Support passed in parameters
            if (item is IDbDataParameter idbParam)
            {
                idbParam.ParameterName = string.Format(Culture, "{0}{1}", ParameterPrefix, cmd.Parameters.Count);
                cmd.Parameters.Add(idbParam);
                return;
            }
            var p = cmd.CreateParameter();
            p.ParameterName = string.Format(Culture, "{0}{1}", ParameterPrefix, cmd.Parameters.Count);

            if (item == null)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                var t = item.GetType();
                if (t.IsEnum)        // PostgreSQL .NET driver wont cast enum to int
                {
                    p.Value = (int)item;
                }
                else if (t == typeof(Guid))
                {
                    p.Value = item.ToString();
                    p.DbType = DbType.String;
                    p.Size = 40;
                }
                else if (t == typeof(string))
                {
                    p.Size = Math.Max((item as string).Length + 1, 4000);        // Help query plan caching by using common size
                    p.Value = item;
                }
                else if (t == typeof(AnsiString))
                {
                    // Thanks @DataChomp for pointing out the SQL Server indexing performance hit of using wrong string type on varchar
                    p.Size = Math.Max((item as AnsiString).Value.Length + 1, 4000);
                    p.Value = (item as AnsiString).Value;
                    p.DbType = DbType.AnsiString;
                }
                else if (t == typeof(bool) && _rdbType != RDBType.PostgreSql)
                {
                    p.Value = ((bool)item) ? 1 : 0;
                }
                else if (item.GetType().Name == "SqlGeography") //SqlGeography is a CLR Type
                {
                    p.GetType().GetProperty("UdtTypeName").SetValue(p, "geography", null); //geography is the equivalent SQL Server Type
                    p.Value = item;
                }

                else if (item.GetType().Name == "SqlGeometry") //SqlGeometry is a CLR Type
                {
                    p.GetType().GetProperty("UdtTypeName").SetValue(p, "geometry", null); //geography is the equivalent SQL Server Type
                    p.Value = item;
                }
                // added by [kirishu]
                else if (t == typeof(byte[]))
                {
                    /*
                     *  image型などの列にnullをセットする場合、
                     *      rec.Photo = null;
                     *  ってやると例外吐いちゃう。
                     *
                     *  しょうがないので、nullをセットしたい場合は、空のバイト配列をセットしてください。
                     *     rec.Photo = new byte[] { };
                     *
                     * */
                    if ((item as byte[]).Length == 0)
                    {
                        p.Value = DBNull.Value;
                    }
                    else
                    {
                        p.Value = item;
                    }
                    p.DbType = DbType.Binary;
                }
                else
                {
                    p.Value = item;
                }
            }

            cmd.Parameters.Add(p);
        }

        // Create a command
        static readonly Regex rxParamsPrefix = new Regex(@"(?<!@)@\w+", RegexOptions.Compiled);
        // [kirishu] このメソッドをprivate からprotectedに変更
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        protected IDbCommand CreateCommand(IDbConnection connection, string sql, params object[] args)
        {
            if (connection == null) { throw new ArgumentNullException(nameof(connection)); }
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }

            // Perform parameter prefix replacements
            if (ParamPrefix != "@")
            {
                sql = rxParamsPrefix.Replace(sql, m => ParamPrefix + m.Value.Substring(1));
            }
            sql = sql.Replace("@@", "@");           // <- double @@ escapes a single @

            // Create the command and add parameters
            IDbCommand cmd = connection.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = sql;
            cmd.Transaction = _transaction;

            foreach (var item in args)
            {
                AddParam(cmd, item, ParamPrefix);
            }

            if (_rdbType == RDBType.Oracle)
                cmd.GetType().GetProperty("BindByName").SetValue(cmd, true, null);

            if (_rdbType == RDBType.Oracle || _rdbType == RDBType.MySql)
                cmd.CommandText = cmd.CommandText.Replace("/*peta_dual*/", "from dual");

            if (!String.IsNullOrEmpty(sql))
                DoPreExecute(cmd);

            return cmd;
        }

        // Override this to log/capture exceptions
        public virtual void OnException(Exception x)
        {
            System.Diagnostics.Debug.WriteLine(x?.ToString());
            System.Diagnostics.Debug.WriteLine(LastCommand);
        }

        // Override this to log commands, or modify command before execution
        public virtual IDbConnection OnConnectionOpened(IDbConnection conn) { return conn; }
        public virtual void OnConnectionClosing(IDbConnection conn) { }
        public virtual void OnExecutingCommand(IDbCommand cmd) { }
        public virtual void OnExecutedCommand(IDbCommand cmd) { }

        // Execute a non-query command
        public int Execute(string sql, params object[] args)
        {
            return Execute(new Sql(sql, args));
        }

        public int Execute(Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }

            var sql_ = sql.SQL;
            var args = sql.Arguments.ToArray();

            try
            {
                OpenSharedConnection();
                try
                {
                    using (var cmd = CreateCommand(_sharedConnection, sql_, args))
                    {
                        var result = cmd.ExecuteNonQuery();
                        OnExecutedCommand(cmd);
                        return result;
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                OnException(x);
                throw;
            }
        }

        // Execute and cast a scalar property
        public T ExecuteScalar<T>(string sql, params object[] args)
        {
            return ExecuteScalar<T>(new Sql(sql, args));
        }

        public T ExecuteScalar<T>(Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }

            var sql_ = sql.SQL;
            var args = sql.Arguments.ToArray();

            try
            {
                OpenSharedConnection();
                try
                {
                    using (var cmd = CreateCommand(_sharedConnection, sql_, args))
                    {
                        object val = cmd.ExecuteScalar();
                        OnExecutedCommand(cmd);

                        Type t = typeof(T);
                        Type u = Nullable.GetUnderlyingType(t);

                        if (val == null || val == DBNull.Value)
                            return default(T);

                        return u != null ? (T)Convert.ChangeType(val, u, Culture) : (T)Convert.ChangeType(val, t, Culture);
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                OnException(x);
                throw;
            }
        }

        static readonly Regex rxSelect = new Regex(@"\A\s*(SELECT|EXECUTE|CALL|EXEC)\s", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        static readonly Regex rxFrom = new Regex(@"\A\s*FROM\s", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Multiline);
        string AddSelectClause<T>(string sql)
        {
            if (sql.StartsWith(";", StringComparison.OrdinalIgnoreCase))
                return sql.Substring(1);

            if (!rxSelect.IsMatch(sql))
            {
                var pd = PocoData.ForType(typeof(T));
                var tableName = EscapeTableName(pd.TableInfo.TableName);
                string cols = string.Join(", ", pd.QueryColumns.Select(c => EscapeSqlIdentifier(c)).ToArray());
                if (!rxFrom.IsMatch(sql))
                    sql = string.Format(Culture, "SELECT {0} FROM {1} {2}", cols, tableName, sql);
                else
                    sql = string.Format(Culture, "SELECT {0} {1}", cols, sql);
            }
            return sql;
        }

        // Return a typed list of pocos
        public List<T> Fetch<T>(string sql, params object[] args)
        {
            return Fetch<T>(new Sql(sql, args));
        }

        public List<T> Fetch<T>(Sql sql)
        {
            return Query<T>(sql).ToList();
        }

        public List<T> Fetch<T>()
        {
            return Fetch<T>("");
        }

        // modified by [kirishu]
        // ↓これら正規表現パターンはprivateだったけど、継承側で使いたいのでprotectedにしました
        protected static readonly Regex rxColumns = new Regex(@"\A\s*SELECT\s+((?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|.)*?)(?<!,\s+)\bFROM\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        protected static readonly Regex rxOrderBy = new Regex(@"\bORDER\s+BY\s+(?!.*?(?:\)|\s+)AS\s)(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?(?:\s*,\s*(?:\((?>\((?<depth>)|\)(?<-depth>)|.?)*(?(depth)(?!))\)|[\w\(\)\.])+(?:\s+(?:ASC|DESC))?)*", RegexOptions.RightToLeft | RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);
        protected static readonly Regex rxDistinct = new Regex(@"\ADISTINCT\s", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.Compiled);

        public static bool SplitSqlForPaging(string sql, out string sqlCount, out string sqlSelectRemoved, out string sqlOrderBy)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }

            sqlSelectRemoved = null;
            sqlCount = null;
            sqlOrderBy = null;

            // Extract the columns from "SELECT <whatever> FROM"
            var m = rxColumns.Match(sql);
            if (!m.Success)
                return false;

            // Save column list and replace with COUNT(*)
            Group g = m.Groups[1];
            sqlSelectRemoved = sql.Substring(g.Index);

            if (rxDistinct.IsMatch(sqlSelectRemoved))
                sqlCount = sql.Substring(0, g.Index) + "COUNT(" + m.Groups[1].ToString().Trim() + ") " + sql.Substring(g.Index + g.Length);
            else
                sqlCount = sql.Substring(0, g.Index) + "COUNT(*) " + sql.Substring(g.Index + g.Length);

            // Look for an "ORDER BY <whatever>" clause
            m = rxOrderBy.Match(sqlCount);
            if (m.Success)
            {
                g = m.Groups[0];
                sqlOrderBy = g.ToString();
                sqlCount = sqlCount.Substring(0, g.Index) + sqlCount.Substring(g.Index + g.Length);
            }

            return true;
        }

        public void BuildPageQueries<T>(long skip, long take, string sql, ref object[] args, out string sqlCount, out string sqlPage)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            if (args == null) { throw new ArgumentNullException(nameof(args)); }

            // Add auto select clause
            sql = AddSelectClause<T>(sql);

            // Split the SQL into the bits we need
            if (!SplitSqlForPaging(sql, out sqlCount, out string sqlSelectRemoved, out string sqlOrderBy))
                throw new Exception(string.Format(Culture, "Unable to parse SQL statement for paged query"));
            if (_rdbType == RDBType.Oracle && sqlSelectRemoved.StartsWith("*", StringComparison.OrdinalIgnoreCase))
                throw new Exception(string.Format(Culture, "Query must alias '*' when performing a paged query.\neg. select t.* from table t order by t.id"));

            // Build the SQL for the actual final result
            if (_rdbType == RDBType.SqlServer || _rdbType == RDBType.Oracle)
            {
                sqlSelectRemoved = rxOrderBy.Replace(sqlSelectRemoved, "");
                if (rxDistinct.IsMatch(sqlSelectRemoved))
                {
                    sqlSelectRemoved = "peta_inner.* FROM (SELECT " + sqlSelectRemoved + ") peta_inner";
                }
                sqlPage = string.Format(Culture, "SELECT * FROM (SELECT ROW_NUMBER() OVER ({0}) peta_rn, {1}) peta_paged WHERE peta_rn>@{2} AND peta_rn<=@{3}",
                                        sqlOrderBy ?? "ORDER BY (SELECT NULL /*peta_dual*/)", sqlSelectRemoved, args.Length, args.Length + 1);
                args = args.Concat(new object[] { skip, skip + take }).ToArray();
            }
            else
            {
                sqlPage = string.Format(Culture, "{0}\r\nLIMIT @{1} OFFSET @{2}", sql, args.Length, args.Length + 1);
                args = args.Concat(new object[] { take, skip }).ToArray();
            }

        }

        // Fetch a page
        public Page<T> Page<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            BuildPageQueries<T>((page - 1) * itemsPerPage, itemsPerPage, sql, ref args, out string sqlCount, out string sqlPage);

            // Save the one-time command time out and use it for both queries
            int saveTimeout = OneTimeCommandTimeout;

            // Setup the paged result
            var result = new Page<T>
            {
                CurrentPage = page,
                ItemsPerPage = itemsPerPage,
                TotalItems = ExecuteScalar<long>(sqlCount, args)
            };
            result.TotalPages = result.TotalItems / itemsPerPage;
            if ((result.TotalItems % itemsPerPage) != 0)
                result.TotalPages++;

            OneTimeCommandTimeout = saveTimeout;

            // Get the records
            result.Items = Fetch<T>(sqlPage, args);

            // Done
            return result;
        }

        public Page<T> Page<T>(long page, long itemsPerPage, Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return Page<T>(page, itemsPerPage, sql.SQL, sql.Arguments.ToArray());
        }

        public List<T> Fetch<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            return SkipTake<T>((page - 1) * itemsPerPage, itemsPerPage, sql, args);
        }

        public List<T> Fetch<T>(long page, long itemsPerPage, Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return SkipTake<T>((page - 1) * itemsPerPage, itemsPerPage, sql.SQL, sql.Arguments.ToArray());
        }

        public List<T> SkipTake<T>(long skip, long take, string sql, params object[] args)
        {
            BuildPageQueries<T>(skip, take, sql, ref args, out _, out string sqlPage);
            return Fetch<T>(sqlPage, args);
        }

        public List<T> SkipTake<T>(long skip, long take, Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return SkipTake<T>(skip, take, sql.SQL, sql.Arguments.ToArray());
        }

        public Dictionary<TKey, TValue> Dictionary<TKey, TValue>(Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return Dictionary<TKey, TValue>(sql.SQL, sql.Arguments.ToArray());
        }

        public Dictionary<TKey, TValue> Dictionary<TKey, TValue>(string sql, params object[] args)
        {
            var newDict = new Dictionary<TKey, TValue>();
            bool isConverterSet = false;
            Func<object, object> converter1 = x => x, converter2 = x => x;

            foreach (var line in Query<Dictionary<string, object>>(sql, args))
            {
                object key = line.ElementAt(0).Value;
                object value = line.ElementAt(1).Value;

                if (isConverterSet == false)
                {
                    converter1 = PocoData.GetConverter(ForceDateTimesToUtc, null, typeof(TKey), key.GetType()) ?? (x => x);
                    converter2 = PocoData.GetConverter(ForceDateTimesToUtc, null, typeof(TValue), value.GetType()) ?? (x => x);
                    isConverterSet = true;
                }

                var keyConverted = (TKey)Convert.ChangeType(converter1(key), typeof(TKey), Culture);

                var valueType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
                var valConv = converter2(value);
                var valConverted = valConv != null ? (TValue)Convert.ChangeType(valConv, valueType, Culture) : default(TValue);

                if (keyConverted != null)
                {
                    newDict.Add(keyConverted, valConverted);
                }
            }
            return newDict;
        }

        // Return an enumerable collection of pocos
        public IEnumerable<T> Query<T>(string sql, params object[] args)
        {
            return Query<T>(new Sql(sql, args));
        }

        public IEnumerable<T> Query<T>(Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return Query<T>(default(T), sql);
        }

        private IEnumerable<T> Query<T>(T instance, Sql sql)
        {
            var sql_ = sql.SQL;
            var args = sql.Arguments.ToArray();

            if (EnableAutoSelect)
                sql_ = AddSelectClause<T>(sql_);

            OpenSharedConnection();
            try
            {
                using (var cmd = CreateCommand(_sharedConnection, sql_, args))
                {
                    IDataReader r;
                    var pd = PocoData.ForType(typeof(T));
                    try
                    {
                        r = cmd.ExecuteReader();
                        OnExecutedCommand(cmd);
                    }
                    catch (Exception x)
                    {
                        OnException(x);
                        throw;
                    }

                    using (r)
                    {
                        var factory = pd.GetFactory(cmd.CommandText, _sharedConnection.ConnectionString, ForceDateTimesToUtc, 0, r.FieldCount, r, instance) as Func<IDataReader, T, T>;
                        while (true)
                        {
                            T poco;
                            try
                            {
                                if (!r.Read())
                                    yield break;
                                poco = factory(r, instance);
                            }
                            catch (Exception x)
                            {
                                OnException(x);
                                throw;
                            }

                            yield return poco;
                        }
                    }
                }
            }
            finally
            {
                CloseSharedConnection();
            }
        }

        // Multi Fetch
        public List<TRet> Fetch<T1, T2, TRet>(Func<T1, T2, TRet> cb, string sql, params object[] args) { return Query<T1, T2, TRet>(cb, sql, args).ToList(); }
        public List<TRet> Fetch<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, string sql, params object[] args) { return Query<T1, T2, T3, TRet>(cb, sql, args).ToList(); }
        public List<TRet> Fetch<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, string sql, params object[] args) { return Query<T1, T2, T3, T4, TRet>(cb, sql, args).ToList(); }

        // Multi Query
        public IEnumerable<TRet> Query<T1, T2, TRet>(Func<T1, T2, TRet> cb, string sql, params object[] args) { return Query<TRet>(new Type[] { typeof(T1), typeof(T2) }, cb, new Sql(sql, args)); }
        public IEnumerable<TRet> Query<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, string sql, params object[] args) { return Query<TRet>(new Type[] { typeof(T1), typeof(T2), typeof(T3) }, cb, new Sql(sql, args)); }
        public IEnumerable<TRet> Query<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, string sql, params object[] args) { return Query<TRet>(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, cb, new Sql(sql, args)); }

        // Multi Fetch (SQL builder)
        public List<TRet> Fetch<T1, T2, TRet>(Func<T1, T2, TRet> cb, Sql sql) { return Query<T1, T2, TRet>(cb, sql).ToList(); }
        public List<TRet> Fetch<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, Sql sql) { return Query<T1, T2, T3, TRet>(cb, sql).ToList(); }
        public List<TRet> Fetch<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, Sql sql) { return Query<T1, T2, T3, T4, TRet>(cb, sql).ToList(); }

        // Multi Query (SQL builder)
        public IEnumerable<TRet> Query<T1, T2, TRet>(Func<T1, T2, TRet> cb, Sql sql) { return Query<TRet>(new Type[] { typeof(T1), typeof(T2) }, cb, sql); }
        public IEnumerable<TRet> Query<T1, T2, T3, TRet>(Func<T1, T2, T3, TRet> cb, Sql sql) { return Query<TRet>(new Type[] { typeof(T1), typeof(T2), typeof(T3) }, cb, sql); }
        public IEnumerable<TRet> Query<T1, T2, T3, T4, TRet>(Func<T1, T2, T3, T4, TRet> cb, Sql sql) { return Query<TRet>(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, cb, sql); }

        // Multi Fetch (Simple)
        public List<T1> Fetch<T1, T2>(string sql, params object[] args) { return Query<T1, T2>(sql, args).ToList(); }
        public List<T1> Fetch<T1, T2, T3>(string sql, params object[] args) { return Query<T1, T2, T3>(sql, args).ToList(); }
        public List<T1> Fetch<T1, T2, T3, T4>(string sql, params object[] args) { return Query<T1, T2, T3, T4>(sql, args).ToList(); }

        // Multi Query (Simple)
        public IEnumerable<T1> Query<T1, T2>(string sql, params object[] args) { return Query<T1>(new Type[] { typeof(T1), typeof(T2) }, null, new Sql(sql, args)); }
        public IEnumerable<T1> Query<T1, T2, T3>(string sql, params object[] args) { return Query<T1>(new Type[] { typeof(T1), typeof(T2), typeof(T3) }, null, new Sql(sql, args)); }
        public IEnumerable<T1> Query<T1, T2, T3, T4>(string sql, params object[] args) { return Query<T1>(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, null, new Sql(sql, args)); }

        // Multi Fetch (Simple) (SQL builder)
        public List<T1> Fetch<T1, T2>(Sql sql) { return Query<T1, T2>(sql).ToList(); }
        public List<T1> Fetch<T1, T2, T3>(Sql sql) { return Query<T1, T2, T3>(sql).ToList(); }
        public List<T1> Fetch<T1, T2, T3, T4>(Sql sql) { return Query<T1, T2, T3, T4>(sql).ToList(); }

        // Multi Query (Simple) (SQL builder)
        public IEnumerable<T1> Query<T1, T2>(Sql sql) { return Query<T1>(new Type[] { typeof(T1), typeof(T2) }, null, sql); }
        public IEnumerable<T1> Query<T1, T2, T3>(Sql sql) { return Query<T1>(new Type[] { typeof(T1), typeof(T2), typeof(T3) }, null, sql); }
        public IEnumerable<T1> Query<T1, T2, T3, T4>(Sql sql) { return Query<T1>(new Type[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, null, sql); }

        // Automagically guess the property relationships between various POCOs and create a delegate that will set them up
        object GetAutoMapper(Type[] types)
        {
            // Build a key
            var kb = new StringBuilder();
            foreach (var t in types)
            {
                kb.Append(t.ToString());
                kb.Append(":");
            }
            var key = kb.ToString();

            // Check cache
            RWLock.EnterReadLock();
            try
            {
                if (AutoMappers.TryGetValue(key, out object mapper))
                    return mapper;
            }
            finally
            {
                RWLock.ExitReadLock();
            }

            // Create it
            RWLock.EnterWriteLock();
            try
            {
                // Try again
                if (AutoMappers.TryGetValue(key, out object mapper))
                    return mapper;

                // Create a method
                var m = new DynamicMethod("petapoco_automapper", types[0], types, true);
                var il = m.GetILGenerator();

                for (int i = 1; i < types.Length; i++)
                {
                    bool handled = false;
                    for (int j = i - 1; j >= 0; j--)
                    {
                        // Find the property
                        var candidates = types[j].GetProperties().Where(p => p.PropertyType == types[i]).Select(p => p);
                        if (!candidates.Any())
                            continue;
                        if (candidates.Count() > 1)
                            throw new InvalidOperationException(string.Format(Culture, "Can't auto join {0} as {1} has more than one property of type {0}", types[i], types[j]));

                        // Generate code
                        il.Emit(OpCodes.Ldarg_S, j);
                        il.Emit(OpCodes.Ldarg_S, i);
                        il.Emit(OpCodes.Callvirt, candidates.First().GetSetMethod(true));
                        handled = true;
                    }

                    if (!handled)
                        throw new InvalidOperationException(string.Format(Culture, "Can't auto join {0}", types[i]));
                }

                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ret);

                // Cache it
                var del = m.CreateDelegate(Expression.GetFuncType(types.Concat(types.Take(1)).ToArray()));
                AutoMappers.Add(key, del);
                return del;
            }
            finally
            {
                RWLock.ExitWriteLock();
            }
        }

        // Find the split point in a result set for two different pocos and return the poco factory for the first
        Delegate FindSplitPoint(Type typeThis, Type typeNext, string sql, IDataReader r, ref int pos)
        {
            // Last?
            if (typeNext == null)
                return PocoData.ForType(typeThis).GetFactory(sql, _sharedConnection.ConnectionString, ForceDateTimesToUtc, pos, r.FieldCount - pos, r, null);

            // Get PocoData for the two types
            PocoData pdThis = PocoData.ForType(typeThis);
            PocoData pdNext = PocoData.ForType(typeNext);

            // Find split point
            int firstColumn = pos;
            var usedColumns = new Dictionary<string, bool>();
            for (; pos < r.FieldCount; pos++)
            {
                // Split if field name has already been used, or if the field doesn't exist in current poco but does in the next
                string fieldName = r.GetName(pos);
                if (usedColumns.ContainsKey(fieldName) || (!pdThis.Columns.ContainsKey(fieldName) && pdNext.Columns.ContainsKey(fieldName)))
                {
                    return pdThis.GetFactory(sql, _sharedConnection.ConnectionString, ForceDateTimesToUtc, firstColumn, pos - firstColumn, r, null);
                }
                usedColumns.Add(fieldName, true);
            }

            throw new InvalidOperationException(string.Format(Culture, "Couldn't find split point between {0} and {1}", typeThis, typeNext));
        }

        // Instance data used by the Multipoco factory delegate - essentially a list of the nested poco factories to call
        class MultiPocoFactory
        {
            public List<Delegate> m_Delegates;
            public Delegate GetItem(int index) { return m_Delegates[index]; }
        }

        // Create a multi-poco factory
        Func<IDataReader, object, TRet> CreateMultiPocoFactory<TRet>(Type[] types, string sql, IDataReader r)
        {
            var m = new DynamicMethod("petapoco_multipoco_factory", typeof(TRet), new Type[] { typeof(MultiPocoFactory), typeof(IDataReader), typeof(object) }, typeof(MultiPocoFactory));
            var il = m.GetILGenerator();

            // Load the callback
            il.Emit(OpCodes.Ldarg_2);

            // Call each delegate
            var dels = new List<Delegate>();
            int pos = 0;
            for (int i = 0; i < types.Length; i++)
            {
                // Add to list of delegates to call
                var del = FindSplitPoint(types[i], i + 1 < types.Length ? types[i + 1] : null, sql, r, ref pos);
                dels.Add(del);

                // Get the delegate
                il.Emit(OpCodes.Ldarg_0);                                                   // callback,this
                il.Emit(OpCodes.Ldc_I4, i);                                                 // callback,this,Index
                il.Emit(OpCodes.Callvirt, typeof(MultiPocoFactory).GetMethod("GetItem"));   // callback,Delegate
                il.Emit(OpCodes.Ldarg_1);                                                   // callback,delegate, datareader
                il.Emit(OpCodes.Ldnull);                                                    // callback,delegate, datareader,null

                // Call Invoke
                var tDelInvoke = del.GetType().GetMethod("Invoke");
                il.Emit(OpCodes.Callvirt, tDelInvoke);                                      // Poco left on stack
            }

            // By now we should have the callback and the N pocos all on the stack.  Call the callback and we're done
            il.Emit(OpCodes.Callvirt, Expression.GetFuncType(types.Concat(new Type[] { typeof(TRet) }).ToArray()).GetMethod("Invoke"));
            il.Emit(OpCodes.Ret);

            // Finish up
            return (Func<IDataReader, object, TRet>)m.CreateDelegate(typeof(Func<IDataReader, object, TRet>), new MultiPocoFactory() { m_Delegates = dels });
        }

        // Various cached stuff
        static readonly Dictionary<string, object> MultiPocoFactories = new Dictionary<string, object>();
        static readonly Dictionary<string, object> AutoMappers = new Dictionary<string, object>();
        static readonly System.Threading.ReaderWriterLockSlim RWLock = new System.Threading.ReaderWriterLockSlim();

        // Get (or create) the multi-poco factory for a query
        Func<IDataReader, object, TRet> GetMultiPocoFactory<TRet>(Type[] types, string sql, IDataReader r)
        {
            // Build a key string  (this is crap, should address this at some point)
            var kb = new StringBuilder();
            kb.Append(typeof(TRet).ToString());
            kb.Append(":");
            foreach (var t in types)
            {
                kb.Append(":");
                kb.Append(t.ToString());
            }
            kb.Append(":"); kb.Append(_sharedConnection.ConnectionString);
            kb.Append(":"); kb.Append(ForceDateTimesToUtc);
            kb.Append(":"); kb.Append(sql);
            string key = kb.ToString();

            // Check cache
            RWLock.EnterReadLock();
            try
            {
                if (MultiPocoFactories.TryGetValue(key, out object oFactory))
                    return (Func<IDataReader, object, TRet>)oFactory;
            }
            finally
            {
                RWLock.ExitReadLock();
            }

            // Cache it
            RWLock.EnterWriteLock();
            try
            {
                // Check again
                if (MultiPocoFactories.TryGetValue(key, out object oFactory))
                    return (Func<IDataReader, object, TRet>)oFactory;

                // Create the factory
                var Factory = CreateMultiPocoFactory<TRet>(types, sql, r);

                MultiPocoFactories.Add(key, Factory);
                return Factory;
            }
            finally
            {
                RWLock.ExitWriteLock();
            }

        }

        // Actual implementation of the multi-poco query
        public IEnumerable<TRet> Query<TRet>(Type[] types, object cb, Sql sql)
        {
            if (types == null) { throw new ArgumentNullException(nameof(types)); }
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }

            OpenSharedConnection();
            try
            {
                using (var cmd = CreateCommand(_sharedConnection, sql.SQL, sql.Arguments.ToArray()))
                {
                    IDataReader r;
                    try
                    {
                        r = cmd.ExecuteReader();
                        OnExecutedCommand(cmd);
                    }
                    catch (Exception x)
                    {
                        OnException(x);
                        throw;
                    }
                    var factory = GetMultiPocoFactory<TRet>(types, sql.SQL, r);
                    if (cb == null)
                        cb = GetAutoMapper(types.ToArray());
                    bool bNeedTerminator = false;
                    using (r)
                    {
                        while (true)
                        {
                            TRet poco;
                            try
                            {
                                if (!r.Read())
                                    break;
                                poco = factory(r, cb);
                            }
                            catch (Exception x)
                            {
                                OnException(x);
                                throw;
                            }

                            if (poco != null)
                                yield return poco;
                            else
                                bNeedTerminator = true;
                        }
                        if (bNeedTerminator)
                        {
                            var poco = (TRet)(cb as Delegate).DynamicInvoke(new object[types.Length]);
                            if (poco != null)
                                yield return poco;
                            else
                                yield break;
                        }
                    }
                }
            }
            finally
            {
                CloseSharedConnection();
            }
        }

        #region Tupple methods
        public TRet FetchMultiple<T1, T2, TRet>(Func<List<T1>, List<T2>, TRet> cb, string sql, params object[] args)
        {
            return FetchMultiple<T1, T2, DontMap, DontMap, TRet>(new[] { typeof(T1), typeof(T2) }, cb, new Sql(sql, args));
        }
        public TRet FetchMultiple<T1, T2, T3, TRet>(Func<List<T1>, List<T2>, List<T3>, TRet> cb, string sql, params object[] args)
        {
            return FetchMultiple<T1, T2, T3, DontMap, TRet>(new[] { typeof(T1), typeof(T2), typeof(T3) }, cb, new Sql(sql, args));
        }
        public TRet FetchMultiple<T1, T2, T3, T4, TRet>(Func<List<T1>, List<T2>, List<T3>, List<T4>, TRet> cb, string sql, params object[] args)
        {
            return FetchMultiple<T1, T2, T3, T4, TRet>(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, cb, new Sql(sql, args));
        }
        public TRet FetchMultiple<T1, T2, TRet>(Func<List<T1>, List<T2>, TRet> cb, Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return FetchMultiple<T1, T2, DontMap, DontMap, TRet>(new[] { typeof(T1), typeof(T2) }, cb, sql);
        }
        public TRet FetchMultiple<T1, T2, T3, TRet>(Func<List<T1>, List<T2>, List<T3>, TRet> cb, Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return FetchMultiple<T1, T2, T3, DontMap, TRet>(new[] { typeof(T1), typeof(T2), typeof(T3) }, cb, sql);
        }
        public TRet FetchMultiple<T1, T2, T3, T4, TRet>(Func<List<T1>, List<T2>, List<T3>, List<T4>, TRet> cb, Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return FetchMultiple<T1, T2, T3, T4, TRet>(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, cb, sql);
        }

        public Tuple<List<T1>, List<T2>> FetchMultiple<T1, T2>(string sql, params object[] args)
        {
            return FetchMultiple<T1, T2, DontMap, DontMap, Tuple<List<T1>, List<T2>>>(new[] { typeof(T1), typeof(T2) }, new Func<List<T1>, List<T2>, Tuple<List<T1>, List<T2>>>((y, z) => new Tuple<List<T1>, List<T2>>(y, z)), new Sql(sql, args));
        }
        public Tuple<List<T1>, List<T2>, List<T3>> FetchMultiple<T1, T2, T3>(string sql, params object[] args)
        {
            return FetchMultiple<T1, T2, T3, DontMap, Tuple<List<T1>, List<T2>, List<T3>>>(new[] { typeof(T1), typeof(T2), typeof(T3) }, new Func<List<T1>, List<T2>, List<T3>, Tuple<List<T1>, List<T2>, List<T3>>>((x, y, z) => new Tuple<List<T1>, List<T2>, List<T3>>(x, y, z)), new Sql(sql, args));
        }
        public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> FetchMultiple<T1, T2, T3, T4>(string sql, params object[] args)
        {
            return FetchMultiple<T1, T2, T3, T4, Tuple<List<T1>, List<T2>, List<T3>, List<T4>>>(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, new Func<List<T1>, List<T2>, List<T3>, List<T4>, Tuple<List<T1>, List<T2>, List<T3>, List<T4>>>((w, x, y, z) => new Tuple<List<T1>, List<T2>, List<T3>, List<T4>>(w, x, y, z)), new Sql(sql, args));
        }
        public Tuple<List<T1>, List<T2>> FetchMultiple<T1, T2>(Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return FetchMultiple<T1, T2, DontMap, DontMap, Tuple<List<T1>, List<T2>>>(new[] { typeof(T1), typeof(T2) }, new Func<List<T1>, List<T2>, Tuple<List<T1>, List<T2>>>((y, z) => new Tuple<List<T1>, List<T2>>(y, z)), sql);
        }
        public Tuple<List<T1>, List<T2>, List<T3>> FetchMultiple<T1, T2, T3>(Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return FetchMultiple<T1, T2, T3, DontMap, Tuple<List<T1>, List<T2>, List<T3>>>(new[] { typeof(T1), typeof(T2), typeof(T3) }, new Func<List<T1>, List<T2>, List<T3>, Tuple<List<T1>, List<T2>, List<T3>>>((x, y, z) => new Tuple<List<T1>, List<T2>, List<T3>>(x, y, z)), sql);
        }
        public Tuple<List<T1>, List<T2>, List<T3>, List<T4>> FetchMultiple<T1, T2, T3, T4>(Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return FetchMultiple<T1, T2, T3, T4, Tuple<List<T1>, List<T2>, List<T3>, List<T4>>>(new[] { typeof(T1), typeof(T2), typeof(T3), typeof(T4) }, new Func<List<T1>, List<T2>, List<T3>, List<T4>, Tuple<List<T1>, List<T2>, List<T3>, List<T4>>>((w, x, y, z) => new Tuple<List<T1>, List<T2>, List<T3>, List<T4>>(w, x, y, z)), sql);
        }

        public class DontMap { }

        #endregion

        // Actual implementation of the multi query
        private TRet FetchMultiple<T1, T2, T3, T4, TRet>(Type[] types, object cb, Sql sql)
        {
            var sql_ = sql.SQL;
            var args = sql.Arguments.ToArray();

            OpenSharedConnection();
            try
            {
                using (var cmd = CreateCommand(_sharedConnection, sql_, args))
                {
                    IDataReader r;
                    try
                    {
                        r = cmd.ExecuteReader();
                        OnExecutedCommand(cmd);
                    }
                    catch (Exception x)
                    {
                        OnException(x);
                        throw;
                    }

                    using (r)
                    {
                        var typeIndex = 1;
                        var list1 = new List<T1>();
                        var list2 = new List<T2>();
                        var list3 = new List<T3>();
                        var list4 = new List<T4>();
                        do
                        {
                            if (typeIndex > types.Length)
                                break;

                            var pd = PocoData.ForType(types[typeIndex - 1]);
                            var factory = pd.GetFactory(cmd.CommandText, _sharedConnection.ConnectionString, ForceDateTimesToUtc, 0, r.FieldCount, r, null);

                            while (true)
                            {
                                try
                                {
                                    if (!r.Read())
                                        break;

                                    switch (typeIndex)
                                    {
                                        case 1:
                                            list1.Add(((Func<IDataReader, T1, T1>)factory)(r, default(T1)));
                                            break;
                                        case 2:
                                            list2.Add(((Func<IDataReader, T2, T2>)factory)(r, default(T2)));
                                            break;
                                        case 3:
                                            list3.Add(((Func<IDataReader, T3, T3>)factory)(r, default(T3)));
                                            break;
                                        case 4:
                                            list4.Add(((Func<IDataReader, T4, T4>)factory)(r, default(T4)));
                                            break;
                                    }
                                }
                                catch (Exception x)
                                {
                                    OnException(x);
                                    throw;
                                }
                            }

                            typeIndex++;
                        } while (r.NextResult());

                        switch (types.Length)
                        {
                            case 2:
                                return ((Func<List<T1>, List<T2>, TRet>)cb)(list1, list2);
                            case 3:
                                return ((Func<List<T1>, List<T2>, List<T3>, TRet>)cb)(list1, list2, list3);
                            case 4:
                                return ((Func<List<T1>, List<T2>, List<T3>, List<T4>, TRet>)cb)(list1, list2, list3, list4);
                        }

                        return default(TRet);
                    }
                }
            }
            finally
            {
                CloseSharedConnection();
            }
        }

        public bool Exists<T>(object primaryKey)
        {
            var index = 0;
            var primaryKeyValuePairs = GetPrimaryKeyValues(PocoData.ForType(typeof(T)).TableInfo.PrimaryKey, primaryKey);
            return FirstOrDefault<T>(string.Format(Culture, "WHERE {0}", BuildPrimaryKeySql(primaryKeyValuePairs, ref index)), primaryKeyValuePairs.Select(x => x.Value).ToArray()) != null;
        }
        public T SingleById<T>(object primaryKey)
        {
            var index = 0;
            var primaryKeyValuePairs = GetPrimaryKeyValues(PocoData.ForType(typeof(T)).TableInfo.PrimaryKey, primaryKey);
            return Query<T>(string.Format(Culture, "WHERE {0}", BuildPrimaryKeySql(primaryKeyValuePairs, ref index)), primaryKeyValuePairs.Select(x => x.Value).ToArray()).Single();
        }
        public T SingleOrDefaultById<T>(object primaryKey)
        {
            var index = 0;
            var primaryKeyValuePairs = GetPrimaryKeyValues(PocoData.ForType(typeof(T)).TableInfo.PrimaryKey, primaryKey);
            return SingleOrDefault<T>(string.Format(Culture, "WHERE {0}", BuildPrimaryKeySql(primaryKeyValuePairs, ref index)), primaryKeyValuePairs.Select(x => x.Value).ToArray());
        }
        public T SingleOrDefault<T>(string sql, params object[] args)
        {
            return Query<T>(sql, args).SingleOrDefault();
        }
        public T First<T>(string sql, params object[] args)
        {
            return Query<T>(sql, args).First();
        }
        public T FirstOrDefault<T>(string sql, params object[] args)
        {
            return Query<T>(sql, args).FirstOrDefault();
        }
        public T SingleOrDefault<T>(Sql sql)
        {
            return Query<T>(sql).SingleOrDefault();
        }
        public T First<T>(Sql sql)
        {
            return Query<T>(sql).First();
        }
        public T FirstOrDefault<T>(Sql sql)
        {
            return Query<T>(sql).FirstOrDefault();
        }
        public string EscapeTableName(string str)
        {
            if (str == null) { throw new ArgumentNullException(nameof(str)); }

            // Assume table names with "dot" are already escaped
            return str.IndexOf('.') >= 0 ? str : EscapeSqlIdentifier(str);
        }

        public string EscapeSqlIdentifier(string str)
        {
            if (str == null) { throw new ArgumentNullException(nameof(str)); }

            switch (_rdbType)
            {
                case RDBType.MySql:
                    return string.Format(Culture, "`{0}`", str);

                case RDBType.PostgreSql:
                    return string.Format(Culture, "\"{0}\"", str);

                case RDBType.Oracle:
                    return string.Format(Culture, "\"{0}\"", str.ToUpperInvariant());

                default:
                    return string.Format(Culture, "[{0}]", str);
            }
        }

        // Insert an annotated poco object
        public object Insert<T>(T poco) where T : IPetaPocoRecord<T>
        {
            var pd = PocoData.ForType(poco.GetType());
            return this.InsertExecute(pd.TableInfo.TableName, pd.TableInfo.PrimaryKey, pd.TableInfo.AutoIncrement, poco);
        }

        private object InsertExecute(string tableName, string primaryKeyName, bool autoIncrement, object poco)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    var pd = PocoData.ForObject(poco, primaryKeyName);
                    var names = new List<string>();
                    var values = new List<string>();
                    var rawvalues = new List<object>();
                    var index = 0;

                    foreach (var i in pd.Columns)
                    {
                        // Don't insert result columns
                        if (i.Value.ResultColumn)
                            continue;

                        // Don't insert the primary key (except under oracle where we need bring in the next sequence value)
                        if (autoIncrement
                            && primaryKeyName != null
                            && string.Compare(i.Key, primaryKeyName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (_rdbType == RDBType.Oracle && !string.IsNullOrEmpty(pd.TableInfo.SequenceName))
                            {
                                names.Add(i.Key);
                                values.Add(string.Format(Culture, "{0}.nextval", pd.TableInfo.SequenceName));
                            }
                            continue;
                        }

                        names.Add(EscapeSqlIdentifier(i.Key));
                        values.Add(string.Format(Culture, "{0}{1}", ParamPrefix, index++));

                        rawvalues.Add(i.Value.GetValue(poco));
                    }

                    using (var cmd = CreateCommand(_sharedConnection, ""))
                    {
                        var sql = string.Empty;
                        if (names.Count > 0 || _rdbType == RDBType.MySql)
                        {
                            sql = string.Format(Culture, "INSERT INTO {0} ({1}) VALUES ({2})", EscapeTableName(tableName), string.Join(",", names.ToArray()), string.Join(",", values.ToArray()));
                        }
                        else
                        {
                            sql = string.Format(Culture, "INSERT INTO {0} DEFAULT VALUES", EscapeTableName(tableName));
                        }

                        cmd.CommandText = sql;
                        rawvalues.ForEach(x => AddParam(cmd, x, ParamPrefix));

                        object id;

                        if (!autoIncrement)
                        {
                            DoPreExecute(cmd);
                            cmd.ExecuteNonQuery();
                            OnExecutedCommand(cmd);
                            id = -1;
                        }
                        else
                        {

                            switch (_rdbType)
                            {
                                case RDBType.SqlServer:
                                    cmd.CommandText += ";\r\nSELECT SCOPE_IDENTITY() AS NewID;";
                                    DoPreExecute(cmd);
                                    id = cmd.ExecuteScalar();
                                    OnExecutedCommand(cmd);
                                    break;
                                case RDBType.MySql:
                                    cmd.CommandText += ";\r\nSELECT LAST_INSERT_ID();";
                                    DoPreExecute(cmd);
                                    id = cmd.ExecuteScalar();
                                    OnExecutedCommand(cmd);
                                    break;
                                case RDBType.PostgreSql:
                                    if (primaryKeyName != null)
                                    {
                                        cmd.CommandText += string.Format(Culture, " returning {0} as NewID", EscapeSqlIdentifier(primaryKeyName));
                                        DoPreExecute(cmd);
                                        id = cmd.ExecuteScalar();
                                    }
                                    else
                                    {
                                        id = -1;
                                        DoPreExecute(cmd);
                                        cmd.ExecuteNonQuery();
                                    }
                                    OnExecutedCommand(cmd);
                                    break;
                                case RDBType.Oracle:
                                    if (primaryKeyName != null)
                                    {
                                        cmd.CommandText += string.Format(Culture, " returning {0} into :newid", EscapeSqlIdentifier(primaryKeyName));
                                        var param = cmd.CreateParameter();
                                        param.ParameterName = ":newid";
                                        param.Value = DBNull.Value;
                                        param.Direction = ParameterDirection.ReturnValue;
                                        param.DbType = DbType.Int64;
                                        cmd.Parameters.Add(param);
                                        DoPreExecute(cmd);
                                        cmd.ExecuteNonQuery();
                                        id = param.Value;
                                    }
                                    else
                                    {
                                        id = -1;
                                        DoPreExecute(cmd);
                                        cmd.ExecuteNonQuery();
                                    }
                                    OnExecutedCommand(cmd);
                                    break;
                                case RDBType.SQLite:
                                    if (primaryKeyName != null)
                                    {
                                        cmd.CommandText += ";\r\nSELECT last_insert_rowid();";
                                        DoPreExecute(cmd);
                                        id = cmd.ExecuteScalar();
                                    }
                                    else
                                    {
                                        id = -1;
                                        DoPreExecute(cmd);
                                        cmd.ExecuteNonQuery();
                                    }
                                    OnExecutedCommand(cmd);
                                    break;
                                default:
                                    cmd.CommandText += ";\r\nSELECT @@IDENTITY AS NewID;";
                                    DoPreExecute(cmd);
                                    id = cmd.ExecuteScalar();
                                    OnExecutedCommand(cmd);
                                    break;
                            }

                            // Assign the ID back to the primary key property
                            if (primaryKeyName != null)
                            {
                                if (pd.Columns.TryGetValue(primaryKeyName, out PocoColumn pc))
                                {
                                    pc.SetValue(poco, pc.ChangeType(id));
                                }
                            }
                        }

                        return id;
                    }
                }
                finally
                {
                    CloseSharedConnection();
                }
            }
            catch (Exception x)
            {
                OnException(x);
                throw;
            }
        }

        /// <summary>
        /// Returns primary key conditions
        /// </summary>
        /// <param name="primaryKeyValuePair"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[
        ///     Dictionary<string, object> keys = {{ "Key01", "123" }, { "Key02", 456 }};
        ///     int i = 0;
        ///     
        ///     BuildPrimaryKeySql(keys, ref i);
        ///       -> "\"Key01\" = @0 AND \"Key02\" = @1"
        /// ]]>
        /// </example>
        private string BuildPrimaryKeySql(Dictionary<string, object> primaryKeyValuePair, ref int index)
        {
            var tempIndex = index;
            index += primaryKeyValuePair.Count;
            return string.Join(" AND ", primaryKeyValuePair.Select((x, i) => string.Format(Culture, "{0} = @{1}", EscapeSqlIdentifier(x.Key), tempIndex + i)).ToArray());
        }

        /// <summary>
        /// Returns the specified primary key value
        /// </summary>
        /// <param name="primaryKeyName"></param>
        /// <param name="primaryKeyValue"></param>
        /// <returns></returns>
        /// <example>
        /// <![CDATA[
        ///     // キーが1つなら「値」を渡す
        ///     GetPrimaryKeyValues("Key01", "123");
        ///       -> [{ "Key01", "123" }]
        /// 
        ///     // キーが2つ以上なら匿名型で渡す
        ///     GetPrimaryKeyValues("Key01,Key02", new { Key01 = "123", Key02 = 456, });
        ///       -> [{ "Key01", "123" }, { "Key02", 456 }]
        /// 
        ///     // キーが1つなのに匿名型を渡すと正しくない
        ///     GetPrimaryKeyValues("Key01", new { Key01 = "123" });
        ///       -> これは間違いなので注意！・・・[{ "Key01",  [{"Key01", "123"}]}]
        /// ]]>
        /// </example>
        private static Dictionary<string, object> GetPrimaryKeyValues(string primaryKeyName, object primaryKeyValue)
        {
            Dictionary<string, object> kvs;

            var multiplePrimaryKeysNames = primaryKeyName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            if (primaryKeyValue != null)
            {
                if (multiplePrimaryKeysNames.Length == 1)
                    kvs = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase) { { primaryKeyName, primaryKeyValue } };
                else
                    kvs = multiplePrimaryKeysNames.ToDictionary(x => x,
                        x => primaryKeyValue.GetType().GetProperties()
                            .Where(y => string.Equals(x, y.Name, StringComparison.OrdinalIgnoreCase))
                            .Single().GetValue(primaryKeyValue, null), StringComparer.OrdinalIgnoreCase);
            }
            else
            {
                kvs = multiplePrimaryKeysNames.ToDictionary(x => x, x => (object)null, StringComparer.OrdinalIgnoreCase);
            }
            return kvs;
        }

        public int Update<T>(T poco) where T : IPetaPocoRecord<T>
        {
            return Update<T>(poco, null, null);
        }

        public int Update<T>(T poco, object primaryKey) where T : IPetaPocoRecord<T>
        {
            return Update<T>(poco, primaryKey, null);
        }

        public int Update<T>(T poco, IEnumerable<string> columns) where T : IPetaPocoRecord<T>
        {
            return Update(poco, null, columns);
        }

        /// <summary>
        /// Update a record with values from a poco.  primary key value can be either supplied or read from the poco
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco"></param>
        /// <param name="primaryKey"></param>
        /// <param name="columns"></param>
        /// <returns>Count of updated records.</returns>
        /// <example>
        /// <![CDATA[
        ///     var rec = new Database.THoge
        ///     {
        ///         UserName = "Taro Nanigashi",
        ///         BirthDay = DateTime.Parse("1970/04/04"),
        ///     };
        ///     var pk = new {
        ///         Key01 = 123,
        ///         Key02 = 456,
        ///     };
        ///     int cnt = db.Update(pk);
        /// ]]>
        /// </example>
        public int Update<T>(T poco, object primaryKey, IEnumerable<string> columns) where T : IPetaPocoRecord<T>
        {
            if (poco == null) { throw new ArgumentNullException(nameof(poco)); }
            if (columns != null && !columns.Any()) { return 0; }

            var pd = PocoData.ForType(typeof(T));
            string tableName = pd.TableInfo.TableName;
            string pkName = pd.TableInfo.PrimaryKey;

            Dictionary<string, object> primaryKeyValuePairs;
            if (primaryKey == null)
            {
                primaryKeyValuePairs = pkName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToDictionary(x => x, x => (object)null);
            }
            else
            {
                primaryKeyValuePairs = GetPrimaryKeyValues(pkName, primaryKey);
            }

            if (columns == null)
            {
                columns = poco.GetModifiedColumns();
            }

            var sb = new StringBuilder();
            var index = 0;
            var rawvalues = new List<object>();

            foreach (var col in pd.Columns)
            {
                // Don't update the primary key, but grab the value if we don't have it
                if (primaryKey == null && primaryKeyValuePairs.ContainsKey(col.Key))
                {
                    primaryKeyValuePairs[col.Key] = col.Value.GetValue(poco);
                    continue;
                }

                // Dont update result only columns
                if (col.Value.ResultColumn)
                {
                    continue;
                }

                if (columns != null
                    && !columns.Contains(col.Value.ColumnName, StringComparer.OrdinalIgnoreCase))
                {
                    continue;
                }

                object value = col.Value.GetValue(poco);

                // Build the sql
                if (index > 0)
                {
                    sb.Append(", ");
                }
                sb.AppendFormat(Culture, "{0} = @{1}", EscapeSqlIdentifier(col.Key), index++);

                rawvalues.Add(value);
            }

            if (columns != null && columns.Any() && sb.Length == 0)
            {
                throw new ArgumentException(string.Format(Culture, "There were no columns in the columns list that matched your table"), nameof(columns));
            }

            var sql = string.Format(Culture, "UPDATE {0} SET {1} WHERE {2}", EscapeTableName(tableName), sb, BuildPrimaryKeySql(primaryKeyValuePairs, ref index));

            rawvalues.AddRange(primaryKeyValuePairs.Select(keyValue => keyValue.Value));

            return Execute(sql, rawvalues.ToArray());
        }

        public int Update<T>(string sql, params object[] args) where T : IPetaPocoRecord<T>
        {
            var pd = PocoData.ForType(typeof(T));
            return Execute(string.Format(Culture, "UPDATE {0} {1}", EscapeTableName(pd.TableInfo.TableName), sql), args);
        }

        public int Update<T>(Sql sql) where T : IPetaPocoRecord<T>
        {
            var pd = PocoData.ForType(typeof(T));
            return Execute(new Sql(string.Format(Culture, "UPDATE {0}", EscapeTableName(pd.TableInfo.TableName))).Append(sql));
        }

        public int Delete<T>(string sql, params object[] args) where T : IPetaPocoRecord<T>
        {
            var pd = PocoData.ForType(typeof(T));
            return Execute(string.Format(Culture, "DELETE FROM {0} {1}", EscapeTableName(pd.TableInfo.TableName), sql), args);
        }

        public int Delete<T>(Sql sql) where T : IPetaPocoRecord<T>
        {
            var pd = PocoData.ForType(typeof(T));
            return Execute(new Sql(string.Format(Culture, "DELETE FROM {0}", EscapeTableName(pd.TableInfo.TableName))).Append(sql));
        }

        public int Delete<T>(T poco) where T : IPetaPocoRecord<T>
        {
            if (poco == null) { throw new ArgumentNullException(nameof(poco)); }

            var pd = PocoData.ForType(typeof(T));
            string tableName = pd.TableInfo.TableName;
            string pkName = pd.TableInfo.PrimaryKey;

            var primaryKeyValuePairs = pkName.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToDictionary(x => x, x => (object)null);

            foreach (var key in primaryKeyValuePairs.Keys.ToArray())
            {
                primaryKeyValuePairs[key] = pd.Columns.Single(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase)).Value.GetValue(poco);
            }

            var index = 0;
            var sql = string.Format(Culture, "DELETE FROM {0} WHERE {1}", tableName, BuildPrimaryKeySql(primaryKeyValuePairs, ref index));
            return Execute(sql, primaryKeyValuePairs.Select(x => x.Value).ToArray());
        }

        void DoPreExecute(IDbCommand cmd)
        {
            // Setup command timeout
            if (OneTimeCommandTimeout != 0)
            {
                cmd.CommandTimeout = OneTimeCommandTimeout;
                OneTimeCommandTimeout = 0;
            }
            else if (CommandTimeout != 0)
            {
                cmd.CommandTimeout = CommandTimeout;
            }

            // Call hook
            OnExecutingCommand(cmd);

            // Save it
            _lastSql = cmd.CommandText;
            _lastArgs = (from IDataParameter parameter in cmd.Parameters select parameter.Value).ToArray();
        }

        public string LastSQL { get { return _lastSql; } }
        public ReadOnlyCollection<object> LastArgs { get { return new ReadOnlyCollection<object>(_lastArgs.ToList()); } }
        public string LastCommand
        {
            get { return FormatCommand(_lastSql, _lastArgs); }
        }

        public static readonly IFormatProvider Culture = System.Globalization.CultureInfo.InvariantCulture;

        public string FormatCommand(IDbCommand cmd)
        {
            if (cmd == null) { throw new ArgumentNullException(nameof(cmd)); }
            return FormatCommand(cmd.CommandText, (from IDataParameter parameter in cmd.Parameters select parameter.Value).ToArray());
        }

        public string FormatCommand(string sql, object[] args)
        {
            var sb = new StringBuilder();
            if (sql == null)
                return "";
            sb.Append(sql);
            if (args != null && args.Length > 0)
            {
                sb.Append("\r\n");
                for (int i = 0; i < args.Length; i++)
                {
                    sb.AppendFormat(Culture, "\t -> {0}{1} [{2}] = \"{3}\"\r\n", ParamPrefix, i, args[i].GetType().Name, args[i]);
                }
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public class PocoColumn
        {
            public string ColumnName;
            public PropertyInfo PropertyInfo;
            public bool ResultColumn;
            public virtual void SetValue(object target, object val) { PropertyInfo.SetValue(target, val, null); }
            public virtual object GetValue(object target) { return PropertyInfo.GetValue(target, null); }
            public virtual object ChangeType(object val) { return Convert.ChangeType(val, PropertyInfo.PropertyType, Culture); }
        }

        public class ExpandoColumn : PocoColumn
        {
            public override void SetValue(object target, object val)
            {
                if (target == null) { throw new ArgumentNullException(nameof(target)); }
                ((IDictionary<string, object>)target)[ColumnName] = val;
            }
            public override object GetValue(object target)
            {
                if (target == null) { throw new ArgumentNullException(nameof(target)); }
                ((IDictionary<string, object>)target).TryGetValue(ColumnName, out object val);
                return val;
            }
            public override object ChangeType(object val) { return val; }
        }

        public static readonly Func<Type, PocoData> PocoDataFactory = type => new PocoData(type);
        public class PocoData
        {
            static readonly EnumMapper EnumMapper = new EnumMapper();

            public static PocoData ForObject(object o, string primaryKeyName)
            {
                if (o == null) { throw new ArgumentNullException(nameof(o)); }

                var t = o.GetType();
                if (t == typeof(System.Dynamic.ExpandoObject))
                {
                    var pd = new PocoData
                    {
                        TableInfo = new TableInfo(),
                        Columns = new Dictionary<string, PocoColumn>(StringComparer.OrdinalIgnoreCase)
                        {
                            {
                                primaryKeyName, new ExpandoColumn() { ColumnName = primaryKeyName }
                            }
                        }
                    };
                    pd.TableInfo.PrimaryKey = primaryKeyName;
                    pd.TableInfo.AutoIncrement = true;
                    foreach (var col in ((IDictionary<string, object>)o).Keys)
                    {
                        if (col != primaryKeyName)
                            pd.Columns.Add(col, new ExpandoColumn() { ColumnName = col });
                    }
                    return pd;
                }
                else
                {
                    return ForType(t);
                }
            }
            static readonly System.Threading.ReaderWriterLockSlim RWLock = new System.Threading.ReaderWriterLockSlim();
            public static PocoData ForType(Type t)
            {
                if (t == typeof(System.Dynamic.ExpandoObject))
                {
                    throw new InvalidOperationException(string.Format(Culture, "Can't use dynamic types with this method"));
                }

                // Check cache
                RWLock.EnterReadLock();
                PocoData pd;
                try
                {
                    if (m_PocoDatas.TryGetValue(t, out pd))
                        return pd;
                }
                finally
                {
                    RWLock.ExitReadLock();
                }

                // Cache it
                RWLock.EnterWriteLock();
                try
                {
                    // Check again
                    if (m_PocoDatas.TryGetValue(t, out pd))
                        return pd;

                    // Create it
                    pd = PocoDataFactory(t);
                    m_PocoDatas.Add(t, pd);
                }
                finally
                {
                    RWLock.ExitWriteLock();
                }

                return pd;
            }

            public PocoData()
            {
            }

            public PocoData(Type t)
            {
                type = t ?? throw new ArgumentNullException(nameof(t));
                TableInfo = new TableInfo();

                // Get the table name
                var a = t.GetCustomAttributes(typeof(TableNameAttribute), true);
                TableInfo.TableName = a.Length == 0 ? t.Name : (a[0] as TableNameAttribute).Value;

                // Get the primary key
                a = t.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
                TableInfo.PrimaryKey = a.Length == 0 ? "ID" : (a[0] as PrimaryKeyAttribute).Value;
                TableInfo.SequenceName = a.Length == 0 ? null : (a[0] as PrimaryKeyAttribute).SequenceName;
                TableInfo.AutoIncrement = a.Length != 0 && (a[0] as PrimaryKeyAttribute).AutoIncrement;

                // Set autoincrement false if primary key has multiple columns
                TableInfo.AutoIncrement = TableInfo.AutoIncrement ? !TableInfo.PrimaryKey.Contains(',') : TableInfo.AutoIncrement;

                // Call column mapper
                if (Database.Mapper != null)
                {
                    Database.Mapper.GetTableInfo(t, TableInfo);
                }

                // Work out bound properties
                bool isExplicit = t.GetCustomAttributes(typeof(ExplicitColumnsAttribute), true).Length > 0;
                Columns = new Dictionary<string, PocoColumn>(StringComparer.OrdinalIgnoreCase);
                foreach (var pi in t.GetProperties())
                {
                    // Work out if properties is to be included
                    var colAttrs = pi.GetCustomAttributes(typeof(ColumnAttribute), true);
                    if (isExplicit)
                    {
                        // When TableInfo has ExplicitColumnsAttribute, must column attributes.
                        if (colAttrs.Length == 0)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        // When TableInfo has NOT ExplicitColumnsAttribute, all public columns mapping
                        if (pi.GetCustomAttributes(typeof(IgnoreAttribute), true).Length > 0)
                        {
                            continue;
                        }
                    }

                    var pc = new PocoColumn
                    {
                        PropertyInfo = pi
                    };

                    // Work out the DB column name
                    if (colAttrs.Length > 0)
                    {
                        var colattr = (ColumnAttribute)colAttrs[0];
                        pc.ColumnName = colattr.Name;
                        if ((colattr as ResultColumnAttribute) != null)
                        {
                            pc.ResultColumn = true;
                        }
                    }
                    if (pc.ColumnName == null)
                    {
                        pc.ColumnName = pi.Name;
                        if (Database.Mapper != null
                            && !Database.Mapper.MapPropertyToColumn(pi, ref pc.ColumnName, ref pc.ResultColumn))
                        {
                            continue;
                        }
                    }

                    // Store it
                    Columns.Add(pc.ColumnName, pc);
                }

                // Build column list for automatic select
                QueryColumns = new ReadOnlyCollection<string>(Columns.Select(c => c.Key).ToArray());

            }

            static bool IsIntegralType(Type t)
            {
                var tc = Type.GetTypeCode(t);
                return tc >= TypeCode.SByte && tc <= TypeCode.UInt64;
            }

            static object GetDefault(Type type)
            {
                if (type.IsValueType)
                {
                    return Activator.CreateInstance(type);
                }
                return null;
            }

            // Create factory function that can convert a IDataReader record into a POCO
            public Delegate GetFactory(string sql, string connString, bool ForceDateTimesToUtc, int firstColumn, int countColumns, IDataReader r, object instance)
            {
                if (r == null) { throw new ArgumentNullException(nameof(r)); }

                // Check cache
                var key = string.Format(Culture, "{0}:{1}:{2}:{3}:{4}:{5}",
                                    sql,
                                    connString,
                                    ForceDateTimesToUtc,
                                    firstColumn,
                                    countColumns,
                                    instance != GetDefault(type));
                RWLock.EnterReadLock();
                try
                {
                    // Have we already created it?
                    if (PocoFactories.TryGetValue(key, out Delegate factory))
                        return factory;
                }
                finally
                {
                    RWLock.ExitReadLock();
                }

                // Take the writer lock
                RWLock.EnterWriteLock();

                try
                {
                    // Check again, just in case
                    if (PocoFactories.TryGetValue(key, out Delegate factory))
                    {
                        return factory;
                    }

                    // Create the method
                    var m = new DynamicMethod("petapoco_factory_" + PocoFactories.Count.ToString(Culture), type, new Type[] { typeof(IDataReader), type }, true);
                    var il = m.GetILGenerator();

                    if (type == typeof(object))
                    {
                        // var poco=new T()
                        il.Emit(OpCodes.Newobj, typeof(System.Dynamic.ExpandoObject).GetConstructor(Type.EmptyTypes));            // obj

                        MethodInfo fnAdd = typeof(IDictionary<string, object>).GetMethod("Add");

                        // Enumerate all fields generating a set assignment for the column
                        for (int i = firstColumn; i < firstColumn + countColumns; i++)
                        {
                            var srcType = r.GetFieldType(i);

                            il.Emit(OpCodes.Dup);                        // obj, obj
                            il.Emit(OpCodes.Ldstr, r.GetName(i));        // obj, obj, fieldname

                            // Get the converter
                            Func<object, object> converter = null;
                            if (Database.Mapper != null)
                            {
                                converter = Database.Mapper.GetFromDbConverter((PropertyInfo)null, srcType);
                            }

                            if (ForceDateTimesToUtc && converter == null && srcType == typeof(DateTime))
                            {
                                converter = delegate (object src)
                                {
                                    return new DateTime(((DateTime)src).Ticks, DateTimeKind.Utc);
                                };
                            }

                            // Setup stack for call to converter
                            AddConverterToStack(il, converter);

                            // r[i]
                            il.Emit(OpCodes.Ldarg_0);                     // obj, obj, fieldname, converter?,    rdr
                            il.Emit(OpCodes.Ldc_I4, i);                   // obj, obj, fieldname, converter?,  rdr,i
                            il.Emit(OpCodes.Callvirt, fnGetValue);        // obj, obj, fieldname, converter?,  value

                            // Convert DBNull to null
                            il.Emit(OpCodes.Dup);                         // obj, obj, fieldname, converter?,  value, value
                            il.Emit(OpCodes.Isinst, typeof(DBNull));      // obj, obj, fieldname, converter?,  value, (value or null)
                            var lblNotNull = il.DefineLabel();
                            il.Emit(OpCodes.Brfalse_S, lblNotNull);       // obj, obj, fieldname, converter?,  value
                            il.Emit(OpCodes.Pop);                         // obj, obj, fieldname, converter?
                            if (converter != null)
                                il.Emit(OpCodes.Pop);                     // obj, obj, fieldname,
                            il.Emit(OpCodes.Ldnull);                      // obj, obj, fieldname, null
                            if (converter != null)
                            {
                                var lblReady = il.DefineLabel();
                                il.Emit(OpCodes.Br_S, lblReady);
                                il.MarkLabel(lblNotNull);
                                il.Emit(OpCodes.Callvirt, fnInvoke);
                                il.MarkLabel(lblReady);
                            }
                            else
                            {
                                il.MarkLabel(lblNotNull);
                            }

                            il.Emit(OpCodes.Callvirt, fnAdd);
                        }
                    }
                    else
                    {
                        if (type.IsValueType || type == typeof(string) || type == typeof(byte[]))
                        {
                            // Do we need to install a converter?
                            var srcType = r.GetFieldType(0);
                            var converter = GetConverter(ForceDateTimesToUtc, null, srcType, type);

                            // "if (!rdr.IsDBNull(i))"
                            il.Emit(OpCodes.Ldarg_0);                                       // rdr
                            il.Emit(OpCodes.Ldc_I4_0);                                      // rdr,0
                            il.Emit(OpCodes.Callvirt, fnIsDBNull);                          // bool
                            var lblCont = il.DefineLabel();
                            il.Emit(OpCodes.Brfalse_S, lblCont);
                            il.Emit(OpCodes.Ldnull);                                        // null
                            var lblFin = il.DefineLabel();
                            il.Emit(OpCodes.Br_S, lblFin);

                            il.MarkLabel(lblCont);

                            // Setup stack for call to converter
                            AddConverterToStack(il, converter);

                            il.Emit(OpCodes.Ldarg_0);                                       // rdr
                            il.Emit(OpCodes.Ldc_I4_0);                                      // rdr,0
                            il.Emit(OpCodes.Callvirt, fnGetValue);                          // value

                            // Call the converter
                            if (converter != null)
                                il.Emit(OpCodes.Callvirt, fnInvoke);

                            il.MarkLabel(lblFin);
                            il.Emit(OpCodes.Unbox_Any, type);                               // value converted
                        }
                        else if (type == typeof(Dictionary<string, object>))
                        {
                            Func<IDataReader, object, Dictionary<string, object>> func = (reader, inst) =>
                            {
                                var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                                for (int i = firstColumn; i < firstColumn + countColumns; i++)
                                {
                                    var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                    var name = reader.GetName(i);
                                    if (!dict.ContainsKey(name))
                                        dict.Add(name, value);
                                }
                                return dict;
                            };

                            var delegateType = typeof(Func<,,>).MakeGenericType(typeof(IDataReader), type, typeof(Dictionary<string, object>));
                            var localDel = Delegate.CreateDelegate(delegateType, func.Target, func.Method);
                            PocoFactories.Add(key, localDel);
                            return localDel;
                        }
                        else if (type == typeof(object[]))
                        {
                            Func<IDataReader, object, object[]> func = (reader, inst) =>
                            {
                                var obj = new object[countColumns - firstColumn];
                                for (int i = firstColumn; i < firstColumn + countColumns; i++)
                                {
                                    var value = reader.IsDBNull(i) ? null : reader.GetValue(i);
                                    obj[i - firstColumn] = value;
                                }
                                return obj;
                            };

                            var delegateType = typeof(Func<,,>).MakeGenericType(typeof(IDataReader), type, typeof(object[]));
                            var localDel = Delegate.CreateDelegate(delegateType, func.Target, func.Method);
                            PocoFactories.Add(key, localDel);
                            return localDel;
                        }
                        else
                        {
                            if (instance != null)
                            {
                                il.Emit(OpCodes.Ldarg_1);
                            }
                            else
                            {
                                // var poco=new T()
                                il.Emit(OpCodes.Newobj,
                                    type.GetConstructor(
                                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                                        null,
                                        Array.Empty<Type>(),
                                        null));
                            }

                            // Enumerate all fields generating a set assignment for the column
                            for (int i = firstColumn; i < firstColumn + countColumns; i++)
                            {
                                // Get the PocoColumn for this db column, ignore if not known
                                if (!Columns.TryGetValue(r.GetName(i), out PocoColumn pc)
                                    && !Columns.TryGetValue(r.GetName(i).Replace("_", ""), out pc))
                                {
                                    continue;
                                }

                                // Get the source type for this column
                                var srcType = r.GetFieldType(i);
                                var dstType = pc.PropertyInfo.PropertyType;

                                // "if (!rdr.IsDBNull(i))"
                                il.Emit(OpCodes.Ldarg_0);                                         // poco,rdr
                                il.Emit(OpCodes.Ldc_I4, i);                                       // poco,rdr,i
                                il.Emit(OpCodes.Callvirt, fnIsDBNull);                            // poco,bool
                                var lblNext = il.DefineLabel();
                                il.Emit(OpCodes.Brtrue_S, lblNext);                               // poco

                                il.Emit(OpCodes.Dup);                                             // poco,poco

                                // Do we need to install a converter?
                                var converter = GetConverter(ForceDateTimesToUtc, pc, srcType, dstType);

                                // Fast
                                bool Handled = false;
                                if (converter == null)
                                {
                                    var valuegetter = typeof(IDataRecord).GetMethod("Get" + srcType.Name, new Type[] { typeof(int) });
                                    if (valuegetter != null
                                            && valuegetter.ReturnType == srcType
                                            && (valuegetter.ReturnType == dstType || valuegetter.ReturnType == Nullable.GetUnderlyingType(dstType)))
                                    {
                                        il.Emit(OpCodes.Ldarg_0);                                          // *,rdr
                                        il.Emit(OpCodes.Ldc_I4, i);                                        // *,rdr,i
                                        il.Emit(OpCodes.Callvirt, valuegetter);                            // *,value

                                        // Convert to Nullable
                                        if (Nullable.GetUnderlyingType(dstType) != null)
                                        {
                                            il.Emit(OpCodes.Newobj, dstType.GetConstructor(new Type[] { Nullable.GetUnderlyingType(dstType) }));
                                        }

                                        il.Emit(OpCodes.Callvirt, pc.PropertyInfo.GetSetMethod(true));      // poco
                                        Handled = true;
                                    }
                                }

                                // Not so fast
                                if (!Handled)
                                {
                                    // Setup stack for call to converter
                                    AddConverterToStack(il, converter);

                                    // "value = rdr.GetValue(i)"
                                    il.Emit(OpCodes.Ldarg_0);                                             // *,rdr
                                    il.Emit(OpCodes.Ldc_I4, i);                                           // *,rdr,i
                                    il.Emit(OpCodes.Callvirt, fnGetValue);                                // *,value

                                    // Call the converter
                                    if (converter != null)
                                        il.Emit(OpCodes.Callvirt, fnInvoke);

                                    // Assign it
                                    il.Emit(OpCodes.Unbox_Any, pc.PropertyInfo.PropertyType);             // poco,poco,value
                                    il.Emit(OpCodes.Callvirt, pc.PropertyInfo.GetSetMethod(true));        // poco
                                }

                                il.MarkLabel(lblNext);
                            }

                            var fnOnLoaded = RecurseInheritedTypes<MethodInfo>(type, (x) => x.GetMethod("OnLoaded", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Array.Empty<Type>(), null));
                            if (fnOnLoaded != null)
                            {
                                il.Emit(OpCodes.Dup);
                                il.Emit(OpCodes.Callvirt, fnOnLoaded);
                            }
                        }
                    }
                    il.Emit(OpCodes.Ret);

                    // Cache it, return it
                    var del = m.CreateDelegate(Expression.GetFuncType(typeof(IDataReader), type, type));
                    PocoFactories.Add(key, del);
                    return del;
                }
                finally
                {
                    RWLock.ExitWriteLock();
                }
            }

            private static void AddConverterToStack(ILGenerator il, Func<object, object> converter)
            {
                if (converter != null)
                {
                    // Add the converter
                    int converterIndex = m_Converters.Count;
                    m_Converters.Add(converter);

                    // Generate IL to push the converter onto the stack
                    il.Emit(OpCodes.Ldsfld, fldConverters);
                    il.Emit(OpCodes.Ldc_I4, converterIndex);
                    il.Emit(OpCodes.Callvirt, fnListGetItem);                    // Converter
                }
            }

            public static Func<object, object> GetConverter(bool forceDateTimesToUtc, PocoColumn pc, Type srcType, Type dstType)
            {
                if (dstType == null) { throw new ArgumentNullException(nameof(dstType)); }

                Func<object, object> converter = null;

                // Get converter from the mapper
                if (Database.Mapper != null && pc != null)
                {
                    converter = Database.Mapper.GetFromDbConverter(pc.PropertyInfo, srcType);
                    if (converter != null)
                    {
                        return converter;
                    }
                }

                // Standard DateTime->Utc mapper
                if (pc != null && forceDateTimesToUtc && srcType == typeof(DateTime) && (dstType == typeof(DateTime) || dstType == typeof(DateTime?)))
                {
                    return delegate (object src) { return new DateTime(((DateTime)src).Ticks, DateTimeKind.Utc); };
                }

                // added by [kirishu]
                // unwrap nullable types
                Type underlyingDstType = Nullable.GetUnderlyingType(dstType);
                if (underlyingDstType != null)
                {
                    dstType = underlyingDstType;
                }

                // Forced type conversion including integral types -> enum
                if (dstType.IsEnum && IsIntegralType(srcType))
                {
                    var backingDstType = Enum.GetUnderlyingType(dstType);
                    if (underlyingDstType != null)
                    {
                        // if dstType is Nullable<Enum>, convert to enum value
                        return delegate (object src) { return Enum.ToObject(dstType, src); };
                    }
                    else if (srcType != backingDstType)
                    {
                        return delegate (object src) { return Convert.ChangeType(src, backingDstType, null); };
                    }
                }
                else if (!dstType.IsAssignableFrom(srcType))
                {
                    if (dstType.IsEnum && srcType == typeof(string))
                    {
                        return delegate (object src) { return EnumMapper.EnumFromString(dstType, (string)src); };
                    }

                    if (dstType == typeof(Guid) && srcType == typeof(string))
                    {
                        return delegate (object src) { return Guid.Parse((string)src); };
                    }

                    return delegate (object src) { return Convert.ChangeType(src, dstType, null); };
                }

                return null;
            }

            static T RecurseInheritedTypes<T>(Type t, Func<Type, T> cb)
            {
                while (t != null)
                {
                    T info = cb(t);
                    if (info != null)
                        return info;
                    t = t.BaseType;
                }
                return default;
            }


            static readonly Dictionary<Type, PocoData> m_PocoDatas = new Dictionary<Type, PocoData>();
            static readonly List<Func<object, object>> m_Converters = new List<Func<object, object>>();
            static readonly MethodInfo fnGetValue = typeof(IDataRecord).GetMethod("GetValue", new Type[] { typeof(int) });
            static readonly MethodInfo fnIsDBNull = typeof(IDataRecord).GetMethod("IsDBNull");
            static readonly FieldInfo fldConverters = typeof(PocoData).GetField("m_Converters", BindingFlags.Static | BindingFlags.GetField | BindingFlags.NonPublic);
            static readonly MethodInfo fnListGetItem = typeof(List<Func<object, object>>).GetProperty("Item").GetGetMethod();
            static readonly MethodInfo fnInvoke = typeof(Func<object, object>).GetMethod("Invoke");
            public Type type;
            public ReadOnlyCollection<string> QueryColumns { get; protected set; }
            public TableInfo TableInfo { get; protected set; }
            public Dictionary<string, PocoColumn> Columns { get; protected set; }
            readonly Dictionary<string, Delegate> PocoFactories = new Dictionary<string, Delegate>();
        }

        class EnumMapper : IDisposable
        {
            readonly Dictionary<Type, Dictionary<string, object>> _stringsToEnums = new Dictionary<Type, Dictionary<string, object>>();
            readonly Dictionary<Type, Dictionary<int, string>> _enumNumbersToStrings = new Dictionary<Type, Dictionary<int, string>>();
            readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

            public object EnumFromString(Type type, string value)
            {
                PopulateIfNotPresent(type);
                return _stringsToEnums[type][value];
            }

            public string StringFromEnum(object theEnum)
            {
                Type typeOfEnum = theEnum.GetType();
                PopulateIfNotPresent(typeOfEnum);
                return _enumNumbersToStrings[typeOfEnum][(int)theEnum];
            }

            void PopulateIfNotPresent(Type type)
            {
                _lock.EnterUpgradeableReadLock();
                try
                {
                    if (!_stringsToEnums.ContainsKey(type))
                    {
                        _lock.EnterWriteLock();
                        try
                        {
                            Populate(type);
                        }
                        finally
                        {
                            _lock.ExitWriteLock();
                        }
                    }
                }
                finally
                {
                    _lock.ExitUpgradeableReadLock();
                }
            }

            void Populate(Type type)
            {
                Array values = Enum.GetValues(type);
                _stringsToEnums[type] = new Dictionary<string, object>(values.Length);
                _enumNumbersToStrings[type] = new Dictionary<int, string>(values.Length);

                for (int i = 0; i < values.Length; i++)
                {
                    object value = values.GetValue(i);
                    _stringsToEnums[type].Add(value.ToString(), value);
                    _enumNumbersToStrings[type].Add((int)value, value.ToString());
                }
            }

            public void Dispose()
            {
                _lock.Dispose();
            }
        }
    }

    /// <summary>
    /// Transaction object helps maintain transaction depth counts
    /// </summary>
    public class Transaction : IDisposable
    {
        protected Database _db;

        public Transaction(Database db) : this(db, null) { }

        public Transaction(Database db, IsolationLevel? isolationLevel)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _db.BeginTransaction(isolationLevel);
        }

        public virtual void Complete()
        {
            _db.CompleteTransaction();
            _db = null;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                {
                    _db.AbortTransaction();
                }
            }
        }
    }

    /// <summary>
    /// A simple helper class for build SQL statements
    /// </summary>
    public class Sql : ICloneable
    {
        private string _sql;
        private object[] _args;
        private Sql _rhs;
        private string _sqlFinal;
        private ReadOnlyCollection<object> _argsFinal;

        /// <summary>
        /// Default, empty constructor
        /// </summary>
        public Sql()
        {
        }

        /// <summary>
        /// Construct an SQL statement with the supplied SQL and arguments
        /// </summary>
        /// <param name="sql">The SQL statement or fragment</param>
        /// <param name="args">Arguments to any parameters embedded in the SQL</param>
        public Sql(string sql, params object[] args)
        {
            _sql = sql;
            _args = args;
        }

        /// <summary>
        /// Construct an SQL statement with the supplied SQL and arguments
        /// </summary>
        /// <param name="isBuilt"></param>
        /// <param name="sql">The SQL statement or fragment</param>
        /// <param name="args">Arguments to any parameters embedded in the SQL</param>
        public Sql(bool isBuilt, string sql, params object[] args)
        {
            _sql = sql;
            _args = args;
            if (isBuilt)
            {
                _sqlFinal = _sql;
                _argsFinal = new ReadOnlyCollection<object>(_args);
            }
        }

        /// <summary>
        /// Instantiate a new SQL Builder object.  Weirdly implemented as a property but makes
        /// for more elegantly readable fluent style construction of SQL Statements
        /// eg: db.Query(Sql.Builder.Append(....))
        /// </summary>
        public static Sql Builder
        {
            get { return new Sql(); }
        }

        private void Build()
        {
            // already built?
            if (_sqlFinal != null)
                return;

            // Build it
            var sb = new StringBuilder();
            var args = new List<object>();
            Build(sb, args, null);
            _sqlFinal = sb.ToString();
            _argsFinal = new ReadOnlyCollection<object>(args);
        }

        /// <summary>
        /// Returns the final SQL statement represented by this builder
        /// </summary>
        public string SQL
        {
            get
            {
                Build();
                return _sqlFinal;
            }
        }

        /// <summary>
        /// Gets the complete, final set of arguments collected by this builder.
        /// </summary>
        public ReadOnlyCollection<object> Arguments
        {
            get
            {
                Build();
                return _argsFinal;
            }
        }

        /// <summary>
        /// Append another SQL builder instance to the right-hand-side of this SQL builder
        /// </summary>
        /// <param name="sql">A reference to another SQL builder instance</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public Sql Append(Sql sql)
        {
            if (_sqlFinal != null)
                _sqlFinal = null;

            if (_rhs != null)
            {
                _rhs.Append(sql);
            }
            else if (_sql != null)
            {
                _rhs = sql;
            }
            else
            {
                if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
                _sql = sql._sql;
                _args = sql._args;
                _rhs = sql._rhs;
            }

            return this;
        }

        /// <summary>
        /// Append an SQL fragment to the right-hand-side of this SQL builder
        /// </summary>
        /// <param name="sql">The SQL statement or fragment</param>
        /// <param name="args">Arguments to any parameters embedded in the SQL</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public Sql Append(string sql, params object[] args)
        {
            return Append(new Sql(sql, args));
        }

        private static bool Is(Sql sql, string sqltype)
        {
            return sql != null && sql._sql != null && sql._sql.StartsWith(sqltype, StringComparison.OrdinalIgnoreCase);
        }

        private void Build(StringBuilder sb, List<object> args, Sql lhs)
        {
            if (!String.IsNullOrEmpty(_sql))
            {
                // Add SQL to the string
                if (sb.Length > 0)
                {
                    sb.Append("\r\n");
                }

                var sql = Database.ProcessParams(_sql, _args, args);

                if (Is(lhs, "WHERE ") && Is(this, "WHERE "))
                    sql = "AND " + sql.Substring(6);
                if (Is(lhs, "ORDER BY ") && Is(this, "ORDER BY "))
                    sql = ", " + sql.Substring(9);
                // add set clause
                if (Is(lhs, "SET ") && Is(this, "SET "))
                    sql = ", " + sql.Substring(4);

                sb.Append(sql);
            }

            // Now do rhs
            if (_rhs != null)
                _rhs.Build(sb, args, this);
        }

        // added by [kirishu]
        // ディープコピーメソッドを追加しました
        #region ICloneable Member
        object ICloneable.Clone()
        {
            return Clone();
        }

        /// <summary>
        /// ディープコピーを返します
        /// </summary>
        /// <returns>Sqlオブジェクトの複製</returns>
        public Sql Clone()
        {
            Sql cloned = (Sql)MemberwiseClone();
            // 参照型フィールドの複製を作成する
            if (_args != null)
            {
                cloned._args = (object[])_args.Clone();
            }
            if (_rhs != null)
            {
                cloned._rhs = (Sql)_rhs.Clone();
            }
            if (_argsFinal != null)
            {
                cloned._argsFinal = new ReadOnlyCollection<object>(_argsFinal); //  (object[])_argsFinal.Clone();
            }
            cloned._sql = _sql;
            cloned._sqlFinal = _sqlFinal;
            return cloned;
        }
        #endregion


        /// <summary>
        /// Appends an SQL WHERE clause to this SQL builder
        /// </summary>
        /// <param name="sql">The condition of the WHERE clause</param>
        /// <param name="args">Arguments to any parameters embedded in the supplied SQL</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public Sql Where(string sql, params object[] args)
        {
            return Append(new Sql("WHERE (" + sql + ")", args));
        }

        /// <summary>
        /// added by [kirishu]
        /// Sqlオブジェクトを引数に取るWhereメソッドを追加しました
        /// </summary>
        /// <param name="sql">A reference to another SQL builder instance</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public Sql Where(Sql sql)
        {
            /*
             * いきなり直接 sql._sqlFinalを使うと、nullが返るのでダメ。
             * SQLプロパティにアクセスすると、Build()メソッドが呼ばれてSQL文として
             * 整形される。
             *
             * なので、SQL、Argumentsプロパティの値を使うべし
             * */
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return Where(sql.SQL, sql.Arguments.ToArray());
        }

        /// <summary>
        ///     Appends an SQL SET clause to this SQL builder
        /// </summary>
        /// <param name="sql">The SET clause like "{field} = {value}"</param>
        /// <param name="args">Arguments to any parameters embedded in the supplied SQL</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public Sql Set(string sql, params object[] args)
        {
            return Append(new Sql("SET " + sql, args));
        }

        /// <summary>
        /// Appends an SQL ORDER BY clause to this SQL builder
        /// </summary>
        /// <param name="columns">A collection of SQL column names to order by</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public Sql OrderBy(params object[] columns)
        {
            return Append(new Sql("ORDER BY " + String.Join(", ", (from x in columns select x.ToString()).ToArray())));
        }

        /// <summary>
        /// Appends an SQL SELECT clause to this SQL builder
        /// </summary>
        /// <param name="columns">A collection of SQL column names to select</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public Sql Select(params object[] columns)
        {
            return Append(new Sql("SELECT " + String.Join(", ", (from x in columns select x.ToString()).ToArray())));
        }

        /// <summary>
        /// Appends an SQL FROM clause to this SQL builder
        /// </summary>
        /// <param name="tables">A collection of table names to be used in the FROM clause</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public Sql From(params object[] tables)
        {
            return Append(new Sql("FROM " + String.Join(", ", (from x in tables select x.ToString()).ToArray())));
        }

        /// <summary>
        /// Appends an SQL GROUP BY clause to this SQL builder
        /// </summary>
        /// <param name="columns">A collection of column names to be grouped by</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public Sql GroupBy(params object[] columns)
        {
            return Append(new Sql("GROUP BY " + String.Join(", ", (from x in columns select x.ToString()).ToArray())));
        }

        private SqlJoinClause Join(string JoinType, string table)
        {
            return new SqlJoinClause(Append(new Sql(JoinType + table)));
        }

        /// <summary>
        /// Appends an SQL INNER JOIN clause to this SQL builder
        /// </summary>
        /// <param name="table">The name of the table to join</param>
        /// <returns>A reference an SqlJoinClause through which the join condition can be specified</returns>
        public SqlJoinClause InnerJoin(string table) { return Join("INNER JOIN ", table); }

        /// <summary>
        /// Appends an SQL LEFT JOIN clause to this SQL builder
        /// </summary>
        /// <param name="table">The name of the table to join</param>
        /// <returns>A reference an SqlJoinClause through which the join condition can be specified</returns>
        public SqlJoinClause LeftJoin(string table) { return Join("LEFT JOIN ", table); }

        /// <summary>
        /// The SqlJoinClause is a simple helper class used in the construction of SQL JOIN statements with the SQL builder
        /// </summary>
        public class SqlJoinClause
        {
            private readonly Sql _sql;

            public SqlJoinClause(Sql sql)
            {
                _sql = sql;
            }

            /// <summary>
            /// Appends a SQL ON clause after a JOIN statement
            /// </summary>
            /// <param name="onClause">The ON clause to be appended</param>
            /// <param name="args">Arguments to any parameters embedded in the supplied SQL</param>
            /// <returns>A reference to the parent SQL builder, allowing for fluent style concatenation</returns>
            public Sql On(string onClause, params object[] args)
            {
                return _sql.Append("ON " + onClause, args);
            }
        }

        //public static implicit operator Sql(SqlBuilder.Template template)
        //{
        //    return new Sql(true, template.RawSql, template.Parameters);
        //}
    }

}
