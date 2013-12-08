using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HSMS.Model;
using System.Data;
using System.Data.SqlClient;

namespace HSMS.DAL
{
    public class T_OperationLogDAL
    {
        private static T_OperationLog ToModel(DataRow row)
        {
            T_OperationLog model = new T_OperationLog();
            model.Id = (System.Guid)row["Id"];
            model.OperatorId = (System.Guid)row["OperatorId"];
            model.MakeDate = (System.DateTime)row["MakeDate"];
            model.ActionDesc = (System.String)row["ActionDesc"];
            return model;
        }

        public static T_OperationLog[] ListAll()
        {
            DataTable dt = SqlHelper.ExecuteDataTable("select * from T_OperationLog");
            T_OperationLog[] models = new T_OperationLog[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                models[i] = ToModel(dt.Rows[i]);
            }
            return models;
        }


        public static void Insert(Guid Id,string actionDesc)
        {
            SqlHelper.ExcuteNonQuery(@"insert into T_OperationLog(Id,OperatorId,MakeDate,ActionDesc)
values(newid(),@OperatorId,getdate(),@ActionDesc)",
            new SqlParameter("@OperatorId", Id),
            new SqlParameter("@ActionDesc", actionDesc));
        }

        public static T_OperationLog[] Search(string sql, SqlParameter[] paramters)
        {
            DataTable table = SqlHelper.ExecuteDataTable(sql, paramters);
            T_OperationLog[] operators = new T_OperationLog[table.Rows.Count];
            for (int i = 0; i < table.Rows.Count; i++)
            {
                operators[i] = ToModel(table.Rows[i]);
            }
            return operators;
        }

    }
}
