using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HSMS.Model;
using System.Data;
using System.Collections;
using System.Data.SqlClient;

namespace HSMS.DAL
{
  public  class DepartmentDAL
    {
      private static Department ToModel(DataRow row)
      {
          Department department = new Department();
          department.Id = (Guid)row["Id"];
          department.Name = (string)row["Name"];
          return department;
      }

      public static IEnumerable<Department> ListALL()
      {
          DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Department where IsStopped=0");
          List<Department> list = new List<Department>();
          foreach (DataRow row in dt.Rows)
          {
              list.Add(ToModel(row));
          }
          return list;
      }
      public static Department GetById(Guid id)
      {
          DataTable dt = SqlHelper.ExecuteDataTable("select * from T_Department where Id=@Id",
                            new SqlParameter("@Id", id));
          if (dt.Rows.Count <= 0)
          {
              return null;
          }
          else
          {
              Department department = ToModel(dt.Rows[0]);
              return department;
          }
         
      }

//      public static void Insert(Department department)
//      {
//          SqlHelper.ExcuteNonQuery(@"insert into T_Department(Id,Name,IsStopped) 
//                                    values(@Id,@Name,0)",
//                                                       new SqlParameter("@Id", department.Id),
//                                                       new SqlParameter("@Name", department.Name));
      
      public static void Insert(string name)
      {
          SqlHelper.ExcuteNonQuery(@"insert into T_Department(Id,Name,IsStopped)
                                             values(newid(),@Name,0)",
                                                                     new SqlParameter("@Name", name));
      }
      //public static void Update(Department department)
      //{
      //    SqlHelper.ExcuteNonQuery("Update T_Department set Id=@Id,Name=@Name,IsStopped=@IsStopped",
      //                                      new SqlParameter("@Id", department.Id),
      //                                      new SqlParameter("@Name", department.Name),
      //                                      new SqlParameter("@IsStopped", department.IsStopped));
      //}

      public static void Update(Guid id, string name)
      {
          SqlHelper.ExcuteNonQuery(@"Update T_Department Set Name=@Name where Id=@Id",
              new SqlParameter("@Name", name), new SqlParameter("@Id", id));
      }
      public static void Delete(Department department)
      {
          SqlHelper.ExcuteNonQuery("update T_Department set IsStopped=1 where Id=@Id",
                                    new SqlParameter("@Id",department.Id));
                                            
      }
    }
}
