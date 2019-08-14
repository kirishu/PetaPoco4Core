/*
 * DatabaseExtension v2.0.0
 * PetaPoco.Database 拡張クラス
 */
#region 変更履歴
// 25-Feb-2013 新規作成
// 22-Dec-2013 [OnExecutingCommand] DEBUG時のSQLログをクエリアナライザから実行可能な形式にする
// 21-Apr-2014 [CreateTemporaryFromTable] ユーザ一時表作成メソッド
// 30-Oct-2014 AbstractRecordを追加しました
// 19-Apr-2016 PageWithHavingを追加しました
// 14-Mar-2017 DebugのSQL出力をMySQLに対応しました
// 03-Nov-2018 DebugのSQL出力をPostgreSqlに対応しました
// 21-Jun-2019 .NET Standard 2.0 に対応
#endregion

using System;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using NLog;

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
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        /// <summary> SQL文実行時の時間 </summary>
        private DateTime _execTime = DateTime.MinValue;

        /// <summary> デバッグ出力にA5Mk2のSetParameter構文を使用する </summary>
        private readonly bool _useA5Mk2Params;

        /// <summary>
        /// constractor
        /// </summary>s
        /// <param name="connectionString"></param>
        /// <param name="dbType"></param>
        public DatabaseExtension(string connectionString, DBType dbType)
            : base(connectionString, dbType)
        {
            _dbType = dbType;

#if DEBUG
            _logger.Debug("[New Instance] {0}", connectionString);
#endif
            base.CommandTimeout = 30;    // default 30sec

            _useA5Mk2Params = (dbType == DBType.PostgreSQL);
        }

        /// <summary>
        /// SQL文実行前
        /// </summary>
        /// <param name="cmd"></param>
        public override void OnExecutingCommand(IDbCommand cmd)
        {
#if DEBUG
            var text = cmd.CommandText;
            if (_useA5Mk2Params)
            {
                var regex = new Regex("@([0-9]+)");
                while (regex.IsMatch(text))
                {
                    text = regex.Replace(text, ":p$1");
                }
            }

            var log = new StringBuilder();
            log.AppendLine("[OnExecutingCommand]");
            log.AppendLine("-- BEGIN COMMAND");
            log.AppendLine(GetLogParameterDeclare(cmd.Parameters));
            log.AppendLine("--");
            log.AppendLine(text);
            log.AppendLine("-- END COMMAND");

            System.Diagnostics.Debug.WriteLine(log.ToString());
            _logger.Debug(log.ToString());
            _execTime = DateTime.Now;
#endif
        }

        #region private methods for DEBUG log
        /// <summary>
        /// DEBUG時に[A5:SQL Mk-2]で実行可能なSQL文を出力する
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private string GetLogParameterDeclare(IDataParameterCollection parameters)
        {
            var log = new StringBuilder();
            foreach (IDataParameter param in parameters)
            {
                if (_useA5Mk2Params)
                {
                    log.Append("SetParameter ");
                    log.Append(param.ParameterName.Replace(this.ParamPrefix, "p"));      // "@"を"p"に変更する
                    log.Append(' ');
                    log.Append(GetLogQuotedParameterValue(param.Value));
                    log.Append(' ');
                    log.Append(GetLogParameterType(param));
                }
                else
                {
                    if (this._dbType == DBType.SqlServer)
                    {
                        log.Append("DECLARE ");
                        log.Append(param.ParameterName);
                        log.Append(' ');
                        log.Append(GetLogParameterType(param));
                        log.Append("; ");
                    }
                    log.Append("SET ");
                    log.Append(param.ParameterName);
                    log.Append(" = ");
                    log.Append(GetLogQuotedParameterValue(param.Value));
                    log.Append(";");
                }
                log.AppendLine();
            }

            if (_useA5Mk2Params)
            {
                log.Insert(0, "/**\n");
                log.Append("*/");
            }
            return log.ToString();
        }

        const string DATETIME_FORMAT_ROUNDTRIP = "o";
        private string GetLogQuotedParameterValue(object value)
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
                        // True -> 1, False -> 0
                        log.Append(Convert.ToInt32(value));
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
                        if (_dbType == DBType.SqlServer)
                        {
                            // MSSQL
                            log.Append("CAST('");
                            log.Append(((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff"));
                            log.Append("' as datetime)");
                        }
                        else
                        {
                            log.Append(((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss"));
                        }
                    }
                    else if (value is TimeSpan)
                    {
                        log.Append(((TimeSpan)value).ToString("g"));    //g [-][d:]h:mm:ss[.FFFFFFF]
                    }
                    else if (value is Guid)
                    {
                        log.Append('\'');
                        log.Append(((Guid)value).ToString());
                        log.Append('\'');
                    }
                    else if (value is byte[] data)
                    {
                        if (data.Length == 0)
                        {
                            log.Append("NULL");
                        }
                        else
                        {
                            log.Append("0x");
                            for (int i = 0; i < data.Length; i++)
                            {
                                log.Append(data[i].ToString("h2"));
                            }
                        }
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

            catch (Exception ex)
            {
                log.AppendLine("/* Exception occurred while converting parameter: ");
                log.AppendLine(ex.ToString());
                log.AppendLine("*/");
            }
            return log.ToString();
        }

        private object UnboxNullable(object value)
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
                    null, value, null);
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
        private string GetLogParameterType(System.Data.IDataParameter param)
        {
            string dbtype = string.Empty;

            switch (param.DbType)
            {
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.String:
                case DbType.StringFixedLength:
                    dbtype = _useA5Mk2Params ? "String" : "NVARCHAR(4000)";
                    break;
                case DbType.Boolean:
                    dbtype = _useA5Mk2Params ? "Boolean" : "BOOLEAN";
                    break;
                case DbType.Byte:
                    dbtype = _useA5Mk2Params ? "Integer" : "TINYINT";
                    break;
                case DbType.Int16:
                case DbType.SByte:
                    dbtype = _useA5Mk2Params ? "Integer" : "SMALLINT";
                    break;
                case DbType.Int32:
                case DbType.UInt16:
                    dbtype = _useA5Mk2Params ? "Integer" : "INT";
                    break;
                case DbType.Int64:
                case DbType.UInt32:
                    dbtype = _useA5Mk2Params ? "Integer" : "BIGINT";
                    break;
                case DbType.UInt64:
                case DbType.Currency:
                case DbType.Decimal:
                case DbType.VarNumeric:
                    dbtype = _useA5Mk2Params ? "Currency" : "DECIMAL";
                    break;
                case DbType.Single:
                    dbtype = _useA5Mk2Params ? "Float" : "FLOAT";
                    break;
                case DbType.Double:
                    dbtype = _useA5Mk2Params ? "Float" : "REAL";
                    break;
                case DbType.Time:
                    dbtype = _useA5Mk2Params ? "Time" : "TIME";
                    break;
                case DbType.Date:
                    dbtype = _useA5Mk2Params ? "Date" : "DATE";
                    break;
                case DbType.DateTime:
                case DbType.DateTime2:
                case DbType.DateTimeOffset:
                    dbtype = _useA5Mk2Params ? "DateTime" : "DATETIME";
                    break;
                case DbType.Binary:
                case DbType.Guid:
                case DbType.Object:
                case DbType.Xml:
                    // バイナリ
                    dbtype = _useA5Mk2Params ? "String" : param.DbType.ToString().ToUpper();      // そのまま返す
                    break;
                default:
                    // なんだかわかんないもの
                    break;
            }
            return dbtype;
        }
        #endregion

        /// <summary>
        /// SQL実行完了時
        /// </summary>
        /// <param name="cmd"></param>
        public override void OnExecutedCommand(IDbCommand cmd)
        {
#if DEBUG
            TimeSpan ts = DateTime.Now - _execTime;
            var log = string.Format("[OnExecutedCommand] {0} milliseconds", ts.TotalMilliseconds);
            System.Diagnostics.Debug.WriteLine(log);
            _logger.Debug(log);
#endif
        }

        /// <summary>
        /// 例外発生時
        /// </summary>
        /// <param name="x"></param>
        public override void OnException(Exception x)
        {
            _logger.Debug(LastCommand);
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
            return GetDataTable(sql.SQL, sql.Arguments);
        }

        /// <summary>
        /// SQL Server専用
        /// 指定したテーブルから構造を読み取り、新しいテーブルを作成する。
        ///
        ///     「SELECT * INTO hoge FROM fuga」のようなもの。
        ///     何故かPetaPocoでは↑で一時表が作成できないので、代わりにこれを使います。
        ///
        /// ※ユーザ一時表を使うときは、db.KeepConnectionAliveをtrueにすること！
        /// </summary>
        /// <param name="tempname">作成テーブル名（#付き）</param>
        /// <param name="sourcename">参照元のテーブル名</param>
        public void CreateTemporaryFromTable(string tempname, string sourcename)
        {
            if (_dbType != DBType.SqlServer)
            {
                throw new ApplicationException("SQL Serverのみです");
            }

            try
            {
                // sp_columnsから列情報を読み取ってCREATE TABLE文を発行する
                var dt = GetDataTable(string.Format("sp_columns '{0}'", sourcename));
                var sb = new StringBuilder();
                foreach (DataRow dr in dt.Rows)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(" , ");
                    }

                    // カラム名
                    sb.AppendFormat(" {0} ", dr["COLUMN_NAME"].ToString());
                    // 型名
                    sb.AppendFormat(" {0} ", dr["TYPE_NAME"].ToString().ToLower().Replace("identity", ""));
                    // サイズ
                    if (dr["RADIX"] == null || dr["RADIX"].Equals(DBNull.Value))
                    {
                        // RADIXがnullなら数値以外
                        if (dr["SCALE"] == null || dr["SCALE"].Equals(DBNull.Value))
                        {
                            // 文字列
                            if (dr["PRECISION"].ToString().Trim().Length < 5)
                            {
                                sb.AppendFormat(" ({0}) ", dr["PRECISION"].ToString());
                            }
                        }
                    }
                    // nullの可否
                    sb.AppendFormat(" {0} ", (dr["NULLABLE"].ToString().Trim() == "0") ? "not null" : "null");
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
        /// 変更した列だけを更新するUpdateメソッド
        /// （正確には列プロパティへsetが実行された列）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco"></param>
        /// <returns></returns>
        public int Update<T>(PetaPoco.IPetaPocoRecord<T> poco)
        {
            return base.Update(poco, null, poco.GetModifiedColumns());
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
            return PageWithHaving<T>(page, itemsPerPage, sql.SQL, sql.Arguments);
        }

    }
    // end of class
    #endregion

}