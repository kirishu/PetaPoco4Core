/* PetaPoco v4.0.3.12 - A Tiny ORMish thing for your POCO's.
 * Copyright 2011-2012 Topten Software.  All Rights Reserved.
 *
 * Apache License 2.0 - http://www.toptensoftware.com/petapoco/license
 *
 * Special thanks to Rob Conery (@robconery) for original inspiration (ie:Massive) and for
 * use of Subsonic's T4 templates, Rob Sullivan (@DataChomp) for hard core DBA advice
 * and Adam Schroder (@schotime) for lots of suggestions, improvements and Oracle support
 */
/*
* 元々はPetaPoco本体ファイルに含まれていたけど、3階層C/SのDTOライブラリのために、
* Attribute部分だけをこのファイルに移しました。
*/

using System;
using System.Collections.Generic;

namespace PetaPoco
{
    /// <summary>
    ///     Represents the attribute which decorates a poco class to state all columns must be explicitly mapped using either a
    ///     <seealso cref="ColumnAttribute" /> or <seealso cref="ResultColumnAttribute" />.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExplicitColumnsAttribute : Attribute
    {
    }

    /// <summary>
    ///     Represents an attribute which can decorate a Poco property to ensure PetaPoco does not map column, and therefore
    ///     ignores the column.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }

    /// <summary>
    ///     Represents an attribute which can decorate a Poco property to mark the property as a column. It may also optionally
    ///     supply the DB column name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnAttribute : Attribute
    {
        public ColumnAttribute() { }
        public ColumnAttribute(string name) { Name = name; }
        public string Name { get; set; }
    }

    /// <summary>
    ///     Represents an attribute which can decorate a poco property as a result only column. A result only column is a
    ///     column that is only populated in queries and is not used for updates or inserts operations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ResultColumnAttribute : ColumnAttribute
    {
        public ResultColumnAttribute() { }
        public ResultColumnAttribute(string name) : base(name) { }
    }

    /// <summary>
    /// Represents an attribute, which when applied to a Poco class, specifies the the DB table name which it maps to
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class TableNameAttribute : Attribute
    {
        public TableNameAttribute(string tableName)
        {
            Value = tableName;
        }
        public string Value { get; private set; }
    }

    /// <summary>
    /// Is an attribute, which when applied to a Poco class, specifies primary key column. Additionally, specifies whether
    /// the column is auto incrementing and the optional sequence name for Oracle sequence columns.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute(string primaryKey)
        {
            Value = primaryKey;
            AutoIncrement = true;
        }

        public string Value { get; private set; }
        public string SequenceName { get; set; }
        public bool AutoIncrement { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class AutoJoinAttribute : Attribute
    {
        public AutoJoinAttribute() { }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class VersionColumnAttribute : ColumnAttribute
    {
        public VersionColumnAttribute() { }
        public VersionColumnAttribute(string name) : base(name) { }
    }

    #region PetaPocoRecord
    /// <summary>
    /// 拡張PetaPocoレコードオブジェクト インターフェイス
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPetaPocoRecord<T>
    {
        /// <summary>変更追跡列コレクション</summary>
        IEnumerable<string> GetModifiedColumns();

        /// <summary>変更追跡列コレクションをクリア</summary>
        void ClearModifiedColumns();
    }

    /// <summary>
    /// 変更列自動マーク機能を実装したPetaPocoオブジェクト
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PetaPocoRecord<T> : IPetaPocoRecord<T>
    {
        /// <summary>変更追跡列コレクション</summary>
        protected Dictionary<string, bool> modifiedColumns = new Dictionary<string, bool>();

        /// <summary>
        /// On Loaded event
        /// </summary>
        /// <remarks>
        /// PetaPoco.Core.PetaDataで実装されているDynamicMethodを使った動的メソッド
        ///
        ///     IL生成メソッドdelegate
        ///     キャッシュ処理をやっているけど、大量レコードを格納すると遅くなるので注意
        /// </remarks>
        protected void OnLoaded()
        {
            modifiedColumns = new Dictionary<string, bool>();
        }

        /// <summary>
        /// 変更フラッグをセットする
        /// </summary>
        /// <param name="column_name">Column name</param>
        protected void MarkColumnModified(string column_name)
        {
            if (modifiedColumns != null)
            {
                modifiedColumns[column_name] = true;
            }
        }

        /// <summary>
        /// 変更追跡列コレクションを返す
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetModifiedColumns()
        {
            if (modifiedColumns == null)
            {
                return null;
            }
            return modifiedColumns.Keys;
        }

        /// <summary>変更追跡列コレクションをクリア</summary>
        public void ClearModifiedColumns()
        {
            if (modifiedColumns != null)
            {
                modifiedColumns.Clear();
            }
        }
    }
    #endregion

}
