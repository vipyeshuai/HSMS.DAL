using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using HSMS.Model;
using System.Data;

namespace HSMS.DAL
{
  public  class SalarySheetDAL
    {
      //判断工资表是否存在
      public static bool IsExist(int year, int month, Guid DeptId)
      {
          object obj=SqlHelper.ExecuteScalar(@"select count(*) from T_SalarySheet where Year=@Year and
                Month=@Month and DeptId=@DeptId",
                                            new SqlParameter("@Year", year),
                                            new SqlParameter("@Month", month),
                                            new SqlParameter("@DeptId", DeptId));
          return (int)obj > 0;
      }

      //清除原有工资报表
       public static void DeleteSalarySheet(int year,int month,Guid DeptId)
       {
           object obj = SqlHelper.ExecuteScalar(@"select Id from T_SalarySheet where
                    Year=@Year and Month=@Month and DeptId=@DeptId",
                                                            new SqlParameter("@Year",year),
                                                            new SqlParameter("@Month",month),
                                                            new SqlParameter("@DeptId",DeptId));

           Guid sheetId = (Guid)obj;

           SqlHelper.ExcuteNonQuery("delete  from T_SalarySheetItem where SheetId=@SheetId",
               new SqlParameter("@SheetId", sheetId));

           SqlHelper.ExcuteNonQuery("delete  from T_SalarySheet where Id=@Id",
               new SqlParameter("@Id", sheetId));
       }

       public static void Build(int year, int month, Guid DeptId)
       {
           Guid sheetId=Guid.NewGuid();
           SqlHelper.ExcuteNonQuery(@"insert into T_SalarySheet(Id,Year,Month,DeptId) 
                values(@Id,@Year,@Month,@DeptId)",
                                                 new SqlParameter("@Id", sheetId),
                                                 new SqlParameter("@Year", year),
                                                 new SqlParameter("@Month", month),
                                                 new SqlParameter("@DeptId", DeptId));
           Employee[] employees=EmployeeDAL.GetEmployeeByDept(DeptId);

           foreach(Employee employee in employees)
           {
               SqlHelper.ExcuteNonQuery(@"insert into T_SalarySheetItem(Id,SheetId,EmployeeId,
                        Bonus,BaseSalary,Fine,Other) values(newid(),@SheetId,@EmployeeId,
                        0,0,0,0)", new SqlParameter("@SheetId", sheetId),
                                new SqlParameter("@EmployeeId", employee.Id));
           }
          
       }

       public static SalarySheetItem[] GetSalarySheetItem(int year, int month, Guid DepartmentId)
       {
           object obj = SqlHelper.ExecuteScalar(@"select Id from T_SalarySheet where
                                Year=@Year and Month=@Month and DeptId=@DeptId",
                                                            new SqlParameter("@Year", year),
                                                            new SqlParameter("@Month", month),
                                                            new SqlParameter("@DeptId", DepartmentId));
           Guid sheetId = (Guid)obj;

           DataTable table = SqlHelper.ExecuteDataTable("select * from T_SalarySheetItem where SheetId=@sheetId",
                                            new SqlParameter("@sheetId", sheetId));
           SalarySheetItem[] items = new SalarySheetItem[table.Rows.Count];
           for (int i = 0; i < table.Rows.Count; i++)
           {
               SalarySheetItem item = new SalarySheetItem();
               DataRow row = table.Rows[i];
               item.Id = (Guid)row["Id"];
              item.EmployeeId = (Guid)row["EmployeeId"];
               item.SheetId = (Guid)row["SheetId"];
               item.BaseSalary = (decimal)row["BaseSalary"];
               item.Bonus = (decimal)row["Bonus"];
               item.Fine = (decimal)row["Fine"];
               item.Other = (decimal)row["Other"];

               items[i] = item;
           }
           return items;
       }


      public static void UpdateSalarySheet(SalarySheetItem item)
      {
          SqlHelper.ExcuteNonQuery(@"update T_SalarySheetItem set BaseSalary=@BaseSalary,
                    Bonus=@Bonus,Fine=@Fine,Other=@Other where Id=@Id",
                                                   new SqlParameter("@BaseSalary",item.BaseSalary),
                                                   new SqlParameter("@Bonus",item.Bonus),
                                                   new SqlParameter("@Fine",item.Fine),
                                                   new SqlParameter("@Other",item.Other),
                                                   new SqlParameter("@Id",item.Id));
      }
    }
}
