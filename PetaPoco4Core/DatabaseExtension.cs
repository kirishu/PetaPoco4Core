/* --------------------------------------------------------------------------
 * DatabaseExtension - PetaPoco.Database Extension class
 * Created by kirishu (zapshu@gmail.com)
 * v4.7.1.5
 * https://github.com/kirishu/PetaPoco4Core
 * -------------------------------------------------------------------------- */

#region Suppressions
#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable IDE0034 // default expression can be simplified
#pragma warning disable IDE0057 // Substring can be simplified
#pragma warning disable IDE0063 // 'using' statement can be simplified
#pragma warning disable IDE0066 // Convert switch statement to switch expression
#endregion

using NLog;
using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PetaPoco
{
    #region DatabaseExtension
    /// <summary>
    /// PetaPoco.Database を継承して拡張したヤツ
    /// ※[PetaPoco.cs]にもちょびっと手を加えてるけど・・・
    ///
    /// [主な拡張機能]
    ///     CommandTimeoutの設定
    ///     logの出力
    ///     DataTableを返す
    ///     既存テーブルからユーザ一時表の簡単作成
    /// </summary>
    public class DatabaseExtension : PetaPoco.Database
    {
        /// <summary> Logging Instance </summary>
        private static ILogger _logger;

        /// <summary> SQL文実行時の時間 </summary>
        private DateTime _execTime = DateTime.MinValue;

        /// <summary> デバッグ出力にA5Mk2のSetParameter構文を使用する </summary>
        protected bool UseA5Mk2Params { get; set; }

        /// <summary>
        /// constractor
        /// </summary>s
        /// <param name="connectionString"></param>
        /// <param name="rdbType"></param>
        public DatabaseExtension(string connectionString, RDBType rdbType)
            : base(connectionString, rdbType)
        {
            _logger = LogManager.GetCurrentClassLogger();
            _logger.Debug(string.Format(Culture, "[New Instance] {0}", connectionString));
        }

        /// <summary>
        /// constractor
        /// </summary>s
        /// <param name="connectionString"></param>
        /// <param name="rdbType"></param>
        /// <param name="logger"></param>
        public DatabaseExtension(string connectionString, RDBType rdbType, Logger logger)
            : base(connectionString, rdbType)
        {
            _logger = logger;
            _logger.Debug(string.Format(Culture, "[New Instance] {0}", connectionString));
        }

        /// <summary>
        /// SQL文実行前
        /// </summary>
        /// <param name="cmd"></param>
        public override void OnExecutingCommand(IDbCommand cmd)
        {
            if (cmd == null) { throw new ArgumentNullException(nameof(cmd)); }

            var sql = cmd.CommandText;

            var log = new StringBuilder();
#if DEBUG
            if (UseA5Mk2Params)
            {
                var regex = new Regex("@([0-9]+)");
                while (regex.IsMatch(sql))
                {
                    sql = regex.Replace(sql, ":p$1");
                }
            }
            log.AppendLine("[OnExecutingCommand]");
            log.AppendLine("-- BEGIN COMMAND");
            log.AppendLine(GetLogParameterDeclare(cmd.Parameters));
            log.AppendLine("--");
            log.AppendLine(sql);
            log.AppendLine("-- END COMMAND");
#else
            // Release時はパフォーマンスを考慮して簡易な出力に留める
            log.AppendLine(LastCommand);
#endif
            _logger.Debug(log.ToString());
            _execTime = DateTime.Now;
#if DEBUG
            System.Diagnostics.Debug.WriteLine(log.ToString());
#endif
        }

        #region private methods for DEBUG log
        /// <summary>
        /// DEBUG時に実行可能なSQL文を出力する
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        /// <remarks>
        /// 実際に発行されるSQL文とは異なる場合があるので、参考程度にする
        /// 例：ここではパラメータ値が"True"と出るけど、実際には"1"が出力される・・・など
        /// </remarks>
        private string GetLogParameterDeclare(IDataParameterCollection parameters)
        {
            var log = new StringBuilder();

            foreach (IDataParameter param in parameters)
            {
                string typename = GetLogParameterType(param);
                string logvalue = GetLogQuotedParameterValue(typename, param.Value);

                if (UseA5Mk2Params)
                {
                    log.AppendFormat(Culture, "SetParameter {0} {1}",
                        param.ParameterName.Replace(this.ParamPrefix, "p"),     // "@"を"p"に変更する
                        logvalue
                    );
                }
                else
                {
                    if (this._rdbType == RDBType.SqlServer)
                    {
                        log.AppendFormat(Culture, "DECLARE {0} {1} = {2}", param.ParameterName, typename, logvalue);
                    }
                    else
                    {
                        log.AppendFormat(Culture, "SET {0} = {1};", param.ParameterName, logvalue);
                    }
                }
                log.AppendLine();
            }

            if (UseA5Mk2Params)
            {
                log.Insert(0, "/**\r\n");
                log.Append("*/");
            }
            return log.ToString();
        }

        //const string DATETIME_FORMAT_ROUNDTRIP = "o";
        private string GetLogQuotedParameterValue(string typename, object value)
        {
            var log = new StringBuilder();
            try
            {
                if (value == null)
                {
                    log.Append("NULL");
                }
                else
                {
                    value = UnboxNullable(value);

                    if (value is DBNull)
                    {
                        log.Append("NULL");
                    }
                    else if (value is string
                        || value is char
                        || value is char[]
                        || value is System.Xml.Linq.XElement
                        || value is System.Xml.Linq.XDocument)
                    {
                        log.Append('\'');
                        log.Append(value.ToString().Replace("'", "''"));
                        log.Append('\'');
                    }
                    else if (value is bool)
                    {
                        if (_rdbType == RDBType.MySql || _rdbType == RDBType.PostgreSql)
                        {
                            log.Append(value.ToString());
                        }
                        else
                        {
                            // True -> 1, False -> 0
                            log.Append(Convert.ToInt32(value, Culture));
                        }
                    }
                    else if (value is sbyte
                        || value is byte
                        || value is short
                        || value is ushort
                        || value is int
                        || value is uint
                        || value is long
                        || value is ulong
                        || value is float
                        || value is double
                        || value is decimal)
                    {
                        log.Append(value.ToString());
                    }
                    else if (value is DateTime || value is DateTimeOffset)
                    {
                        log.Append('\'');
                        if (typename.Equals("DATE", StringComparison.InvariantCultureIgnoreCase))
                        {
                            log.Append(((DateTime)value).ToString("yyyy/MM/dd", Culture));
                        }
                        else
                        {
                            log.Append(((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss.fff", Culture));
                        }
                        log.Append('\'');
                    }
                    else if (value is TimeSpan span)
                    {
                        log.Append('\'');
                        log.Append(span.ToString("g", Culture));    //g [-][d:]h:mm:ss[.FFFFFFF]
                        log.Append('\'');
                    }
                    else if (value is Guid guid)
                    {
                        log.Append('\'');
                        log.Append(guid.ToString());
                        log.Append('\'');
                    }
                    else if (value is byte[])
                    {
                        log.Append("/* BINARY */");
                    }
                    else
                    {
                        log.Append("/* UNKNOWN DATATYPE: ");
                        log.Append(value.GetType().ToString());
                        log.Append(" *" + "/ '");
                        log.Append(value.ToString());
                        log.Append('\'');
                    }
                }
            }
            finally { }

            return log.ToString();
        }

        private static object UnboxNullable(object value)
        {
            var typeOriginal = value.GetType();
            if (typeOriginal.IsGenericType
                && typeOriginal.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // generic value, unboxing needed
                return typeOriginal.InvokeMember("GetValueOrDefault",
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.InvokeMethod,
                    null, value, null, System.Globalization.CultureInfo.InvariantCulture);
            }
            else
            {
                return value;
            }
        }

        /// <summary>
        /// DEBUGコンソールに出力するためのパラメータ編集
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private string GetLogParameterType(IDataParameter param)
        {
            string datatype;

            switch (param.DbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    datatype = UseA5Mk2Params ? "STRING" : "NVARCHAR(4000)";
                    break;
                case DbType.Boolean:
                    datatype = "BOOLEAN";
                    break;
                case DbType.Byte:
                    datatype = "TINYINT";
                    break;
                case DbType.Int16:
                case DbType.SByte:
                    datatype = "SMALLINT";
                    break;
                case DbType.Int32:
                case DbType.UInt16:
                    datatype = "INT";
                    break;
                case DbType.Int64:
                case DbType.UInt32:
                    datatype = "BIGINT";
                    break;
                case DbType.UInt64:
                case DbType.Currency:
                case DbType.Decimal:
                case DbType.VarNumeric:
                    datatype = "DECIMAL";
                    break;
                case DbType.Single:
                    datatype = "FLOAT";
                    break;
                case DbType.Double:
                    datatype = "REAL";
                    break;
                case DbType.Time:
                    datatype = "TIME";
                    break;
                case DbType.Date:
                    datatype = "DATE";
                    break;
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    datatype = "DATETIME";
                    break;
                case DbType.Binary:
                case DbType.Guid:
                case DbType.Object:
                case DbType.Xml:
                    // バイナリ
                    datatype = UseA5Mk2Params ? "STRING" : param.DbType.ToString().ToUpperInvariant();
                    break;
                default:
                    // なんだかわかんないもの
                    datatype = "STRING";
                    break;
            }
            return datatype;
        }
        #endregion

        /// <summary>
        /// SQL実行完了時
        /// </summary>
        /// <param name="cmd"></param>
        public override void OnExecutedCommand(IDbCommand cmd)
        {
            TimeSpan ts = DateTime.Now - _execTime;
            var log = string.Format(Culture, "[OnExecutedCommand] {0} milliseconds", ts.TotalMilliseconds);
            _logger.Debug(log);
#if DEBUG
            System.Diagnostics.Debug.WriteLine(log);
#endif
        }

        /// <summary>
        /// 例外発生時
        /// </summary>
        /// <param name="x"></param>
        public override void OnException(Exception x)
        {
            _logger.Error(LastCommand);
            //* stack traceは呼び出し元でロギングしましょう *//
            base.OnException(x);
        }

        /// <summary>
        /// Select文の結果をDataTableで返します
        /// （DataTableをそのまんまWCFで返すのはやめよう）
        /// </summary>
        /// <param name="sql">SELECT文</param>
        /// <param name="args">SQLパラメータ</param>
        /// <returns>SELECT文の結果</returns>
        public DataTable GetDataTable(string sql, params object[] args)
        {
            try
            {
                OpenSharedConnection();
                try
                {
                    using (var cmd = CreateCommand(Connection, sql, args))  // ※オリジナルのPetaPocoでは、このCreateCommandがprivateだったので、protectedに変更しました。
                    {
                        var val = cmd.ExecuteReader();
                        OnExecutedCommand(cmd);
                        var dt = new DataTable();
                        dt.Load(val);
                        return dt;
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
        /// Select文の結果をDataTableで返します
        /// （DataTableをそのまんまWCFで返すのはやめよう）
        /// </summary>
        /// <param name="sql">SQL object</param>
        /// <returns>SELECT文の結果</returns>
        public DataTable GetDataTable(PetaPoco.Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return GetDataTable(sql.SQL, sql.Arguments.ToArray());
        }

        /// <summary>
        /// SQL Server専用
        /// 指定したテーブルから構造を読み取り、新しい一時テーブルを作成する。
        ///
        ///     「SELECT * INTO hoge FROM fuga」のようなもの。
        ///     PetaPocoでは↑で一時表が作成できないので、代わりにこれを使います。
        ///
        /// ※ユーザ一時表を使うときは、db.KeepConnectionAliveをtrueにすること！
        /// </summary>
        /// <param name="tempname">作成テーブル名（#付き）</param>
        /// <param name="sourcename">参照元のテーブル名</param>
        public void CreateTemporaryFromTable(string tempname, string sourcename)
        {
            if (_rdbType != RDBType.SqlServer)
            {
                throw new ArgumentException(string.Format(Culture, "SQL Serverのみです"));
            }

            try
            {
                // sp_columnsから列情報を読み取ってCREATE TABLE文を発行する
                var sb = new StringBuilder();
                using (var dt = GetDataTable(string.Format(Culture, "sp_columns '{0}'", sourcename)))
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (sb.Length > 0)
                        {
                            sb.Append(" , ");
                        }

                        // カラム名
                        sb.AppendFormat(Culture, " {0} ", dr["COLUMN_NAME"].ToString());
                        // 型名
                        sb.AppendFormat(Culture, " {0} ", dr["TYPE_NAME"].ToString().ToUpperInvariant().Replace("identity", ""));
                        // サイズ
                        if (dr["RADIX"] == null || dr["RADIX"].Equals(DBNull.Value))
                        {
                            // RADIXがnullなら数値以外
                            if (dr["SCALE"] == null || dr["SCALE"].Equals(DBNull.Value))
                            {
                                // 文字列
                                if (dr["PRECISION"].ToString().Trim().Length < 5)
                                {
                                    sb.AppendFormat(Culture, " ({0}) ", dr["PRECISION"].ToString());
                                }
                            }
                        }
                        // nullの可否
                        sb.AppendFormat(Culture, " {0} ", (dr["NULLABLE"].ToString().Trim() == "0") ? "not null" : "null");
                    }
                }

                sb.Insert(0, "CREATE TABLE " + tempname + " ( ");
                sb.Append(" ) ");

                Execute(sb.ToString());
            }
            catch (Exception x)
            {
                OnException(x);
                throw;
            }
        }

        /// <summary>
        /// HAVING句やGROUP句を含んだクエリのPageメソッド
        ///     使い方はPageメソッドと同じ
        ///
        ///
        /// オリジナルのPageメソッドは、全体の件数を取得する際、
        /// SQL文からSELECT,HAVING,ORDER句を取り除いて
        /// COUNT(*)を行う。
        ///
        /// でもこれだとHAVING句やGROUP句があるクエリでは具合が悪い。
        /// そこで、元のSQL文からORDER句だけを取り除き、
        /// 全体をインラインクエリにして件数を取得する。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="sql"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public Page<T> PageWithHaving<T>(long page, long itemsPerPage, string sql, params object[] args)
        {
            // まずは普通にcount用とpage用のSQLを構築する
            BuildPageQueries<T>((page - 1) * itemsPerPage, itemsPerPage, sql, ref args, out string sqlCount, out string sqlPage);

            // 元のSQLからORDER句だけを取り除く
            var m = rxOrderBy.Match(sql);
            if (m.Success)
            {
                Group g = m.Groups[0];
                sqlCount = sql.Substring(0, g.Index) + sql.Substring(g.Index + g.Length);
            }
            // 全体を「SELECT COUNT(*)」で囲んで、count用SQLとする
            sqlCount = "SELECT COUNT(*) FROM (" + sqlCount + ") AS _temp";
            // ※以下はPage<T>メソッドと同じ

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

        /// <summary>
        /// HAVING句やGROUP句を含んだクエリのPageメソッド
        ///     使い方はPageメソッドと同じ
        /// </summary>
        public Page<T> PageWithHaving<T>(long page, long itemsPerPage, Sql sql)
        {
            if (sql == null) { throw new ArgumentNullException(nameof(sql)); }
            return PageWithHaving<T>(page, itemsPerPage, sql.SQL, sql.Arguments.ToArray());
        }

    }
    // end of class
    #endregion

}
