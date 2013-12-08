using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace HSMS.DAL
{
  public  class SettingDAL
    {

      public static void SetValue(string name, string value)
      {
          SqlHelper.ExcuteNonQuery("update T_Setting set Value=@Value where Name=@Name",
                                new SqlParameter("@Name", name),
                                new SqlParameter("@Value", value));
      }

      public static void SetValue(string name, bool value)
      {
          SetValue(name, value.ToString());
      }

      public static void SetValue(string name, int value)
      {
          SetValue(name, value.ToString());
      }

      public static string GetValue(string name)
      {
          DataTable table = SqlHelper.ExecuteDataTable("select * from T_Setting where Name=@Name",
                    new SqlParameter("@Name", name));
          if (table.Rows.Count <= 0)
          {
              throw new Exception(name + "不存在");
          }
          else if (table.Rows.Count > 1)
          {
              throw new Exception("出现" + table.Rows.Count + "条name=" + name + "条setting数据");
          }
          else
          {
              DataRow row = table.Rows[0];
              return (string)row["Value"];
          }
      }
      public static bool GetBoolValue(string name)
      {
          return Convert.ToBoolean(GetValue(name));
      }
      public static int GetIntValue(string name)
      {
          return Convert.ToInt32(GetValue(name));
      }
    }
}
