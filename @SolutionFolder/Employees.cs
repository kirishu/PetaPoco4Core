
// This file was automatically generated by the PetaPoco T4 Template
// Do not make changes directly to this file - edit the template instead
// 
// The following connection settings were used to generate this file
// 
//     Connection String:      `server=localhost;uid=testman;pwd=**zapped**;database=employees;SslMode=None;`
//     Provider:               `MySql.Data.MySqlClient`
//     Schema:                 ``
//     Include Views:          `True`
//     Genetated:              `2020/02/10 14:35:24`

using System;
using PetaPoco;

namespace PetaPoco4Core.Database.Employees
{
    /// <summary>
    /// employees 接続文字列
    /// </summary>
    public static class Config
    {
        /// <summary>デフォルトの接続文字列</summary>
        public static string ConnectionString { get; set; }
    }

    /// <summary>
    /// employees Database Object
    /// </summary>
    public class DB : DatabaseExtension
    {
        /// <summary>
        /// employees Database Object
        /// </summary>
        public DB() : base(Config.ConnectionString, "MySql.Data.MySqlClient")
        {
        }
    }


    /// <summary>VIEW</summary>
    [TableName("current_dept_emp")]
    [ExplicitColumns]
    public class CurrentDeptEmp
    {
        /// <summary></summary>
        [Column("emp_no")] public int EmpNo { get; set; }
        /// <summary></summary>
        [Column("dept_no")] public string DeptNo { get; set; }
        /// <summary></summary>
        [Column("from_date")] public DateTime? FromDate { get; set; }
        /// <summary></summary>
        [Column("to_date")] public DateTime? ToDate { get; set; }
    }

    /// <summary></summary>
    [TableName("departments")]
    [PrimaryKey("dept_no", AutoIncrement=false)]
    [ExplicitColumns]
    public class Department: PetaPoco.PetaPocoRecord<Department>
    {
        /// <summary></summary>
        [Column("dept_no")] public string DeptNo { get { return _DeptNo; } set { _DeptNo = value; MarkColumnModified("dept_no"); } } string _DeptNo;
        /// <summary></summary>
        [Column("dept_name")] public string DeptName { get { return _DeptName; } set { _DeptName = value; MarkColumnModified("dept_name"); } } string _DeptName;
    }

    /// <summary></summary>
    [TableName("dept_emp")]
    [PrimaryKey("emp_no,dept_no", AutoIncrement=false)]
    [ExplicitColumns]
    public class DeptEmp: PetaPoco.PetaPocoRecord<DeptEmp>
    {
        /// <summary></summary>
        [Column("emp_no")] public int EmpNo { get { return _EmpNo; } set { _EmpNo = value; MarkColumnModified("emp_no"); } } int _EmpNo;
        /// <summary></summary>
        [Column("dept_no")] public string DeptNo { get { return _DeptNo; } set { _DeptNo = value; MarkColumnModified("dept_no"); } } string _DeptNo;
        /// <summary></summary>
        [Column("from_date")] public DateTime FromDate { get { return _FromDate; } set { _FromDate = value; MarkColumnModified("from_date"); } } DateTime _FromDate;
        /// <summary></summary>
        [Column("to_date")] public DateTime ToDate { get { return _ToDate; } set { _ToDate = value; MarkColumnModified("to_date"); } } DateTime _ToDate;
    }

    /// <summary>VIEW</summary>
    [TableName("dept_emp_latest_date")]
    [ExplicitColumns]
    public class DeptEmpLatestDate
    {
        /// <summary></summary>
        [Column("emp_no")] public int EmpNo { get; set; }
        /// <summary></summary>
        [Column("from_date")] public DateTime? FromDate { get; set; }
        /// <summary></summary>
        [Column("to_date")] public DateTime? ToDate { get; set; }
    }

    /// <summary></summary>
    [TableName("dept_manager")]
    [PrimaryKey("emp_no,dept_no", AutoIncrement=false)]
    [ExplicitColumns]
    public class DeptManager: PetaPoco.PetaPocoRecord<DeptManager>
    {
        /// <summary></summary>
        [Column("emp_no")] public int EmpNo { get { return _EmpNo; } set { _EmpNo = value; MarkColumnModified("emp_no"); } } int _EmpNo;
        /// <summary></summary>
        [Column("dept_no")] public string DeptNo { get { return _DeptNo; } set { _DeptNo = value; MarkColumnModified("dept_no"); } } string _DeptNo;
        /// <summary></summary>
        [Column("from_date")] public DateTime FromDate { get { return _FromDate; } set { _FromDate = value; MarkColumnModified("from_date"); } } DateTime _FromDate;
        /// <summary></summary>
        [Column("to_date")] public DateTime ToDate { get { return _ToDate; } set { _ToDate = value; MarkColumnModified("to_date"); } } DateTime _ToDate;
    }

    /// <summary></summary>
    [TableName("employees")]
    [PrimaryKey("emp_no", AutoIncrement=false)]
    [ExplicitColumns]
    public class Employee: PetaPoco.PetaPocoRecord<Employee>
    {
        /// <summary></summary>
        [Column("emp_no")] public int EmpNo { get { return _EmpNo; } set { _EmpNo = value; MarkColumnModified("emp_no"); } } int _EmpNo;
        /// <summary></summary>
        [Column("birth_date")] public DateTime BirthDate { get { return _BirthDate; } set { _BirthDate = value; MarkColumnModified("birth_date"); } } DateTime _BirthDate;
        /// <summary></summary>
        [Column("first_name")] public string FirstName { get { return _FirstName; } set { _FirstName = value; MarkColumnModified("first_name"); } } string _FirstName;
        /// <summary></summary>
        [Column("last_name")] public string LastName { get { return _LastName; } set { _LastName = value; MarkColumnModified("last_name"); } } string _LastName;
        /// <summary></summary>
        [Column("gender")] public string Gender { get { return _Gender; } set { _Gender = value; MarkColumnModified("gender"); } } string _Gender;
        /// <summary></summary>
        [Column("hire_date")] public DateTime HireDate { get { return _HireDate; } set { _HireDate = value; MarkColumnModified("hire_date"); } } DateTime _HireDate;
    }

    /// <summary></summary>
    [TableName("salaries")]
    [PrimaryKey("emp_no,from_date", AutoIncrement=false)]
    [ExplicitColumns]
    public class Salary: PetaPoco.PetaPocoRecord<Salary>
    {
        /// <summary></summary>
        [Column("emp_no")] public int EmpNo { get { return _EmpNo; } set { _EmpNo = value; MarkColumnModified("emp_no"); } } int _EmpNo;
        /// <summary></summary>
        [Column("salary")] public int _Salary { get { return __Salary; } set { __Salary = value; MarkColumnModified("salary"); } } int __Salary;
        /// <summary></summary>
        [Column("from_date")] public DateTime FromDate { get { return _FromDate; } set { _FromDate = value; MarkColumnModified("from_date"); } } DateTime _FromDate;
        /// <summary></summary>
        [Column("to_date")] public DateTime ToDate { get { return _ToDate; } set { _ToDate = value; MarkColumnModified("to_date"); } } DateTime _ToDate;
    }

    /// <summary></summary>
    [TableName("titles")]
    [PrimaryKey("emp_no,title,from_date", AutoIncrement=false)]
    [ExplicitColumns]
    public class Title: PetaPoco.PetaPocoRecord<Title>
    {
        /// <summary></summary>
        [Column("emp_no")] public int EmpNo { get { return _EmpNo; } set { _EmpNo = value; MarkColumnModified("emp_no"); } } int _EmpNo;
        /// <summary></summary>
        [Column("title")] public string _Title { get { return __Title; } set { __Title = value; MarkColumnModified("title"); } } string __Title;
        /// <summary></summary>
        [Column("from_date")] public DateTime FromDate { get { return _FromDate; } set { _FromDate = value; MarkColumnModified("from_date"); } } DateTime _FromDate;
        /// <summary></summary>
        [Column("to_date")] public DateTime? ToDate { get { return _ToDate; } set { _ToDate = value; MarkColumnModified("to_date"); } } DateTime? _ToDate;
    }
}


